using System.Data;
using System.Windows.Forms;
using DVLD.Applications.Driving_License_Services;
using DVLDBusinessLayer;

namespace DVLD.Applications.Manage_Applications
{
    public partial class LocalDrivingLicenseApplications : Form
    {
        public LocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void _Filter(string field, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _RefreshApplicationsList();
                return;
            }

            DataTable data = LocalDrivingLicenseApplication.ListApplications();
            string filter = field == "L.D.L_AppID" ? $"L.D.L_AppID = {value}" : $"{field} LIKE '%{value.Trim()}%'";
            DataRow[] filteredRows = data.Select(filter);
            DataTable filteredData = data.Clone();

            foreach (DataRow filteredRow in filteredRows)
            {
                filteredData.ImportRow(filteredRow);
            }

            gridApplications.DataSource = filteredData;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void _RefreshApplicationsList()
        {
            DataTable data = LocalDrivingLicenseApplication.ListApplications();
            gridApplications.DataSource = data;
            lblRecords.Text = data.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void LocalDrivingLicenseApplications_Load(object sender, System.EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;

            _RefreshApplicationsList();
        }

        private void btnAddApplication_Click(object sender, System.EventArgs e)
        {
            NewLocalDrivingLicenseApplication application = new NewLocalDrivingLicenseApplication(-1);
            application.ShowDialog();
            _RefreshApplicationsList();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cmbFilter.Text == "None")
            {
                txtFilter.Visible = false;
                _RefreshApplicationsList();
            }
            else
            {
                txtFilter.Visible = true;
                txtFilter.Text = string.Empty;
            }
        }

        private void txtFilter_TextChanged(object sender, System.EventArgs e)
        {
            if ((cmbFilter.Text == "L.D.L_AppID") && !int.TryParse(txtFilter.Text, out _))
            {
                txtFilter.Text = string.Empty;
            }

            _Filter(cmbFilter.Text, txtFilter.Text);
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete this application?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            LocalDrivingLicenseApplication localDrivingLicenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value);
            int appID = localDrivingLicenseApplication.ApplicationID;
            bool deleteFailed = false;

            if (LocalDrivingLicenseApplication.DeleteApplication(localDrivingLicenseApplication.LocalDrivingLicenseApplicationID))
            {
                if (DVLDBusinessLayer.Application.DeleteApplication(localDrivingLicenseApplication.ApplicationID))
                {
                    MessageBox.Show("Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    deleteFailed = true;
                }
            }
            else
            {
                deleteFailed = true;
            }

            if (deleteFailed)
            {
                MessageBox.Show("Delete Failed.");
            }

            _RefreshApplicationsList();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to cancel this application?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            LocalDrivingLicenseApplication localDrivingLicenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value);
            DVLDBusinessLayer.Application app = DVLDBusinessLayer.Application.FindApplication(localDrivingLicenseApplication.ApplicationID);
            app.ApplicationStatus = 2;

            if(app.Save())
            {
                MessageBox.Show("Cancelled Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Cancel Failed.");
            }    

            _RefreshApplicationsList();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            NewLocalDrivingLicenseApplication application = new NewLocalDrivingLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value,
                (string)gridApplications.CurrentRow.Cells[2].Value);
            application.ShowDialog();
            _RefreshApplicationsList();
        }

        private void contextMenuStrip1_Opened(object sender, System.EventArgs e)
        {
            int passedTestCount = (int)gridApplications.CurrentRow.Cells[5].Value;
            string status = (string)gridApplications.CurrentRow.Cells[6].Value;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            scheduleTestsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = true;
            deleteApplicationToolStripMenuItem.Enabled = true;
            cancelApplicationToolStripMenuItem.Enabled = true;

            switch (passedTestCount)
            {
                case 0:
                    scheduleVisionTestToolStripMenuItem.Enabled = true;
                    scheduleWritingTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;
                    break;

                case 1:
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWritingTestToolStripMenuItem.Enabled = true;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;
                    break;

                case 2:
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWritingTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = true;
                    break;

                case 3:
                    scheduleTestsToolStripMenuItem.Enabled = false;
                    showLicenseToolStripMenuItem.Enabled = true;
                    editApplicationToolStripMenuItem.Enabled = false;
                    deleteApplicationToolStripMenuItem.Enabled = false;
                    cancelApplicationToolStripMenuItem.Enabled = false;

                    if(status == "New")
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                        showLicenseToolStripMenuItem.Enabled = false;
                    }
                    else if(status == "Completed")
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                        showLicenseToolStripMenuItem.Enabled = true;
                    }
                    break;
            }
        }

        private void _ScheduleTest(int type)
        {
            LocalDrivingLicenseApplication licenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value);
            TestAppointments appointment = new TestAppointments(licenseApplication.ApplicationID, licenseApplication.LocalDrivingLicenseApplicationID,
                (string)gridApplications.CurrentRow.Cells[1].Value, (int)gridApplications.CurrentRow.Cells[5].Value,
                (string)gridApplications.CurrentRow.Cells[3].Value, type);
            appointment.ShowDialog();
            _RefreshApplicationsList();
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ScheduleTest(1);
        }

        private void scheduleWritingTestToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ScheduleTest(2);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            _ScheduleTest(3);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LocalDrivingLicenseApplication licenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value);

            IssueDrivingLicenseForTheFirstTime licenseForm = new IssueDrivingLicenseForTheFirstTime(licenseApplication.ApplicationID,
                licenseApplication.LocalDrivingLicenseApplicationID, (string)gridApplications.CurrentRow.Cells[1].Value,
                (int)gridApplications.CurrentRow.Cells[5].Value);

            licenseForm.ShowDialog();
            _RefreshApplicationsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LocalDrivingLicenseApplication licenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value);

            LicenseInfo infoForm = new LicenseInfo(licenseApplication.ApplicationID);
            infoForm.ShowDialog();
            _RefreshApplicationsList(); 
        }

        private void showPersonLicenseHistorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LocalDrivingLicenseApplication licenseApplication = LocalDrivingLicenseApplication.FindLicenseApplication((int)gridApplications.CurrentRow.Cells[0].Value);

            LicenseHistory historyForm = new LicenseHistory(licenseApplication.ApplicationID);
            historyForm.ShowDialog();
            _RefreshApplicationsList();

        }
    }
}
