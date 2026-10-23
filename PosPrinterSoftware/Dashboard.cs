using System;
using System.Linq;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using MetroFramework.Forms;

namespace iRestaurantPosPrinterSoftware
{
    public partial class Dashboard : MetroForm
    {
        public ChromiumWebBrowser browser;
        private readonly string companyEmail;
        private readonly string employeeEmail;
        private readonly string employeePassword; // kept in memory only, to open the panels again
        private readonly short language;
        private readonly int branchId;

        public Dashboard(string companyEmail, string employeeEmail, string employeePassword, short language, LoginResult login)
        {
            this.companyEmail = companyEmail;
            this.employeeEmail = employeeEmail;
            this.employeePassword = employeePassword;
            this.language = language;
            branchId = login.BranchId;

            InitializeComponent();
            InitializeChromium(ServerApi.PanelUrl("Cashier", login.Token));
            browser.RegisterJsObject("BrowserMethods", new BrowserMethods(browser, this));
        }

        private void InitializeChromium(string startUrl)
        {
            if (!Cef.IsInitialized)
                Cef.Initialize(new CefSettings());

            // File URL access stays disabled: the panels are regular web pages.
            browser = new ChromiumWebBrowser(startUrl) { Dock = DockStyle.Fill };
            htmlPanel.Controls.Add(browser);
        }

        /// <summary>Signs in again to get a fresh one-time token and opens the given panel.</summary>
        private void OpenPanel(string panel)
        {
            browser.Visible = true;
            settingsPanel.Visible = false;

            LoginResult login = ServerApi.Login(companyEmail, employeeEmail, employeePassword);
            if (!login.Success)
            {
                MessageBox.Show(login.Message);
                return;
            }
            browser.Load(ServerApi.PanelUrl(panel, login.Token));
        }

        private void cashierPanelButton_Click(object sender, EventArgs e)
        {
            OpenPanel("Cashier");
        }

        private void adminPanelButton_Click_1(object sender, EventArgs e)
        {
            OpenPanel("BranchAdmin");
        }

        private void signOutButton_Click(object sender, EventArgs e)
        {
            CredentialStore.Delete();
            new LoginPanel(companyEmail, employeeEmail, language).Show();
            Hide();
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
            Application.Exit();
        }

        private void settingsButton_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                browser.Hide();
                settingsPanel.Show();
                using (var db = new iRestaurantEntities())
                {
                    Branches branch = db.Branches.First(x => x.branchId == branchId);
                    isAnnouncementAreaActive.CheckState = branch.isAnnouncementAreaActive ? CheckState.Checked : CheckState.Unchecked;
                    isEmployeePrinterAutomatic.CheckState = branch.isEmployeePrinterAutomatic ? CheckState.Checked : CheckState.Unchecked;
                    cashierPrinterName.Text = branch.cashierPrinterName;
                    cashierPrinterIp.Text = branch.cashierPrinterIp;
                    employeePrinterName.Text = branch.employeePrinterName;
                    employeePrinterIp.Text = branch.employeePrinterIp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while opening the settings panel.");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsPrivateIPv4(cashierPrinterIp.Text) || !IsPrivateIPv4(employeePrinterIp.Text))
            {
                MessageBox.Show("Printer addresses must be local network IP addresses (for example 192.168.1.20).");
                return;
            }

            try
            {
                using (var db = new iRestaurantEntities())
                {
                    Branches branch = db.Branches.First(x => x.branchId == branchId);
                    branch.isAnnouncementAreaActive = isAnnouncementAreaActive.Checked;
                    branch.isEmployeePrinterAutomatic = isEmployeePrinterAutomatic.Checked;
                    branch.employeePrinterIp = employeePrinterIp.Text.Trim();
                    branch.employeePrinterName = employeePrinterName.Text;
                    branch.cashierPrinterName = cashierPrinterName.Text;
                    branch.cashierPrinterIp = cashierPrinterIp.Text.Trim();
                    db.SaveChanges();
                }
                MessageBox.Show("Settings saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        /// <summary>Same rule as the web server: empty, or a 10.x / 172.16-31.x / 192.168.x address.</summary>
        private static bool IsPrivateIPv4(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(text.Trim(), out ip) || ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                return false;
            byte[] b = ip.GetAddressBytes();
            return b[0] == 10 || (b[0] == 172 && b[1] >= 16 && b[1] <= 31) || (b[0] == 192 && b[1] == 168);
        }
    }
}
