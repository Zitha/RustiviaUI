namespace TussoTechWebsite.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MySql.Data.Entity;
    internal sealed class Configuration : DbMigrationsConfiguration<TussoTechWebsite.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            this.SetSqlGenerator("MySql.Data.MySqlClient",
                             new MySqlMigrationSqlGenerator());
            //SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema)); //here s the thing.
        }

        protected override void Seed(TussoTechWebsite.Data.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
