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

namespace zd23_Hojaeva
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shop Shop = new Shop();
        private decimal selectedProductPrice = 0;
        private Playlist playlist = new Playlist();
        public MainWindow()
        {
            InitializeComponent();
            //несколько стартовых товаров, чтобы магазин не был пустым
            Shop.CreateProduct("Молоко", 80, 10);
            Shop.CreateProduct("Хлеб", 45, 15);
            Shop.CreateProduct("Шоколад", 120, 5);
            //несколько стартовых песен, чтобы плейлист не был пустым
            playlist.AddTrack("Bush", "Comedown", "bush2.mp3");
            playlist.AddTrack("Metallica", "Until It Sleep", "metl1.mp3");
            playlist.AddTrack("Foo Fighters", "Everlong", "FF1.mp3");
            playlist.AddTrack("Travis", "Closer", "travis2.mp3");
            playlist.AddTrack("Linkin Park", "Crawling", "LP2.mp3");
            playlist.AddTrack("Design19", "Stairs", "d19.mp3");
            playlist.AddTrack("Massive Attack", "Karmacoma", "MA7.mp3");
            UpdateScreen();
            RefreshListSongs();
        }
        private void UpdateScreen()     //обновление списка товаров и прибыли
        {
            LstProducts.ItemsSource = Shop.GetProductsList();
            TxtTotalProfit.Text = $"{Shop.GetProfit()} руб.";
            try
            {
                Song current = playlist.CurrentSong();
                LblCurrentSong.Text = $"{current.Author} - {current.Title}";
            }
            catch(IndexOutOfRangeException)
            {
                LblCurrentSong.Text = "Плейлист пуст";
            }
        }
        private void RefreshListSongs()
        {
            LstPlaylistSongs.Items.Clear();
            List<Song> list = playlist.GetSongs();
            if (list.Count > 0)
            {
                foreach (Song song in list)
                {
                    LstPlaylistSongs.Items.Add($"{song.Author} - {song.Title}");
                }
            }
        }
        private void BtnSell_Click(object sender, RoutedEventArgs e) // кнопка Купить (Вкладка 1)
        {
            TxtStatusLog.Text = string.Empty;
            if (LstProducts.SelectedItem == null)
            {
                TxtStatusLog.Text = "Статус: Выберите товар в списке!";
                return;
            }
            if (!string.IsNullOrWhiteSpace(LblSelectedProduct.Text) && int.TryParse(TxtSellCount.Text, out int count))
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
            if (!string.IsNullOrWhiteSpace(TxtProductName.Text) && decimal.TryParse(TxtProductPrice.Text, out decimal price)
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
            PanelPlaylist.Visibility = Visibility.Collapsed;
            PanelSongs.Visibility = Visibility.Collapsed;
            UpdateScreen();
            TxtStatusLog.Text = string.Empty;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)       //выбор склада
        {
            PanelShop.Visibility = Visibility.Collapsed;
            PanelWarehouse.Visibility = Visibility.Visible;
            PanelPlaylist.Visibility = Visibility.Collapsed;
            PanelSongs.Visibility = Visibility.Collapsed;
            UpdateScreen();
            TxtTargetStatus.Text = string.Empty;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)       //выбор плейлиста
        {
            PanelShop.Visibility = Visibility.Collapsed;
            PanelWarehouse.Visibility = Visibility.Collapsed;
            PanelSongs.Visibility = Visibility.Collapsed;
            PanelPlaylist.Visibility = Visibility.Visible;
            UpdateScreen();
            RefreshListSongs();
            TxtStatusPlay.Text = string.Empty;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)       //выбор списка песен
        {
            PanelShop.Visibility = Visibility.Collapsed;
            PanelWarehouse.Visibility = Visibility.Collapsed;
            PanelPlaylist.Visibility = Visibility.Collapsed;
            PanelSongs.Visibility = Visibility.Visible;
            UpdateScreen();
            TxtStatusSongs.Text = string.Empty;
        }
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            playlist.ToStart();
            UpdateScreen();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            playlist.Previous();
            UpdateScreen();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            playlist.Next();
            UpdateScreen();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            playlist.Clear();
            UpdateScreen();
            RefreshListSongs();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            TxtStatusSongs.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(TxtAuthor.Text) && !string.IsNullOrWhiteSpace(Txtfilename.Text)
                && !string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                playlist.AddTrack(new Song(TxtAuthor.Text, TxtTitle.Text, Txtfilename.Text));
                TxtAuthor.Clear();
                TxtTitle.Clear();
                Txtfilename.Clear();
                UpdateScreen();
                TxtStatusSongs.Text = "Статус: Трек успешно добавлен в плейлист!";
            }
            else
            {
                TxtStatusSongs.Text = "Статус: Ошибка! Введите всю информации о песне для добавления";
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            TxtStatusSongs.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(TxtAuthor.Text) && !string.IsNullOrWhiteSpace(Txtfilename.Text)
                && !string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                Song song = new Song(TxtAuthor.Text, TxtTitle.Text, Txtfilename.Text);
                bool isDeleted = playlist.Remove(song);
                if (isDeleted)
                {
                    TxtAuthor.Clear();
                    TxtTitle.Clear();
                    Txtfilename.Clear();
                    UpdateScreen();
                    TxtStatusSongs.Text = "Статус: Трек успешно найден и удален!";
                }
                else
                {
                    TxtStatusSongs.Text = "Статус: Трек с такими данными не найден в плейлисте!";
                }
            }
            else
            {
                TxtStatusSongs.Text = "Статус: Ошибка! Введите всю информации о песне для удаления";
            }
        }

        private void BtnRemoveAt_Click(object sender, RoutedEventArgs e)
        {
            TxtStatusPlay.Text = string.Empty;
            if (LstPlaylistSongs.SelectedIndex != -1)
            {
                playlist.Remove(LstPlaylistSongs.SelectedIndex);
                UpdateScreen();
                RefreshListSongs();
            }
            else
            {
                TxtStatusPlay.Text = "Статус: Ошибка! Выберите песню из списка для удаления";
            }
        }

        private void LstPlaylistSongs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LstPlaylistSongs.SelectedIndex != -1)
            {
                playlist.GoTo(LstPlaylistSongs.SelectedIndex);
                UpdateScreen();
            }
        }

    }
}
