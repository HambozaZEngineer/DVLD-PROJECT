using DVLDBusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.Detain_Licenses
{
    public partial class ReleaseDetainedLicense : Form
    {
        private DateTime _currentDate = DateTime.Now;
        private bool _licenseEquipped = false;
        private decimal _detainFees = 150.0m;
        private decimal _applicationFees = 0.0m;
        private decimal _totalFees = 0.0m;
        private int _licenseID;
        private int _detainID;

        public ReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLicensesHistory_Click(object sender, EventArgs e)
        {
            if (!_licenseEquipped)
            {
                MessageBox.Show("The license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LicenseHistory historyForm = new LicenseHistory(driverLicenseInfo1.personID, true);
            historyForm.ShowDialog();
        }

        private void btnLicenseInfo_Click(object sender, EventArgs e)
        {
            LicenseInfo form = new LicenseInfo(driverLicenseInfo1.GetLicense.LicenseID, true);
            form.ShowDialog();
        }

        private void btnFindLicense_Click(object sender, EventArgs e)
        {
            int licenseID = -1;

            if (!int.TryParse(txtSearchBar.Text, out licenseID))
            {
                MessageBox.Show("the ID is invalid, please enter only an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Licenses.DoesLicenseExist(licenseID))
            {
                MessageBox.Show("the license with the entered ID was not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            driverLicenseInfo1.ShowLicenseInformationWithLicenseID(licenseID);
            _licenseID = licenseID;
            lblLicenseID.Text = licenseID.ToString();
            _licenseEquipped = true;

            if (!Licenses.IsLicenseDetained(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show("This License is not detained.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _detainID = DetainedLicense.GetLicenseDetainID(_licenseID);
            lblDetainID.Text = _detainID.ToString();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (!_licenseEquipped)
            {
                MessageBox.Show("The license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Licenses.IsLicenseDetained(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show("This License is not detained.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to release this license?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;

            DVLDBusinessLayer.Application app = new DVLDBusinessLayer.Application();
            app.ApplicantPersonID = driverLicenseInfo1.personID;
            app.ApplicationDate = _currentDate;
            app.ApplicationTypeID = 5;
            app.ApplicationStatus = 3;
            app.LastStatusDate = _currentDate;
            app.PaidFees = _applicationFees;
            app.CreateByUserID = Global.currentUser.UserID;

            if (!app.Save())
            {
                MessageBox.Show("Application Creation Failed.");
                return;
            }

            DetainedLicense detainedLicense = DetainedLicense.FindDetainedLicense(_detainID);
            detainedLicense.IsReleased = true;
            detainedLicense.ReleaseDate = _currentDate;
            detainedLicense.ReleasedByUserID = Global.currentUser.UserID;
            detainedLicense.ReleaseApplicationID = app.ApplicationID;

            if (detainedLicense.Save())
            {
                MessageBox.Show($"License Released Successfully!", "License Released",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Detain license failed.");
            }

            btnRelease.Visible = false;
            btnLicenseInfo.Visible = true;
            grpFilter.Visible = false;
            lblApplicationID.Text = app.ApplicationID.ToString();
        }

        private void ReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblDetainFees.Text = _detainFees.ToString();
            ApplicationType appType = ApplicationType.FindType(5);
            lblApplicationFees.Text = appType.ApplicationFees.ToString();
            _applicationFees = (decimal)appType.ApplicationFees;
            _totalFees = (decimal)appType.ApplicationFees + _detainFees;
            lblTotalFees.Text = _totalFees.ToString();
            lblCreatedBy.Text = Global.currentUser.UserName;
        }
    }
}
