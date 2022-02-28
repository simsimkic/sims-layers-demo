using System.Collections.Generic;
using System.Linq;
using SimsLayersDemo.Model;
using SimsLayersDemo.Repository;

namespace SimsLayersDemo.Service
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepo;
        private readonly AccountService _accountService;

        public ClientService(ClientRepository clientRepo, AccountService accountService)
        {
            _clientRepo = clientRepo;
            _accountService = accountService;
        }

        public IEnumerable<Client> GetAll()
        {
            var accounts = _accountService.GetAll();
            var clients = _clientRepo.GetAll();
            BindAccountsWithClients(accounts, clients);
            return clients;
        }

        public Client Get(long id)
        {
            var client = _clientRepo.Get(id);
            client.Account = _accountService.Get(client.Account.Id);
            return client;
        }

        private void BindAccountsWithClients(IEnumerable<Account> accounts, IEnumerable<Client> clients)
        {
            clients.ToList().ForEach(client => client.Account = GetAccountById(accounts, client.Id));
        }

        private Account GetAccountById(IEnumerable<Account> accounts, long id)
        {
            return accounts.SingleOrDefault(account => account.Id == id);
        }
    }
}
