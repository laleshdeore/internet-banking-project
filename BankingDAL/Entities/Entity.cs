using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BankingDAL.Entities
{
    public class Entity
    {
        [Key]
        public long Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Entity && Equals((Entity) obj);
        }

        protected bool Equals(Entity other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
