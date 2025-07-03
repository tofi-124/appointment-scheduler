using SchedulingApp.Business;
using SchedulingApp.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SchedulingApp
{
    public partial class CustomerForm : Form
    {
        private Customer? selectedCustomer;

        public CustomerForm()
        {
            InitializeComponent();
            SetupLocalization();
            LoadCustomers();
        }

        private void SetupLocalization()
        {
            this.Text = LocalizationHelper.Translate("Customers");
            btnAdd.Text = LocalizationHelper.Translate("Add");
            btnUpdate.Text = LocalizationHelper.Translate("Update");
            btnDelete.Text = LocalizationHelper.Translate("Delete");
            lblName.Text = LocalizationHelper.Translate("Name");
            lblAddress.Text = LocalizationHelper.Translate("Address");
            lblPhone.Text = LocalizationHelper.Translate("Phone");
            lblCity.Text = LocalizationHelper.Translate("City");
            lblPostalCode.Text = LocalizationHelper.Translate("PostalCode");
            lblCountry.Text = LocalizationHelper.Translate("Country");
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = CustomerService.GetAllCustomers();
                dgvCustomers.DataSource = customers.Select(c => new
                {
                    c.CustomerId,
                    c.CustomerName,
                    c.Address,
                    c.City,
                    c.PostalCode,
                    c.Phone,
                    c.Country
                }).ToList();

                dgvCustomers.Columns["CustomerId"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                var row = dgvCustomers.SelectedRows[0];
                var customerId = Convert.ToInt32(row.Cells["CustomerId"].Value);
                
                try
                {
                    var customers = CustomerService.GetAllCustomers();
                    selectedCustomer = customers.FirstOrDefault(c => c.CustomerId == customerId);
                    
                    if (selectedCustomer != null)
                    {
                        txtName.Text = selectedCustomer.CustomerName;
                        txtAddress.Text = selectedCustomer.Address;
                        txtAddress2.Text = selectedCustomer.Address2;
                        txtPhone.Text = selectedCustomer.Phone;
                        txtCity.Text = selectedCustomer.City;
                        txtPostalCode.Text = selectedCustomer.PostalCode;
                        txtCountry.Text = selectedCustomer.Country;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading customer details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var customer = new Customer
                {
                    CustomerName = txtName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Address2 = txtAddress2.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    PostalCode = txtPostalCode.Text.Trim(),
                    Country = txtCountry.Text.Trim()
                };

                CustomerService.AddCustomer(customer);
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationHelper.Translate("ValidationError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                selectedCustomer.CustomerName = txtName.Text.Trim();
                selectedCustomer.Address = txtAddress.Text.Trim();
                selectedCustomer.Address2 = txtAddress2.Text.Trim();
                selectedCustomer.Phone = txtPhone.Text.Trim();
                selectedCustomer.City = txtCity.Text.Trim();
                selectedCustomer.PostalCode = txtPostalCode.Text.Trim();
                selectedCustomer.Country = txtCountry.Text.Trim();

                CustomerService.UpdateCustomer(selectedCustomer);
                MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationHelper.Translate("ValidationError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete customer '{selectedCustomer.CustomerName}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    CustomerService.DeleteCustomer(selectedCustomer.CustomerId);
                    MessageBox.Show("Customer deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers();
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
            txtName.Clear();
            txtAddress.Clear();
            txtAddress2.Clear();
            txtPhone.Clear();
            txtCity.Clear();
            txtPostalCode.Clear();
            txtCountry.Clear();
            selectedCustomer = null;
        }

        private void InitializeComponent()
        {
            this.dgvCustomers = new DataGridView();
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.lblAddress = new Label();
            this.txtAddress = new TextBox();
            this.txtAddress2 = new TextBox();
            this.lblPhone = new Label();
            this.txtPhone = new TextBox();
            this.lblCity = new Label();
            this.txtCity = new TextBox();
            this.lblPostalCode = new Label();
            this.txtPostalCode = new TextBox();
            this.lblCountry = new Label();
            this.txtCountry = new TextBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.SuspendLayout();
            
            this.dgvCustomers.AllowUserToAddRows = false;
            this.dgvCustomers.AllowUserToDeleteRows = false;
            this.dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomers.Location = new System.Drawing.Point(20, 20);
            this.dgvCustomers.MultiSelect = false;
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.ReadOnly = true;
            this.dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomers.Size = new System.Drawing.Size(750, 250);
            this.dgvCustomers.TabIndex = 0;
            this.dgvCustomers.SelectionChanged += new System.EventHandler(this.dgvCustomers_SelectionChanged);
            
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 290);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(44, 17);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            
            this.txtName.Location = new System.Drawing.Point(20, 310);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(200, 23);
            this.txtName.TabIndex = 2;
            
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(250, 290);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(58, 17);
            this.lblAddress.TabIndex = 3;
            this.lblAddress.Text = "Address";
            
            this.txtAddress.Location = new System.Drawing.Point(250, 310);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(200, 23);
            this.txtAddress.TabIndex = 4;
            
            this.txtAddress2.Location = new System.Drawing.Point(250, 340);
            this.txtAddress2.Name = "txtAddress2";
            this.txtAddress2.Size = new System.Drawing.Size(200, 23);
            this.txtAddress2.TabIndex = 5;
            
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(480, 290);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(44, 17);
            this.lblPhone.TabIndex = 6;
            this.lblPhone.Text = "Phone";
            
            this.txtPhone.Location = new System.Drawing.Point(480, 310);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(200, 23);
            this.txtPhone.TabIndex = 7;
            
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(20, 370);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(30, 17);
            this.lblCity.TabIndex = 8;
            this.lblCity.Text = "City";
            
            this.txtCity.Location = new System.Drawing.Point(20, 390);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(200, 23);
            this.txtCity.TabIndex = 9;
            
            this.lblPostalCode.AutoSize = true;
            this.lblPostalCode.Location = new System.Drawing.Point(250, 370);
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(83, 17);
            this.lblPostalCode.TabIndex = 10;
            this.lblPostalCode.Text = "Postal Code";
            
            this.txtPostalCode.Location = new System.Drawing.Point(250, 390);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(200, 23);
            this.txtPostalCode.TabIndex = 11;
            
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(480, 370);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(57, 17);
            this.lblCountry.TabIndex = 12;
            this.lblCountry.Text = "Country";
            
            this.txtCountry.Location = new System.Drawing.Point(480, 390);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(200, 23);
            this.txtCountry.TabIndex = 13;
            
            this.btnAdd.Location = new System.Drawing.Point(20, 440);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 30);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            this.btnUpdate.Location = new System.Drawing.Point(120, 440);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 30);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            
            this.btnDelete.Location = new System.Drawing.Point(220, 440);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 30);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 490);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.txtPostalCode);
            this.Controls.Add(this.lblPostalCode);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.txtAddress2);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.dgvCustomers);
            this.Name = "CustomerForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Customer Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DataGridView dgvCustomers;
        private Label lblName;
        private TextBox txtName;
        private Label lblAddress;
        private TextBox txtAddress;
        private TextBox txtAddress2;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblCity;
        private TextBox txtCity;
        private Label lblPostalCode;
        private TextBox txtPostalCode;
        private Label lblCountry;
        private TextBox txtCountry;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
    }
}
