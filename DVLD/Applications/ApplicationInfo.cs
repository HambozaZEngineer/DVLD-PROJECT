using System.Configuration;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class ApplicationInfo : UserControl
    {
        DVLDBusinessLayer.Application _app;
        Person _person;

        public ApplicationInfo()
        {
            InitializeComponent();
        }

        public void ShowInformation(int appID)
        {
            _app = DVLDBusinessLayer.Application.FindApplication(appID);

            lblID.Text = appID.ToString();

            if (_app.ApplicationStatus == 1) lblStatus.Text = "New";
            else if (_app.ApplicationStatus == 2) lblStatus.Text = "Cancelled";
            else lblStatus.Text = "Completed";

            lblFees.Text = _app.PaidFees.ToString();

            TestType testType = TestType.FindType(_app.ApplicationTypeID);
            lblType.Text = testType.Title;

            _person = Person.FindPersonWithID(_app.ApplicantPersonID);
            lblApplicant.Text = $"{_person.FirstName} {_person.SecondName} {_person.ThirdName} {_person.LastName}";

            lblDate.Text = _app.ApplicationDate.ToString();
            lblStatusDate.Text = _app.LastStatusDate.ToString();

            User user = User.FindUser(_app.CreateByUserID);
            lblCreatedBy.Text = user.UserName;
        }

        private void btnViewPerson_Click(object sender, System.EventArgs e)
        {
            Country country = Country.FindCountry(_person.NationalityCountryID);

            PersonDetails detailsForm = new PersonDetails(_app.ApplicantPersonID, country.CountryName);
            detailsForm.ShowDialog();
        }
    }
}
