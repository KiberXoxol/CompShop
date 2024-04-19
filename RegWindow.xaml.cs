using System;
using ComputerShop.Properties;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        public void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            Close();
        }

        public void RegUser(object sender, RoutedEventArgs e)
        {
            // Имя Фамилия Телефон УкрПошта Пароль
            bool checkData = true;
            if (!controlTelephoneNumbers(PNumber_inputTextBox.Text.ToString()))
            {
                checkData = false;
            }

            if (!IsValidEmail(Email_inputTextBox.Text.ToString())) 
            {
                checkData = false;
            }

            if (FirstName_inputTextBox.Text.Length == 0)
            {
                checkData = false;
            }

            if (SecName_inputTextBox.Text.Length == 0)
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
                    {"Имя", FirstName_inputTextBox.Text.ToString()},
                    {"Фамилия", SecName_inputTextBox.Text.ToString()},
                    {"Телефон", PNumber_inputTextBox.Text.ToString()},
                    {"Почта",Email_inputTextBox.Text.ToString()},
                    {"Пароль", Password_inputTextBox.Text.ToString()},
                };

                new DataBaseConnection().UserRegistration(RegInfo);
            }
        }

        // Проверка номера телефона
        public bool controlTelephoneNumbers(string telephoneNumberData)
        {
            Regex outPlusPhoneNumberController = new Regex(
                "[8][0-9]{10}");

            Console.WriteLine("Results 1: " + outPlusPhoneNumberController.IsMatch(telephoneNumberData));
            return outPlusPhoneNumberController.IsMatch(telephoneNumberData);
        }

        // Проверка почты
        public bool IsValidEmail(string email)
        {

            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
