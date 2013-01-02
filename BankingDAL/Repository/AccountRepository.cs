using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class AccountRepository : DatabaseRepository, IAccountRepository
    {
        private readonly Random _random;

        public AccountRepository(DatabaseContext database) : base(database)
        {
            _random = new Random();
        }

        public string GenerateNumber()
        {
            var number = "";

            do
            {
                number = String.Format("{0:0000}-{1:0000}-{2:0000}-{3:0000}", _random.Next(9999), _random.Next(9999), _random.Next(9999), _random.Next(9999));
            } while (GetAccountByNumber(number) != null);

            return number;
        }

        public Account GetAccountByNumber(string number)
        {
            return Database.Accounts.SingleOrDefault(account => account.Number == number);
        }

        public void Add(Account account)
        {
            Database.Accounts.Add(account);
            SaveAllChanges();
        }
    }
}
