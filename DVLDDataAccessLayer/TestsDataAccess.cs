using System.Data.SqlClient;
using System;

namespace DVLDDataAccessLayer
{
    public static class TestsDataAccess
    {
        public static int AddTest(int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)

                             VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@TestResult", testResult);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            if (!string.IsNullOrEmpty(notes)) command.Parameters.AddWithValue("@Notes", notes);
            else command.Parameters.AddWithValue("@Notes", DBNull.Value);

            int id = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    id = insertedID;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return id;
        }

        public static bool CheckTestPassed(int type, int licenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT Found=1 FROM Tests INNER JOIN TestAppointments 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             WHERE TestResult = 1
                             AND TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             AND TestAppointments.TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", type);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", licenseID);

            bool result = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public static bool CheckedTestFailed(int type, int licenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT Found=1 FROM Tests INNER JOIN TestAppointments 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             WHERE TestResult = 0
                             AND TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             AND TestAppointments.TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", type);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", licenseID);

            bool result = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }
    }
}
