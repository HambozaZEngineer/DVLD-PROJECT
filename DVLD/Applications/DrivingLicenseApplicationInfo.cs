using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class DrivingLicenseApplicationInfo : UserControl
    {
        public DrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void ShowInformation(int licenseAppID, string licenseClass, int passedTests)
        {
            LocalDrivingLicenseApplication licenseApp = LocalDrivingLicenseApplication.FindLicenseApplication(licenseAppID);
            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(licenseApp.ApplicationID);

            lblID.Text = licenseAppID.ToString();
            lblLicense.Text = licenseClass;
            lblPassedTests.Text = $"{passedTests}/3";
        }

        private void DrivingLicenseApplicationInfo_Load(object sender, System.EventArgs e)
        {
            lblLicenseNotFound.Visible = !btnShowLicense.Enabled;
        }
    }
}
