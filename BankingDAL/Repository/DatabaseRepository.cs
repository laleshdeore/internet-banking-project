using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class DatabaseRepository: IDisposable
    {
        protected readonly DatabaseContext Database;
        protected readonly StringComparison StringComparison = StringComparison.InvariantCultureIgnoreCase;

        public DatabaseRepository(DatabaseContext database)
        {
            Database = database;
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
            }
        }

        protected void Update(Entity entity, Entity newEntity)
        {
            Database.Entry(entity).CurrentValues.SetValues(newEntity);
            SaveAllChanges();
        }

        protected void SaveAllChanges()
        {
            var id = 0;
            var prevId = id;

            while ((id = Database.SaveChanges()) != prevId && id != 0) {}
        }
    }
}
