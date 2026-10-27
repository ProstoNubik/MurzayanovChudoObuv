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
    /// Логика взаимодействия для ProductsItemsPage.xaml
    /// </summary>
    public partial class ProductsItemsPage : Page
    {
        private void updateProductItems()
        {
            var currentProductItems = MurzayanovEntities.getContext().ProductItems.ToList();

            // Search
            currentProductItems = currentProductItems.Where(p => (p.Products.Name.ToLower().Replace("«", "").Replace("»", "").Contains(SearchTextBox.Text.ToLower().Replace("«", "").Replace("»", "")))).ToList();

            // Filter
            if (FilterComboBox.SelectedIndex > 0)
                currentProductItems = currentProductItems.Where(p => (p.Products.CategoryID == FilterComboBox.SelectedIndex)).ToList();

            // Sort
            if (SortComboBox.SelectedIndex == 0)
                currentProductItems = currentProductItems.OrderBy(p => p.ID).ToList();
            else if (SortComboBox.SelectedIndex == 1)
                currentProductItems = currentProductItems.OrderByDescending(p => p.ID).ToList();
            else if (SortComboBox.SelectedIndex == 2)
                currentProductItems = currentProductItems.OrderBy(p => p.Sizes.Size).ToList();
            else if (SortComboBox.SelectedIndex == 3)
                currentProductItems = currentProductItems.OrderByDescending(p => p.Sizes.Size).ToList();
            else if (SortComboBox.SelectedIndex == 4)
                currentProductItems = currentProductItems.OrderBy(p => p.Products.Cost).ToList();
            else if (SortComboBox.SelectedIndex == 5)
                currentProductItems = currentProductItems.OrderByDescending(p => p.Products.Cost).ToList();

            ProductItemsListView.ItemsSource = currentProductItems;
        }

        public ProductsItemsPage()
        {
            InitializeComponent();

            FilterComboBox.SelectedIndex = 0;
            SortComboBox.SelectedIndex = 0;

            if (Manager.currentUser == null)
                ProductItemsListView.ContextMenu = null;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateProductItems();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateProductItems();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateProductItems();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(ProductItemsListView.SelectedItems as List<ProductItems>);
            orderWindow.ShowDialog();

            if (orderWindow.DialogResult == true)
            {
                
            }
        }

        private void ProductItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Manager.currentUser != null)
            {
                if (ProductItemsListView.SelectedItems.Count > 0)
                    OrderButton.Visibility = Visibility.Visible;
                else
                    OrderButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
