using DVLDBusinessLayer;
using System;
using System.Windows.Forms;
namespace DVLD.Applications
{
    public partial class EditTestType : Form
    {
        private TestType testType;

        public EditTestType(int testID)
        {
            InitializeComponent();

            testType = TestType.FindType(testID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditTestType_Load(object sender, EventArgs e)
        {
            lblID.Text = testType.ID.ToString();
            txtTitle.Text = testType.Title;
            txtDescription.Text = testType.Description;
            txtFees.Text = testType.Fees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            testType.Title = txtTitle.Text;
            testType.Description = txtDescription.Text;
            testType.Fees = float.Parse(txtFees.Text);

            if (testType.Save())
            {
                MessageBox.Show($"Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Saving Failed.");
            }
        }
    }
}
