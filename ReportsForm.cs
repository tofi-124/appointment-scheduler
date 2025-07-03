using SchedulingApp.Business;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedulingApp
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
            SetupLocalization();
        }

        private void SetupLocalization()
        {
            this.Text = LocalizationHelper.Translate("Reports");
            btnAppointmentTypes.Text = "Appointment Types by Month";
            btnUserSchedules.Text = "User Schedules";
            btnCustomerReport.Text = "Customer Appointment Summary";
        }

        private void btnAppointmentTypes_Click(object sender, EventArgs e)
        {
            try
            {
                var appointments = AppointmentService.GetAllAppointments();
                
                var typesByMonth = appointments
                    .GroupBy(a => new { Month = a.Start.ToString("yyyy-MM"), a.Type })
                    .Select(g => new 
                    { 
                        Month = g.Key.Month, 
                        Type = g.Key.Type, 
                        Count = g.Count() 
                    })
                    .OrderBy(x => x.Month)
                    .ThenBy(x => x.Type)
                    .ToList();

                dgvReport.DataSource = typesByMonth;
                lblReportTitle.Text = "Number of Appointment Types by Month";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUserSchedules_Click(object sender, EventArgs e)
        {
            try
            {
                var appointments = AppointmentService.GetAllAppointments();
                
                var userSchedules = appointments
                    .GroupBy(a => a.UserName)
                    .Select(g => new 
                    { 
                        UserName = g.Key,
                        TotalAppointments = g.Count(),
                        UpcomingAppointments = g.Count(a => a.Start > DateTime.Now),
                        NextAppointment = g.Where(a => a.Start > DateTime.Now)
                                          .OrderBy(a => a.Start)
                                          .Select(a => a.Start.ToString("yyyy-MM-dd HH:mm"))
                                          .FirstOrDefault() ?? "None"
                    })
                    .OrderBy(x => x.UserName)
                    .ToList();

                dgvReport.DataSource = userSchedules;
                lblReportTitle.Text = "Schedule for Each User";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCustomerReport_Click(object sender, EventArgs e)
        {
            try
            {
                var appointments = AppointmentService.GetAllAppointments();
                var customers = CustomerService.GetAllCustomers();
                
                var customerSummary = customers
                    .Select(c => new 
                    { 
                        c.CustomerName,
                        c.Phone,
                        c.City,
                        TotalAppointments = appointments.Count(a => a.CustomerId == c.CustomerId),
                        LastAppointment = appointments
                                        .Where(a => a.CustomerId == c.CustomerId)
                                        .OrderByDescending(a => a.Start)
                                        .Select(a => a.Start.ToString("yyyy-MM-dd"))
                                        .FirstOrDefault() ?? "None",
                        NextAppointment = appointments
                                        .Where(a => a.CustomerId == c.CustomerId && a.Start > DateTime.Now)
                                        .OrderBy(a => a.Start)
                                        .Select(a => a.Start.ToString("yyyy-MM-dd"))
                                        .FirstOrDefault() ?? "None"
                    })
                    .OrderByDescending(x => x.TotalAppointments)
                    .ToList();

                dgvReport.DataSource = customerSummary;
                lblReportTitle.Text = "Customer Appointment Summary";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.btnAppointmentTypes = new Button();
            this.btnUserSchedules = new Button();
            this.btnCustomerReport = new Button();
            this.lblReportTitle = new Label();
            this.dgvReport = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            
            this.btnAppointmentTypes.Location = new System.Drawing.Point(20, 20);
            this.btnAppointmentTypes.Name = "btnAppointmentTypes";
            this.btnAppointmentTypes.Size = new System.Drawing.Size(180, 40);
            this.btnAppointmentTypes.TabIndex = 0;
            this.btnAppointmentTypes.Text = "Appointment Types by Month";
            this.btnAppointmentTypes.UseVisualStyleBackColor = true;
            this.btnAppointmentTypes.Click += new System.EventHandler(this.btnAppointmentTypes_Click);
            
            this.btnUserSchedules.Location = new System.Drawing.Point(220, 20);
            this.btnUserSchedules.Name = "btnUserSchedules";
            this.btnUserSchedules.Size = new System.Drawing.Size(180, 40);
            this.btnUserSchedules.TabIndex = 1;
            this.btnUserSchedules.Text = "User Schedules";
            this.btnUserSchedules.UseVisualStyleBackColor = true;
            this.btnUserSchedules.Click += new System.EventHandler(this.btnUserSchedules_Click);
            
            this.btnCustomerReport.Location = new System.Drawing.Point(420, 20);
            this.btnCustomerReport.Name = "btnCustomerReport";
            this.btnCustomerReport.Size = new System.Drawing.Size(180, 40);
            this.btnCustomerReport.TabIndex = 2;
            this.btnCustomerReport.Text = "Customer Appointment Summary";
            this.btnCustomerReport.UseVisualStyleBackColor = true;
            this.btnCustomerReport.Click += new System.EventHandler(this.btnCustomerReport_Click);
            
            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.Location = new System.Drawing.Point(20, 80);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Size = new System.Drawing.Size(150, 24);
            this.lblReportTitle.TabIndex = 3;
            this.lblReportTitle.Text = "Select a Report";
            
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.Location = new System.Drawing.Point(20, 120);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(760, 350);
            this.dgvReport.TabIndex = 4;
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 490);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.lblReportTitle);
            this.Controls.Add(this.btnCustomerReport);
            this.Controls.Add(this.btnUserSchedules);
            this.Controls.Add(this.btnAppointmentTypes);
            this.Name = "ReportsForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Reports";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Button btnAppointmentTypes;
        private Button btnUserSchedules;
        private Button btnCustomerReport;
        private Label lblReportTitle;
        private DataGridView dgvReport;
    }
}
