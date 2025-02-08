using System.Data;
using System.Windows.Forms;
using DVLD.Properties;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class TestAppointments : Form
    {
        private int _appointmentType = 1;
        private int _licenseAppID = -1;
        private string _licenseClass;
        private string _name;

        public TestAppointments(int appID, int licenseAppID, string licenseClass, int passedTests, string name, int appointmentType = 1)
        {
            InitializeComponent();

            this._appointmentType = appointmentType;
            this._licenseAppID = licenseAppID;
            this._licenseClass = licenseClass;
            this._name = name;

            applicationInfo1.ShowInformation(appID);
            drivingLicenseApplicationInfo1.ShowInformation(licenseAppID, licenseClass, passedTests);

            if(_appointmentType == 1)
            {
                picTestIcon.Image = Resources.VisionTestLarge;
                lblTitle.Text = "Vision Test Appointments";
            }
            else if(_appointmentType == 2)
            {
                picTestIcon.Image = Resources.WrittenTestIcon;
                lblTitle.Text = "Written Test Appointments";
            }
            else
            {
                picTestIcon.Image = Resources.StreetTestIcon;
                lblTitle.Text = "Street Test Appointments";
            }

            _RefreshAppointmentsList();
        }

        private void _RefreshAppointmentsList()
        {
            DataTable data = DVLDBusinessLayer.TestAppointments.ListAppointments(_appointmentType, _licenseAppID);
            gridAppointments.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }


        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnAddAppointment_Click(object sender, System.EventArgs e)
        {
            if(Tests.CheckTestPassed(_appointmentType, _licenseAppID))
            {
                MessageBox.Show("This person already has passed this test before, you can only retake failed test", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(DVLDBusinessLayer.TestAppointments.AddAppointmentValidation(_appointmentType, _licenseAppID))
            {
                ScheduleTest testForm = new ScheduleTest(_licenseAppID, _appointmentType, _licenseClass, _name, Tests.CheckTestFailed(_appointmentType, _licenseAppID));
                testForm.ShowDialog();
                _RefreshAppointmentsList();
            }
            else
            {
                MessageBox.Show("Person already has an active appointment for this test, you can't add a new appointment", "Not Allowed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ScheduleTest testForm = new ScheduleTest((int)gridAppointments.CurrentRow.Cells[0].Value);
            testForm.ShowDialog();
            _RefreshAppointmentsList();
        }

        private void contextMenuStrip1_Opened(object sender, System.EventArgs e)
        {
            if ((bool)gridAppointments.CurrentRow.Cells[3].Value)
            {
                editApplicationToolStripMenuItem.Enabled = false;
                takeTestApplicationToolStripMenuItem.Enabled = false;
            }
            else
            {
                editApplicationToolStripMenuItem.Enabled = true;
                takeTestApplicationToolStripMenuItem.Enabled = true;
            }
        }

        private void takeTestApplicationToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            TakeTest testForm = new TakeTest((int)gridAppointments.CurrentRow.Cells[0].Value);
            testForm.ShowDialog();
            _RefreshAppointmentsList();
        }
    }
}
