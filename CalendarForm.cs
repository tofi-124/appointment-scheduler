using SchedulingApp.Business;
using SchedulingApp.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedulingApp
{
    public partial class CalendarForm : Form
    {
        public CalendarForm()
        {
            InitializeComponent();
            SetupLocalization();
            monthCalendar.DateChanged += MonthCalendar_DateChanged;
            LoadAppointmentsForDate(monthCalendar.SelectionStart);
        }

        private void SetupLocalization()
        {
            this.Text = LocalizationHelper.Translate("Calendar");
        }

        private void MonthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            LoadAppointmentsForDate(e.Start);
        }

        private void LoadAppointmentsForDate(DateTime selectedDate)
        {
            try
            {
                var appointments = AppointmentService.GetAppointmentsByDate(selectedDate);
                
                dgvDailyAppointments.DataSource = appointments.Select(a => new
                {
                    Time = a.Start.ToString("HH:mm") + " - " + a.End.ToString("HH:mm"),
                    a.Title,
                    a.CustomerName,
                    a.Type,
                    a.Location
                }).ToList();

                lblSelectedDate.Text = $"Appointments for {selectedDate:dddd, MMMM d, yyyy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.monthCalendar = new MonthCalendar();
            this.lblSelectedDate = new Label();
            this.dgvDailyAppointments = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyAppointments)).BeginInit();
            this.SuspendLayout();
            
            this.monthCalendar.Location = new System.Drawing.Point(20, 20);
            this.monthCalendar.MaxSelectionCount = 1;
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 0;
            
            this.lblSelectedDate.AutoSize = true;
            this.lblSelectedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedDate.Location = new System.Drawing.Point(300, 20);
            this.lblSelectedDate.Name = "lblSelectedDate";
            this.lblSelectedDate.Size = new System.Drawing.Size(200, 20);
            this.lblSelectedDate.TabIndex = 1;
            this.lblSelectedDate.Text = "Selected Date";
            
            this.dgvDailyAppointments.AllowUserToAddRows = false;
            this.dgvDailyAppointments.AllowUserToDeleteRows = false;
            this.dgvDailyAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDailyAppointments.Location = new System.Drawing.Point(300, 50);
            this.dgvDailyAppointments.Name = "dgvDailyAppointments";
            this.dgvDailyAppointments.ReadOnly = true;
            this.dgvDailyAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvDailyAppointments.Size = new System.Drawing.Size(450, 300);
            this.dgvDailyAppointments.TabIndex = 2;
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 370);
            this.Controls.Add(this.dgvDailyAppointments);
            this.Controls.Add(this.lblSelectedDate);
            this.Controls.Add(this.monthCalendar);
            this.Name = "CalendarForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Calendar View";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private MonthCalendar monthCalendar;
        private Label lblSelectedDate;
        private DataGridView dgvDailyAppointments;
    }
}
