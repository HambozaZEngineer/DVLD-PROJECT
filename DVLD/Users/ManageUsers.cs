using DVLDBusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ManageUsers : Form
    {
        public ManageUsers()
        {
            InitializeComponent();
        }

        private void _RefreshUsersList()
        {
            DataTable data = User.ListUsers();
            gridUsers.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _AddUser()
        {
            AddEditUser userForm = new AddEditUser(-1);
            userForm.ShowDialog();
            _RefreshUsersList();
        }

        private void _Filter(string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _RefreshUsersList();
                return;
            }

            DataTable data = User.ListUsers();
            string filter = field == "PersonID" || field == "UserID" || field == "IsActive" ? $"{field} = {value}"
                : $"{field} LIKE '%{value.Trim()}%'";
            DataRow[] filteredRows = data.Select(filter);
            DataTable filteredData = data.Clone();

            foreach (DataRow filteredRow in filteredRows)
            {
                filteredData.ImportRow(filteredRow);
            }

            gridUsers.DataSource = filteredData;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;

            _RefreshUsersList();
        }

        private void btnAddUser_Click_1(object sender, EventArgs e)
        {
            _AddUser();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _AddUser();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditUser userForm = new AddEditUser((int)gridUsers.CurrentRow.Cells[0].Value, (int)gridUsers.CurrentRow.Cells[1].Value);
            userForm.ShowDialog();
            _RefreshUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userID = (int)gridUsers.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Are you sure you want to delete the user with id [{userID}]?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (User.Delete(userID))
                {
                    MessageBox.Show("User deleted successfully.", "User Deleted", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("You can't delete this user because it's been connected in the system", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _RefreshUsersList();
        }

        private void changePassworToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword changePasswordForm = new ChangePassword((int)gridUsers.CurrentRow.Cells[0].Value);
            changePasswordForm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInfo userForm = new UserInfo((int)gridUsers.CurrentRow.Cells[0].Value);
            userForm.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("We haven't put this feature yet.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("We haven't put this feature yet.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if ((cmbFilter.Text == "PersonID" || cmbFilter.Text == "UserID") && !int.TryParse(txtFilter.Text, out _))
            {
                txtFilter.Text = string.Empty;
            }

            _Filter(cmbFilter.Text, txtFilter.Text);
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "None")
            {
                txtFilter.Visible = false;
                cmbIsActive.Visible = false;
                _RefreshUsersList();
            }
            else if(cmbFilter.Text == "IsActive")
            {
                txtFilter.Visible = false;
                cmbIsActive.Visible = true;
                cmbIsActive.SelectedIndex = 0;
            }
            else
            {
                txtFilter.Visible = true;
                cmbIsActive.Visible = false;
                txtFilter.Text = string.Empty;
            }
        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbIsActive.Text == "Yes")
            {
                _Filter("IsActive", "1");
            }
            else if(cmbIsActive.Text == "No")
            {
                _Filter("IsActive", "0");
            }
            else
            {
                _RefreshUsersList();
            }
        }
    }
}
