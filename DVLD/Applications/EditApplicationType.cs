using System;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class EditApplicationType : Form
    {
        private ApplicationType applicationType;

        public EditApplicationType(int typeID)
        {
            InitializeComponent();

            applicationType = ApplicationType.FindType(typeID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditApplicationType_Load(object sender, EventArgs e)
        {
            lblID.Text = applicationType.ApplicationTypeID.ToString();
            txtTitle.Text = applicationType.ApplicationTypeTitle;
            txtFees.Text = applicationType.ApplicationFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            applicationType.ApplicationTypeTitle = txtTitle.Text;
            applicationType.ApplicationFees = float.Parse(txtFees.Text);

            if(applicationType.Save())
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
