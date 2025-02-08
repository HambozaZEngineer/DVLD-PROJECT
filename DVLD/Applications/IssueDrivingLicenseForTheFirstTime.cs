using System;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class IssueDrivingLicenseForTheFirstTime : Form
    {
        private int _appID, _licenseAppID;
        private Drivers _driver;

        public IssueDrivingLicenseForTheFirstTime(int appID, int licenseAppID, string licenseClass, int passedTests)
        {
            InitializeComponent();

            _appID = appID;
            _licenseAppID = licenseAppID;
            applicationInfo1.ShowInformation(appID);
            drivingLicenseApplicationInfo1.ShowInformation(licenseAppID, licenseClass, passedTests);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(_appID);

            int driverID = Drivers.GetDriverIDForPerson(app.ApplicantPersonID);

            if(driverID == -1)
            {
                _driver = new Drivers();
                _driver.PersonID = app.ApplicantPersonID;
                _driver.CreatedByUserID = Global.currentUser.UserID;
                _driver.CreatedDate = DateTime.Now;

                if (!_driver.Save())
                {
                    MessageBox.Show("Saving Data Failed.");
                    return;
                }

                driverID = _driver.DriverID;
            }

            app.ApplicationStatus = 3;

            if (!app.Save())
            {
                MessageBox.Show("Saving Data Failed.");
                return;
            }

            Licenses license = new Licenses();
            license.ApplicationID = _appID;
            license.DriverID = driverID;
            LocalDrivingLicenseApplication licenseApp = LocalDrivingLicenseApplication.FindLicenseApplication(_licenseAppID);
            license.LicenseClass = licenseApp.LicenseClassID;
            license.IssueDate = DateTime.Now;
            LicenseClasses licenseClass = LicenseClasses.FindLicenseClass(license.LicenseClass);
            DateTime currentDate = DateTime.Now;
            DateTime expirationDate = new DateTime(currentDate.Year + licenseClass.DefaultValidityLength, currentDate.Month, currentDate.Day);
            license.ExpirationDate = expirationDate;
            license.Notes = txtNotes.Text;
            license.PaidFees = licenseClass.ClassFees;
            license.IsActive = true;
            license.IssueReason = 1;
            license.CreatedByUserID = Global.currentUser.UserID;

            if(license.Save())
            {
                MessageBox.Show($"License issued successfully with LicenseID = {license.LicenseID}", "Succeeded", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Saving License Data Failed.");
            }
        }
    }
 }
