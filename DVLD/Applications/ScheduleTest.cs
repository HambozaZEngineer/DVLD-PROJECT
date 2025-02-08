using DVLD.Properties;
using DVLDBusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class ScheduleTest : Form
    {
        DVLDBusinessLayer.TestAppointments _appointment;
        private int _appointmentType;
        private int _licenseAppID;
        private bool _retakeTest;
        private decimal _totalFees;

        private void _SetImage()
        {
            if (_appointmentType == 1)
            {
                picTestIcon.Image = Resources.VisionTestLarge;
            }
            else if (_appointmentType == 2)
            {
                picTestIcon.Image = Resources.WrittenTestIcon;
            }
            else
            {
                picTestIcon.Image = Resources.StreetTestIcon;
            }
        }

        public ScheduleTest(int licenseAppID, int appointmentType, string licenseClass, string name, bool retakeTest)
        {
            InitializeComponent();

            this._appointmentType = appointmentType;
            this._licenseAppID = licenseAppID;
            _appointment = new DVLDBusinessLayer.TestAppointments();

            _SetImage();

            lblID.Text = licenseAppID.ToString();
            lblClass.Text = licenseClass;
            lblName.Text = name;
            lblTrial.Text = DVLDBusinessLayer.TestAppointments.GetTrails(_appointmentType, _licenseAppID).ToString();

            TestType testType = TestType.FindType(appointmentType);
            _totalFees = (decimal)testType.Fees;
            lblFees.Text = testType.Fees.ToString();

            _retakeTest = retakeTest;
            if (retakeTest)
            {
                lblTitle.Text = "Schedule Retake Test";
                grpRetakeTest.Visible = true;

                ApplicationType appType = ApplicationType.FindType(7);
                lblRAppFees.Text = appType.ApplicationFees.ToString();
                _totalFees += (decimal)appType.ApplicationFees;
                lblTotalFees.Text = _totalFees.ToString();
                lblRAppID.Text = "N/A";
            }
        }

        public ScheduleTest(int appointmentID)
        {
            InitializeComponent();

            _appointment = DVLDBusinessLayer.TestAppointments.FindAppointment(appointmentID);
            LocalDrivingLicenseApplication licenseApp = LocalDrivingLicenseApplication.FindLicenseApplication(_appointment.LocalDrivingLicenseApplicationID);

            lblID.Text = _appointment.LocalDrivingLicenseApplicationID.ToString();

            _appointmentType = _appointment.TestTypeID;
            _licenseAppID = _appointment.LocalDrivingLicenseApplicationID;

            _SetImage();

            LicenseClasses licenseClass = LicenseClasses.FindLicenseClass(licenseApp.LicenseClassID);
            lblClass.Text = licenseClass.ClassName;

            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(licenseApp.ApplicationID);
            Person person = Person.FindPersonWithID(app.ApplicantPersonID);
            lblName.Text = $"{person.FirstName} {person.SecondName} {person.ThirdName} {person.LastName}";

            lblTrial.Text = DVLDBusinessLayer.TestAppointments.GetTrails(_appointmentType, _licenseAppID).ToString();
            lblFees.Text = _appointment.PaidFees.ToString();
            _totalFees = _appointment.PaidFees;

            date.Value = _appointment.AppointmentDate;

            if (_appointment.RetakeTestApplicationID != null)
            {
                lblTitle.Text = "Schedule Retake Test";
                grpRetakeTest.Visible = true;

                ApplicationType appType = ApplicationType.FindType(7);
                lblRAppFees.Text = appType.ApplicationFees.ToString();
                _totalFees += (decimal)appType.ApplicationFees;
                lblTotalFees.Text = _totalFees.ToString();
                lblRAppID.Text = _appointment.RetakeTestApplicationID.ToString();
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int? retakeTestID = null;

            if(_retakeTest)
            {
                DVLDBusinessLayer.Application app = new DVLDBusinessLayer.Application();
                LocalDrivingLicenseApplication licenseApp = LocalDrivingLicenseApplication.FindLicenseApplication(_licenseAppID);
                DVLDBusinessLayer.Application appForPersonID = DVLDBusinessLayer.Application.FindApplication(licenseApp.ApplicationID);
                app.ApplicantPersonID = appForPersonID.ApplicantPersonID;
                app.ApplicationDate = DateTime.Now;
                app.ApplicationTypeID = 7;
                app.ApplicationStatus = 3;
                app.LastStatusDate = DateTime.Now;
                app.PaidFees = decimal.Parse(lblRAppFees.Text);
                app.CreateByUserID = Global.currentUser.UserID;

                if(!app.Save())
                {
                    MessageBox.Show("Saving Retake Application Failed.");
                    return;
                }

                retakeTestID = app.ApplicationID;
            }

            _appointment.TestTypeID = _appointmentType;
            _appointment.LocalDrivingLicenseApplicationID = _licenseAppID;
            _appointment.AppointmentDate = date.Value;
            _appointment.PaidFees = decimal.Parse(lblFees.Text);
            _appointment.CreatedByUserID = Global.currentUser.UserID;
            _appointment.IsLocked = false;
            _appointment.RetakeTestApplicationID = retakeTestID;

            if (_appointment.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Saving Failed");
            }

            this.Close();
        }
    }
}
