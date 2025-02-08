using System;
using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class DetainedLicense
    {
        private enum Mode { Add_New, Edit }
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? ReleasedByUserID { get; set; }
        public int? ReleaseApplicationID { get; set; }
        private Mode _mode = Mode.Add_New;

        public DetainedLicense()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            FineFees = 0.0m;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = null;
            ReleasedByUserID = null;
            ReleaseApplicationID = null;
            _mode = Mode.Add_New;
        }

        private DetainedLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID, bool isReleased, 
            DateTime? releaseDate, int? releasedByUserID, int? releaseAppID)
        {
            DetainID = detainID;
            LicenseID = licenseID;
            DetainDate = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseAppID;
            _mode = Mode.Edit;
        }

        private bool _AddNewDetainedLicense()
        {
            DetainID = DetainedLicenseDataAccess.AddNewDetainedLicense(LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, 
                ReleasedByUserID, ReleaseApplicationID);
            return DetainID != -1;
        }

        private bool _EditDetainedLicense()
        {
            return DetainedLicenseDataAccess.ReleaseLicense(DetainID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddNewDetainedLicense())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }
            else
            {
                return _EditDetainedLicense();
            }

            return false;
        }

        public static int GetLicenseDetainID(int licenseID)
        {
            return DetainedLicenseDataAccess.GetLicenseDetainID(licenseID);
        }

        public static DetainedLicense FindDetainedLicense(int detainID)
        {
            int licenseID = -1, cUserID = -1;
            int? rUserID = -1, rAppID = -1;
            DateTime detainDate = DateTime.Now;
            DateTime? releaseDate = DateTime.Now;
            bool isReleased = false;
            decimal fineFees = 0.0m;

            DetainedLicenseDataAccess.FindDetainedLicense(detainID, ref licenseID, ref detainDate, ref fineFees, ref cUserID, ref isReleased, ref releaseDate,
                ref rUserID, ref rAppID);

            return new DetainedLicense(detainID, licenseID, detainDate, fineFees, cUserID, isReleased, releaseDate, rUserID, rAppID);
        }

        public static DataTable ListDetainedLicenses()
        {
            return DetainedLicenseDataAccess.ListDetainedLicenses();
        }
    }
}
