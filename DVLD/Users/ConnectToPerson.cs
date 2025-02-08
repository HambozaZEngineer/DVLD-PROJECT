using DVLDBusinessLayer;
using System;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class ConnectToPerson : UserControl
    {
        private User user;
        private bool connectedToPerson = false;

        public User GetUser => user;
        public Person GetPerson => personInformation.GetPerson;
        public bool IsConnected => connectedToPerson;

        public event Action OnNextPressed;
        public bool isDrivingApplication = false;

        public ConnectToPerson()
        {
            InitializeComponent();
        }

        public void Connect(string nationalNo = "", int userID = -1)
        {
            if (!string.IsNullOrEmpty(nationalNo))
            {
                txtSearchBar.Text = nationalNo;
                cmbFilterType.Text = "National No";
                groupFilter.Enabled = false;
            }

            if(userID != -1)
            {
                user = User.FindUser(userID);
            }
            else
            {
                user = new User();
            }

            if (cmbFilterType.Text == "National No")
            {
                Person person = Person.FindPersonWithNationalNo(txtSearchBar.Text);
                if (person.ID == -1)
                {
                    personInformation.ResetInformation();
                    MessageBox.Show($"No person was found with the national no {txtSearchBar.Text}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connectedToPerson = false;
                    return;
                }

                personInformation.ShowPersonInformation(person);
                connectedToPerson = true;
            }
        }

        private void _OnPersonSaved(Person person)
        {
            personInformation.ShowPersonInformation(person);
            txtSearchBar.Text = person.NationalNo.ToString();
        }

        protected virtual void NextPressed()
        {
            Action handler = OnNextPressed;
            if (handler != null) OnNextPressed();
        }

        public void Initialize(int userID)
        {
            user = User.FindUser(userID);
            Person person = Person.FindPersonWithID(user.PersonID);
            connectedToPerson = true;

            personInformation.ShowPersonInformation(person);
            txtSearchBar.Text = person.NationalNo;

        }

        private void ConnectToPerson_Load(object sender, EventArgs e)
        {
            cmbFilterType.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!CheckValidation()) return;

            NextPressed();
        }

        public bool CheckValidation()
        {
            if (!groupFilter.Enabled) return true;

            if (personInformation.GetPerson == null || personInformation.GetPerson.ID == -1)
            {
                MessageBox.Show($"No person is connected, please connect a person and try again.", "No person connected",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (isDrivingApplication) return true;

            if (User.IsPersonConnected(personInformation.GetPerson.ID))
            {
                MessageBox.Show($"Selected person already has a user, choose another one.", "Select another person",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddEditPerson newPersonForm = new AddEditPerson(-1);
            newPersonForm.onPersonSaved += _OnPersonSaved;
            newPersonForm.ShowDialog();
        }
    }
}
