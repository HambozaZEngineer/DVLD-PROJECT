using DVLD.Properties;
using DVLDBusinessLayer;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class TakeTest : Form
    {
        DVLDBusinessLayer.TestAppointments _appointment;
        private int _appointmentType;
        private int _licenseAppID;
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

        public TakeTest(int appointmentID)
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

            lblDate.Text = _appointment.AppointmentDate.ToString();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? you won't be able to change the Pass/Fall after you save?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            Tests test = new Tests();
            test.TestAppointmentID = _appointment.TestAppointmentID;
            test.TestResult = rdPass.Checked;
            test.Notes = txtNotes.Text;
            test.CreatedByUserID = Global.currentUser.UserID;

            if(test.Save())
            {
                _appointment.IsLocked = true;

                if (_appointment.Save())
                {
                    MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Saving Failed.");
            }

            this.Close();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
