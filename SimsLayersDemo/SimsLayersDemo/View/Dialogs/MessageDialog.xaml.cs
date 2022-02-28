using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SimsLayersDemo.View.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        private string _message;

        public MessageDialog(string content, Window owner)
        {
            InitializeComponent();
            DataContext = this;
            _message = content;
            Owner = owner;
        }

        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
