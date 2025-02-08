using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public static class TestAppointmentsDataAccess
    {
        public static DataTable ListAppointments(int testTypeID, int licenseAppID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT TestAppointmentID AS AppointmentID, AppointmentDate, PaidFees, IsLocked
                             FROM TestAppointments WHERE TestTypeID = @TestTypeID
                             AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", licenseAppID);

            DataTable people = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                people.Load(reader);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return people;
        }

        public static bool AddAppointmentValidation(int testTypeID, int licenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT Found=1 FROM TestAppointments WHERE TestTypeID = @TestTypeID
                             AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND IsLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", licenseID);

            bool validation = true;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    validation = false;
                }
            }
            catch (Exception ex)
            {
                validation = false;
            }
            finally
            {
                connection.Close();
            }

            return validation;
        }

        public static int GetTrails(int testTypeID, int licenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT COUNT(TestAppointmentID) FROM TestAppointments WHERE TestTypeID = @TestTypeID
                             AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", licenseID);

            int trails = 0;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int trailsNum))
                {
                    trails = trailsNum;
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }

            return trails;
        }

        public static int AddNewAppointment(int testTypeID, int localDrivingLicenseApplicationID, DateTime appointmentDate, decimal paidFees,
            int createdByUserID, bool isLocked, int? retakeTestID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID,
                             IsLocked, RetakeTestApplicationID)

                             VALUES (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID,
                             @IsLocked, @RetakeTestApplicationID);

                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsLocked", isLocked);

            if(retakeTestID.HasValue) command.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestID);
            else command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

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

        public static bool EditAppointment(int appointmentID, DateTime appointmentDate, bool isLocked)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"UPDATE TestAppointments
                             SET AppointmentDate = @AppointmentDate,
                             IsLocked = @IsLocked
                             WHERE TestAppointmentID = @TestAppointmentID
                             ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", appointmentID);
            command.Parameters.AddWithValue("@IsLocked", isLocked);
            command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }

        public static void FindAppointment(int appointmentID, ref int typeID, ref int licenseAppID, ref DateTime appointmentDate, ref decimal paidFees,
                ref int userID, ref bool isLocked, ref int? retakeTestID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", appointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    typeID = (int)reader["TestTypeID"];
                    licenseAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    appointmentDate = (DateTime)reader["AppointmentDate"];
                    paidFees = (decimal)reader["PaidFees"];
                    userID = (int)reader["CreatedByUserID"];
                    isLocked = (bool)reader["IsLocked"];
                    if (reader["RetakeTestApplicationID"] == DBNull.Value) retakeTestID = null;
                    else retakeTestID = (int)reader["RetakeTestApplicationID"];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
