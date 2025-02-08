using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class Country
    {
        public int ID { get; set; }
        public string CountryName { get; set; }

        private Country(int id, string countryName)
        {
            this.ID = id;
            this.CountryName = countryName;
        }

        public static DataTable ListCountries()
        {
            return CountryDataAccess.ListCountries();
        }

        public static Country FindCountry(int id)
        {
            string countryName = "";

            CountryDataAccess.FindCountry(id, ref countryName);

            if (!string.IsNullOrEmpty(countryName)) return new Country(id, countryName);
            else return null;

        }

        public static Country FindCountry(string name)
        {
            int countryID = -1;

            CountryDataAccess.FindCountry(name, ref countryID);

            return new Country(countryID, name);

        }
    }
}
