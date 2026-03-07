using NotepadPlus.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotepadPlus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void MenuOpenFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSaveFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}