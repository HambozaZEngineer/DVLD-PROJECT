using System; 
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public static class PersonDataAccess
    {
        public static DataTable ListPeople()
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, (
                             CASE
	                             WHEN Gender = 0 THEN 'Male'
	                            ELSE 'Female'
                            END
                            ) as Gender, DateOfBirth, Countries.CountryName as Nationality, Phone, Email FROM People INNER JOIN Countries
                            ON People.NationalityCountryID = Countries.CountryID";

            SqlCommand command = new SqlCommand(query, connection);

            DataTable people = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                people.Load(reader);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return people;
        }

        public static bool DoesPersonExists(string nationalNo)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", nationalNo);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) isFound = true;
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewPerson(int ID, string NationalNo, string FirstName, string SecondName, string ThirdName,
                string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone, 
                string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, 
                             Address, Phone, Email, Gender, NationalityCountryID, ImagePath)

                             VALUES (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, 
                             @Address, @Phone, @Email, @Gender, @NationalityCountryID, @ImagePath);

                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            
            if(ImagePath != "") command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

            int id = -1;

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null && int.TryParse(result.ToString(), out int insertedID))
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

        public static bool UpdatePerson(int ID, string NationalNo, string FirstName, string SecondName, string ThirdName,
        string LastName, DateTime DateOfBirth, byte Gender, string Address, string Phone,
        string Email, int NationalityCountryID, string ImagePath)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"UPDATE People
                             SET NationalNo = @NationalNo,
                             FirstName = @FirstName,
                             SecondName = @SecondName,
                             ThirdName = @ThirdName,
                             LastName = @LastName,
                             DateOfBirth = @DateOfBirth,
                             Gender = @Gender,
                             Address = @Address,
                             Phone = @Phone,
                             Email = @Email,
                             NationalityCountryID = @NationalityCountryID,
                             ImagePath = @ImagePath
                             WHERE PersonID = @PersonID
                             ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", ID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);


            if (ImagePath != "") command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else command.Parameters.AddWithValue("@ImagePath", DBNull.Value);

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

        public static void FindPersonWithID(int personID, ref string nationalNo, ref string firstName, ref string secondName, 
                ref string thirdName, ref string lastName, ref DateTime dateOfBirth, ref byte gender, 
                ref string address, ref string phone, ref string email, ref int nationalityCountryID, ref string imagePath)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    nationalNo = (string)reader["NationalNo"];
                    firstName = (string)reader["FirstName"];
                    secondName = (string)reader["SecondName"];
                    thirdName = (string)reader["ThirdName"];
                    lastName = (string)reader["LastName"];
                    dateOfBirth = (DateTime)reader["DateOfBirth"];
                    gender = (byte)reader["Gender"];
                    address = (string)reader["Address"];
                    phone = (string)reader["Phone"];
                    email = (string)reader["Email"];
                    nationalityCountryID = (int)reader["NationalityCountryID"];
                    if (reader["ImagePath"] == DBNull.Value) imagePath = "";
                    else imagePath = (string)reader["ImagePath"];
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

        public static void FindPersonWithNationalNo(string nationalNo, ref int personID, ref string firstName, ref string secondName,
        ref string thirdName, ref string lastName, ref DateTime dateOfBirth, ref byte gender,
        ref string address, ref string phone, ref string email, ref int nationalityCountryID, ref string imagePath)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", nationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    personID = (int)reader["PersonID"];
                    firstName = (string)reader["FirstName"];
                    secondName = (string)reader["SecondName"];
                    thirdName = (string)reader["ThirdName"];
                    lastName = (string)reader["LastName"];
                    dateOfBirth = (DateTime)reader["DateOfBirth"];
                    gender = (byte)reader["Gender"];
                    address = (string)reader["Address"];
                    phone = (string)reader["Phone"];
                    email = (string)reader["Email"];
                    nationalityCountryID = (int)reader["NationalityCountryID"];
                    if (reader["ImagePath"] == DBNull.Value) imagePath = "";
                    else imagePath = (string)reader["ImagePath"];
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

        public static bool Delete(int personID)
        {
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);
            string query = @"DELETE FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", personID);

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
    }
}