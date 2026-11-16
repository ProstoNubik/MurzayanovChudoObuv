using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для AddProductToOrderPage.xaml
    /// </summary>
    public partial class AddProductToOrderPage : Page
    {
        public event Action<ProductItems, int> Result;

        private Products _currentProduct;

        private int _productItemCount = 0;

        public AddProductToOrderPage(Products selectedProduct)
        {
            InitializeComponent();

            _currentProduct = selectedProduct;
            this.DataContext = selectedProduct;

            AvailableSizesComboBox.ItemsSource = _currentProduct.ProductItems.Select(pi => pi.Sizes);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_productItemCount == 0)
            {
                MessageBox.Show("Пожалуйсте, выберите количество товарной позиции, отличного от нуля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProductItems currentProductItem = _currentProduct.ProductItems.First(pi => pi.Sizes == AvailableSizesComboBox.SelectedItem);

            Result.Invoke(currentProductItem, _productItemCount);

            MessageBox.Show("Текущая модель с выбранным размером и количеством добавлено к заказу.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PlusCountButton_Click(object sender, RoutedEventArgs e)
        {
            int currentQuantity = _currentProduct.ProductItems.First(pi => pi.Sizes == AvailableSizesComboBox.SelectedItem).Quantity;

            if (currentQuantity == 0)
            {
                MessageBox.Show("К сожалению, данной модели с выбранным размером сейчас нет в наличии.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            setProductItemCount(Math.Min(
                currentQuantity,
                _productItemCount + 1
                ));
        }

        private void MinusCountButton_Click(object sender, RoutedEventArgs e)
        {
            setProductItemCount(Math.Max(
                0,
                _productItemCount - 1
                ));
        }

        private void setProductItemCount(int newValue)
        {
            _productItemCount = newValue;
            CountTextBlock.Text = _productItemCount.ToString();
        }

        private void AvailableSizesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setProductItemCount(0);
        }
    }
}
