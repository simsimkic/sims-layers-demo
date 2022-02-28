using System;
using System.Collections.Generic;
using System.Linq;
using SimsLayersDemo.Model;
using SimsLayersDemo.Repository;

namespace SimsLayersDemo.Service
{
    public class TransactionService
    {
        private readonly ClientService _clientService;
        private readonly TransactionRepository _transactionRepo;

        public TransactionService(ClientService clientService, TransactionRepository transactionRepo)
        {
            _clientService = clientService;
            _transactionRepo = transactionRepo;
        }

        internal IEnumerable<Transaction> GetAll()
        {
            var clients = _clientService.GetAll();
            var transactions = _transactionRepo.GetAll();
            BindClientsWithTransactions(clients, transactions);
            return transactions;
        }

        private void BindClientsWithTransactions(IEnumerable<Client> clients, IEnumerable<Transaction> transactions)
        {
            transactions.ToList().ForEach(transaction =>
            {
                transaction.Payer = FindClientById(clients, transaction.Payer.Id);
                transaction.Receiver = FindClientById(clients, transaction.Receiver.Id);
            });
        }

        private Client FindClientById(IEnumerable<Client> clients, long id)
        {
            return clients.SingleOrDefault(client => client.Id == id);
        }

        public Transaction Create(Transaction transaction)
        {
            ExecuteTransaction(transaction);

            // save accounts
            return _transactionRepo.Create(transaction);
        }

        private void ExecuteTransaction(Transaction transaction)
        {
            transaction.Payer.Account.Balance -= transaction.Amount;
            transaction.Receiver.Account.Balance += transaction.Amount;
            transaction.Date = DateTime.Now;
        }
    }
}
