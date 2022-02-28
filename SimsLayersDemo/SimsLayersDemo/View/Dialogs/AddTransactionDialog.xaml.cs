using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimsLayersDemo.Model;
using SimsLayersDemo.View.Converter;
using SimsLayersDemo.View.Model;

namespace SimsLayersDemo.View.Dialogs
{
    /// <summary>
    /// Interaction logic for AddTransactionDialog.xaml
    /// </summary>
    public partial class AddTransactionDialog : Window
    {
        private Bank _bank;
        public ObservableCollection<string> PayerAccounts { get; set; }
        public ObservableCollection<string> ReceiverAccounts { get; set; }

        private const string ERROR_MESSAGE = "All fields are mandatory, please fill them!";
        private static readonly Regex _decimalRegex = new Regex("[^0-9.-]+");
        private readonly DataView _dataView;
        private string _purpose;
        private double _amount;

        public AddTransactionDialog()
        {
            InitializeComponent();
            DataContext = this;
            _dataView = (Application.Current.MainWindow as MainWindow).GetDataView();

            _bank = Bank.GetInstance();

            PayerAccounts = new ObservableCollection<string>(FindAccountNumbersFromClients());
            ReceiverAccounts = new ObservableCollection<string>(FindAccountNumbersFromClients());
        }

        private IList<string> FindAccountNumbersFromClients()
        {
            return _bank.Clients
                .Select(client => client.Account.Number)
                .ToList();
        }

        public string Purpose
        {
            get => _purpose;
            set
            {
                if (_purpose != value)
                {
                    _purpose = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreAccountsSelected(PayerAccount, ReceiverAccount))
            {
                var transaction = CreateTransaction();
                UpdateDataView(transaction);
                Close();
            }
            else
            {
                new MessageDialog(ERROR_MESSAGE, this).ShowDialog();
            }
        }

        private bool AreAccountsSelected(ComboBox payer, ComboBox receiver)
        {
            return IsAccountSelected(payer) && IsAccountSelected(receiver);
        }

        private bool IsAccountSelected(ComboBox comboBox)
        {
            return comboBox.SelectedItem != null;
        }

        private Transaction CreateTransaction()
        {
            var transaction = new Transaction(
                _purpose,
                _amount,
                FindClientFromAccountNumber(PayerAccount.SelectedItem.ToString()),
                FindClientFromAccountNumber(ReceiverAccount.SelectedItem.ToString()));
            return _bank.Create(transaction);
        }

        private Client FindClientFromAccountNumber(string accountNumber)
        {
            return _bank.Clients
                .First(client => client.Account.Number.Equals(accountNumber));
        }

        private void UpdateDataView(Transaction transaction)
        {
            _dataView.Data.Add(TransactionConverter.ConvertTransactionToTransactionView(transaction));
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

        private bool IsTextDecimal(string input)
        {
            return !_decimalRegex.IsMatch(input);
        }

        private void MaxDecimalInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextDecimal(e.Text);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
