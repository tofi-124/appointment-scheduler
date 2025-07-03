using System;
using System.Windows.Forms;
using SchedulingApp.Business;

namespace SchedulingApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetupLocalization();
        }

        private void SetupLocalization()
        {
            this.Text = $"{LocalizationHelper.Translate("Welcome")} - {SessionManager.CurrentUser?.UserName}";
            btnCustomers.Text = LocalizationHelper.Translate("Customers");
            btnAppointments.Text = LocalizationHelper.Translate("Appointments");
            btnCalendar.Text = LocalizationHelper.Translate("Calendar");
            btnReports.Text = LocalizationHelper.Translate("Reports");
            btnLogout.Text = LocalizationHelper.Translate("Logout");
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            var customerForm = new CustomerForm();
            customerForm.ShowDialog();
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            var appointmentForm = new AppointmentForm();
            appointmentForm.ShowDialog();
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            var calendarForm = new CalendarForm();
            calendarForm.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.Logout();
            this.Close();
        }

        private void InitializeComponent()
        {
            this.btnCustomers = new Button();
            this.btnAppointments = new Button();
            this.btnCalendar = new Button();
            this.btnReports = new Button();
            this.btnLogout = new Button();
            this.SuspendLayout();
            
            this.btnCustomers.Location = new System.Drawing.Point(50, 50);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(120, 40);
            this.btnCustomers.TabIndex = 0;
            this.btnCustomers.Text = "Customers";
            this.btnCustomers.UseVisualStyleBackColor = true;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            
            this.btnAppointments.Location = new System.Drawing.Point(200, 50);
            this.btnAppointments.Name = "btnAppointments";
            this.btnAppointments.Size = new System.Drawing.Size(120, 40);
            this.btnAppointments.TabIndex = 1;
            this.btnAppointments.Text = "Appointments";
            this.btnAppointments.UseVisualStyleBackColor = true;
            this.btnAppointments.Click += new System.EventHandler(this.btnAppointments_Click);
            
            this.btnCalendar.Location = new System.Drawing.Point(350, 50);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(120, 40);
            this.btnCalendar.TabIndex = 2;
            this.btnCalendar.Text = "Calendar";
            this.btnCalendar.UseVisualStyleBackColor = true;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            
            this.btnReports.Location = new System.Drawing.Point(125, 120);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(120, 40);
            this.btnReports.TabIndex = 3;
            this.btnReports.Text = "Reports";
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            
            this.btnLogout.Location = new System.Drawing.Point(275, 120);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(120, 40);
            this.btnLogout.TabIndex = 4;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 220);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnReports);
            this.Controls.Add(this.btnCalendar);
            this.Controls.Add(this.btnAppointments);
            this.Controls.Add(this.btnCustomers);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Main Menu";
            this.ResumeLayout(false);
        }

        private Button btnCustomers;
        private Button btnAppointments;
        private Button btnCalendar;
        private Button btnReports;
        private Button btnLogout;
    }
}
