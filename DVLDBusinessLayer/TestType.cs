using DVLDDataAccessLayer;
using System.Data;

namespace DVLDBusinessLayer
{
    public class TestType
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }

        private TestType(int id, string title, string description, float fees)
        {
            this.ID = id;
            this.Title = title;
            this.Description = description;
            this.Fees = fees;
        }

        private bool _EditTestType()
        {
            return TestTypeDataAccess.EditTestType(ID, Title, Description, Fees);
        }

        public static DataTable ListTypes()
        {
            return TestTypeDataAccess.ListTypes();
        }

        public static TestType FindType(int id)
        {
            string title = "", description = "";
            float fees = 0.0f;

            TestTypeDataAccess.FindType(id, ref title, ref description, ref fees);

            return new TestType(id, title, description, fees);
        }

        public bool Save()
        {
            return _EditTestType();
        }
    }
}
