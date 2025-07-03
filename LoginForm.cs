using SchedulingApp.Business;
using System;
using System.Windows.Forms;

namespace SchedulingApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            SetupLocalization();
        }

        private void SetupLocalization()
        {
            // Get user's current location and set up UI text
            string userLocation = LocalizationHelper.DetectUserLocation();
            
            this.Text = LocalizationHelper.Translate("Login");
            lblUsername.Text = LocalizationHelper.Translate("Username");
            lblPassword.Text = LocalizationHelper.Translate("Password");
            btnLogin.Text = LocalizationHelper.Translate("Login");
            lblLocation.Text = LocalizationHelper.Translate("LocationDetected", userLocation);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var user = UserService.ValidateUser(txtUsername.Text, txtPassword.Text);
                if (user != null)
                {
                    SessionManager.SetCurrentUser(user);
                    
                    // Check for upcoming appointments - helpful reminder feature
                    var upcomingAppointments = AppointmentService.GetUpcomingAppointments(user.UserId);
                    if (upcomingAppointments.Count > 0)
                    {
                        var appointment = upcomingAppointments[0];
                        MessageBox.Show(
                            LocalizationHelper.Translate("AppointmentReminder", appointment.Title),
                            LocalizationHelper.Translate("AppointmentReminder"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            LocalizationHelper.Translate("NoUpcomingAppointments"),
                            LocalizationHelper.Translate("Welcome"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    this.Hide();
                    var mainForm = new MainForm();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show(
                        LocalizationHelper.Translate("InvalidCredentials"),
                        LocalizationHelper.Translate("Login"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.lblUsername = new Label();
            this.txtUsername = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.lblLocation = new Label();
            this.SuspendLayout();
            
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(30, 30);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(70, 17);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            
            this.txtUsername.Location = new System.Drawing.Point(30, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 1;
            
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(30, 90);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(66, 17);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            
            this.txtPassword.Location = new System.Drawing.Point(30, 110);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            
            this.btnLogin.Location = new System.Drawing.Point(30, 150);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 30);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(30, 200);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(100, 17);
            this.lblLocation.TabIndex = 5;
            this.lblLocation.Text = "Location detected:";
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 250);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblLocation;
    }
}
