using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.DAL.DTO;

namespace WpfApp1.DAL.DAO
{
    internal class UserDao
    {
        public static List<User> GetAll()
        {
            using SqlCommand cmd = new(
                "Select * from Users", 
                App.msConnection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader(); //виконання команд з поверненням

                List<User> users = new();
                while (reader.Read())
                {
                    users.Add(new(reader));
                }
                return users;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public static Boolean AddUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            try
            {
                using var cmd = new SqlCommand(
                    $"insert into Users values(NEWID(), @name, @login, @birthDate , @passHash)", App.msConnection);
                cmd.Parameters.Add(new SqlParameter("@birthDate", System.Data.SqlDbType.DateTime)
                {
                    Value = user.BirthDate
                }); 
                cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar,64)
                {
                    Value = user.Name
                });
                cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
                {
                    Value = user.Login
                });
                cmd.Parameters.Add(new SqlParameter("@passHash", System.Data.SqlDbType.Char, 32)
                {
                    Value = user.PasswordHash
                }) ;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static User? GetUserByCredentials(String login, String password)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);

            using var cmd = new SqlCommand(
                "select * from Users u where u.login = @login", App.msConnection);
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = login
            });
            try
            {
                cmd.Prepare();
                var reader = cmd.ExecuteReader();
                if (reader.Read())      
                {
                    MessageBox.Show("User is found");
                    var user =  new User(reader);
                    if (user.PasswordHash != password) MessageBox.Show("Login or password is wrong!");
                    else MessageBox.Show("Authentification is successful");
                    return user;
                }
                else // user не знайдений
                {
                    MessageBox.Show("Login or password is wrong! (Login)");
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

                
        }
    }
}
