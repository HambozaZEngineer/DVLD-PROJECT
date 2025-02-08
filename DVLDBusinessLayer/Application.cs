using System;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class Application
    {
        private enum Mode { Add_New, Edit }

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public short ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreateByUserID { get; set; }
        private Mode _mode = Mode.Add_New;

        public Application()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = 0;
            ApplicationStatus = 0;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreateByUserID = -1;
            _mode = Mode.Add_New;
        }

        private Application(int appID, int personID, DateTime appDate, int typeID, short status, DateTime lastStatusDate, decimal paidFees,
            int userID)
        {
            this.ApplicationID = appID;
            this.ApplicantPersonID = personID;
            this.ApplicationDate = appDate;
            this.ApplicationTypeID = typeID;
            this.ApplicationStatus = status;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreateByUserID = userID;
            _mode = Mode.Edit;
        }

        private bool _AddNewApplication()
        {
            ApplicationID = ApplicationDataAccess.AddNewApplication(ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                ApplicationStatus, LastStatusDate, PaidFees, CreateByUserID);

            return ApplicationID != -1;
        }

        private bool _EditApplication()
        {
            return ApplicationDataAccess.EditApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                ApplicationStatus, LastStatusDate, PaidFees, CreateByUserID);
        }

        public static Application FindApplication(int appID)
        {
            int personID = -1, typeID = -1, userID = -1;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            byte status = 0;
            decimal paidFees = 0.0m;

            ApplicationDataAccess.FindApplication(appID, ref personID, ref applicationDate, ref typeID, ref status, ref lastStatusDate,
                ref paidFees, ref userID);

            if (userID != -1) return new Application(appID, personID, applicationDate, typeID, status, lastStatusDate, paidFees, userID);
            else return null;
        }


        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddNewApplication())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }
            else if(_mode == Mode.Edit)
            {
                return _EditApplication();
            }

            return false;
        }

        public static bool DeleteApplication(int appID)
        {
            return ApplicationDataAccess.DeleteApplication(appID);
        }
    }
}
