using System.Data;
using System.Runtime.InteropServices;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class LocalDrivingLicenseApplication
    {
        private enum Mode { Add_New, Edit }

        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        private Mode _mode = Mode.Add_New;

        public LocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;
            _mode = Mode.Add_New;
        }

        private LocalDrivingLicenseApplication(int licenseApplicationID, int applicationID, int licenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = licenseApplicationID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassID;
            _mode = Mode.Edit;
        }

        private bool _AddNewLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationDataAccess.AddNewLicenseApplication(ApplicationID, LicenseClassID);
            return LocalDrivingLicenseApplicationID != -1;
        }

        private bool _EditLicenseApplication()
        {
            return LocalDrivingLicenseApplicationDataAccess.EditLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);
        }

        public static DataTable ListApplications()
        {
            return LocalDrivingLicenseApplicationDataAccess.ListApplications();
        }

        public static LocalDrivingLicenseApplication FindLicenseApplication(int licenseApplicationID)
        {
            int applicationID = -1, licenseClassID = -1;

            LocalDrivingLicenseApplicationDataAccess.FindLicenseApplication(licenseApplicationID, ref applicationID, ref licenseClassID);

            return new LocalDrivingLicenseApplication(licenseApplicationID, applicationID, licenseClassID);
        }

        public static int CheckLicenseValidation(int licenseClassID, int personID)
        {
            return LocalDrivingLicenseApplicationDataAccess.CheckLicenseValidation(licenseClassID, personID);
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddNewLicenseApplication())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }
            else if(_mode == Mode.Edit)
            {
                return _EditLicenseApplication();
            }

            return false;
        }

        public static bool DeleteApplication(int appID)
        {
            return LocalDrivingLicenseApplicationDataAccess.DeleteApplication(appID);
        }
    }
}
