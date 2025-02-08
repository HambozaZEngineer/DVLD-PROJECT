using DVLDBusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.Applications.Detain_Licenses
{
    public partial class ManageDetainedLicenses : Form
    {
        public ManageDetainedLicenses()
        {
            InitializeComponent();

            _RefreshLicensesList();
        }

        private void _RefreshLicensesList()
        {
            DataTable data = DetainedLicense.ListDetainedLicenses();
            gridLicenses.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _Filter(string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _RefreshLicensesList();
                return;
            }

            DataTable data = DetainedLicense.ListDetainedLicenses();
            string filter = field == "DetainID" || field == "UserID" || field == "IsReleased" || field == "ReleaseApplicationID" ? $"{field} = {value}"
                : $"{field} LIKE '%{value.Trim()}%'";
            DataRow[] filteredRows = data.Select(filter);
            DataTable filteredData = data.Clone();
 
            foreach (DataRow filteredRow in filteredRows)
            {
                filteredData.ImportRow(filteredRow);
            }

            gridLicenses.DataSource = filteredData;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            DetainLicense form = new DetainLicense();
            form.ShowDialog();
            _RefreshLicensesList();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicense form = new ReleaseDetainedLicense();
            form.ShowDialog();
            _RefreshLicensesList();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "None")
            {
                txtFilter.Visible = false;
                cmbIsActive.Visible = false;
                _RefreshLicensesList();
            }
            else if (cmbFilter.Text == "IsReleased")
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
            if (cmbIsActive.Text == "Yes")
            {
                _Filter("IsReleased", "1");
            }
            else if (cmbIsActive.Text == "No")
            {
                _Filter("IsReleased", "0");
            }
            else
            {
                _RefreshLicensesList();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if ((cmbFilter.Text == "DetainID" || cmbFilter.Text == "ReleaseApplicationID") && !int.TryParse(txtFilter.Text, out _))
            {
                txtFilter.Text = string.Empty;
            }

            _Filter(cmbFilter.Text, txtFilter.Text);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Person person = Person.FindPersonWithNationalNo((string)gridLicenses.CurrentRow.Cells[6].Value);
            Country country = Country.FindCountry(person.NationalityCountryID);
            PersonDetails form = new PersonDetails(person.ID, country.CountryName);
            form.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseInfo form = new LicenseInfo((int)gridLicenses.CurrentRow.Cells[1].Value, true);
            form.ShowDialog();
        }

        private void showPersonLicenseHistorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Person person = Person.FindPersonWithNationalNo((string)gridLicenses.CurrentRow.Cells[6].Value);
            LicenseHistory form = new LicenseHistory(person.ID, true);
            form.ShowDialog();
        }
    }
}
