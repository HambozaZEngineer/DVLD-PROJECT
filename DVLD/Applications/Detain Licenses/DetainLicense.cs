using DVLDBusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.Detain_Licenses
{
    public partial class DetainLicense : Form
    {
        private DateTime _currentDate = DateTime.Now;
        private bool _licenseEquipped = false;
        private decimal _detainFees = 150.0m;
        private int _licenseID;
        private int _replacedLicenseID;

        public DetainLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblDetainFees.Text = _detainFees.ToString();
            lblCreatedBy.Text = Global.currentUser.UserName;
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

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!_licenseEquipped)
            {
                MessageBox.Show("The license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Licenses.IsLicenseDetained(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show("This License is already detained.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;

            DetainedLicense detainLicense = new DetainedLicense();
            detainLicense.LicenseID = driverLicenseInfo1.GetLicense.LicenseID;
            detainLicense.DetainDate = _currentDate;
            detainLicense.FineFees = _detainFees;
            detainLicense.CreatedByUserID = Global.currentUser.UserID;
            detainLicense.IsReleased = false;
            detainLicense.ReleaseDate = null;
            detainLicense.ReleasedByUserID = null;
            detainLicense.ReleaseApplicationID = null;

            if (detainLicense.Save())
            {
                MessageBox.Show($"License detained successfully with ID {detainLicense.DetainID}", "License Detained",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Detain license failed.");
            }

            btnDetain.Visible = false;
            btnLicenseInfo.Visible = true;
            grpFilter.Visible = false;
            lblDetainID.Text = detainLicense.DetainID.ToString();
        }

        private void btnLicenseInfo_Click(object sender, EventArgs e)
        {
            LicenseInfo form = new LicenseInfo(driverLicenseInfo1.GetLicense.LicenseID, true);
            form.ShowDialog();
        }
    }
}
