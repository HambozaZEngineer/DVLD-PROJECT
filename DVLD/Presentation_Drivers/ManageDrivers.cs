using System;
using System.Data;
using System.Windows.Forms;
using DVLD.Applications;
using DVLDBusinessLayer;

namespace DVLD.Presentation_Drivers
{
    public partial class ManageDrivers : Form
    {
        public ManageDrivers()
        {
            InitializeComponent();
            _RefreshDriversList();
        }

        private void _RefreshDriversList()
        {
            DataTable data = Drivers.ListDrivers();
            gridDrivers.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _Filter(string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _RefreshDriversList();
                return;
            }

            DataTable data = Drivers.ListDrivers();
            string filter = field == "DriverID" || field == "PersonID" ? $"{field} = {value}" : $"{field} LIKE '%{value.Trim()}%'";
            DataRow[] filteredRows = data.Select(filter);
            DataTable filteredData = data.Clone();

            foreach (DataRow filteredRow in filteredRows)
            {
                filteredData.ImportRow(filteredRow);
            }

            gridDrivers.DataSource = filteredData;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "None")
            {
                txtFilter.Visible = false;
                _RefreshDriversList();
            }
            else
            {
                txtFilter.Visible = true;
                txtFilter.Text = string.Empty;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if ((cmbFilter.Text == "DriverID" || cmbFilter.Text == "PersonID") && !int.TryParse(txtFilter.Text, out _))
            {
                txtFilter.Text = string.Empty;
            }

            _Filter(cmbFilter.Text, txtFilter.Text);
        }

        private void ManageDrivers_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Person person = Person.FindPersonWithNationalNo((string)gridDrivers.CurrentRow.Cells[2].Value);
            Country country = Country.FindCountry(person.NationalityCountryID);
            PersonDetails form = new PersonDetails(person.ID, country.CountryName);
            form.ShowDialog();
        }

        private void showPersonLicenseHistorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Person person = Person.FindPersonWithNationalNo((string)gridDrivers.CurrentRow.Cells[2].Value);
            LicenseHistory form = new LicenseHistory(person.ID, true);
            form.ShowDialog();
        }
    }
}
