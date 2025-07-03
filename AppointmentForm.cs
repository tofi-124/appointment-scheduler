using SchedulingApp.Business;
using SchedulingApp.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedulingApp
{
    public partial class AppointmentForm : Form
    {
        private Appointment? selectedAppointment;

        public AppointmentForm()
        {
            InitializeComponent();
            SetupLocalization();
            LoadCustomers();
            LoadUsers();
            LoadAppointments();
        }

        private void SetupLocalization()
        {
            this.Text = LocalizationHelper.Translate("Appointments");
            btnAdd.Text = LocalizationHelper.Translate("Add");
            btnUpdate.Text = LocalizationHelper.Translate("Update");
            btnDelete.Text = LocalizationHelper.Translate("Delete");
            lblTitle.Text = LocalizationHelper.Translate("Title");
            lblDescription.Text = LocalizationHelper.Translate("Description");
            lblLocation.Text = LocalizationHelper.Translate("Location");
            lblContact.Text = LocalizationHelper.Translate("Contact");
            lblType.Text = LocalizationHelper.Translate("Type");
            lblStart.Text = LocalizationHelper.Translate("Start");
            lblEnd.Text = LocalizationHelper.Translate("End");
            lblCustomer.Text = LocalizationHelper.Translate("Customer");
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = CustomerService.GetAllCustomers();
                cmbCustomer.DataSource = customers;
                cmbCustomer.DisplayMember = "CustomerName";
                cmbCustomer.ValueMember = "CustomerId";
                cmbCustomer.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                var users = UserService.GetAllUsers();
                cmbUser.DataSource = users;
                cmbUser.DisplayMember = "UserName";
                cmbUser.ValueMember = "UserId";
                cmbUser.SelectedValue = SessionManager.CurrentUser?.UserId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAppointments()
        {
            try
            {
                var appointments = AppointmentService.GetAllAppointments();
                dgvAppointments.DataSource = appointments.Select(a => new
                {
                    a.AppointmentId,
                    a.Title,
                    a.CustomerName,
                    a.UserName,
                    a.Type,
                    Start = a.Start.ToString("yyyy-MM-dd HH:mm"),
                    End = a.End.ToString("yyyy-MM-dd HH:mm"),
                    a.Location
                }).ToList();

                dgvAppointments.Columns["AppointmentId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAppointments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count > 0)
            {
                var row = dgvAppointments.SelectedRows[0];
                var appointmentId = Convert.ToInt32(row.Cells["AppointmentId"].Value);
                
                try
                {
                    var appointments = AppointmentService.GetAllAppointments();
                    selectedAppointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
                    
                    if (selectedAppointment != null)
                    {
                        txtTitle.Text = selectedAppointment.Title;
                        txtDescription.Text = selectedAppointment.Description;
                        txtLocation.Text = selectedAppointment.Location;
                        txtContact.Text = selectedAppointment.Contact;
                        txtType.Text = selectedAppointment.Type;
                        dtpStart.Value = selectedAppointment.Start;
                        dtpEnd.Value = selectedAppointment.End;
                        cmbCustomer.SelectedValue = selectedAppointment.CustomerId;
                        cmbUser.SelectedValue = selectedAppointment.UserId;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading appointment details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustomer.SelectedValue == null)
                {
                    MessageBox.Show("Please select a customer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var appointment = new Appointment
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Location = txtLocation.Text.Trim(),
                    Contact = txtContact.Text.Trim(),
                    Type = txtType.Text.Trim(),
                    Start = dtpStart.Value,
                    End = dtpEnd.Value,
                    CustomerId = Convert.ToInt32(cmbCustomer.SelectedValue),
                    UserId = Convert.ToInt32(cmbUser.SelectedValue)
                };

                AppointmentService.AddAppointment(appointment);
                MessageBox.Show("Appointment added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAppointments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationHelper.Translate("ValidationError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedAppointment == null)
            {
                MessageBox.Show("Please select an appointment to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                selectedAppointment.Title = txtTitle.Text.Trim();
                selectedAppointment.Description = txtDescription.Text.Trim();
                selectedAppointment.Location = txtLocation.Text.Trim();
                selectedAppointment.Contact = txtContact.Text.Trim();
                selectedAppointment.Type = txtType.Text.Trim();
                selectedAppointment.Start = dtpStart.Value;
                selectedAppointment.End = dtpEnd.Value;
                selectedAppointment.CustomerId = Convert.ToInt32(cmbCustomer.SelectedValue);
                selectedAppointment.UserId = Convert.ToInt32(cmbUser.SelectedValue);

                AppointmentService.UpdateAppointment(selectedAppointment);
                MessageBox.Show("Appointment updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAppointments();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationHelper.Translate("ValidationError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedAppointment == null)
            {
                MessageBox.Show("Please select an appointment to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete appointment '{selectedAppointment.Title}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    AppointmentService.DeleteAppointment(selectedAppointment.AppointmentId);
                    MessageBox.Show("Appointment deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            txtDescription.Clear();
            txtLocation.Clear();
            txtContact.Clear();
            txtType.Clear();
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now.AddHours(1);
            cmbCustomer.SelectedIndex = -1;
            cmbUser.SelectedValue = SessionManager.CurrentUser?.UserId;
            selectedAppointment = null;
        }

        private void InitializeComponent()
        {
            this.dgvAppointments = new DataGridView();
            this.lblTitle = new Label();
            this.txtTitle = new TextBox();
            this.lblDescription = new Label();
            this.txtDescription = new TextBox();
            this.lblLocation = new Label();
            this.txtLocation = new TextBox();
            this.lblContact = new Label();
            this.txtContact = new TextBox();
            this.lblType = new Label();
            this.txtType = new TextBox();
            this.lblStart = new Label();
            this.dtpStart = new DateTimePicker();
            this.lblEnd = new Label();
            this.dtpEnd = new DateTimePicker();
            this.lblCustomer = new Label();
            this.cmbCustomer = new ComboBox();
            this.lblUser = new Label();
            this.cmbUser = new ComboBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            this.SuspendLayout();
            
            this.dgvAppointments.AllowUserToAddRows = false;
            this.dgvAppointments.AllowUserToDeleteRows = false;
            this.dgvAppointments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.Location = new System.Drawing.Point(20, 20);
            this.dgvAppointments.MultiSelect = false;
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.ReadOnly = true;
            this.dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvAppointments.Size = new System.Drawing.Size(850, 250);
            this.dgvAppointments.TabIndex = 0;
            this.dgvAppointments.SelectionChanged += new System.EventHandler(this.dgvAppointments_SelectionChanged);
            
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 290);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(36, 17);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            
            this.txtTitle.Location = new System.Drawing.Point(20, 310);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(150, 23);
            this.txtTitle.TabIndex = 2;
            
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(190, 290);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(79, 17);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description";
            
            this.txtDescription.Location = new System.Drawing.Point(190, 310);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(150, 23);
            this.txtDescription.TabIndex = 4;
            
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(360, 290);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(62, 17);
            this.lblLocation.TabIndex = 5;
            this.lblLocation.Text = "Location";
            
            this.txtLocation.Location = new System.Drawing.Point(360, 310);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(150, 23);
            this.txtLocation.TabIndex = 6;
            
            this.lblContact.AutoSize = true;
            this.lblContact.Location = new System.Drawing.Point(530, 290);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(56, 17);
            this.lblContact.TabIndex = 7;
            this.lblContact.Text = "Contact";
            
            this.txtContact.Location = new System.Drawing.Point(530, 310);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(150, 23);
            this.txtContact.TabIndex = 8;
            
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(700, 290);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(36, 17);
            this.lblType.TabIndex = 9;
            this.lblType.Text = "Type";
            
            this.txtType.Location = new System.Drawing.Point(700, 310);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(150, 23);
            this.txtType.TabIndex = 10;
            
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(20, 350);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(36, 17);
            this.lblStart.TabIndex = 11;
            this.lblStart.Text = "Start";
            
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpStart.Format = DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(20, 370);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowUpDown = true;
            this.dtpStart.Size = new System.Drawing.Size(150, 23);
            this.dtpStart.TabIndex = 12;
            
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(190, 350);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(29, 17);
            this.lblEnd.TabIndex = 13;
            this.lblEnd.Text = "End";
            
            this.dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEnd.Format = DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(190, 370);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.ShowUpDown = true;
            this.dtpEnd.Size = new System.Drawing.Size(150, 23);
            this.dtpEnd.TabIndex = 14;
            
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(360, 350);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(67, 17);
            this.lblCustomer.TabIndex = 15;
            this.lblCustomer.Text = "Customer";
            
            this.cmbCustomer.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(360, 370);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(150, 25);
            this.cmbCustomer.TabIndex = 16;
            
            this.lblUser.AutoSize = true;
            this.lblUser.Location = new System.Drawing.Point(530, 350);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(35, 17);
            this.lblUser.TabIndex = 17;
            this.lblUser.Text = "User";
            
            this.cmbUser.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(530, 370);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(150, 25);
            this.cmbUser.TabIndex = 18;
            
            this.btnAdd.Location = new System.Drawing.Point(20, 420);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            this.btnUpdate.Location = new System.Drawing.Point(120, 420);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 30);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            
            this.btnDelete.Location = new System.Drawing.Point(220, 420);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 470);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cmbUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.cmbCustomer);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.lblStart);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.txtContact);
            this.Controls.Add(this.lblContact);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.dgvAppointments);
            this.Name = "AppointmentForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Appointment Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DataGridView dgvAppointments;
        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblLocation;
        private TextBox txtLocation;
        private Label lblContact;
        private TextBox txtContact;
        private Label lblType;
        private TextBox txtType;
        private Label lblStart;
        private DateTimePicker dtpStart;
        private Label lblEnd;
        private DateTimePicker dtpEnd;
        private Label lblCustomer;
        private ComboBox cmbCustomer;
        private Label lblUser;
        private ComboBox cmbUser;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
    }
}
