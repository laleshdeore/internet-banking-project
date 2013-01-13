using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class AccountRepository : DatabaseRepository, IAccountRepository
    {
        public const int PartCount = 4;
        public const int MinPart = 1000;

        private readonly Random _random;

        public AccountRepository(DatabaseContext database)
            : base(database)
        {
            _random = new Random();
        }

        public long GenerateNumber()
        {
            long number;

            do
            {
                number = 0;
                for (var i = 0; i < PartCount; i++)
                {
                    number += number * MinPart + _random.Next(MinPart, MinPart*10 - 1);
                }
            } while (GetAccountByNumber(number) != null);

            return number;
        }

        public Account GetAccountByNumber(long number)
        {
            return Database.Accounts.SingleOrDefault(account => account.Number == number);
        }

        public Account GetAccountById(long id)
        {
            return Database.Accounts.SingleOrDefault(account => account.Id == id);
        }

        public IList<Account> GetExpiredAccounts(bool isActive)
        {
            return Database.Accounts.Where(account => account.ExpirationDate <= DateTime.Now && isActive).ToList();
        }

        public void AddOrUpdate(Account account)
        {
            if (account.ExpirationDate <= DateTime.Now)
            {
                account.IsActive = false;
            }
            if (account.Id == 0)
            {
                Database.Accounts.Add(account);
                SaveAllChanges();
            }
            else
            {
                Update(GetAccountById(account.Id), account);
            }
        }

        public void Delete(Account account)
        {
            account.Owner = null;
            AddOrUpdate(account);
        }
    }
}
