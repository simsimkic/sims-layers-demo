using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimsLayersDemo.Controller;
using SimsLayersDemo.Exception;
using SimsLayersDemo.Model;
using SimsLayersDemo.View.Converter;
using SimsLayersDemo.View.Model;

namespace SimsLayersDemo.View.Dialogs
{
    /// <summary>
    /// Interaction logic for AddLoanDialog.xaml
    /// </summary>
    public partial class AddLoanDialog : Window
    {
        private const string ERROR_MESSAGE = "All fields are mandatory, please fill them!";
        private const string DATE_FORMAT_ERROR_MESSAGE = "Invalid date format. Valid format is: 15.05.2020. !";
        private const string INVALID_DEADLINE_ERROR_MESSAGE = "Invalid deadline value. Valid date starts from next month!";
        private static readonly Regex _decimalRegex = new Regex("[^0-9.-]+");

        private DataView _dataView;
        private IList<Client> _clients;
        private double _base;
        private double _interestRate;
        private DateTime _deadline;
        private DateTime _deadlineLowerLimit;
        private ClientController _clientController;
        private LoanController _loanController;

        public ObservableCollection<string> Accounts { get; set; }

        public AddLoanDialog()
        {
            InitializeComponent();
            DataContext = this;
            _dataView = (Application.Current.MainWindow as MainWindow).GetDataView();
            var app = Application.Current as App;
            _clientController = app.ClientController;
            _loanController = app.LoanController;

            _clients = _clientController.GetAll().ToList();
            Accounts = new ObservableCollection<string>(FindAccountNumbersFromClients());

            _deadlineLowerLimit = DateTime.Now.AddMonths(1);
            Deadline.Text = DateTime.Now.AddYears(1).ToString("dd.MM.yyyy.");
        }

        private IList<string> FindAccountNumbersFromClients()
        {
            return _clients
                .Select(client => client.Account.Number)
                .ToList();
        }

        public double Base
        {
            get => _base;
            set
            {
                if (_base != value)
                {
                    _base = value;
                    OnPropertyChanged();
                }
            }
        }

        public double InterestRate
        {
            get => _interestRate;
            set
            {
                if (_interestRate != value)
                {
                    _interestRate = value;
                    OnPropertyChanged();
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            if (!IsValidDeadlineFormatDate())
            {
                ShowError(DATE_FORMAT_ERROR_MESSAGE);
            }
            else if (IsDeadlineLowerThanLowerLimit())
            {
                ShowError(INVALID_DEADLINE_ERROR_MESSAGE);
            }
            else if (!IsClientSelected())
            {
                ShowError(ERROR_MESSAGE);
            }
            else
            {
                try
                {
                    UpdateDataView(CreateLoan());
                    Close();
                }
                catch (InvalidDateException error)
                {
                    ShowError(error.Message);
                }

            }
        }

        private bool IsValidDeadlineFormatDate()
        {
            if (!DateTime.TryParse(Deadline.Text, out DateTime deadline))
            {
                return false;
            }
            else
            {
                _deadline = deadline;
                return true;
            }
        }

        private bool IsDeadlineLowerThanLowerLimit()
        {
            return _deadline < _deadlineLowerLimit;
        }

        private void ShowError(string s)
        {
            new MessageDialog(s, this).ShowDialog();
        }

        private bool IsClientSelected()
        {
            return Client.SelectedItem != null;
        }

        private Loan CreateLoan()
        {
            try
            {
                return _loanController.Create(new Loan(
                    FindClientFromAccountNumber(Client.SelectedItem.ToString()),
                    _deadline,
                    _base,
                    _interestRate));
            }
            catch (InvalidDateException)
            {
                throw;
            }
        }

        private Client FindClientFromAccountNumber(string accountNumber)
        {
            return _clients
                .First(client => client.Account.Number.Equals(accountNumber));
        }

        private void UpdateDataView(Loan loan)
        {
            _dataView.Data.Add(LoanConverter.ConvertLoanToLoanView(loan));
        }

        private void MaxDecimalInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextDecimal(e.Text);
        }

        private void MaxDecimalPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextDecimal(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextDecimal(string input) => !_decimalRegex.IsMatch(input);

        private void InterestRateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (double.TryParse(textbox.Text, out double value))
            {
                if (value > 100)
                    textbox.Text = "100";
                else if (value < 0)
                    textbox.Text = "0";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
