using System.Collections.Generic;
using SimsLayersDemo.Model;
using SimsLayersDemo.Repository;

namespace SimsLayersDemo.Service
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepo;

        public AccountService(AccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountRepo.GetAll();
        }

        public Account Get(long id)
        {
            return _accountRepo.Get(id);
        }
    }
}
