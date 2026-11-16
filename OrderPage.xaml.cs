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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public event Action<bool> NeedToClearList;

        private List<ProductItemForOrder> _currentProductItemsForOrder;

        private Orders _currentOrder;

        public OrderPage(List<ProductItemForOrder> selectedProductItems)
        {
            InitializeComponent();

            _currentProductItemsForOrder = selectedProductItems;

            ProductItemsListView.ItemsSource = selectedProductItems;

            _currentOrder = new Orders();
        }

        private void ConfirmOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentOrder.UserID = Manager.currentUser.ID;
                _currentOrder.OrderDate = DateTime.Now;

                MurzayanovEntities.getContext().Orders.Add(_currentOrder);

                foreach (ProductItemForOrder pifo in _currentProductItemsForOrder)
                {
                    pifo.productItem.Quantity -= pifo.itemCount;

                    if (pifo.productItem.Quantity < 0)
                    {
                        MessageBox.Show($"На данный момент модель {pifo.productItem.Products.Name} с размером {pifo.productItem.Sizes.Size} в таком количество нету на складе для оформления заказа.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    OrderProductItems newOrderProductItems = new OrderProductItems();
                    newOrderProductItems.Orders = _currentOrder;
                    newOrderProductItems.ProductItemID = pifo.productItem.ID;
                    newOrderProductItems.ProductCount = pifo.itemCount;

                    MurzayanovEntities.getContext().OrderProductItems.Add(newOrderProductItems);
                }

                MurzayanovEntities.getContext().SaveChanges();

                NeedToClearList.Invoke(true);

                MessageBox.Show("Заказ успешно оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.ToString()}");
            }
        }
    }
}
