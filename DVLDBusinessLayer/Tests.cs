using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class Tests
    {
        private enum Mode { Add_New, Edit };
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        private Mode _mode = Mode.Add_New;

        public Tests()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = string.Empty;
            CreatedByUserID = -1;
            _mode = Mode.Add_New;
        }

        private Tests(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            TestID = testID;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdByUserID;
            _mode = Mode.Edit;
        }

        private bool _AddTest()
        {
            TestID = TestsDataAccess.AddTest(TestAppointmentID, TestResult, Notes, CreatedByUserID);
            return TestAppointmentID != -1;
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddTest())
                {
                    _mode = Mode.Edit;
                    return true;
                }
            }

            return false;
        }

        public static bool CheckTestPassed(int type, int licenseID)
        {
            return TestsDataAccess.CheckTestPassed(type, licenseID);
        }

        public static bool CheckTestFailed(int type, int licenseID)
        {
            return TestsDataAccess.CheckedTestFailed(type, licenseID);
        }
    }
}
