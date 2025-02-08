using System;
using System.Data;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class LicenseHistory : Form
    {
        private Person _person;
        private int _driverID;

        public LicenseHistory(int appID, bool isPerson = false)
        {
            InitializeComponent();

            if(isPerson)
            {
                _person = Person.FindPersonWithID(appID);
            }
            else
            {
                DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(appID);
                _person = Person.FindPersonWithID(app.ApplicantPersonID);
            }
            
            personInformation1.ShowPersonInformation(_person);

            _driverID = Drivers.GetDriverIDForPerson(_person.ID);

            _RefreshLocalLicensesList();
            _RefreshInternationalLicensesList();
        }

        private void _RefreshLocalLicensesList()
        {
            DataTable data = Licenses.ListLicenses(_driverID);
            gridLocalLicenses.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _RefreshInternationalLicensesList()
        {
            DataTable data = InternationalLicense.ListInternationalLicenses(_driverID);
            gridInternationalLicenses.DataSource = data;
            lblInternationalRecords.Text = data.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
