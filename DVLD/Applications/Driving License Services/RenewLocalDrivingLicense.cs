using System;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications.Driving_License_Services
{
    public partial class RenewLocalDrivingLicense : Form
    {
        private DateTime _currentDate = DateTime.Now;
        private DateTime _expirationDate;
        private decimal _totalFees = 0.0m, _applicationFees = 0.0m;
        private bool _licenseEquipped = false;
        private int _oldLicenseID;
        private int _renewedLicenseID;
        private int _applicationID;

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
            LicenseClasses licenseClass = LicenseClasses.FindLicenseClass(driverLicenseInfo1.GetLicense.LicenseClass);
            lblLicenseFees.Text = licenseClass.ClassFees.ToString();
            _totalFees = _applicationFees + licenseClass.ClassFees;
            lblTotalFees.Text = _totalFees.ToString();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (!_licenseEquipped)
            {
                MessageBox.Show("The local license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (driverLicenseInfo1.GetLicense.IssueDate < driverLicenseInfo1.GetLicense.ExpirationDate)
            {
                MessageBox.Show($"Selected license is not yet expired. It'll expire in {driverLicenseInfo1.GetLicense.ExpirationDate}", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!driverLicenseInfo1.GetLicense.IsActive)
            {
                MessageBox.Show("Renew License issue failed, because the license isn't active.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Licenses.IsLicenseDetained(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show("Renew License issue failed, because the license is detained.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to renew this license?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;


            DVLDBusinessLayer.Application app = new DVLDBusinessLayer.Application();
            app.ApplicantPersonID = driverLicenseInfo1.personID;
            app.ApplicationDate = _currentDate;
            app.ApplicationTypeID = 2;
            app.ApplicationStatus = 3;
            app.LastStatusDate = _currentDate;
            app.PaidFees = _applicationFees;
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

            Licenses renewLicense = new Licenses();
            renewLicense.ApplicationID = app.ApplicationID;
            renewLicense.DriverID = driverLicenseInfo1.GetLicense.DriverID;
            renewLicense.LicenseClass = driverLicenseInfo1.GetLicense.LicenseClass;
            renewLicense.IssueDate = _currentDate;
            renewLicense.ExpirationDate = _expirationDate;
            renewLicense.Notes = txtNotes.Text;
            renewLicense.PaidFees = renewLicense.PaidFees;
            renewLicense.IsActive = true;
            renewLicense.IssueReason = 2;
            renewLicense.CreatedByUserID = Global.currentUser.UserID;

            if (renewLicense.Save())
            {
                MessageBox.Show($"Renewed License issued successfully with ID {renewLicense.LicenseID}", "License Issued",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _renewedLicenseID = renewLicense.LicenseID;
            }
            else
            {
                MessageBox.Show("Saving renewed license failed.");
                return;
            }

            btnRenew.Visible = false;
            btnLicenseInfo.Visible = true;
            grpFilter.Visible = false;
            lblRenewedLicenseID.Text = _renewedLicenseID.ToString();
            lbl_RL_AppID.Text = app.ApplicationID.ToString();
            _applicationID = app.ApplicationID;
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

        private void btnLicenseInfo_Click(object sender, EventArgs e)
        {
            LicenseInfo form = new LicenseInfo(_renewedLicenseID, true);
            form.ShowDialog();
        }

        public RenewLocalDrivingLicense()
        {
            InitializeComponent();

            _expirationDate = new DateTime(_currentDate.Year + 1, _currentDate.Month, _currentDate.Day);
            ApplicationType appType = ApplicationType.FindType(6);
            _applicationFees = (decimal)appType.ApplicationFees;

            lblApplicationDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblIssueDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblAppExpirationDate.Text = _expirationDate.ToString("yyyy-MM-dd");
            lblApplicationFees.Text = _applicationFees.ToString();
            lblCreatedBy.Text = Global.currentUser.UserName;

        }
    }
}
