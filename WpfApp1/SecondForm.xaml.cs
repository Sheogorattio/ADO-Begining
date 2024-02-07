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
using System.IO;
using System.Text.Json;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SecondForm.xaml
    /// </summary>
    public partial class SecondForm : Window
    {
        private readonly string _msConnectionString;
        private SqlConnection msConnection=null;
        public SecondForm()
        {
            InitializeComponent();
            var config = JsonSerializer.Deserialize<JsonElement>(
                         File.ReadAllText("appsettings.json"));
            var connectionStrings = config.GetProperty("ConnectionStrings");

            _msConnectionString = connectionStrings.GetProperty("LocalMS").GetString()!;
        }

        private void ConnectMSButton_Click(object sender, RoutedEventArgs e)
        {
            if (msConnection == null)
            {
                msConnection = new(_msConnectionString);
                try
                {
                    msConnection.Open();
                    MSConneectionStatusLabel.Content = "Connected";
                }
                catch (Exception ex)
                {
                    MSConneectionStatusLabel.Content = ex.Message;
                }
                CancelMsButton.IsEnabled = true;
                ConnectMsButton.IsEnabled = false;
            }
        }

        private void CancelMsButton_Click(object sender, RoutedEventArgs e)
        {
            if (msConnection != null)
            {
                try
                {
                    msConnection.Close();
                    MSConneectionStatusLabel.Content = "Disonnected";
                    msConnection.Dispose();
                    msConnection = null;
                }
                catch (Exception ex)
                {
                    MSConneectionStatusLabel.Content = ex.Message;
                }
                CancelMsButton.IsEnabled = false;
                ConnectMsButton.IsEnabled = true;
            }
        }
    }
}

/*
    ADO.NET Вступ
    ADO.NET - тезнологія доступу до данних, яка вводить єдиний інтерфейс
    для роботи з різними джерелами даних(з різними СУБД)
    - Сама БД: ПКМ (Project) - Add new item - Service-based database - OK
    -Параметри підключення. Зазвичай їх закладють в окремий файл конфігурацій.
        (appsettings.json)
    -Драйвер підключення  - додаткові бібліотеки (NuGet), які містять інстурменти
        для з'єднаяння з відповідною СУБД
        SQL Server - System.Data.SqlClient;
        MySQL/MariaDB -- MySql.Data.MySqlClient;
 */
