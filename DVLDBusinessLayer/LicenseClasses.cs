using System;
using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class LicenseClasses
    {
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        private LicenseClasses(int licenseClassID, string className, string classDescription, byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = className;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;
        }

        public static LicenseClasses FindLicenseClass(int licenseClassID)
        {
            string className = "", classDescription = "";
            byte minAllowedAge = 0;
            byte defaultValidityLength = 0;
            decimal classFees = 0.0m;

            LicenseClassesDataAccess.FindLicenseClass(licenseClassID, ref className, ref classDescription, ref minAllowedAge, ref defaultValidityLength,
                ref classFees);

            return new LicenseClasses(licenseClassID, className, classDescription, minAllowedAge, defaultValidityLength, classFees);
        }
    }
}
