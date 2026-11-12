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
using System.Windows.Shapes;

namespace MurzayanovChudoObuv
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private int ProductItemCount;

        private Products CurrentProduct;

        public OrderWindow(Products currentProduct)
        {
            InitializeComponent();

            this.DataContext = currentProduct;
            CurrentProduct = currentProduct;

            AvailableSizesComboBox.ItemsSource = currentProduct.ProductItems.Select(pi => pi.Sizes);
        }

        private void MinusCountButton_Click(object sender, RoutedEventArgs e)
        {
            setProductItemCount(Math.Max(
                0, 
                ProductItemCount - 1
            ));
        }

        private void PlusCountButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentProduct.ProductItems.First(pi => pi.Sizes == AvailableSizesComboBox.SelectedItem).Quantity == 0)
            {
                MessageBox.Show("К сожалению, данной товарной позиции нет в наличии.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            setProductItemCount(Math.Min(
                CurrentProduct.ProductItems.First(pi => pi.Sizes == AvailableSizesComboBox.SelectedItem).Quantity,
                ProductItemCount + 1
            ));
        }

        private void AvailableSizesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setProductItemCount(0);
        }

        private void setProductItemCount(int newValue)
        {
            ProductItemCount = newValue;
            CountTextBlock.Text = ProductItemCount.ToString();
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductItemCount == 0)
            {
                MessageBox.Show("Пожалуйсте, выберите количество товарной позиции, отличного от нуля.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProductItems currentProductItem = CurrentProduct.ProductItems.First(pi => pi.Sizes == AvailableSizesComboBox.SelectedItem);

            try
            {
                Orders newOrder = new Orders();
                newOrder.UserID = Manager.currentUser.ID;
                newOrder.OrderDate = DateTime.Now;

                OrderProductItems newOrderProductItems = new OrderProductItems();
                newOrderProductItems.ProductItemID = currentProductItem.ID;
                newOrderProductItems.OrderID = newOrder.ID;
                newOrderProductItems.ProductCount = ProductItemCount;

                MurzayanovEntities.getContext().Orders.Add(newOrder);
                MurzayanovEntities.getContext().OrderProductItems.Add(newOrderProductItems);

                currentProductItem.Quantity -= ProductItemCount;

                MurzayanovEntities.getContext().SaveChanges();

                MessageBox.Show("Заказ успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                setProductItemCount(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
