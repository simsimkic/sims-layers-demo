using System;
using System.Windows;
using SimsLayersDemo.Controller;
using SimsLayersDemo.Repository;
using SimsLayersDemo.Service;

namespace SimsLayersDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string _projectPath = System.Reflection.Assembly.GetExecutingAssembly().Location
            .Split(new string[] { "bin" }, StringSplitOptions.None)[0];
        private string CLIENT_FILE = _projectPath + "\\Resources\\Data\\clients.csv";
        private string ACCOUNT_FILE = _projectPath + "\\Resources\\Data\\accounts.csv";
        private string LOAN_FILE = _projectPath + "\\Resources\\Data\\loans.csv";
        private string TRANSACTION_FILE = _projectPath + "\\Resources\\Data\\transactions.csv";
        private const string CSV_DELIMITER = ";";
        private const string DATETIME_FORMAT = "dd.MM.yyyy.";

        public LoanController LoanController { get; set; }
        public TransactionController TransactionController { get; set; }
        public ClientController ClientController { get; set; }

        public App()
        {
            var accountRepository = new AccountRepository(ACCOUNT_FILE, CSV_DELIMITER);
            var clientRepository = new ClientRepository(CLIENT_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var loanRepository = new LoanRepository(LOAN_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var transactionRepo = new TransactionRepository(TRANSACTION_FILE, CSV_DELIMITER, DATETIME_FORMAT);

            var accountService = new AccountService(accountRepository);
            var clientService = new ClientService(clientRepository, accountService);
            var loanService = new LoanService(clientService, loanRepository);
            var transactionService = new TransactionService(clientService, transactionRepo);

            LoanController = new LoanController(loanService);
            TransactionController = new TransactionController(transactionService);
            ClientController = new ClientController(clientService);
        }
    }
}
