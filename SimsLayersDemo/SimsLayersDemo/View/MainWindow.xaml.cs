using System.Windows;
using SimsLayersDemo.View.Model;

namespace SimsLayersDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DataView GetDataView()
        {
            return DataViewer;
        }
    }
}
