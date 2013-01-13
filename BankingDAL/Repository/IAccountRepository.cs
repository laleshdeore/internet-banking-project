using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IAccountRepository
    {
        long GenerateNumber();
        Account GetAccountByNumber(long number);
        Account GetAccountById(long id);
        IList<Account> GetExpiredAccounts(bool isActive); 
        void AddOrUpdate(Account account);
        void Delete(Account account);
    }
}
