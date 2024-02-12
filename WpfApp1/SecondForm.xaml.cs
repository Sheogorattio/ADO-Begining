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
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SecondForm.xaml
    /// </summary>
    public partial class SecondForm : Window
    {
        private readonly string _msConnectionString;
        private SqlConnection? msConnection=null;
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
                if (DoesTableExists("Users"))
                {
                    DeleteMSButton.IsEnabled = true;
                }
                else
                {
                    CreateMSButton.IsEnabled = true;
                }
            }
        }

        private bool DoesTableExists(string TableName)
        {
            return msConnection?.GetSchema("TABLES", //проверка существует ли таблица
                    new string[] { null, null, TableName }).Rows.Count > 0;
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

        private void CreateMSButton_Click(object sender, RoutedEventArgs e)
        {
            if(msConnection!=null)
            {
                if (!DoesTableExists("Users"))
                {
                    //виконання SQL запитів
                    //загальна схема
                    using SqlCommand cmd = new(); //інструмент передачі команди (SQL)
                    cmd.Connection = msConnection;
                    cmd.CommandText = @"
                CREATE TABLE Users(
                    ID           uniqueidentifier   primary key,
                    Name         nvarchar(64)       not null,
                    Login        nvarchar(64)       not null,
                    BirthDate    date               not null,
                    PasswordHash char(32)           not null
                )
                ";
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MSConneectionStatusLabel.Content = "Execute OK";
                        this.DeleteMSButton.IsEnabled = true;
                        this.CreateMSButton.IsEnabled = false;
                    }
                    catch (Exception ex)
                    {
                        MSCreateStatusLabel.Content = ex.Message;
                    }
                }
                else
                {
                    MSConneectionStatusLabel.Content = "Table already exists.";
                }
            }
        }

        private String? GetInputError()
        {
            if (String.IsNullOrEmpty(UserNameTextBox.Text))
            {
                return "Fill Name box";
            }
            if (String.IsNullOrEmpty(UserLoginTextBox.Text))
            {
                return "Fill Name box";
            }
            if (String.IsNullOrEmpty(UserPasswordTextBox.Password))
            {
                return "Fill Name box";
            }
            return null;
        }

        private void AddUserMSButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var errorMessage = GetInputError();
                if (errorMessage != null)
                {
                    MessageBox.Show(errorMessage);
                    return;
                }
                using var cmd = new SqlCommand(
                    $"insert into Users values(NEWID(), N'{UserNameTextBox.Text}', N'{UserLoginTextBox.Text}', N'{BirthDateTextBox.SelectedDate}' ,'{md5(UserPasswordTextBox.Password)}')", msConnection);
                cmd.ExecuteNonQuery();
                MSAddUserStatusLabel.Content = "Insert OK";
            }
            catch(Exception ex)
            {
                MSAddUserStatusLabel.Content = ex.Message;
            }
        }

        private string md5(String input)
        {
            using var hasher = System.Security.Cryptography.MD5.Create();
            return Convert.ToHexString(hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)));
        }

        private void SelectMSButton_Click(object sender, RoutedEventArgs e)
        {
            if(msConnection == null ||
               msConnection.State == System.Data.ConnectionState.Closed)
            {
                MessageBox.Show("Необхідно встановити підключення", "Виконання зупинено", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using SqlCommand cmd = new("Select * from Users", msConnection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader(); //виконання команд з поверненням
                /*
                    Reader -  відображення таблиці (з довільною кількістю полів)
                Його схема робои: читання даних по одному рядку.
                Метод .Read() передає рядок даних у сам об'єкт reader,
                після чого дані полів рядку достуні
                    а) За ключем reader["id"] (-> object)
                    б) за допомогою гетерів raader.GetGuid("id") (-> Guid)
                Для переходу на наступний рядок знов подається команда .Read
                Коли дані закінчаться, виклик .Read поврне false
                */
                SelectMsTextBlock.Text = "";
                while (reader.Read())
                {
                    var id = reader.GetGuid("Id");
                    var name = reader.GetString("Name");
                    var login = reader.GetString("Login");
                    var birthDate = reader.GetString("BirthDate");
                    var hash = reader.GetString("PasswordHash");
                    SelectMsTextBlock.Text += $"{id.ToString()[..5]}... {name} {login} {birthDate} {hash[..5]}...\n";
                }
            }
            catch (Exception ex )
            {
                SelectMsTextBlock.Text = ex.Message;
            }
        }

        private void DeleteMSButton_Click(object sender, RoutedEventArgs e)
        {
            if(msConnection != null)
            {
                if (DoesTableExists("Users"))
                {
                    try
                    {
                        using var cmd = new SqlCommand("DROP TABLE Users", msConnection);
                        cmd.BeginExecuteNonQuery();
                        MSAddUserStatusLabel.Content = "Delete OK";
                        this.DeleteMSButton.IsEnabled = false;
                        this.CreateMSButton.IsEnabled=true;
                    }
                    catch(Exception ex)
                    {
                        MSCreateStatusLabel.Content = ex.Message;
                    }
                }
                else
                {
                    MSConneectionStatusLabel.Content = "Table already does not exist.";
                }
            }
        }
    }
}

/*
    ADO.NET Вступ
    ADO.NET - теxнологія доступу до данних, яка вводить єдиний інтерфейс
    для роботи з різними джерелами даних(з різними СУБД)
    - Сама БД: ПКМ (Project) - Add new item - Service-based database - OK
    -Параметри підключення. Зазвичай їх закладють в окремий файл конфігурацій.
        (appsettings.json)
    -Драйвер підключення  - додаткові бібліотеки (NuGet), які містять інстурменти
        для з'єднаяння з відповідною СУБД
        SQL Server - System.Data.SqlClient;
        MySQL/MariaDB -- MySql.Data.MySqlClient;
 */
