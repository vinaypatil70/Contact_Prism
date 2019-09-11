using System.Windows;
using PrismMVVMTestProject.ViewModels;
namespace PrismMVVMTestProject.Views
{
    /// <summary>
    /// Interaction logic for ContactView.xaml
    /// </summary>
    public partial class ContactView : Window
    {
        public ContactView()
        {
            InitializeComponent();
            this.DataContext = new ContactViewModel();
        }
    }
}
