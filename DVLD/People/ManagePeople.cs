using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class ManagePeople : Form
    {
        private void _RefreshPeopleList()
        {
            DataTable data = Person.ListPeople();
            gridPeople.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _AddPerson()
        {
            AddEditPerson addEditPerson = new AddEditPerson(-1);
            addEditPerson.ShowDialog();
            _RefreshPeopleList();
        }

        private void _Filter(string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _RefreshPeopleList();
                return;
            }

            DataTable data = Person.ListPeople();
            string filter = field == "PersonID" ? $"PersonID = {value}" : $"{field} LIKE '%{value.Trim()}%'";
            DataRow[] filteredRows = data.Select(filter);
            DataTable filteredData = data.Clone();

            foreach(DataRow filteredRow in filteredRows)
            {
                filteredData.ImportRow(filteredRow);
            }

            gridPeople.DataSource = filteredData;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        public ManagePeople()
        {
            InitializeComponent();
        }

        private void ManagePeople_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;
            _RefreshPeopleList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            _AddPerson();
        }

        private void addPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _AddPerson();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditPerson addEditPerson = new AddEditPerson((int)gridPeople.CurrentRow.Cells[0].Value);
            addEditPerson.ShowDialog();
            _RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int personID = (int)gridPeople.CurrentRow.Cells[0].Value;

            Person person = Person.FindPersonWithID(personID);

            if(MessageBox.Show($"Are you sure you want to delete person[{personID}]?", "Confirm Delete", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if(Person.Delete(personID))
                {
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    if(person.ImagePath != "") File.Delete(person.ImagePath);
                    
                }
                else
                {
                    MessageBox.Show("Person was not deleted because it has data linked to it", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _RefreshPeopleList();   
        }
        
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonDetails personDetailsForm = new PersonDetails((int)gridPeople.CurrentRow.Cells[0].Value,
                (string)gridPeople.CurrentRow.Cells[8].Value);
            personDetailsForm.ShowDialog();
            _RefreshPeopleList();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbFilter.Text == "None")
            {
                txtFilter.Visible = false;
                _RefreshPeopleList();
            }
            else
            {
                txtFilter.Visible = true;
                txtFilter.Text = string.Empty;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if ((cmbFilter.Text == "PersonID" || cmbFilter.Text == "Phone") && !int.TryParse(txtFilter.Text, out _))
            {
                txtFilter.Text = string.Empty;
            }

            _Filter(cmbFilter.Text, txtFilter.Text);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("We haven't put this feature yet.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void phoneCAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("We haven't put this feature yet.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
