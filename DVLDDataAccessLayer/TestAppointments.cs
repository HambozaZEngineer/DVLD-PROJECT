using System;
using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class TestAppointments
    {
        private enum Mode { Add_New, Edit };
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }
        private Mode _mode = Mode.Add_New;

        public TestAppointments()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0.0m;
            CreatedByUserID = -1;
            IsLocked = false;
            RetakeTestApplicationID = null;
            _mode = Mode.Add_New;
        }

        private TestAppointments(int testAppointmentID, int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, decimal paidFees,
            int createdByUserID, bool isLocked, int? retakeTestID)
        {
            TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestID;
            _mode = Mode.Edit;
        }

        public static TestAppointments FindAppointment(int appointmentID)
        {
            int typeID = -1, licenseAppID = -1, userID = -1;
            DateTime appointmentDate = DateTime.Now;
            decimal paidFees = 0.0m;
            bool isLocked = false;
            int? retakeTestID = null;

            TestAppointmentsDataAccess.FindAppointment(appointmentID, ref typeID, ref licenseAppID, ref appointmentDate, ref paidFees,
                ref userID, ref isLocked, ref retakeTestID);

            return new TestAppointments(appointmentID, typeID, licenseAppID, appointmentDate, paidFees, userID, isLocked, retakeTestID);
        }

        private bool _AddNewAppointment()
        {
            TestAppointmentID = TestAppointmentsDataAccess.AddNewAppointment(TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
                PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            return TestAppointmentID != -1;
        }

        private bool _EditAppointment()
        {
            return TestAppointmentsDataAccess.EditAppointment(TestAppointmentID, AppointmentDate, IsLocked);
        }

        public static DataTable ListAppointments(int testTypeID, int licenseAppID)
        {
            return TestAppointmentsDataAccess.ListAppointments(testTypeID, licenseAppID);
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddNewAppointment())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }
            else
            {
                return _EditAppointment();
            }

            return false;
        }

        public static bool AddAppointmentValidation()
        {
            return true;
            //return TestAppointmentsDataAccess.AddAppointmentValidation();
        }
    }
}
