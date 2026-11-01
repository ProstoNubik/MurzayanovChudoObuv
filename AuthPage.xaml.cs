using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MurzayanovChudoObuv
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void LoginLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var users = MurzayanovEntities.getContext().Users.Where(p => (p.Login == LoginTextBox.Text)).ToList();

            if (users.Any())
            {
                Manager.currentUser = users.First();
                Manager.MainFrame.Navigate(new ProductPage());
            }
            else
                WrongLoginTextBlock.Visibility = Visibility.Visible;
        }

        private void GuestLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.currentUser = null;
            Manager.MainFrame.Navigate(new ProductPage());
        }
    }
}
