using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static SqlConnection? _msConnection;

        public static SqlConnection msConnection
        {
            get
            {
                if(_msConnection == null)
                {
                    _msConnection = new(
                    JsonSerializer.Deserialize<JsonElement>(
                        System.IO.File.ReadAllText("appsettings.json")
                        ).GetProperty("ConnectionStrings")
                        .GetProperty("LocalMS")
                        .GetString()!
                    );
                    _msConnection.Open();
                     
                }
                return _msConnection;
            }
        }

        public static string md5(String input)
        {
            using var hasher = System.Security.Cryptography.MD5.Create();
            return Convert.ToHexString(hasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)));
        }
    }
}
