using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using BankingDAL;
using BankingDAL.Entities;
using BankingDAL.Repository;
using BankingWeb.Controllers;

namespace BankingWeb.Utils
{
    public class DailyChecker
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public DailyChecker(DatabaseContext context)
        {
            _paymentRepository = new PaymentRepository(context);
            _currencyRepository = new CurrencyRepository(context);
            _accountRepository = new AccountRepository(context);
            _userRepository = new UserRepository(context);
            _roleRepository = new RoleRepository(context);
        }

        public void Check()
        {
            while (true)
            {
                if (DateTime.Now.Day == 1)
                {
                    foreach (var autoPayment in _paymentRepository.GetAutoPayments())
                    {
                        try
                        {
                            _paymentRepository.Pay(new Payment
                            {
                                From = autoPayment.From,
                                To = autoPayment.To,
                                Value = autoPayment.Value,
                                Date = DateTime.Now,
                                ServiceIdentifier = autoPayment.ServiceIdentifier
                            }, _currencyRepository);
                        }
                        catch
                        { }
                    }
                    var roles = new List<Role> { _roleRepository.GetRoleByName(BaseController.Client) };
                    var page = new Page {Capacity = BaseController.PageCapacity, Number = 1};
                    IList<User> users;

                    while ((users = _userRepository.GetUsersByRoles(roles, page)).Count > 0)
                    {
                        var accounts = new List<Account>();

                        foreach (var user in users)
                        {
                            accounts.AddRange(user.Accounts.Where(account => account.IsActive));
                        }
                    }
                }
                foreach (var account in _accountRepository.GetExpiredAccounts(true))
                {
                    account.IsActive = false;
                    _accountRepository.AddOrUpdate(account);
                }
                //Thread.Sleep(TimeSpan.FromSeconds(1));
                Thread.Sleep(TimeSpan.FromDays(1));
            }
        }
    }
}