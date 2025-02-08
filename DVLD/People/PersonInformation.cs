using System.Drawing;
using System.Windows.Forms;
using DVLD.Properties;
using DVLDBusinessLayer;
using System.IO;

namespace DVLD
{
    public partial class PersonInformation : UserControl
    {   
        Person person = new Person();
        int personID;
        string countryName;

        public Person GetPerson => person;

        public PersonInformation()
        {
            InitializeComponent();
        }

        private void _ShowInfo(Person person, string countryName = "")
        {
            btnEdit.Visible = true;
            lblPersonID.Text = person.ID.ToString();
            lblName.Text = $"{person.FirstName} {person.SecondName} {person.ThirdName} {person.LastName}";
            lblNationalNo.Text = person.NationalNo;
            lblGender.Text = person.Gender == (byte)0 ? "Male" : "Female";
            if (person.Email != "") lblEmail.Text = person.Email;
            else lblEmail.Text = "No Email";
            lblAddress.Text = person.Address;
            lblDateOfBirth.Text = person.DateOfBirth.ToString();
            lblPhone.Text = person.Phone;

            if (!string.IsNullOrEmpty(countryName)) 
            { 
                lblCountry.Text = countryName; 
            }
            else
            {
                Country country = Country.FindCountry(person.NationalityCountryID);
                lblCountry.Text = country.CountryName;
            }

            if (person.ImagePath == "")
            {
                picProfile.SizeMode = PictureBoxSizeMode.Normal;

                if (person.Gender == (byte)0)
                {
                    picProfile.Image = Resources.ProfileImageDefaultMale;
                }
                else
                {
                    picProfile.Image = Resources.ProfileImageDefaultFemale;
                }
            }
            else
            {
                picProfile.SizeMode = PictureBoxSizeMode.Zoom;

                byte[] imageBytes = File.ReadAllBytes(person.ImagePath);
                picProfile.Image = Image.FromStream(new MemoryStream(imageBytes));
            }
        }

        public void ResetInformation()
        {
            person = null;
            btnEdit.Visible = false;
            lblPersonID.Text = "[???]";
            lblName.Text = "[???]";
            lblNationalNo.Text = "[???]";
            lblGender.Text = "[???]";
            lblEmail.Text = "[???]";
            lblAddress.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            lblPhone.Text = "[???]";
            lblCountry.Text = "[???]";
            picProfile.Image = Resources.ProfileImageDefaultMale;
        }

        public void ShowPersonInformation(int personID, string countryName)
        {
            person = Person.FindPersonWithID(personID);
            this.personID = personID;
            this.countryName = countryName;

            _ShowInfo(person, this.countryName);
        }

        public void ShowPersonInformation(Person person)
        {
            this.person = person;
            this.personID = person.ID;
            this.countryName = "";

            _ShowInfo(person);
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            AddEditPerson addEditPerson = new AddEditPerson(person.ID);
            addEditPerson.ShowDialog();
            _ShowInfo(Person.FindPersonWithID(personID), this.countryName);
        }
    }
}
