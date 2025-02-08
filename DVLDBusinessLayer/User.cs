using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class User
    {
        private enum Mode { Add_User, Update}

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        private Mode _mode;

        public User()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
            _mode = Mode.Add_User;
        }

        private User(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Password = password;
            IsActive = isActive;
            _mode = Mode.Update;
        }

        private bool _AddNewUser()
        {
            this.UserID =  UserDataAccess.AddUser(PersonID, UserName, Password, IsActive);
            return UserID != -1;
        }

        private bool _UpdateUser()
        {
            return UserDataAccess.UpdateUser(UserID, PersonID, UserName, Password, IsActive);
        }

        public static User FindUser(string userName)
        {
            int personID = -1, userID = -1;
            string password = "";
            bool isActive = false;

            UserDataAccess.FindUser(userName, ref userID, ref personID, ref password, ref isActive);

            if (userID != -1) return new User(userID, personID, userName, password, isActive);
            else return null;
        }

        public static User FindUser(int userID)
        {
            int personID = -1;
            string password = "", userName = "";
            bool isActive = false;

            UserDataAccess.FindUser(userID, ref userName, ref personID, ref password, ref isActive);

            if (userID != -1) return new User(userID, personID, userName, password, isActive);
            else return null;
        }

        public static DataTable ListUsers()
        {
            return UserDataAccess.ListUsers();
        }

        public static bool IsPersonConnected(int personID)
        {
            return UserDataAccess.IsPersonConnected(personID);
        }

        public bool Save()
        {
            if(_mode == Mode.Add_User)
            {
                if(_AddNewUser())
                {
                    _mode = Mode.Update;
                    return true;
                }
            }
            else if(_mode == Mode.Update)
            {
                return _UpdateUser();
            }

            return false;
        }

        public static bool Delete(int userID)
        {
            return UserDataAccess.Delete(userID);
        }
    }
}
