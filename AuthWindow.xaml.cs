using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public DataBaseConnection databaseConnection = new DataBaseConnection();
        public AuthWindow()
        {
            InitializeComponent();
            callTable();
        }

        public void callTable()
        {
            databaseConnection.readDatabase();
        }

        public void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            bool checkData = true;
            if (!controlTelephoneNumbers(PNumber_inputTextBox.Text.ToString()))
            {
                checkData = false;
            }

            if (Password_inputTextBox.Text.Length == 0)
            {
                checkData = false;
            }
            if (!checkData)
            {
                MessageBox.Show("Ошибка вводных данных", "Тех Поддержка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                //Регистрация
                Dictionary<string, string> RegInfo = new Dictionary<string, string>()
                {
                 
                    {"Телефон", PNumber_inputTextBox.Text.ToString()},
        
                    {"Пароль", Password_inputTextBox.Text.ToString()},
                };

                if (new DataBaseConnection().UserAuth(RegInfo))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
               
               
            }
            
        }

        public void blackGren(object sender, RoutedEventArgs e)
        {
            
            btn.Background = new SolidColorBrush(Colors.DarkGreen);
        }

        public void Green(object sender, RoutedEventArgs e)
        {

            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#759242");
        }

        public void RegButton_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regWindow = new RegWindow();
            regWindow.Show();
            Close();
        }

        public bool controlTelephoneNumbers(string telephoneNumberData)
        {
            Regex outPlusPhoneNumberController = new Regex(
                "[8][0-9]{10}");

            Console.WriteLine("Results 1: " + outPlusPhoneNumberController.IsMatch(telephoneNumberData));
            return outPlusPhoneNumberController.IsMatch(telephoneNumberData);
        }
    }
}
