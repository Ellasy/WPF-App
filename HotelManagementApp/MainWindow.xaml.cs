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
using System.Linq;
using System.Xml.Linq;

namespace HotelManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public IEnumerable<XElement> GetClientInfoByName(string clientName)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("../../../Clients.xml");
                return xmlDoc.Descendants("client")
                             .Where(c => c.Element("name").Value == clientName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке XML: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<XElement> GetInvestmentsForClient(int clientCode)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("../../../Investments.xml");
                return xmlDoc.Descendants("investment")
                             .Where(i => (int)i.Element("clientCode") == clientCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке XML: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<XElement> GetInvestmentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("../../../Investments.xml");
                return xmlDoc.Descendants("investment")
                             .Where(i =>
                                 DateTime.Parse(i.Element("purchaseDate").Value) >= startDate &&
                                 DateTime.Parse(i.Element("purchaseDate").Value) <= endDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке XML: {ex.Message}");
                return null;
            }
        }


        public IEnumerable<XElement> GetAllSecurities()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("../../../Securities.xml");
                return xmlDoc.Descendants("security");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке XML: {ex.Message}");
                return null;
            }
        }

        public IEnumerable<XElement> GetSecuritiesByRating(string rating)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("../../../Securities.xml");
                return xmlDoc.Descendants("security")
                         .Where(s => s.Element("rating").Value == rating);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке XML: {ex.Message}");
                return null;
            }
        }

        private void ShowAllSecurities_Click(object sender, RoutedEventArgs e)
        {
            var securities = GetAllSecurities();
            DisplayResults(securities);
        }

        private void ShowInvestmentsForClient_Click(object sender, RoutedEventArgs e)
        {
            var investments = GetInvestmentsForClient(201); // Предположим, что код клиента 201
            DisplayResults(investments);
        }

        private void ShowClientInfoByName_Click(object sender, RoutedEventArgs e)
        {
            string clientName = txtClientName.Text; // Получаем значение из TextBox
            var clientInfo = GetClientInfoByName(clientName);
            DisplayResults(clientInfo);
        }

        private void ShowInvestmentsByDateRange_Click(object sender, RoutedEventArgs e)
        {
            // Получаем значения из DatePicker'ов
            DateTime startDate = dpStartDate.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = dpEndDate.SelectedDate ?? DateTime.MaxValue;

            var investments = GetInvestmentsByDateRange(startDate, endDate);
            DisplayResults(investments);
        }

        private void ShowSecuritiesByRating_Click(object sender, RoutedEventArgs e)
        {
            string rating = txtRating.Text; // Получаем значение из TextBox
            var securitiesByRating = GetSecuritiesByRating(rating);
            DisplayResults(securitiesByRating);
        }

        private void DisplayResults(IEnumerable<XElement> results)
        {
            NoDataMessage.Visibility = results == null || !results.Any() ? Visibility.Visible : Visibility.Collapsed;

            DataGridResults.Items.Clear(); // Очищаем DataGridResults

            if (results == null || !results.Any())
                return;

            DataGridResults.Columns.Clear();

            var firstResult = results.First();
            foreach (var element in firstResult.Elements())
            {
                var column = new DataGridTextColumn
                {
                    Header = GetRussianColumnName(element.Name.LocalName),
                    Binding = new Binding($"[{element.Name.LocalName}]")
                };
                DataGridResults.Columns.Add(column);
            }

            foreach (var result in results)
            {
                var values = result.Elements().ToDictionary(e => e.Name.LocalName, e => e.Value);
                DataGridResults.Items.Add(values);
            }
        }

        private string GetRussianColumnName(string englishColumnName)
        {
            // Перевод английских названий в русские
            switch (englishColumnName)
            {
                case "securityCode": return "Код ценной бумаги";
                case "minTransactionAmount": return "Минимальная сумма сделки";
                case "rating": return "Рейтинг";
                case "yieldLastYear": return "Доходность за прошлый год";
                case "additionalInfo": return "Дополнительная информация";
                case "investmentCode": return "Код инвестиции";
                case "quotation": return "Котировка";
                case "purchaseDate": return "Дата покупки";
                case "saleDate": return "Дата продажи";
                case "clientCode": return "Код клиента";
                case "name": return "Название";
                case "ownershipType": return "Вид собственности";
                case "address": return "Адрес";
                case "phone": return "Телефон";
                // Добавьте другие названия по аналогии
                default: return englishColumnName;
            }
        }

        private void DataGridResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}