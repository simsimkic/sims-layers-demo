using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimsLayersDemo.Model
{
    public class Bank
    {
        private static Bank _instance;
        private List<Account> _accounts;
        private List<Client> _clients;
        private List<Transaction> _transactions;
        private List<Loan> _loans;

        public static Bank GetInstance()
        {
            if (_instance == null) _instance = new Bank();
            return _instance;
        }

        private Bank()
        {
        }

        public List<Loan> Loans
        {
            get => _loans;
            set => _loans = value;
        }

        public List<Transaction> Transactions
        {
            get => _transactions;
            set => _transactions = value;
        }

        public List<Client> Clients
        {
            get => _clients;
            set => _clients = value;
        }
    }
}
