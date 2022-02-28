using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SimsLayersDemo.Controller;
using SimsLayersDemo.View.Converter;

namespace SimsLayersDemo.View.Model
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl
    {
        private LoanController _loanController;
        private TransactionController _transactionController;
        public ObservableCollection<UserControl> Data { get; set; }

        public DataView()
        {
            InitializeComponent();
            DataContext = this;
            var app = Application.Current as App;
            _loanController = app.LoanController;
            _transactionController = app.TransactionController;

            Data = new ObservableCollection<UserControl>(
                TransactionConverter.ConvertTransactionListToTransactionViewList(_transactionController.GetAll().ToList()));

            LoanConverter
                .ConvertLoanListToLoanViewList(_loanController.GetAll().ToList())
                .ToList()
                .ForEach(Data.Add);
        }
    }
}
