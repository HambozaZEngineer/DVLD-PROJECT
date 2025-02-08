using System;
using System.Net.Http.Headers;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class AddEditUser : Form
    {
        private enum Mode { Add_User, Edit }
        private Mode _mode = Mode.Add_User;

        public AddEditUser(int userID, int personID = 0)
        {
            InitializeComponent();

            if(userID == -1 && personID == 0)
            {
                _mode = Mode.Add_User;
            }
            else
            {
                _mode = Mode.Edit;

                Person person = Person.FindPersonWithID(personID);

                connectToPerson.Connect(person.NationalNo, userID);

                txtUserName.Text = connectToPerson.GetUser.UserName;
                txtPassword.Text = connectToPerson.GetUser.Password;
                txtConfirmPassword.Text = connectToPerson.GetUser.Password;
                lblHeader.Text = "Edit User";
                lblUserID.Text = connectToPerson.GetUser.UserID.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddEditUser_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!connectToPerson.CheckValidation()) return;

            connectToPerson.GetUser.PersonID = connectToPerson.GetPerson.ID;
            connectToPerson.GetUser.UserName = txtUserName.Text;
            connectToPerson.GetUser.Password = txtPassword.Text;
            connectToPerson.GetUser.IsActive = chkIsActive.Checked;


            if (connectToPerson.GetUser.Save())
            {
                MessageBox.Show($"Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblUserID.Text = connectToPerson.GetUser.UserID.ToString();
                lblHeader.Text = "Edit User";
            }
            else
            {
                MessageBox.Show("Saving Failed.");
            }
        }

        private void txtPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Password can not be blank.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }
        
        private void txtConfirmPassword_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(txtPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Password confirmation doesn't match password.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void connectToPerson_OnNextPressed()
        {
            tabControl1.SelectTab(1);
        }
    }
}
