using System;
using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class InternationalLicense
    {
        private enum Mode { Add_New, Edit }
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }
        private Mode _mode = Mode.Add_New;

        public InternationalLicense()
        {
            InternationalLicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = false;
            CreatedByUserID = -1;
            _mode = Mode.Add_New;
        }

        private InternationalLicense(int internationalLicenseID, int appID, int driverID, int licenseID, DateTime issueDate,
             DateTime expirationDate, bool isActive, int userID)
        {
            InternationalLicenseID = internationalLicenseID;
            ApplicationID = appID;
            DriverID = driverID;
            IssuedUsingLocalLicenseID = licenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            CreatedByUserID = userID;
            _mode = Mode.Edit;
        }

        private bool _AddNewInternationalLicense()
        {
            InternationalLicenseID = InternationalLicenseDataAccess.AddNewInternationalLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID, 
                IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            return InternationalLicenseID != -1;
        }

        public static InternationalLicense FindInternationalLicense(int internationalLicenseID)
        {
            int appID = -1, driverID = -1, userID = -1, licenseID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;

            InternationalLicenseDataAccess.FindInternationalLicense(internationalLicenseID, ref appID, ref driverID, ref licenseID, 
                ref issueDate, ref expirationDate, ref isActive, ref userID);

            return new InternationalLicense(internationalLicenseID, appID, driverID, licenseID, issueDate, expirationDate, isActive, userID);
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            { 
                if(_AddNewInternationalLicense())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }

            return false;
        }

        public static DataTable ListInternationalLicenses(int driverID)
        {
            return InternationalLicenseDataAccess.ListInternationalLicenses(driverID);
        }

        public static DataTable ListApplications()
        {
            return InternationalLicenseDataAccess.ListApplications();
        }

        public static bool DoesLicenseExist(int licenseID)
        {
            return InternationalLicenseDataAccess.DoesLicenseExist(licenseID);
        }
    }
}
