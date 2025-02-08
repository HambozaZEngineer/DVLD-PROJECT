using DVLDBusinessLayer;
using System;
using System.Text;
using System.Windows.Forms;

namespace DVLD.Applications.Driving_License_Services
{
    public partial class NewLocalDrivingLicenseApplication : Form
    {
        private enum Mode { Add_New, Update }
        private Mode _mode = Mode.Add_New;
        LocalDrivingLicenseApplication _licenseApplication;
        DVLDBusinessLayer.Application _application;

        public NewLocalDrivingLicenseApplication(int licenseAppID, string nationalNo = "")
        {
            InitializeComponent();
            connectToPerson.isDrivingApplication = true;

            if(licenseAppID == -1 && nationalNo == string.Empty)
            {
                _mode = Mode.Add_New;

                _licenseApplication = new LocalDrivingLicenseApplication();
                _application = new DVLDBusinessLayer.Application();

                cmbLicenseClass.SelectedIndex = 0;
                lblApplicationDate.Text = DateTime.Now.ToString();

                ApplicationType applicationType = ApplicationType.FindType(1);
                lblApplicationFees.Text = applicationType.ApplicationFees.ToString();
                lblCreatedBy.Text = Global.currentUser.UserName;
            }
            else
            {
                _mode = Mode.Update;
                lblHeader.Text = "Edit Local Driving License Application";

                connectToPerson.Connect(nationalNo);

                _licenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication(licenseAppID);
                _application = DVLDBusinessLayer.Application.FindApplication(_licenseApplication.ApplicationID);

                lblApplicationID.Text = licenseAppID.ToString();
                lblApplicationDate.Text = _application.ApplicationDate.ToString();
                cmbLicenseClass.SelectedIndex = _licenseApplication.LicenseClassID - 1;
                lblApplicationFees.Text = _application.PaidFees.ToString();
                lblCreatedBy.Text = _application.CreateByUserID.ToString();
            }
        }

        private bool _CheckLicenseClassValidation()
        {

            int licenseApplicationID = LocalDrivingLicenseApplication.CheckLicenseValidation(cmbLicenseClass.SelectedIndex + 1, connectToPerson.GetPerson.ID);

            if (licenseApplicationID != -1)
            {
                MessageBox.Show($"Choose another license class, the selected person already has an active application for the selected class with id {licenseApplicationID}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                return false;
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void connectToPerson_OnNextPressed()
        {
            tabControl1.SelectTab(1);
        }

        private void NewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!connectToPerson.CheckValidation()) return;

            if (!_CheckLicenseClassValidation()) return;

            _application.ApplicantPersonID = connectToPerson.GetPerson.ID;
            _application.ApplicationDate = DateTime.Now;
            _application.ApplicationTypeID = 1;
            _application.ApplicationStatus = 1;
            _application.LastStatusDate = _application.ApplicationDate;
            _application.PaidFees = decimal.Parse(lblApplicationFees.Text);
            _application.CreateByUserID = Global.currentUser.UserID;

            if (!_application.Save())
            {
                MessageBox.Show("Saving Failed.");
                return;
            }

            _licenseApplication.ApplicationID = _application.ApplicationID;
            _licenseApplication.LicenseClassID = cmbLicenseClass.SelectedIndex + 1;

            if (_licenseApplication.Save())
            {
                MessageBox.Show($"Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblApplicationID.Text = _application.ApplicationID.ToString();
                lblHeader.Text = "Edit Local Driving License Application";
            }
            else
            {
                MessageBox.Show("Saving Failed.");
                return;
            }
        }
    }
}
