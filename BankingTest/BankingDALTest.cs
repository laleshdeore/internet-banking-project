using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using BankingDAL;
using BankingDAL.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankingTest
{
    [TestClass]
    public class BankingDALTest
    {
        [TestMethod]
        public void InitDatabase()
        {
            using (var context = new DatabaseContext())
            {
                var admin = context.Users.First(user => user.Username == "admin");

                Assert.AreEqual("Administrator", admin.Role.Name);
                Assert.IsNotNull(admin.Region);
            }
        }
    }
}
