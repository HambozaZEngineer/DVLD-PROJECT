using System;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Threading;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class Login : Form
    {
        private string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

        public Login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SecondForm(object obj)
        {
            System.Windows.Forms.Application.Run(new Main());
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = User.FindUser(txtUserName.Text);
            

            if(user == null || user.Password != txtPassword.Text)
            {
                MessageBox.Show("Invalid Username/Password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!user.IsActive)
            {
                MessageBox.Show("Your account is deactivated, please contact your admin.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (chkRememberMe.Checked)
                {
                    Registry.SetValue(keyPath, "UserName", txtUserName.Text, RegistryValueKind.String);
                    Registry.SetValue(keyPath, "Password", txtPassword.Text, RegistryValueKind.String);
                }
                else
                {
                    Registry.SetValue(keyPath, "UserName", "", RegistryValueKind.String);
                    Registry.SetValue(keyPath, "Password", "", RegistryValueKind.String);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error : {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Global.currentUser = user;
            this.Close();
            Thread thread = new Thread(SecondForm);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                string userName = Registry.GetValue(keyPath, "UserName", null) as string;
                string password = Registry.GetValue(keyPath, "Password", null) as string;

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    chkRememberMe.Checked = true;
                    txtUserName.Text = userName;
                    txtPassword.Text = password;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
