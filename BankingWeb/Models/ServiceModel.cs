using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankingDAL.Entities;
using BankingDAL.Repository;

namespace BankingWeb.Models
{
    public class ServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string IdentifierDescription { get; set; }

        public long Account { get; set; }

        public string Region { get; set; }

        public IList<Region> Regions { get; set; }

        public ServiceModel()
        {
            Regions = new List<Region>();
        }

        public Service GetEntity(IRegionRepository regionRepository, IAccountRepository accountRepository)
        {
            return new Service
            {
                Name = Name,
                Description = Description,
                IdentifierDescription = IdentifierDescription,
                Account = accountRepository.GetAccountByNumber(Account),
                Region = regionRepository.GetRegionByName(Region)
            };
        }
    }
}