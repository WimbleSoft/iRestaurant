using System;
using System.Threading;
using System.Windows.Forms;

namespace iRestaurantPosPrinterSoftware
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application. Signs in automatically when credentials were
        /// remembered, then starts the printing service on a background thread.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string companyEmail, employeeEmail, password;
            short language;
            if (CredentialStore.TryLoad(out companyEmail, out employeeEmail, out password, out language))
            {
                LoginResult result = ServerApi.Login(companyEmail, employeeEmail, password);
                if (result.Success)
                {
                    var dashboardThread = new Thread(() => Application.Run(new Dashboard(companyEmail, employeeEmail, password, language, result)));
                    var printerThread = new Thread(() => new PrinterControl(result.BranchId)) { IsBackground = true };
                    dashboardThread.Start();
                    printerThread.Start();
                    return;
                }

                MessageBox.Show(result.Message);
                CredentialStore.Delete();
            }

            Application.Run(new LoginPanel(null, null, null));
        }
    }
}
