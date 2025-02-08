using System;
using System.Data;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class ManageTestTypes : Form
    {
        public ManageTestTypes()
        {
            InitializeComponent();
        }

        private void _RefreshTypesList()
        {
            DataTable data = TestType.ListTypes();
            gridTypes.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void ManageTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshTypesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditTestType testForm = new EditTestType((int)gridTypes.CurrentRow.Cells[0].Value);
            testForm.ShowDialog();
            _RefreshTypesList();
        }
    }
}
