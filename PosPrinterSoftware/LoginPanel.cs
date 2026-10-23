using System;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace iRestaurantPosPrinterSoftware
{
    public partial class LoginPanel : MetroForm
    {
        public LoginPanel(string companyEmail, string employeeEmail, short? languageValue)
        {
            InitializeComponent();
            language.SelectedIndex = 0;
            if (companyEmail != null && employeeEmail != null && languageValue != null)
            {
                this.companyEmail.Text = companyEmail;
                this.employeeEmail.Text = employeeEmail;
                employeePassword.Text = string.Empty;
                rememberMe.Checked = false;
                language.SelectedIndex = languageValue.Value;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            LoginResult result = ServerApi.Login(companyEmail.Text, employeeEmail.Text, employeePassword.Text);
            if (!result.Success)
            {
                errorMessage.Text = result.Message;
                return;
            }

            if (rememberMe.Checked)
                CredentialStore.Save(companyEmail.Text, employeeEmail.Text, employeePassword.Text, language.SelectedIndex);
            else
                CredentialStore.Delete();

            var dashboard = new Dashboard(companyEmail.Text, employeeEmail.Text, employeePassword.Text, (short)language.SelectedIndex, result);
            dashboard.Show();
            Hide();

            // Start the background printing service, as the automatic sign-in at startup does.
            int branchId = result.BranchId;
            new System.Threading.Thread(() => new PrinterControl(branchId)) { IsBackground = true }.Start();
        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("All the terms are listed here.");
        }

        private void acceptTerms_CheckedChanged(object sender, EventArgs e)
        {
            loginButton.Enabled = acceptTerms.Checked;
        }

        private void LoginPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CefSharp.Cef.IsInitialized)
                CefSharp.Cef.Shutdown();
            Application.Exit();
        }
    }
}
