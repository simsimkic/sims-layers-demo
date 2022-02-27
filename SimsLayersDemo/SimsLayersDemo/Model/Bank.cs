using System;
using System.Collections.Generic;
using System.IO;
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

        private static string _projectPath = System.Reflection.Assembly.GetExecutingAssembly().Location
            .Split(new string[] { "bin" }, StringSplitOptions.None)[0];

        private string CLIENT_FILE = _projectPath + "\\Resources\\Data\\clients.csv";
        private string ACCOUNT_FILE = _projectPath + "\\Resources\\Data\\accounts.csv";
        private string LOAN_FILE = _projectPath + "\\Resources\\Data\\loans.csv";
        private string TRANSACTION_FILE = _projectPath + "\\Resources\\Data\\transactions.csv";
        private const string CSV_DELIMITER = ";";

        public static Bank GetInstance()
        {
            if (_instance == null) _instance = new Bank();
            return _instance;
        }

        private Bank()
        {
            GetAllData();
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

        private void GetAllData()
        {
            _accounts = GetAccounts();
            _clients = GetClients();
            _transactions = GetTransactions();
            _loans = GetLoans();

            BindAccountsWithClients();
            BindClientsWithTransactions();
            BindClientsWithLoans();
        }

        private List<Account> GetAccounts()
        {
            return File.ReadAllLines(ACCOUNT_FILE)
                .Select(CSVFormatToAccount)
                .ToList();
        }

        private List<Client> GetClients()
        {
            return File.ReadAllLines(CLIENT_FILE)
                .Select(CSVFormatToClient)
                .ToList();
        }

        private List<Transaction> GetTransactions()
        {
            return File.ReadAllLines(TRANSACTION_FILE)
                .Select(CSVFormatToTransaction)
                .ToList();
        }

        private List<Loan> GetLoans()
        {
            return File.ReadAllLines(LOAN_FILE)
                .Select(CSVFormatToLoan)
                .ToList();
        }

        private void BindAccountsWithClients()
        {
            _clients.ForEach(client => client.Account = FindAccountById(client.Id));
        }

        private Account FindAccountById(long id)
        {
            return _accounts.Find(account => account.Id == id);
        }

        private void BindClientsWithTransactions()
        {
            _transactions.ForEach(transaction =>
            {
                transaction.Payer = FindClientById(transaction.Payer.Id);
                transaction.Receiver = FindClientById(transaction.Receiver.Id);
            });
        }

        private Client FindClientById(long id)
        {
            return _clients.Find(client => client.Id == id);
        }

        private void BindClientsWithLoans()
        {
            _loans.ForEach(loan => loan.Client = FindClientById(loan.Client.Id));
        }

        private Account CSVFormatToAccount(string acountCSVFormat)
        {
            var tokens = acountCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Account(long.Parse(tokens[0]), tokens[1], double.Parse(tokens[2]));
        }

        private Client CSVFormatToClient(string clientCSVFormat)
        {
            var tokens = clientCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Client(
                long.Parse(tokens[0]),
                tokens[1], tokens[2],
                DateTime.Parse(tokens[3]),
                new Account(long.Parse(tokens[4])));
        }

        private Transaction CSVFormatToTransaction(string transactionCSVFormat)
        {
            var tokens = transactionCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Transaction(
                long.Parse(tokens[0]),
                tokens[1],
                DateTime.Parse(tokens[2]),
                double.Parse(tokens[3]),
                new Client(long.Parse(tokens[4])),
                new Client(long.Parse(tokens[5])));
        }

        private Loan CSVFormatToLoan(string loanCSVFormat)
        {
            var tokens = loanCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Loan(
                long.Parse(tokens[0]),
                new Client(long.Parse(tokens[1])),
                DateTime.Parse(tokens[2]),
                DateTime.Parse(tokens[3]),
                double.Parse(tokens[4]),
                double.Parse(tokens[5]),
                long.Parse(tokens[6]),
                double.Parse(tokens[7]),
                long.Parse(tokens[8]));
        }
    }
}
