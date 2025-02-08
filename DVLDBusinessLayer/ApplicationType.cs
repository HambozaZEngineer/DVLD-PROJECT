using System.Data;
using System.Windows.Forms;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class ApplicationType
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees { get; set; }

        private bool _EditApplicationType()
        {
            return ApplicationTypeDataAccess.EditApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
        }

        private ApplicationType(int id, string title, float fees)
        {
            this.ApplicationTypeID = id;
            this.ApplicationTypeTitle = title;
            this.ApplicationFees = fees;
        }

        public static DataTable ListTypes()
        {
            return ApplicationTypeDataAccess.ListTypes();
        }

        public static ApplicationType FindType(int id)
        {
            string title = "";
            float fees = 0.0f;

            ApplicationTypeDataAccess.FindType(id, ref title, ref fees);

            return new ApplicationType(id, title, fees);
        }

        public bool Save()
        {
            return _EditApplicationType();
        }
    }
}
