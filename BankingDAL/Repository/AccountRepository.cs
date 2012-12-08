using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingDAL.Repository
{
    public class AccountRepository: IAccountRepository
    {
        private readonly DatabaseContext _db;

        public AccountRepository(DatabaseContext db)
        {
            _db = db;
        }
    }
}
