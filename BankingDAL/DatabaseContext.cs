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
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CurrencyRate> Rates { get; set; }

        public static void Seed(DatabaseContext context)
        {
            var adminRole = context.Roles.Add(new Role { Name = "Administrator" });
            var clientRole = context.Roles.Add(new Role { Name = "Client" });
            var dollarCurrency = context.Currencies.Add(new Currency { Name = "USA Dollar", ShortName = "USD", Symbol = "$" });
            var bank = context.Banks.Add(new Bank());

            context.SaveChanges();
            var firstAccount = context.Accounts.Add(new Account
            {
                Balance = new List<Money>(),
                ExpirationDate = DateTime.Today,
                IsActive = true,
                Number = "1"
            });
            var secondAccount = context.Accounts.Add(new Account
            {
                Balance = new List<Money>(),
                ExpirationDate = DateTime.Today,
                IsActive = true,
                Number = "2"
            });
            bank.Balance.Add(new Money { Currency = dollarCurrency });
            context.SaveChanges();
            firstAccount.Balance.Add(new Money
            {
                Currency = dollarCurrency,
                Value = 1000
            });
            context.SaveChanges();
            secondAccount.Balance.Add(new Money
            {
                Currency = dollarCurrency,
                Value = 2000
            });
            context.SaveChanges();
            context.Roles.Add(new Role { Name = "Employee" });
            var admin = context.Users.Add(new User { Username = "admin", Password = "admin", Role = adminRole, Region = context.Regions.Add(new Region { Name = "All" }), Birthday = DateTime.Now });
            var firstUser = context.Users.Add(new User { Username = "user1", Password = "user1", Role = clientRole, Region = context.Regions.Add(new Region { Name = "All" }), Birthday = DateTime.Now, Accounts = new List<Account>() });
            //var secondUser = context.Users.Add(new User { Username = "user2", Password = "user2", Role = clientRole, Region = context.Regions.Add(new Region { Name = "All" }), Birthday = DateTime.Now, Accounts = new List<Account>() });

            firstUser.Accounts.Add(firstAccount);
            admin.Accounts.Add(secondAccount);

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
