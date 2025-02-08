using DVLDBusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.Driving_License_Services
{
    public partial class NewInternationalLicenseApplication : Form
    {
        private DateTime _currentDate = DateTime.Now;
        private DateTime _expirationDate;
        private decimal _fees = 0.0m;
        private bool _licenseEquipped = false;
        private int _localLicenseID;
        private int _internationalLicenseID;

        public NewInternationalLicenseApplication()
        {
            InitializeComponent();

            _expirationDate = new DateTime(_currentDate.Year + 1, _currentDate.Month, _currentDate.Day);
            ApplicationType appType = ApplicationType.FindType(6);
            _fees = (decimal)appType.ApplicationFees;

            lblApplicationDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblIssueDate.Text = _currentDate.ToString("yyyy-MM-dd");
            lblAppExpirationDate.Text = _expirationDate.ToString("yyyy-MM-dd");
            lblFees.Text = _fees.ToString();
            lblCreatedBy.Text = Global.currentUser.UserName;
            
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFindLicense_Click(object sender, EventArgs e)
        {
            int licenseID = -1;

            if(!int.TryParse(txtSearchBar.Text, out licenseID))
            {
                MessageBox.Show("the ID is invalid, please enter only an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(!Licenses.DoesLicenseExist(licenseID))
            {
                MessageBox.Show("the license with the entered ID was not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            driverLicenseInfo1.ShowLicenseInformationWithLicenseID(licenseID);
            _localLicenseID = licenseID;
            lblLocalLicenseID.Text = licenseID.ToString();
            _licenseEquipped = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_licenseEquipped)
            {
                MessageBox.Show("The local license is not equipped yet, please equip it first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (driverLicenseInfo1.GetLicense.IssueDate >= driverLicenseInfo1.GetLicense.ExpirationDate)
            {
                MessageBox.Show("International License issue failed, because the local license is expired.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (InternationalLicense.DoesLicenseExist(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show($"Person already have an active international license with the ID {driverLicenseInfo1.GetLicense.LicenseID}", "Not Allowed", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!driverLicenseInfo1.GetLicense.IsActive)
            {
                MessageBox.Show("International License issue failed, because the local license isn't active.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Licenses.IsLicenseDetained(driverLicenseInfo1.GetLicense.LicenseID))
            {
                MessageBox.Show("International License issue failed, because the local license is detained.", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure you want to issue this license?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;


            DVLDBusinessLayer.Application app = new DVLDBusinessLayer.Application();
            app.ApplicantPersonID = driverLicenseInfo1.personID;
            app.ApplicationDate = _currentDate;
            app.ApplicationTypeID = 6;
            app.ApplicationStatus = 1;
            app.LastStatusDate = _currentDate;
            app.PaidFees = _fees;
            app.CreateByUserID = Global.currentUser.UserID;

            if (!app.Save())
            {
                MessageBox.Show("Application Creation Failed.");
                return;
            }

            InternationalLicense internationalLicense = new InternationalLicense();
            internationalLicense.ApplicationID = app.ApplicationID;
            internationalLicense.DriverID = driverLicenseInfo1.driverID;
            internationalLicense.IssuedUsingLocalLicenseID = _localLicenseID;
            internationalLicense.IssueDate = _currentDate;
            internationalLicense.ExpirationDate = _expirationDate;
            internationalLicense.IsActive = true;
            internationalLicense.CreatedByUserID = Global.currentUser.UserID;

            if(internationalLicense.Save())
            {
                MessageBox.Show($"International License issued successfully with ID {internationalLicense.InternationalLicenseID}", "License Issued", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                _internationalLicenseID = internationalLicense.InternationalLicenseID;
            }
            else
            {
                MessageBox.Show("Saving international license failed.");
                return;
            }

            btnSave.Visible = false;
            btnLicenseInfo.Visible = true;
            grpFilter.Visible = false;
            lbl_IL_LicenseID.Text = app.ApplicationID.ToString();
            lbl_IL_LicenseID.Text = _internationalLicenseID.ToString();
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
            InternationalLicenseInfo infoForm = new InternationalLicenseInfo(_internationalLicenseID);
            infoForm.ShowDialog();
        }
    }
}
