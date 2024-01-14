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
using System.Xml.Linq;

namespace HotelManagementApp
{
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Log_in_Click(object sender, RoutedEventArgs e)
        {
            string enteredLogin = TextBoxLogin.Text;
            string enteredPassword = PasswordBox.Password;

            if (AuthenticateUser(enteredLogin, enteredPassword))
            {
                MessageBox.Show("Аутентификация успешна!");
                // Добавьте код для перехода к главной форме или другим действиям
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Закрытие текущей формы (LoginForm)
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка аутентификации. Проверьте логин и пароль.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string enteredLogin = TextBoxLogin.Text;
            string enteredPassword = PasswordBox.Password;

            // Регистрация нового пользователя
            RegisterUser(enteredLogin, enteredPassword);
        }

        private bool AuthenticateUser(string login, string password)
        {
            string xmlFilePath = "../../../Users.xml"; // Укажите путь к вашему XML-файлу

            try
            {
                // Загрузка XML-документа
                XDocument xmlDoc = XDocument.Load(xmlFilePath);

                // Поиск пользователя в XML-файле
                var user = xmlDoc.Descendants("user")
                                .FirstOrDefault(u => u.Element("login").Value == login && u.Element("password").Value == password);

                return user != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке XML: {ex.Message}");
                return false;
            }
        }

        private void RegisterUser(string login, string password)
        {
            string xmlFilePath = "../../../Users.xml"; // Укажите путь к вашему XML-файлу

            try
            {
                // Загрузка XML-документа
                XDocument xmlDoc = XDocument.Load(xmlFilePath);

                // Проверка, что пользователь с таким логином еще не существует
                if (xmlDoc.Descendants("user").Any(u => u.Element("login").Value == login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует. Выберите другой логин.");
                    return;
                }

                // Добавление нового пользователя в XML-файл
                XElement newUser = new XElement("user",
                    new XElement("login", login),
                    new XElement("password", password)
                );

                xmlDoc.Element("users").Add(newUser); // Предполагаем, что корневой элемент называется "users"

                // Сохранение изменений
                xmlDoc.Save(xmlFilePath);

                MessageBox.Show("Регистрация успешна!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке/сохранении XML: {ex.Message}");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
