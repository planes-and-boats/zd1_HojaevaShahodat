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

namespace zd23_Hojaeva
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shop Shop = new Shop();
        private decimal selectedProductPrice = 0;
        public MainWindow()
        {
            InitializeComponent();
            //несколько стартовых товаров, чтобы магазин не был пустым
            Shop.CreateProduct("Молоко", 80, 10);
            Shop.CreateProduct("Хлеб", 45, 15);
            Shop.CreateProduct("Шоколад", 120, 5);
            UpdateScreen();
        }
        private void UpdateScreen()     //обновление списка товаров и прибыли
        {
            LstProducts.ItemsSource = Shop.GetProductsList();
            TxtTotalProfit.Text = $"{Shop.GetProfit()} руб.";
        }
        private void BtnSell_Click(object sender, RoutedEventArgs e) // кнопка Купить (Вкладка 1)
        {
            TxtStatusLog.Text = string.Empty;
            if (LstProducts.SelectedItem == null)
            {
                TxtStatusLog.Text = "Статус: Выберите товар в списке!";
                return;
            }
            if (LblSelectedProduct.Text != string.Empty && int.TryParse(TxtSellCount.Text, out int count))
            {
                if (count > 0)
                {
                    string result = Shop.Sell(LblSelectedProduct.Text, selectedProductPrice, count); ;
                    TxtStatusLog.Text = $"Статус: {result}";
                    UpdateScreen();
                }
                else
                {
                    TxtStatusLog.Text = "Статус: Ошибка! Введите количество больше 0";
                }
            }
            else
            {
                TxtStatusLog.Text = "Статус: Ошибка! Введите название продукта и его количество\nв числовом формате";
            }
        }

        private void BtnClearEmpty_Click(object sender, RoutedEventArgs e)      //кнопка Убрать пустые товары (Вкладка 1)
        {
            string result = Shop.ClearEmptyProducts();
            TxtStatusLog.Text = $"Статус: {result}";
            LblSelectedProduct.Text = "Не выбран";
            selectedProductPrice = 0;
            UpdateScreen();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)      // кнопка Добавить на склад (Вкладка 2)
        {
            TxtTargetStatus.Text = string.Empty;
            if (TxtProductName.Text != string.Empty && decimal.TryParse(TxtProductPrice.Text, out decimal price)
                && int.TryParse(TxtProductCount.Text, out int count))
            {
                if (count > 0 && price > 0)
                {
                    Shop.CreateProduct(TxtProductName.Text, price, count);
                    TxtProductName.Clear();
                    TxtProductPrice.Clear();
                    TxtProductCount.Clear();
                    TxtTargetStatus.Text = "Статус: Товар добавлен!";
                }
                else
                {
                    TxtTargetStatus.Text = "Статус: Ошибка! Введите цену и количество товара больше 0";
                }
            }
            else
            {
                TxtTargetStatus.Text = "Статус: Ошибка! Введите название, цену и количество товара\n(цену и количество в числовом формате)";
            }
        }

        private void BtnCheckTarget_Click(object sender, RoutedEventArgs e)      // кнопка Проверить цель (Вкладка 2)
        {
            TxtTargetStatus.Text = string.Empty;
            if (TxtTargetProfit.Text != string.Empty)
            {
                if (decimal.TryParse(TxtTargetProfit.Text, out decimal targetProfit) && targetProfit > 0)
                {
                    bool done = Shop.GetProfit(targetProfit, out decimal to);
                    if (done)
                    {
                        TxtTargetStatus.Text = "Цель успешно достигнута!";
                    }
                    else
                    {
                        TxtTargetStatus.Text = $"До цели не хватает еще: {to} руб.";
                    }
                }
                else
                {
                    TxtTargetStatus.Text = "Статус: Ошибка! Введите цель прибыли больше 0\n(в числовом формате)";
                }
            }
            else
            {
                TxtTargetStatus.Text = "Статус: Ошибка! Введите цель прибыли";
            }
        }

        private void LstProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)       //выбор товара из списка
        {
            if (LstProducts.SelectedItem != null)
            {
                string[] parts = LstProducts.SelectedItem.ToString().Split(new[] { " - ", " руб." }, StringSplitOptions.None);
                LblSelectedProduct.Text = parts[0];
                decimal.TryParse(parts[1], out selectedProductPrice);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)       //выбор магазина
        {
            PanelShop.Visibility = Visibility.Visible;
            PanelWarehouse.Visibility = Visibility.Collapsed;
            UpdateScreen();
            TxtStatusLog.Text = string.Empty;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)       //выбор склада
        {
            PanelShop.Visibility = Visibility.Collapsed;
            PanelWarehouse.Visibility = Visibility.Visible;
            UpdateScreen();
            TxtTargetStatus.Text = string.Empty;
        }
    }
}