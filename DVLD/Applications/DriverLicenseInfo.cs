using System.Drawing;
using System.Windows.Forms;
using DVLD.Properties;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class DriverLicenseInfo : UserControl
    {
        private Licenses license;
        public Licenses GetLicense => license;
        public int personID;
        public int driverID;

        public DriverLicenseInfo()
        {
            InitializeComponent();
        }

        private void _ShowLicense()
        {
            LicenseClasses licenseClass = LicenseClasses.FindLicenseClass(license.LicenseClass);
            lblClass.Text = licenseClass.ClassName;
            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(license.ApplicationID);
            personID = app.ApplicantPersonID;
            Person person = Person.FindPersonWithID(app.ApplicantPersonID);
            lblName.Text = $"{person.FirstName} {person.SecondName} {person.ThirdName} {person.LastName}";
            lblLicenseID.Text = license.LicenseID.ToString();
            lblNationalNo.Text = person.NationalNo;
            lblGender.Text = person.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = license.IssueDate.ToString("yyyy-MM-dd");
            lblIssueReason.Text = license.IssueReason == 1 ? "First Time" : license.IssueReason == 2 ? "Renew" :
            license.IssueReason == 3 ? "Replacement For Damage" : "Replacement For Lost";
            if (!string.IsNullOrEmpty(license.Notes)) lblNotes.Text = license.Notes;
            else lblNotes.Text = "No Notes";
            lblIsActive.Text = license.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToString("yyyy-MM-dd");
            lblDriverID.Text = license.DriverID.ToString();
            driverID = license.DriverID;
            lblExpirationDate.Text = license.ExpirationDate.ToString("yyyy-MM-dd");
            lblIsDetained.Text = Licenses.IsLicenseDetained(license.LicenseID) ? "Yes" : "No";
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

        public void ShowLicenseInformation(int appID)
        {
            license = Licenses.FindLicenseIDUsingAppID(appID);
            _ShowLicense();
        }

        public void ShowLicenseInformationWithLicenseID(int licenseID)
        {
            license = Licenses.FindLicense(licenseID);
            _ShowLicense();
        }
    }
}
