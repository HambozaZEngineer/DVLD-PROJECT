using System;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class InternationalLicenseInfo : Form
    {
        public InternationalLicenseInfo(int internationalLicenseID)
        {
            InitializeComponent();

            driverInternationalLicenseInfo1.ShowLicense(internationalLicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
