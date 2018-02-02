using System;
using System.Data.Entity;
using System.Text;
using IntroductionMVC5.Models.Integrator;

namespace IntroductionMVC5.Data.Configuration
{
    public class CustomDatabaseInitializer :
        //    DropCreateDatabaseIfModelChanges<DataContext>
        CreateDatabaseIfNotExists<DataContext>
    {
        private readonly Random _random = new Random();

        protected override void Seed(DataContext context)
        {
            //            AddStuff(context);
            //            var user = new User
            //            {
            //                Password = "Ndavhe",
            //                UserName = "Ndavhe",
            //                Roles = new List<Role>()
            //                {
            //                    new Role(){RoleName = "Admin"}
            //                }
            //            };

            var admin = new Role
            {
                RoleName = "Admin"
            };
            var weighbridgeRole = new Role
            {
                RoleName = "WeighBridge"
            };
            var web = new Role
            {
                RoleName = "Web"
            };
            //            context.Users.Add(user);
            context.Roles.Add(admin);
            context.Roles.Add(weighbridgeRole);
            context.Roles.Add(web);
            base.Seed(context);
        }

        private void AddStuff(DataContext context)
        {
            int count = 30;
            while ((count--) != 0)
            {
                var driver = new Driver
                {
                    Firstname = string.Format("Driver{0}", count),
                    Surname = string.Format("Surname{0}", count),
                    Gender = "Male",
                    IdNumber = GetIdNumber(),
                    IdLocation = "IDLocation",
                    ImageName = string.Format("Men.png")
                };

                context.Drivers.Add(driver);
            }
            var user = new User
            {
                Password = "Ndavhe",
                UserName = "Ndavhe"
            };
            context.Users.Add(user);
            base.Seed(context);
        }

        private string GetIdNumber()
        {
            var builder = new StringBuilder();
            while (builder.Length < 14)
            {
                builder.Append(_random.Next(10).ToString());
            }
            return builder.ToString();
        }
    }
}