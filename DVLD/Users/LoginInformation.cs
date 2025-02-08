using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class LoginInformation : UserControl
    {
        public LoginInformation()
        {
            InitializeComponent();
        }
        
        public void ShowLoginInformation(User user)
        {
            lblUserID.Text = user.UserID.ToString();
            lblUserName.Text = user.UserName;
            lblIsActive.Text = user.IsActive ? "Yes" : "No";
        }
    }
}
