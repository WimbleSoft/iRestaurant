namespace iRestaurantPosPrinterSoftware
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroPanel1 = new MetroFramework.Controls.MetroPanel();
            this.settingsButton = new MetroFramework.Controls.MetroButton();
            this.adminPanelButton = new MetroFramework.Controls.MetroButton();
            this.signOutButton = new MetroFramework.Controls.MetroButton();
            this.cashierPanelButton = new MetroFramework.Controls.MetroButton();
            this.htmlPanel = new MetroFramework.Controls.MetroPanel();
            this.settingsPanel = new MetroFramework.Controls.MetroPanel();
            this.metroPanel6 = new MetroFramework.Controls.MetroPanel();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.employeePrinterName = new MetroFramework.Controls.MetroTextBox();
            this.employeePrinterIp = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.isEmployeePrinterAutomatic = new MetroFramework.Controls.MetroToggle();
            this.cashierPrinterName = new MetroFramework.Controls.MetroTextBox();
            this.cashierPrinterIp = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.isAnnouncementAreaActive = new MetroFramework.Controls.MetroToggle();
            this.metroPanel5 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel3 = new MetroFramework.Controls.MetroPanel();
            this.title = new MetroFramework.Controls.MetroLabel();
            this.metroPanel4 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel2 = new MetroFramework.Controls.MetroPanel();
            this.metroPanel1.SuspendLayout();
            this.htmlPanel.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.metroPanel6.SuspendLayout();
            this.metroPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroPanel1
            // 
            this.metroPanel1.Controls.Add(this.settingsButton);
            this.metroPanel1.Controls.Add(this.adminPanelButton);
            this.metroPanel1.Controls.Add(this.signOutButton);
            this.metroPanel1.Controls.Add(this.cashierPanelButton);
            this.metroPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroPanel1.HorizontalScrollbarBarColor = true;
            this.metroPanel1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel1.HorizontalScrollbarSize = 10;
            this.metroPanel1.Location = new System.Drawing.Point(20, 60);
            this.metroPanel1.Name = "metroPanel1";
            this.metroPanel1.Size = new System.Drawing.Size(1240, 33);
            this.metroPanel1.TabIndex = 2;
            this.metroPanel1.VerticalScrollbarBarColor = true;
            this.metroPanel1.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel1.VerticalScrollbarSize = 10;
            // 
            // settingsButton
            // 
            this.settingsButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingsButton.Location = new System.Drawing.Point(296, 0);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(148, 33);
            this.settingsButton.TabIndex = 10;
            this.settingsButton.Text = "SETTINGS";
            this.settingsButton.UseSelectable = true;
            this.settingsButton.UseStyleColors = true;
            this.settingsButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.settingsButton_MouseClick);
            // 
            // adminPanelButton
            // 
            this.adminPanelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.adminPanelButton.Location = new System.Drawing.Point(148, 0);
            this.adminPanelButton.Name = "adminPanelButton";
            this.adminPanelButton.Size = new System.Drawing.Size(148, 33);
            this.adminPanelButton.TabIndex = 9;
            this.adminPanelButton.Text = "ADMIN PANEL";
            this.adminPanelButton.UseSelectable = true;
            this.adminPanelButton.UseStyleColors = true;
            this.adminPanelButton.Click += new System.EventHandler(this.adminPanelButton_Click_1);
            // 
            // signOutButton
            // 
            this.signOutButton.BackColor = System.Drawing.Color.YellowGreen;
            this.signOutButton.DisplayFocus = true;
            this.signOutButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.signOutButton.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.signOutButton.ForeColor = System.Drawing.Color.White;
            this.signOutButton.Highlight = true;
            this.signOutButton.Location = new System.Drawing.Point(1131, 0);
            this.signOutButton.Name = "signOutButton";
            this.signOutButton.Size = new System.Drawing.Size(109, 33);
            this.signOutButton.TabIndex = 8;
            this.signOutButton.Text = "Close Table Session";
            this.signOutButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.signOutButton.UseCustomBackColor = true;
            this.signOutButton.UseCustomForeColor = true;
            this.signOutButton.UseSelectable = true;
            this.signOutButton.Click += new System.EventHandler(this.signOutButton_Click);
            // 
            // cashierPanelButton
            // 
            this.cashierPanelButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.cashierPanelButton.Location = new System.Drawing.Point(0, 0);
            this.cashierPanelButton.Name = "cashierPanelButton";
            this.cashierPanelButton.Size = new System.Drawing.Size(148, 33);
            this.cashierPanelButton.TabIndex = 3;
            this.cashierPanelButton.Text = "CASHIER";
            this.cashierPanelButton.UseSelectable = true;
            this.cashierPanelButton.UseStyleColors = true;
            this.cashierPanelButton.Click += new System.EventHandler(this.cashierPanelButton_Click);
            // 
            // htmlPanel
            // 
            this.htmlPanel.Controls.Add(this.settingsPanel);
            this.htmlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlPanel.HorizontalScrollbarBarColor = true;
            this.htmlPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.htmlPanel.HorizontalScrollbarSize = 10;
            this.htmlPanel.Location = new System.Drawing.Point(20, 93);
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.Size = new System.Drawing.Size(1240, 607);
            this.htmlPanel.TabIndex = 3;
            this.htmlPanel.VerticalScrollbarBarColor = true;
            this.htmlPanel.VerticalScrollbarHighlightOnWheel = false;
            this.htmlPanel.VerticalScrollbarSize = 10;
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.metroPanel6);
            this.settingsPanel.Controls.Add(this.metroPanel5);
            this.settingsPanel.Controls.Add(this.metroPanel3);
            this.settingsPanel.Controls.Add(this.metroPanel4);
            this.settingsPanel.Controls.Add(this.metroPanel2);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.HorizontalScrollbarBarColor = true;
            this.settingsPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.settingsPanel.HorizontalScrollbarSize = 10;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(1240, 607);
            this.settingsPanel.TabIndex = 2;
            this.settingsPanel.VerticalScrollbarBarColor = true;
            this.settingsPanel.VerticalScrollbarHighlightOnWheel = false;
            this.settingsPanel.VerticalScrollbarSize = 10;
            this.settingsPanel.Visible = false;
            // 
            // metroPanel6
            // 
            this.metroPanel6.Controls.Add(this.btnSave);
            this.metroPanel6.Controls.Add(this.employeePrinterName);
            this.metroPanel6.Controls.Add(this.employeePrinterIp);
            this.metroPanel6.Controls.Add(this.metroLabel4);
            this.metroPanel6.Controls.Add(this.metroLabel5);
            this.metroPanel6.Controls.Add(this.metroLabel6);
            this.metroPanel6.Controls.Add(this.isEmployeePrinterAutomatic);
            this.metroPanel6.Controls.Add(this.cashierPrinterName);
            this.metroPanel6.Controls.Add(this.cashierPrinterIp);
            this.metroPanel6.Controls.Add(this.metroLabel3);
            this.metroPanel6.Controls.Add(this.metroLabel2);
            this.metroPanel6.Controls.Add(this.metroLabel1);
            this.metroPanel6.Controls.Add(this.isAnnouncementAreaActive);
            this.metroPanel6.HorizontalScrollbarBarColor = true;
            this.metroPanel6.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel6.HorizontalScrollbarSize = 10;
            this.metroPanel6.Location = new System.Drawing.Point(193, 100);
            this.metroPanel6.Name = "metroPanel6";
            this.metroPanel6.Size = new System.Drawing.Size(847, 407);
            this.metroPanel6.TabIndex = 7;
            this.metroPanel6.VerticalScrollbarBarColor = true;
            this.metroPanel6.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel6.VerticalScrollbarSize = 10;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(337, 138);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(149, 31);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save Settings";
            this.btnSave.UseSelectable = true;
            this.btnSave.UseStyleColors = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // employeePrinterName
            // 
            // 
            // 
            // 
            this.employeePrinterName.CustomButton.Image = null;
            this.employeePrinterName.CustomButton.Location = new System.Drawing.Point(147, 1);
            this.employeePrinterName.CustomButton.Name = "";
            this.employeePrinterName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.employeePrinterName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.employeePrinterName.CustomButton.TabIndex = 1;
            this.employeePrinterName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.employeePrinterName.CustomButton.UseSelectable = true;
            this.employeePrinterName.CustomButton.Visible = false;
            this.employeePrinterName.Lines = new string[0];
            this.employeePrinterName.Location = new System.Drawing.Point(613, 58);
            this.employeePrinterName.MaxLength = 32767;
            this.employeePrinterName.Name = "employeePrinterName";
            this.employeePrinterName.PasswordChar = '\0';
            this.employeePrinterName.PromptText = "Employee Printer Name";
            this.employeePrinterName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.employeePrinterName.SelectedText = "";
            this.employeePrinterName.SelectionLength = 0;
            this.employeePrinterName.SelectionStart = 0;
            this.employeePrinterName.ShortcutsEnabled = true;
            this.employeePrinterName.Size = new System.Drawing.Size(169, 23);
            this.employeePrinterName.TabIndex = 14;
            this.employeePrinterName.UseSelectable = true;
            this.employeePrinterName.WaterMark = "Employee Printer Name";
            this.employeePrinterName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.employeePrinterName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // employeePrinterIp
            // 
            // 
            // 
            // 
            this.employeePrinterIp.CustomButton.Image = null;
            this.employeePrinterIp.CustomButton.Location = new System.Drawing.Point(147, 1);
            this.employeePrinterIp.CustomButton.Name = "";
            this.employeePrinterIp.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.employeePrinterIp.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.employeePrinterIp.CustomButton.TabIndex = 1;
            this.employeePrinterIp.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.employeePrinterIp.CustomButton.UseSelectable = true;
            this.employeePrinterIp.CustomButton.Visible = false;
            this.employeePrinterIp.Lines = new string[0];
            this.employeePrinterIp.Location = new System.Drawing.Point(613, 92);
            this.employeePrinterIp.MaxLength = 32767;
            this.employeePrinterIp.Name = "employeePrinterIp";
            this.employeePrinterIp.PasswordChar = '\0';
            this.employeePrinterIp.PromptText = "Employee Printer IP";
            this.employeePrinterIp.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.employeePrinterIp.SelectedText = "";
            this.employeePrinterIp.SelectionLength = 0;
            this.employeePrinterIp.SelectionStart = 0;
            this.employeePrinterIp.ShortcutsEnabled = true;
            this.employeePrinterIp.Size = new System.Drawing.Size(169, 23);
            this.employeePrinterIp.TabIndex = 13;
            this.employeePrinterIp.UseSelectable = true;
            this.employeePrinterIp.WaterMark = "Employee Printer IP";
            this.employeePrinterIp.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.employeePrinterIp.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(480, 97);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(108, 19);
            this.metroLabel4.TabIndex = 12;
            this.metroLabel4.Text = "Employee Printer IP";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(480, 58);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(117, 19);
            this.metroLabel5.TabIndex = 11;
            this.metroLabel5.Text = "Employee Printer Name";
            // 
            // metroLabel6
            // 
            this.metroLabel6.AutoSize = true;
            this.metroLabel6.Location = new System.Drawing.Point(480, 18);
            this.metroLabel6.Name = "metroLabel6";
            this.metroLabel6.Size = new System.Drawing.Size(160, 19);
            this.metroLabel6.TabIndex = 10;
            this.metroLabel6.Text = "Employee Printer Automatic";
            // 
            // isEmployeePrinterAutomatic
            // 
            this.isEmployeePrinterAutomatic.AutoSize = true;
            this.isEmployeePrinterAutomatic.Location = new System.Drawing.Point(701, 20);
            this.isEmployeePrinterAutomatic.Name = "isEmployeePrinterAutomatic";
            this.isEmployeePrinterAutomatic.Size = new System.Drawing.Size(80, 17);
            this.isEmployeePrinterAutomatic.TabIndex = 9;
            this.isEmployeePrinterAutomatic.Text = "Off";
            this.isEmployeePrinterAutomatic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.isEmployeePrinterAutomatic.UseSelectable = true;
            // 
            // cashierPrinterName
            // 
            // 
            // 
            // 
            this.cashierPrinterName.CustomButton.Image = null;
            this.cashierPrinterName.CustomButton.Location = new System.Drawing.Point(147, 1);
            this.cashierPrinterName.CustomButton.Name = "";
            this.cashierPrinterName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.cashierPrinterName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.cashierPrinterName.CustomButton.TabIndex = 1;
            this.cashierPrinterName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.cashierPrinterName.CustomButton.UseSelectable = true;
            this.cashierPrinterName.CustomButton.Visible = false;
            this.cashierPrinterName.Lines = new string[0];
            this.cashierPrinterName.Location = new System.Drawing.Point(190, 58);
            this.cashierPrinterName.MaxLength = 32767;
            this.cashierPrinterName.Name = "cashierPrinterName";
            this.cashierPrinterName.PasswordChar = '\0';
            this.cashierPrinterName.PromptText = "Cashier Printer Name";
            this.cashierPrinterName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cashierPrinterName.SelectedText = "";
            this.cashierPrinterName.SelectionLength = 0;
            this.cashierPrinterName.SelectionStart = 0;
            this.cashierPrinterName.ShortcutsEnabled = true;
            this.cashierPrinterName.Size = new System.Drawing.Size(169, 23);
            this.cashierPrinterName.TabIndex = 8;
            this.cashierPrinterName.UseSelectable = true;
            this.cashierPrinterName.WaterMark = "Cashier Printer Name";
            this.cashierPrinterName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.cashierPrinterName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cashierPrinterIp
            // 
            // 
            // 
            // 
            this.cashierPrinterIp.CustomButton.Image = null;
            this.cashierPrinterIp.CustomButton.Location = new System.Drawing.Point(147, 1);
            this.cashierPrinterIp.CustomButton.Name = "";
            this.cashierPrinterIp.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.cashierPrinterIp.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.cashierPrinterIp.CustomButton.TabIndex = 1;
            this.cashierPrinterIp.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.cashierPrinterIp.CustomButton.UseSelectable = true;
            this.cashierPrinterIp.CustomButton.Visible = false;
            this.cashierPrinterIp.Lines = new string[0];
            this.cashierPrinterIp.Location = new System.Drawing.Point(190, 92);
            this.cashierPrinterIp.MaxLength = 32767;
            this.cashierPrinterIp.Name = "cashierPrinterIp";
            this.cashierPrinterIp.PasswordChar = '\0';
            this.cashierPrinterIp.PromptText = "Cashier Printer IP";
            this.cashierPrinterIp.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.cashierPrinterIp.SelectedText = "";
            this.cashierPrinterIp.SelectionLength = 0;
            this.cashierPrinterIp.SelectionStart = 0;
            this.cashierPrinterIp.ShortcutsEnabled = true;
            this.cashierPrinterIp.Size = new System.Drawing.Size(169, 23);
            this.cashierPrinterIp.TabIndex = 7;
            this.cashierPrinterIp.UseSelectable = true;
            this.cashierPrinterIp.WaterMark = "Cashier Printer IP";
            this.cashierPrinterIp.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.cashierPrinterIp.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(57, 97);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(85, 19);
            this.metroLabel3.TabIndex = 6;
            this.metroLabel3.Text = "Cashier Printer IP";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(57, 58);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(94, 19);
            this.metroLabel2.TabIndex = 5;
            this.metroLabel2.Text = "Cashier Printer Name";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(57, 18);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(65, 19);
            this.metroLabel1.TabIndex = 4;
            this.metroLabel1.Text = "Announcements";
            // 
            // isAnnouncementAreaActive
            // 
            this.isAnnouncementAreaActive.AutoSize = true;
            this.isAnnouncementAreaActive.Location = new System.Drawing.Point(278, 20);
            this.isAnnouncementAreaActive.Name = "isAnnouncementAreaActive";
            this.isAnnouncementAreaActive.Size = new System.Drawing.Size(80, 17);
            this.isAnnouncementAreaActive.TabIndex = 3;
            this.isAnnouncementAreaActive.Text = "Off";
            this.isAnnouncementAreaActive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.isAnnouncementAreaActive.UseSelectable = true;
            // 
            // metroPanel5
            // 
            this.metroPanel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroPanel5.HorizontalScrollbarBarColor = true;
            this.metroPanel5.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel5.HorizontalScrollbarSize = 10;
            this.metroPanel5.Location = new System.Drawing.Point(193, 507);
            this.metroPanel5.Name = "metroPanel5";
            this.metroPanel5.Size = new System.Drawing.Size(847, 100);
            this.metroPanel5.TabIndex = 6;
            this.metroPanel5.VerticalScrollbarBarColor = true;
            this.metroPanel5.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel5.VerticalScrollbarSize = 10;
            // 
            // metroPanel3
            // 
            this.metroPanel3.Controls.Add(this.title);
            this.metroPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroPanel3.HorizontalScrollbarBarColor = true;
            this.metroPanel3.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel3.HorizontalScrollbarSize = 10;
            this.metroPanel3.Location = new System.Drawing.Point(193, 0);
            this.metroPanel3.Name = "metroPanel3";
            this.metroPanel3.Size = new System.Drawing.Size(847, 100);
            this.metroPanel3.TabIndex = 5;
            this.metroPanel3.VerticalScrollbarBarColor = true;
            this.metroPanel3.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel3.VerticalScrollbarSize = 10;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.title.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.title.Location = new System.Drawing.Point(337, 36);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(149, 25);
            this.title.TabIndex = 2;
            this.title.Text = "BRANCH SETTINGS";
            // 
            // metroPanel4
            // 
            this.metroPanel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.metroPanel4.HorizontalScrollbarBarColor = true;
            this.metroPanel4.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel4.HorizontalScrollbarSize = 10;
            this.metroPanel4.Location = new System.Drawing.Point(1040, 0);
            this.metroPanel4.Name = "metroPanel4";
            this.metroPanel4.Size = new System.Drawing.Size(200, 607);
            this.metroPanel4.TabIndex = 4;
            this.metroPanel4.VerticalScrollbarBarColor = true;
            this.metroPanel4.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel4.VerticalScrollbarSize = 10;
            // 
            // metroPanel2
            // 
            this.metroPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.metroPanel2.HorizontalScrollbarBarColor = true;
            this.metroPanel2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroPanel2.HorizontalScrollbarSize = 10;
            this.metroPanel2.Location = new System.Drawing.Point(0, 0);
            this.metroPanel2.Name = "metroPanel2";
            this.metroPanel2.Size = new System.Drawing.Size(193, 607);
            this.metroPanel2.TabIndex = 2;
            this.metroPanel2.VerticalScrollbarBarColor = true;
            this.metroPanel2.VerticalScrollbarHighlightOnWheel = false;
            this.metroPanel2.VerticalScrollbarSize = 10;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.htmlPanel);
            this.Controls.Add(this.metroPanel1);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "Dashboard";
            this.Text = "iRestaurant Management / Cashier";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Dashboard_FormClosing);
            this.metroPanel1.ResumeLayout(false);
            this.htmlPanel.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.metroPanel6.ResumeLayout(false);
            this.metroPanel6.PerformLayout();
            this.metroPanel3.ResumeLayout(false);
            this.metroPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroPanel metroPanel1;
        private MetroFramework.Controls.MetroButton cashierPanelButton;
        private MetroFramework.Controls.MetroButton signOutButton;
        private MetroFramework.Controls.MetroButton settingsButton;
        private MetroFramework.Controls.MetroButton adminPanelButton;
        private MetroFramework.Controls.MetroPanel htmlPanel;
        private MetroFramework.Controls.MetroPanel settingsPanel;
        private MetroFramework.Controls.MetroPanel metroPanel6;
        private MetroFramework.Controls.MetroTextBox employeePrinterName;
        private MetroFramework.Controls.MetroTextBox employeePrinterIp;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        private MetroFramework.Controls.MetroToggle isEmployeePrinterAutomatic;
        private MetroFramework.Controls.MetroTextBox cashierPrinterName;
        private MetroFramework.Controls.MetroTextBox cashierPrinterIp;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle isAnnouncementAreaActive;
        private MetroFramework.Controls.MetroPanel metroPanel5;
        private MetroFramework.Controls.MetroPanel metroPanel3;
        private MetroFramework.Controls.MetroLabel title;
        private MetroFramework.Controls.MetroPanel metroPanel4;
        private MetroFramework.Controls.MetroPanel metroPanel2;
        private MetroFramework.Controls.MetroButton btnSave;
    }
}