using System;
using System.Data;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD.Applications
{
    public partial class ManageApplicationTypes : Form
    {
        public ManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void _RefreshTypesList()
        {
            DataTable data = ApplicationType.ListTypes();
            gridTypes.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshTypesList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditApplicationType editForm = new EditApplicationType((int)gridTypes.CurrentRow.Cells[0].Value);
            editForm.ShowDialog();
            _RefreshTypesList();
        }
    }
}
