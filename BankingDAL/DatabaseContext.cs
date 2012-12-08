using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Money> Moneys { get; set; }
        public Bank Bank { get { return Banks.First(); } }
        private DbSet<Bank> Banks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public static void Seed(DatabaseContext context)
        {
            var adminRole = context.Roles.Add(new Role { Name = "Administrator" });
            
            context.Roles.Add(new Role { Name = "Client" });
            context.Roles.Add(new Role { Name = "Employee" });
            context.Currencies.Add(new Currency { Name = "USA Dollar", ShortName = "USD", Symbol = "$" });
            context.Users.Add(new User { Username = "admin", Password = "admin", Role = adminRole, Region = context.Regions.Add(new Region { Name = "All" }), Birthday = DateTime.Now});

            context.SaveChanges();
        }

        public class DropCreateAlways : DropCreateDatabaseAlways<DatabaseContext>
        {
            protected override void Seed(DatabaseContext context)
            {
                DatabaseContext.Seed(context);

                base.Seed(context);
            }
        }

        public class CreateInitializer : CreateDatabaseIfNotExists<DatabaseContext>
        {
            protected override void Seed(DatabaseContext context)
            {
                DatabaseContext.Seed(context);

                base.Seed(context);
            }
        }

        static DatabaseContext()
        {
#if DEBUG
            Database.SetInitializer(new DropCreateAlways());
#else
            Database.SetInitializer(new CreateInitializer ());
#endif
        }
    }
}
