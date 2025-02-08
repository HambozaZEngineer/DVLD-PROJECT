using System;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class LicenseInfo : Form
    {
        public LicenseInfo(int appID, bool isLicense = false)
        {
            InitializeComponent();

            if (!isLicense) driverLicenseInfo1.ShowLicenseInformation(appID);
            else driverLicenseInfo1.ShowLicenseInformationWithLicenseID(appID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
