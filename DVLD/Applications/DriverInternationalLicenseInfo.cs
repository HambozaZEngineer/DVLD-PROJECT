using DVLD.Properties;
using DVLDBusinessLayer;
using System.Windows.Forms;
using System.Drawing;

namespace DVLD.Applications
{
    public partial class DriverInternationalLicenseInfo : UserControl
    {
        public DriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public void ShowLicense(int internationalLicenseID)
        {
            InternationalLicense internationalLicense = InternationalLicense.FindInternationalLicense(internationalLicenseID);
            Licenses license = Licenses.FindLicense(internationalLicense.IssuedUsingLocalLicenseID);
            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(license.ApplicationID);
            Person person = Person.FindPersonWithID(app.ApplicantPersonID);
            lblInternationalLicenseID.Text = internationalLicense.InternationalLicenseID.ToString();
            lblLicenseID.Text = license.LicenseID.ToString();
            lblApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblName.Text = $"{person.FirstName} {person.SecondName} {person.ThirdName} {person.LastName}";
            lblLicenseID.Text = license.LicenseID.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblGender.Text = person.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = internationalLicense.IssueDate.ToString("yyyy-MM-dd");
            lblIsActive.Text = internationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToString("yyyy-MM-dd");
            lblDriverID.Text = license.DriverID.ToString();
            lblExpirationDate.Text = internationalLicense.ExpirationDate.ToString("yyyy-MM-dd");

            if (string.IsNullOrEmpty(person.ImagePath))
            {
                if (person.Gender == 0) picProfile.Image = Resources.ProfileImageDefaultMale;
                else picProfile.Image = Resources.ProfileImageDefaultFemale;
            }
            else
            {
                picProfile.SizeMode = PictureBoxSizeMode.Zoom;
                picProfile.Image = Image.FromFile(person.ImagePath);
            }
        }
    }
}
