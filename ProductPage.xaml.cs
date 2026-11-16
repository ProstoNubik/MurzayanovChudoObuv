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
        private List<ProductItemForOrder> selectedProductItemsForOrder = new List<ProductItemForOrder>();

        private void updateProducts()
        {
            var currentProducts = MurzayanovEntities.getContext().Products.ToList();

            // Search
            string SearchText = SearchTextBox.Text
                .ToLower()
                .Replace("«", "")
                .Replace("»", "");

            currentProducts = currentProducts.Where(p =>
                p.Name.ToLower().Replace("«", "").Replace("»", "").Contains(SearchText)
                || 
                p.Description.ToLower().Replace("«", "").Replace("»", "").Contains(SearchText))
            .ToList();

            // Filter
            if (FilterComboBox.SelectedIndex > 0)
                currentProducts = currentProducts.Where(p => p.CategoryID == FilterComboBox.SelectedIndex).ToList();

            // Sort
            if (SortComboBox.SelectedIndex == 1)
                currentProducts = currentProducts.OrderBy(p => p.CostWithDiscount).ToList();
            else if (SortComboBox.SelectedIndex == 2)
                currentProducts = currentProducts.OrderByDescending(p => p.CostWithDiscount).ToList();

            ProductListView.ItemsSource = currentProducts;
        }

        public ProductPage()
        {
            InitializeComponent();

            if (Manager.currentUser == null)
            {
                SearchTextBox.IsEnabled = false;
                FilterComboBox.IsEnabled = false;
                SortComboBox.IsEnabled = false;
            }

            List<String> ProductTypeNamesList = new List<String>();
            ProductTypeNamesList.Add("Все категории");
            foreach (Categories category in MurzayanovEntities.getContext().Categories)
                ProductTypeNamesList.Add(category.Name);
            FilterComboBox.ItemsSource = ProductTypeNamesList;

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

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListView.SelectedItem == null)
                return;

            var addToOrderPage = new AddProductToOrderPage(ProductListView.SelectedItem as Products);

            addToOrderPage.Result += ProductItemReceived;

            ProductListView.SelectedItem = null;

            Manager.MainFrame.Navigate(addToOrderPage);
        }

        private void ProductItemReceived(ProductItems SelectedProductItem, int SelectedProductItemCount)
        {
            if (selectedProductItemsForOrder.Any(pi => pi.productItem.ID == SelectedProductItem.ID))
            {
                var pifo = selectedProductItemsForOrder.First(pi => pi.productItem.ID == SelectedProductItem.ID);
                pifo.itemCount += SelectedProductItemCount;
                return;
            }

            ProductItemForOrder newProductItemForOrder = new ProductItemForOrder(SelectedProductItem, SelectedProductItemCount);

            selectedProductItemsForOrder.Add(newProductItemForOrder);

            updateOrderButtonVisibility();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var orderPage = new OrderPage(selectedProductItemsForOrder);

            orderPage.NeedToClearList += ResetProductItemsForOrderList;

            Manager.MainFrame.Navigate(orderPage);
        }

        private void ResetProductItemsForOrderList(bool NeedClear)
        {
            if (NeedClear)
            {
                selectedProductItemsForOrder.Clear();
                updateOrderButtonVisibility();
            }
        }

        private void updateOrderButtonVisibility()
        {
            if (selectedProductItemsForOrder.Count > 0)
                OrderButton.Visibility = Visibility.Visible;
            else
                OrderButton.Visibility = Visibility.Collapsed;
        }
    }
}
