using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public static class DetainedLicenseDataAccess
    {
        public static int AddNewDetainedLicense(int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID, bool isReleased,
            DateTime? releaseDate, int? releasedByUserID, int? releaseAppID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased, ReleaseDate, 
                             ReleasedByUserID, ReleaseApplicationID)

                             VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased, @ReleaseDate, 
                             @ReleasedByUserID, @ReleaseApplicationID);

                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@DetainDate", detainDate);
            command.Parameters.AddWithValue("@FineFees", fineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsReleased", isReleased);

            if (releaseDate != null) command.Parameters.AddWithValue("@ReleaseDate", releaseDate);
            else command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);

            if (releasedByUserID != null) command.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserID);
            else command.Parameters.AddWithValue("@ReleasedByUserID", DBNull.Value);

            if (releaseAppID != null) command.Parameters.AddWithValue("@ReleaseApplicationID", releaseAppID);
            else command.Parameters.AddWithValue("@ReleaseApplicationID", DBNull.Value);

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

        public static DataTable ListDetainedLicenses()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM DetainedLicenses_View";

            SqlCommand command = new SqlCommand(query, connection);

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

        public static bool ReleaseLicense(int detainID, bool isReleased, DateTime? releaseDate, int? userID, int? appID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"UPDATE DetainedLicenses
                             SET IsReleased = @IsReleased,
                             ReleaseDate=  @ReleaseDate,
                             ReleasedByUserID = @ReleasedByUserID,
                             ReleaseApplicationID = @ReleaseApplicationID
                             WHERE DetainID = @DetainID
                             ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", detainID);
            command.Parameters.AddWithValue("@IsReleased", isReleased);
            command.Parameters.AddWithValue("@ReleaseDate", releaseDate);
            command.Parameters.AddWithValue("@ReleasedByUserID", userID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", appID);
                                         
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

        public static void FindDetainedLicense(int detainID, ref int licenseID, ref DateTime detainDate, ref decimal fineFees, ref int createdByUserID, 
            ref bool isReleased, ref DateTime? releaseDate, ref int? releasedByUserID, ref int? releaseAppID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM DetainedLicenses WHERE DetainID= @DetainID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DetainID", detainID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    licenseID = (int)reader["LicenseID"];
                    detainDate = (DateTime)reader["DetainDate"];
                    fineFees = (decimal)reader["FineFees"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isReleased = (bool)reader["IsReleased"];

                    if (reader["ReleaseDate"] == DBNull.Value) releaseDate = null;
                    else releaseDate = (DateTime)reader["ReleaseDate"];

                    if (reader["ReleasedByUserID"] == DBNull.Value) releasedByUserID = null;
                    else releasedByUserID = (int)reader["ReleasedByUserID"];

                    if (reader["ReleaseApplicationID"] == DBNull.Value) releaseAppID = null;
                    else releaseAppID = (int)reader["ReleaseApplicationID"];

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

        public static int GetLicenseDetainID(int licenseID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT DetainID FROM DetainedLicenses WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", licenseID);

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
    }
}
