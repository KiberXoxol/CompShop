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

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string PATH = Convert.ToString(String.Join("\\", Environment.CurrentDirectory.ToString().Split('\\').Take(
           Environment.CurrentDirectory.ToString().Split('\\').Length - 2))) + "/Images/";
        public MainWindow()
        {
            InitializeComponent();
        }

        public void CallCardGenerator(object sender, RoutedEventArgs e)
        {

            CreateProductCard((sender as Button).Name);
        }

        private void CreateProductCard(string ProductName)
        {
            ProductGrid.Children.Clear();
            int cardIndex = 1;
            foreach (var item in new DataBaseConnection().displayItems(ProductName)) 
            {
                Console.WriteLine("count: " + cardIndex);
                Border ProductCard = generateInformationPlace(cardIndex);
                Grid grid = new Grid();
                foreach (var information in item.Value)
                {
                    switch (information.Key)
                    {
                        case ("Название"):
                            grid.Children.Add(generateProductTitle(information.Value));
                            break;
                        case ("Цена"):
                            grid.Children.Add(generateItemCost(information.Value));
                            break;
                        case ("Фотография"):
                            grid.Children.Add(getProductImage(information.Value));
                            break;
                    }
                }
                ProductCard.Child = grid;
                ProductGrid.Children.Add(ProductCard);
                cardIndex++;
            }
        }

        private Border generateInformationPlace(int index) 
        {
            Border informationPlace = new Border()
            {
                HorizontalAlignment = index%2==0?HorizontalAlignment.Right:HorizontalAlignment.Left,
                VerticalAlignment = index>2?VerticalAlignment.Bottom:VerticalAlignment.Top,
                Width = 220,
                Height = 220,
                Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#D9D9D9"),
                CornerRadius = new CornerRadius(10)
            };
            informationPlace.MouseDown += (sender, args) =>
            {
                Console.WriteLine(index);
            };
            return informationPlace;
        }

        public Image getProductImage(string productImageName)
        {
            Image productImage = new Image()
            {
                Source = new BitmapImage(
                new Uri($@"{PATH}{productImageName}")),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Height = 140,
                Width = 140,
                Margin = new Thickness(0, 20, 0, 0)
            };
            return productImage;
        }

        private TextBlock generateProductTitle(string Title)
        {
            TextBlock informationPlace = new TextBlock()
            {
                Text = Title,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Colors.Black)
            };
            return informationPlace;
        }

        private TextBlock generateItemCost(string Cost)
        {
            TextBlock informationPlace = new TextBlock()
            {
                Text = Cost + " руб",
                Margin = new Thickness(0,0,0,25),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(Colors.Black)
            };
            return informationPlace;
        }
    }
}
