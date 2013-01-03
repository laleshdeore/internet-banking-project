using System.Collections.Generic;
using BankingDAL.Entities;

namespace BankingDAL.Repository
{
    public interface IRegionRepository
    {
        Region GetRegionById(long id);
        Region GetRegionByName(string name);
        IList<Region> GetRegions();
        void AddOrUpdate(Region region);
        void Delete(Region region);
    }
}