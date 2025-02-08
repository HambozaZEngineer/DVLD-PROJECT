using DVLD.Applications.Driving_License_Services;
using DVLDBusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.Applications.Manage_Applications
{
    public partial class InternationalLicenseApplications : Form
    {
        public InternationalLicenseApplications()
        {
            InitializeComponent();

            _RefreshApplicationsList();
        }

        private void _RefreshApplicationsList()
        {
            DataTable data = InternationalLicense.ListApplications();
            gridApplications.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _Filter(string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _RefreshApplicationsList();
                return;
            }

            DataTable data = InternationalLicense.ListApplications();
            string filter = $"{field} = {value}";
            DataRow[] filteredRows = data.Select(filter);
            DataTable filteredData = data.Clone();

            foreach (DataRow filteredRow in filteredRows)
            {
                filteredData.ImportRow(filteredRow);
            }

            gridApplications.DataSource = filteredData;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            NewInternationalLicenseApplication form = new NewInternationalLicenseApplication();
            form.ShowDialog();
            _RefreshApplicationsList();
        }

        private void InternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtFilter.Text, out _))
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
                _RefreshApplicationsList();
            }
            else
            {
                txtFilter.Visible = true;
                txtFilter.Text = string.Empty;
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication((int)gridApplications.CurrentRow.Cells[1].Value);
            Person person = Person.FindPersonWithID(app.ApplicantPersonID);
            Country country = Country.FindCountry(person.NationalityCountryID);
            PersonDetails personForm = new PersonDetails(app.ApplicantPersonID, country.CountryName);
            personForm.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Licenses license = Licenses.FindLicense((int)gridApplications.CurrentRow.Cells[3].Value);
            LicenseInfo form = new LicenseInfo(license.ApplicationID);
            form.ShowDialog();
        }

        private void showPersonLicenseHistorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Licenses license = Licenses.FindLicense((int)gridApplications.CurrentRow.Cells[3].Value);
            LicenseHistory form = new LicenseHistory(license.ApplicationID);
            form.ShowDialog();
        }
    }
}
