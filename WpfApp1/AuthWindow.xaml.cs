using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            
            var errorMessage = GetInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage,
                    "Виконання зупинене",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            using var cmd = new SqlCommand(
                $"INSERT INTO Users VALUES( NEWID(), @name, @login, '{App.md5(RegPassword.Password)}' )",
                App.msConnection);
            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
            {
                Value = RegName.Text
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = RegLogin.Text
            });
            try
            {
                cmd.Prepare();  // підготовка запиту - компіляція без параметрів
                cmd.ExecuteNonQuery();  // виконання - передача даних у скомпільований запит
                MessageBox.Show("Insert OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private String? GetInputError()
        {
            if (String.IsNullOrEmpty(RegName.Text))
            {
                return "Fill Name box";
            }
            if (String.IsNullOrEmpty(RegLogin.Text))
            {
                return "Fill Login box";
            }
            if (String.IsNullOrEmpty(RegPassword.Password))
            {
                return "Fill Password box";
            }
            if (String.IsNullOrEmpty( RegPasswordRepeat.Password))
            {
                return "Fill Repeat Password box";
            }
            return null;
        }

        private String? GetAuthInputError()
        {
            if (String.IsNullOrEmpty(AuthLogin.Text))
            {
                return "Fill Login Field";
            }
            if (String.IsNullOrEmpty(AuthPass.Password))
            {
                return "Fill Password Field";
            }
            return null;
        }

        private void LoginINButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = GetAuthInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage,
                   "Виконання зупинене",
                   MessageBoxButton.OK,
                   MessageBoxImage.Error);
                return;
            }
                //using var cmd = new SqlCommand(
                //    $"SELECT * FROM Users WHERE @name={}",
                //    App.msConnection);
                //cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
                //{
                //    Value = RegName.Text
                //});
                //cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
                //{
                //    Value = RegLogin.Text
                //});
                //try
                //{
                //    cmd.Prepare();  // підготовка запиту - компіляція без параметрів
                //    cmd.ExecuteNonQuery();  // виконання - передача даних у скомпільований запит
                //    MessageBox.Show("Insert OK");
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
        }

        /*
             Підключення різних ресурсів (вікон)
            Підключення до БД - достатньо складний ресурс і відкривати
            декілька підключень до одної БД - витрата ресурсів.
            = З одного боку, сама платформа .NET контролює підключення і
            при спробі 
        */
    }
}
