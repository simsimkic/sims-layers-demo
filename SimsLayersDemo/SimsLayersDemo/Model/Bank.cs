using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimsLayersDemo.Exception;

namespace SimsLayersDemo.Model
{
    public class Bank
    {
        private static Bank _instance;
        private List<Account> _accounts;
        private List<Client> _clients;
        private List<Transaction> _transactions;
        private List<Loan> _loans;

        private long _clientMaxId;
        private long _accountMaxId;
        private long _loanMaxId;
        private long _transactionMaxId;

        private static string _projectPath = System.Reflection.Assembly.GetExecutingAssembly().Location
            .Split(new string[] { "bin" }, StringSplitOptions.None)[0];

        private string CLIENT_FILE = _projectPath + "\\Resources\\Data\\clients.csv";
        private string ACCOUNT_FILE = _projectPath + "\\Resources\\Data\\accounts.csv";
        private string LOAN_FILE = _projectPath + "\\Resources\\Data\\loans.csv";
        private string TRANSACTION_FILE = _projectPath + "\\Resources\\Data\\transactions.csv";
        private const string CSV_DELIMITER = ";";
        private const string DATETIME_FORMAT = "dd.MM.yyyy.";

        public static Bank GetInstance()
        {
            if (_instance == null) _instance = new Bank();
            return _instance;
        }

        private Bank()
        {
            GetAllData();
            InitializeIds();
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

        private void InitializeIds()
        {
            _accountMaxId = FindAccountsMaxId();
            _clientMaxId = FindClientsMaxId();
            _transactionMaxId = FindTransactionsMaxId();
            _loanMaxId = FindLoansMaxId();
        }

        private long FindAccountsMaxId()
        {
            if (_accounts.Count == 0) return 0;
            return _accounts.Max(account => account.Id);
        }

        private long FindClientsMaxId()
        {
            if (_accounts.Count == 0) return 0;
            return _accounts.Max(client => client.Id);
        }

        private long FindTransactionsMaxId()
        {
            if (_accounts.Count == 0) return 0;
            return _accounts.Max(transaction => transaction.Id);
        }

        private long FindLoansMaxId()
        {
            if (_accounts.Count == 0) return 0;
            return _accounts.Max(loan => loan.Id);
        }

        public Transaction Create(Transaction transaction)
        {
            ExecuteTransaction(transaction);

            Transaction newTransaction = Save(transaction);
            SaveAccounts();
            _transactions.Add(newTransaction);
            return newTransaction;
        }

        private void ExecuteTransaction(Transaction transaction)
        {
            transaction.Payer.Account.Balance -= transaction.Amount;
            transaction.Receiver.Account.Balance += transaction.Amount;
            transaction.Date = DateTime.Now;
        }

        public Loan Create(Loan loan)
        {
            if (IsDeadlineAfterApprovalDate(loan))
            {
                loan.NumberOfInstallments = CalculateNumberOfInstallments(loan);
                loan.InstallmentAmount = CalculateInstallmentAmount(loan);
                ApproveLoan(loan);

                Loan newLoan = Save(loan);
                SaveAccounts();
                _loans.Add(newLoan);
                return newLoan;
            }
            else
            {
                throw new InvalidDateException(
                    $"Deadline: {loan.Deadline} is before approval date: {loan.ApprovalDate}");
            }
        }

        private void ApproveLoan(Loan loan)
        {
            loan.Client.Account.Balance += loan.Base;
            loan.ApprovalDate = DateTime.Now;
        }

        private bool IsDeadlineAfterApprovalDate(Loan loan)
        {
            return loan.Deadline > loan.ApprovalDate;
        }

        private long CalculateNumberOfInstallments(Loan loan)
        {
            return ((loan.Deadline.Year - loan.ApprovalDate.Year) * 12) + loan.Deadline.Month - loan.ApprovalDate.Month;
        }

        private double CalculateInstallmentAmount(Loan loan)
        {
            return (loan.Base * (1 + loan.InterestRate / 100)) / loan.NumberOfInstallments;
        }

        private Transaction Save(Transaction transaction)
        {
            transaction.Id = _transactionMaxId++;
            AppendLineToFile(TRANSACTION_FILE, TransactionToCSVFormat(transaction));
            return transaction;
        }

        private Loan Save(Loan loan)
        {
            loan.Id = _loanMaxId++;
            AppendLineToFile(LOAN_FILE, LoanToCSVFormat(loan));
            return loan;
        }

        private Client Save(Client client)
        {
            client.Account = Save(client.Account);
            client.Id = _clientMaxId++;
            AppendLineToFile(CLIENT_FILE, ClientToCSVFormat(client));
            return client;
        }

        private Account Save(Account account)
        {
            account.Id = _accountMaxId++;
            AppendLineToFile(ACCOUNT_FILE, AccountToCSVFormat(account));
            return account;
        }

        private void AppendLineToFile(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        private void SaveAccounts()
        {
            WriteAllLinesToFile(ACCOUNT_FILE,
                _accounts
                    .Select(AccountToCSVFormat)
                    .ToList());
        }

        private void WriteAllLinesToFile(string path, List<string> content)
        {
            File.WriteAllLines(path, content.ToArray());
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

        private string TransactionToCSVFormat(Transaction transaction)
        {
            return string.Join(CSV_DELIMITER,
                transaction.Id,
                transaction.Purpose,
                transaction.Date.ToString(DATETIME_FORMAT),
                transaction.Amount,
                transaction.Payer.Id,
                transaction.Receiver.Id);
        }

        private string LoanToCSVFormat(Loan loan)
        {
            return string.Join(CSV_DELIMITER,
                loan.Id,
                loan.Client.Id,
                loan.ApprovalDate.ToString(DATETIME_FORMAT),
                loan.Deadline.ToString(DATETIME_FORMAT),
                loan.Base,
                loan.InterestRate,
                loan.NumberOfInstallments,
                loan.InstallmentAmount,
                loan.NumberOfPaidInstallments);
        }

        private string ClientToCSVFormat(Client client)
        {
            return string.Join(CSV_DELIMITER,
                client.Id,
                client.FirstName,
                client.LastName,
                client.DateOfBirth.ToString(DATETIME_FORMAT),
                client.Account.Id);
        }

        private string AccountToCSVFormat(Account account)
        {
            return string.Join(CSV_DELIMITER,
                account.Id,
                account.Number,
                account.Balance);
        }
    }
}
