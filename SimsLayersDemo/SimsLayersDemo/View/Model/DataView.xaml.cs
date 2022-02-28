using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using SimsLayersDemo.Model;
using SimsLayersDemo.View.Converter;

namespace SimsLayersDemo.View.Model
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl
    {
        private readonly Bank _bank;
        public ObservableCollection<UserControl> Data { get; set; }

        public DataView()
        {
            InitializeComponent();
            DataContext = this;
            _bank = Bank.GetInstance();

            Data = new ObservableCollection<UserControl>(
                TransactionConverter.ConvertTransactionListToTransactionViewList(_bank.Transactions));

            LoanConverter
                .ConvertLoanListToLoanViewList(_bank.Loans)
                .ToList()
                .ForEach(Data.Add);
        }
    }
}
