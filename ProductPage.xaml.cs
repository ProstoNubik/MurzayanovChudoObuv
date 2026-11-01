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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private void updateProducts()
        {
            var currentProduct = MurzayanovEntities.getContext().Products.ToList();

            // Search


            // Filter


            // Sort


            ProductListView.ItemsSource = currentProduct;
        }

        public ProductPage()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            DateTime lastMonthStart = now.AddMonths(-1);
            DateTime lastMonthEnd = now.AddDays(-1);

            var Products = MurzayanovEntities.getContext().Products.ToList();

            foreach (Products product in Products)
            {
                var productItemsOrders = MurzayanovEntities.getContext().OrderProductItems.Where(pi => pi.ProductItems.ProductID == product.ID).ToList();
                if (!productItemsOrders.Any(o => o.Orders.OrderDate >= lastMonthStart && o.Orders.OrderDate <= lastMonthEnd))
                    product.Discount = 0.25;
                else
                    product.Discount = 0;
            }


            FilterComboBox.SelectedIndex = 0;
            SortComboBox.SelectedIndex = 0;

            if (Manager.currentUser == null)
                ProductListView.ContextMenu = null;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateProducts();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateProducts();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateProducts();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(ProductListView.SelectedItems as List<ProductItems>);
            orderWindow.ShowDialog();

            if (orderWindow.DialogResult == true)
            {

            }
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Manager.currentUser != null)
            {
                if (ProductListView.SelectedItems.Count > 0)
                    OrderButton.Visibility = Visibility.Visible;
                else
                    OrderButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
