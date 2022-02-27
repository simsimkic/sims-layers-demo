using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimsLayersDemo.View.Model
{
    /// <summary>
    /// Interaction logic for LoanView.xaml
    /// </summary>
    public partial class LoanView : UserControl
    {
        private DateTime _approvalDate;
        private DateTime _deadline;
        private string _client;
        private string _clientAccount;
        private double _base;
        private double _interestRate;
        private long _numberOfInstallments;
        private double _installmentsAmount;
        private long _numberOfPaidInstallments;

        public LoanView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime ApprovalDate
        {
            get => _approvalDate;
            set
            {
                if (_approvalDate != value)
                {
                    _approvalDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Client
        {
            get => _client;
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ClientAccount
        {
            get => _clientAccount;
            set
            {
                if (_clientAccount != value)
                {
                    _clientAccount = value;
                    OnPropertyChanged();
                }
            }
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

        public long NumberOfInstallments
        {
            get => _numberOfInstallments;
            set
            {
                if (_numberOfInstallments != value)
                {
                    _numberOfInstallments = value;
                    OnPropertyChanged();
                }
            }
        }

        public double InstallmentAmount
        {
            get => _installmentsAmount;
            set
            {
                if (_installmentsAmount != value)
                {
                    _installmentsAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public long NumberOfPaidInstallments
        {
            get => _numberOfPaidInstallments;
            set
            {
                if (_numberOfPaidInstallments != value)
                {
                    _numberOfPaidInstallments = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
