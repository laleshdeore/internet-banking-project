using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankingDAL.Repository
{
    public class DatabaseRepository
    {
        protected readonly DatabaseContext Database;

        public DatabaseRepository(DatabaseContext database)
        {
            Database = database;
        }
    }
}
