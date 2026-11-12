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
            if (MurzayanovEntities.getContext().Users.Any(p => (p.Login == LoginTextBox.Text)))
            {
                Users user = MurzayanovEntities.getContext().Users.First(p => (p.Login == LoginTextBox.Text));
                Manager.currentUser = user;
                Manager.currentUserTextBlock.Text = $"{user.Surname} {user.Name} {user.Patronymic}";
                Manager.MainFrame.Navigate(new ProductPage());
            }
            else
                WrongLoginTextBlock.Visibility = Visibility.Visible;
        }

        private void GuestLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.currentUser = null;
            Manager.currentUserTextBlock.Text = "Неавторизованный пользователь";
            Manager.MainFrame.Navigate(new ProductPage());
        }
    }
}
