using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DAL.DTO
{
    public class User
    {
        public Guid     Id              { get; set; }
        public String   Name            { get; set; } = null!;
        public String Login           { get; set; } = null!;
        public String PasswordHash    { get; set; } = null!;
        public DateTime? BirthDate       { get; set; }

        public User() { }

        public User(DbDataReader reader) 
        {
            ArgumentNullException.ThrowIfNull(reader);

            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            Login = reader.GetString("Login");
            BirthDate = reader.IsDBNull("BirthDate") ?
                null : reader.GetDateTime("BirthDate");
            PasswordHash = reader.GetString("PasswordHash");
        }
    }


}
