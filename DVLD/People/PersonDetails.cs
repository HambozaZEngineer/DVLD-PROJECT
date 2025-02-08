using System.Windows.Forms;

namespace DVLD
{
    public partial class PersonDetails : Form
    {
        public PersonDetails(int personID, string countryName)
        {
            InitializeComponent();

            personInformation1.ShowPersonInformation(personID, countryName);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
