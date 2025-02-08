using System;
using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class Drivers
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public Drivers()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
        }

        private bool _AddNewDriver()
        {
            DriverID = DriversDataAccess.AddNewDriver(PersonID, CreatedByUserID, CreatedDate);
            return DriverID != -1;
        }

        public static int GetDriverIDForPerson(int personID)
        {
            return DriversDataAccess.GetDriverIDForPerson(personID);
        }

        public static DataTable ListDrivers()
        {
            return DriversDataAccess.ListDrivers();
        }

        public bool Save()
        {
            return _AddNewDriver();
        }
    }
}
