using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class AddEditPerson : Form
    {
        private enum Mode { Add_New, Update };
        private Mode _mode = Mode.Add_New;

        public delegate void OnPersonSaved(Person person);
        public event OnPersonSaved onPersonSaved;

        public AddEditPerson(int personID)
        {
            InitializeComponent();

            if (personID == -1)
            {
                _mode = Mode.Add_New;
                addPersonControl1.AddNewMode();
            }
            else
            {
                _mode = Mode.Update;
                lblID.Text = personID.ToString();
                lblHeader.Text = "Edit Person";
                addPersonControl1.UpdateMode(personID);
            }
        }

        private void addPersonControl1_OnCancel()
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            int personID = addPersonControl1.SaveData();

            lblID.Text = personID.ToString();
            lblHeader.Text = "Edit Person";
            onPersonSaved?.Invoke(addPersonControl1.GetPerson);
        }
    }
}
