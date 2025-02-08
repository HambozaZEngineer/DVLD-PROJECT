using DVLDBusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.Driving_License_Services
{
    public partial class ReplacementForDamagedLicense : Form
    {
        private DateTime _currentDate = DateTime.Now;
        private decimal _fees = 0.0m;
        private decimal _damagedFees = 0.0m;
        private decimal _lostFees = 0.0m;
        private bool _licenseEquipped = false;
        private int _oldLicenseID;
        private int _replacedLicenseID;
        private byte _issueReason = 3;
        private byte _applicationType = 4;

        public ReplacementForDamagedLicense()
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
                MessageBox.Show("The local license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LicenseHistory historyForm = new LicenseHistory(driverLicenseInfo1.personID, true);
            historyForm.ShowDialog();
        }

        private void rdDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if(rdDamaged.Checked)
            {
                lblTitle.Text = "Replacement For Damaged License";
                _issueReason = 3;
                _applicationType = 4;
                _fees = _damagedFees;
            }
            else
            {
                lblTitle.Text = "Replacement For Lost License";
                _issueReason = 4;
                _applicationType = 3;
                _fees = _lostFees;
            }

            lblApplicationFees.Text = _fees.ToString();
        }

        private void ReplacementForDamagedLicense_Load(object sender, EventArgs e)
        {
            ApplicationType damagedApp = ApplicationType.FindType(4);
            ApplicationType lostApp = ApplicationType.FindType(3);
            _damagedFees = (decimal)damagedApp.ApplicationFees;
            _lostFees = (decimal)lostApp.ApplicationFees;
            _fees = _damagedFees;

            lblApplicationDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblApplicationFees.Text = _fees.ToString();
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
            _oldLicenseID = licenseID;
            lblOldLicenseID.Text = licenseID.ToString();
            _licenseEquipped = true;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (!_licenseEquipped)
            {
                MessageBox.Show("The local license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (driverLicenseInfo1.GetLicense.IssueDate >= driverLicenseInfo1.GetLicense.ExpirationDate)
            {
                MessageBox.Show("Replacement License issue failed, because the old license is expired.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!driverLicenseInfo1.GetLicense.IsActive)
            {
                MessageBox.Show("Replacement License issue failed, because the old license isn't active.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Licenses.IsLicenseDetained(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show("Replacement License issue failed, because the old license is detained.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to issue this license?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;


            DVLDBusinessLayer.Application app = new DVLDBusinessLayer.Application();
            app.ApplicantPersonID = driverLicenseInfo1.personID;
            app.ApplicationDate = _currentDate;
            app.ApplicationTypeID = _applicationType;
            app.ApplicationStatus = 3;
            app.LastStatusDate = _currentDate;
            app.PaidFees = _fees;
            app.CreateByUserID = Global.currentUser.UserID;

            if (!app.Save())
            {
                MessageBox.Show("Application Creation Failed.");
                return;
            }

            driverLicenseInfo1.GetLicense.IsActive = false;

            if (!driverLicenseInfo1.GetLicense.Save())
            {
                MessageBox.Show("Old License Edit Failed.");
                return;
            }

            Licenses replacementLicense = new Licenses();
            replacementLicense.ApplicationID = app.ApplicationID;
            replacementLicense.DriverID = driverLicenseInfo1.GetLicense.DriverID;
            replacementLicense.LicenseClass = driverLicenseInfo1.GetLicense.LicenseClass;
            replacementLicense.IssueDate = _currentDate;
            LicenseClasses licenseClass = LicenseClasses.FindLicenseClass(driverLicenseInfo1.GetLicense.LicenseClass);
            replacementLicense.ExpirationDate = new DateTime(_currentDate.Year + licenseClass.DefaultValidityLength, _currentDate.Month, _currentDate.Day);
            replacementLicense.Notes = string.Empty;
            replacementLicense.PaidFees = replacementLicense.PaidFees;
            replacementLicense.IsActive = true;
            replacementLicense.IssueReason = _issueReason;
            replacementLicense.CreatedByUserID = Global.currentUser.UserID;

            if (replacementLicense.Save())
            {
                MessageBox.Show($"Replacement License issued successfully with ID {replacementLicense.LicenseID}", "License Issued",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _replacedLicenseID = replacementLicense.LicenseID;
            }
            else
            {
                MessageBox.Show("Saving renewed license failed.");
                return;
            }

            _replacedLicenseID = replacementLicense.LicenseID;
            btnReplace.Visible = false;
            btnLicenseInfo.Visible = true;
            grpFilter.Visible = false;
            grpReplacementFor.Visible = false;
            lblReplacedLicenseID.Text = _replacedLicenseID.ToString();
            lbl_LR_AppID.Text = app.ApplicationID.ToString();
        }

        private void btnLicenseInfo_Click(object sender, EventArgs e)
        {
            LicenseInfo form = new LicenseInfo(_replacedLicenseID, true);
            form.ShowDialog();
        }
    }
}
