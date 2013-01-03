using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public class RegionRepository: DatabaseRepository, IRegionRepository
    {
        public RegionRepository(DatabaseContext database) : base(database)
        {
        }

        public Region GetRegionById(long id)
        {
            return Database.Regions.SingleOrDefault(region => region.Id == id);
        }

        public Region GetRegionByName(string name)
        {
            return Database.Regions.SingleOrDefault(region => region.Name == name);
        }

        public IList<Region> GetRegions()
        {
            return Database.Regions.ToList();
        }

        public void AddOrUpdate(Region region)
        {
            if (region.Id == 0)
            {
                Database.Regions.Add(region);
                SaveAllChanges();
            }
            else
            {
                Update(GetRegionById(region.Id), region);
            }
        }

        public void Delete(Region region)
        {
            Database.Regions.Remove(region);
            Database.SaveChanges();
        }
    }
}
