using System;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class UserInfo : Form
    {
        public UserInfo(int userID)
        {
            InitializeComponent();

            User user = User.FindUser(userID);

            personInformation.ShowPersonInformation(Person.FindPersonWithID(user.PersonID));
            loginInformation.ShowLoginInformation(user);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
