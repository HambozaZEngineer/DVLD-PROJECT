using DVLDDataAccessLayer;
using System;
using System.Data;

namespace DVLDBusinessLayer
{
    public class Licenses
    {
        private enum Mode { Add_New, Edit }
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public byte IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        private Mode _mode = Mode.Add_New;

        public Licenses()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClass = 0;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = string.Empty;
            PaidFees = 0.0m;
            IsActive = false;
            IssueReason = 0;
            CreatedByUserID = -1;
            _mode = Mode.Add_New;
        }

        private Licenses(int licenseID, int appID, int driverID, int licenseClass, DateTime issueDate, DateTime expirationDate, string notes, 
            decimal paidFees, bool isActive, byte issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            ApplicationID = appID;
            DriverID = driverID;
            LicenseClass = licenseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            PaidFees = paidFees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;
            _mode = Mode.Edit;
        }

        private bool _AddNewLicense()
        {
            LicenseID = LicensesDataAccess.AddNewLicense(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, 
                PaidFees, IsActive, IssueReason, CreatedByUserID);
            return LicenseID != -1;
        }

        private bool _EditLicense()
        {
            return LicensesDataAccess.EditLicense(LicenseID, IsActive);
        }

        public static Licenses FindLicense(int licenseID)
        {
            int appID = -1, driverID = -1, licenseClass = 0, userID = -1;
            byte issueReason = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = string.Empty;
            bool isActive = false;
            decimal paidFees = 0.0m;

            LicensesDataAccess.FindLicense(licenseID, ref appID, ref driverID, ref licenseClass, ref issueDate, ref expirationDate,
                ref notes, ref paidFees, ref isActive, ref issueReason, ref userID);

            return new Licenses(licenseID, appID, driverID, licenseClass, issueDate, expirationDate, notes, paidFees, isActive,
                issueReason, userID);
        }

        public static Licenses FindLicenseIDUsingAppID(int appID)
        {
            int licenseID = -1, driverID = -1, licenseClass = 0, userID = -1;
            byte issueReason = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = string.Empty;
            bool isActive = false;
            decimal paidFees = 0.0m;

            LicensesDataAccess.FindLicenseWithAppID(appID, ref licenseID, ref driverID, ref licenseClass, ref issueDate, ref expirationDate,
                ref notes, ref paidFees, ref isActive, ref issueReason, ref userID);

            return new Licenses(licenseID, appID, driverID, licenseClass, issueDate, expirationDate, notes, paidFees, isActive,
                issueReason, userID);
        }

        public static bool DoesLicenseExist(int licenseID)
        {
            return LicensesDataAccess.DoesLicenseExist(licenseID);
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            return LicensesDataAccess.IsLicenseDetained(licenseID);
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddNewLicense())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }
            else
            {
                return _EditLicense();
            }

            return false;
        }

        public static DataTable ListLicenses(int driverID)
        {
            return LicensesDataAccess.ListLicenses(driverID);
        }
    }
}
