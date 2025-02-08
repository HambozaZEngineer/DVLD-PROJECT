using DVLD.Applications;
using DVLD.Applications.Detain_Licenses;
using DVLD.Applications.Driving_License_Services;
using DVLD.Applications.Manage_Applications;
using DVLD.Presentation_Drivers;
using System;
using System.Threading;
using System.Windows.Forms;

namespace DVLD
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void _SignOut()
        {
            Application.Run(new Login());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Thread thread = new Thread(_SignOut);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManagePeople peopleForm = new ManagePeople();
            peopleForm.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ManageUsers usersForm = new ManageUsers();
            usersForm.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            UserInfo userInfo = new UserInfo(Global.currentUser.UserID);
            userInfo.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword passwordForm = new ChangePassword(Global.currentUser.UserID);
            passwordForm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageApplicationTypes applicationForm = new ManageApplicationTypes();
            applicationForm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageTestTypes testForm = new ManageTestTypes();
            testForm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewLocalDrivingLicenseApplication form = new NewLocalDrivingLicenseApplication(-1);
            form.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplications application = new LocalDrivingLicenseApplications();
            application.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDrivers driversForm = new ManageDrivers();
            driversForm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewInternationalLicenseApplication appForm = new NewInternationalLicenseApplication();
            appForm.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternationalLicenseApplications form = new InternationalLicenseApplications();
            form.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewLocalDrivingLicense form = new RenewLocalDrivingLicense();
            form.ShowDialog();
        }

        private void replacementForLostOrDamageLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacementForDamagedLicense form = new ReplacementForDamagedLicense();
            form.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplications application = new LocalDrivingLicenseApplications();
            application.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetainLicense form = new DetainLicense();
            form.ShowDialog();
        }

        private void managedDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageDetainedLicenses form = new ManageDetainedLicenses();
            form.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicense form = new ReleaseDetainedLicense();
            form.ShowDialog();
        }
    }
}
