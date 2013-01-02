﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IAccountRepository
    {
        string GenerateNumber();
        Account GetAccountByNumber(string number);
        void Add(Account account);
    }
}
