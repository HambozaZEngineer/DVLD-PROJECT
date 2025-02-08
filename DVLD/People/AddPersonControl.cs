using DVLDBusinessLayer;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using DVLD.Properties;

namespace DVLD
{
    public partial class AddPersonControl : UserControl
    {
        private enum Mode { Add_New, Update };
        private Mode _mode = Mode.Add_New;
        private Person person = new Person();
        private bool imageRemoved = false;
        private bool isDefaultImage = true;
        private bool imageChanged = false;

        public Person GetPerson => person;

        public AddPersonControl()
        {
            InitializeComponent();
        }

        private void _ShowDefaultProfilePicture()
        {
            if (!isDefaultImage) return;

            picProfile.SizeMode = PictureBoxSizeMode.Normal;

            if (rdMale.Checked)
            {
                picProfile.Image = Resources.ProfileImageDefaultMale;
            }
            else
            {
                picProfile.Image = Resources.ProfileImageDefaultFemale;
            }
        }

        private void _LoadCountries(Mode mode)
        {
            DataTable countries = Country.ListCountries();

            foreach (DataRow country in countries.Rows)
            {
                cmbCountry.Items.Add(country["CountryName"].ToString());

                if (country["CountryName"].ToString() == "Jordan" && mode == Mode.Add_New)
                {
                    cmbCountry.SelectedIndex = int.Parse(country["CountryID"].ToString()) - 1;
                }
                else if (int.Parse(country["CountryID"].ToString()) == person.NationalityCountryID && mode == Mode.Update)
                {
                    cmbCountry.SelectedIndex = int.Parse(country["CountryID"].ToString()) - 1;
                }
            }
        }

        private void _LoadProfileImage()
        {
            if (person.ImagePath != "")
            {
                picProfile.SizeMode = PictureBoxSizeMode.Zoom;
                byte[] imageBytes = File.ReadAllBytes(person.ImagePath);
                picProfile.Image = Image.FromStream(new MemoryStream(imageBytes));
            }
            else
            {
                _ShowDefaultProfilePicture();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
        }

        private void rdMale_CheckedChanged(object sender, EventArgs e)
        {
            _ShowDefaultProfilePicture();
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(Person.DoesPersonExists(txtNationalNo.Text))
            {
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National number is used for another person.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text)) return;

            if(!(txtEmail.Text.Contains("@") && txtEmail.Text.Contains(".com")))
            {
                e.Cancel = true;
                txtEmail.Focus();
                errorProvider1.SetError(txtEmail, "Invalid Email Format");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                e.Cancel = true;
                txtAddress.Focus();
                errorProvider1.SetError(txtAddress, "Address field can not be empty.");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, "");
            }
        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"c:\";
            openFileDialog1.Title = "Please choose the profile picture";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (person.ImagePath == "")
                {
                    Guid guid = Guid.NewGuid();
                    person.ImagePath = @"c:\DVLD-People-Images\" + guid.ToString() + ".png";
                }

                picProfile.ImageLocation = openFileDialog1.FileName;
                picProfile.SizeMode = PictureBoxSizeMode.Zoom;
                btnRemove.Visible = true;
                imageRemoved = false;
                isDefaultImage = false;
                imageChanged = true;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            imageRemoved = true;
            btnRemove.Visible = false;
            isDefaultImage = true;
            _ShowDefaultProfilePicture();
        }

        public int SaveData()
        {
            if (_mode == Mode.Add_New)
            {
                if (person.ImagePath != "") File.Copy(picProfile.ImageLocation, person.ImagePath, overwrite: true);
            }
            else
            {
                if (imageChanged && person.ImagePath != "")
                {
                    File.Copy(picProfile.ImageLocation, person.ImagePath, overwrite: true);
                }
                else if (imageRemoved && person.ImagePath != "")
                {
                    File.Delete(person.ImagePath);
                    person.ImagePath = "";
                }

            }

            person.NationalNo = txtNationalNo.Text;
            person.FirstName = txtFirstName.Text;
            person.SecondName = txtSecondName.Text;
            person.ThirdName = txtThirdName.Text;
            person.LastName = txtFinalName.Text;
            person.DateOfBirth = dateOfBirth.Value;
            person.Email = txtEmail.Text;
            person.Phone = txtPhone.Text;
            person.Address = txtAddress.Text;
            person.NationalityCountryID = cmbCountry.SelectedIndex + 1;
            person.Gender = rdMale.Checked ? (byte)0 : (byte)1;

            if (person.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _mode = Mode.Update;
            }
            else
            {
                MessageBox.Show("Saving Failed");
            }

            return person.ID;
        }

        public void UpdateMode(int personID)
        {
            _mode = Mode.Update;

            person = Person.FindPersonWithID(personID);

            imageChanged = false;
            txtFirstName.Text = person.FirstName;
            txtSecondName.Text = person.SecondName;
            txtThirdName.Text = person.ThirdName;
            txtFinalName.Text = person.LastName;
            txtNationalNo.Text = person.NationalNo;
            dateOfBirth.Value = person.DateOfBirth;
            if (person.Gender == 0) { rdMale.Checked = true; }
            else if (person.Gender == 1) { rdFemale.Checked = true; }

            isDefaultImage = true;
            if (person.ImagePath != "")
            {
                btnRemove.Visible = true;
                imageRemoved = false;
                isDefaultImage = false;
            }

            txtPhone.Text = person.Phone;
            txtEmail.Text = person.Email;
            txtAddress.Text = person.Address;

            _LoadProfileImage();

            _LoadCountries(Mode.Update);
        }

        public void AddNewMode()
        {
            _mode = Mode.Add_New;
            rdMale.Checked = true;
            _LoadCountries(Mode.Add_New);
        }

        private void AddPersonControl_Load(object sender, EventArgs e)
        {
            DateTime currentDate = DateTime.Now;
            DateTime maxDate = new DateTime(currentDate.Year - 18, currentDate.Month, currentDate.Day);

            dateOfBirth.MaxDate = maxDate;
        }

        private void rdFemale_CheckedChanged(object sender, EventArgs e)
        {
            _ShowDefaultProfilePicture();
        }
    }
}