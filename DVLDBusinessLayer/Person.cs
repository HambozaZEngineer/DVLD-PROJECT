using System;
using System.Data;
using System.Timers;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class Person
    {
        private enum Mode { Add_New, Update };
        public int ID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }
        private Mode _mode = Mode.Update;

        public Person()
        {
            ID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gender = 0;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";
            _mode = Mode.Add_New;
        }

        private Person(int id, string nationalNo, string firstName, string secondName, string thirdName, 
            string lastName, DateTime dateOfBirth, byte gender, string address, string phone, string email,
            int nationalityCountryID, string imagePath)
        {
            this.ID = id;
            this.NationalNo = nationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.NationalityCountryID = nationalityCountryID;
            this.ImagePath = imagePath;
            _mode = Mode.Update;
        }

        private bool _AddNewPerson()
        {
            this.ID = PersonDataAccess.AddNewPerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName, 
                DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            return ID != -1;
        }

        private bool _UpdatePerson()
        {
            return PersonDataAccess.UpdatePerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        public static Person FindPersonWithID(int personID)
        {
            string nationalNo = "", firstName = "", secondName = "", thirdName = "", lastName = "", address = "",
                phone = "", email = "", imagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            byte gender = 0;
            int nationalityCountryID = -1;

            PersonDataAccess.FindPersonWithID(personID, ref nationalNo, ref firstName, ref secondName, ref thirdName,
                ref lastName, ref dateOfBirth, ref gender, ref address, ref phone, ref email, ref nationalityCountryID,
                ref imagePath);

            return new Person(personID, nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gender,
                address, phone, email, nationalityCountryID, imagePath);
        }

        public static Person FindPersonWithNationalNo(string nationalNo)
        {
           
            string firstName = "", secondName = "", thirdName = "", lastName = "", address = "",
                phone = "", email = "", imagePath = "";
            DateTime dateOfBirth = DateTime.Now;
            byte gender = 0;
            int nationalityCountryID = -1, personID = -1;

            PersonDataAccess.FindPersonWithNationalNo(nationalNo, ref personID, ref firstName, ref secondName, ref thirdName,
                ref lastName, ref dateOfBirth, ref gender, ref address, ref phone, ref email, ref nationalityCountryID,
                ref imagePath);

            return new Person(personID, nationalNo, firstName, secondName, thirdName, lastName, dateOfBirth, gender,
                address, phone, email, nationalityCountryID, imagePath);
        }

        public static bool DoesPersonExists(string nationalNo)
        {
            return PersonDataAccess.DoesPersonExists(nationalNo);
        }

        public static DataTable ListPeople()
        {
            return PersonDataAccess.ListPeople();
        }

        public bool Save()
        {
            if(_mode == Mode.Add_New)
            {
                if(_AddNewPerson())
                {
                    _mode = Mode.Update;
                    return true;
                }
            }
            else if(_mode == Mode.Update)
            {
                return _UpdatePerson();
            }

            return false;
        }

        public static bool Delete(int personID)
        {
            return PersonDataAccess.Delete(personID);
        }
    }
}