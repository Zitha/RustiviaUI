using System.Data.Entity;
using MySql.Data.Entity;
using TussoTechWebsite.Model;

namespace TussoTechWebsite.Data
{
    public class CustomDatabaseInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        public CustomDatabaseInitializer()
        {
        }

        protected override void Seed(DataContext context)
        {
            Company company = new Company
            {
                Name = "Tusso Technologies",
                Contacts = "072 631 5461/ 083 473 1660"
            };

            context.Companies.Add(company);
            base.Seed(context);
        }
    }
}