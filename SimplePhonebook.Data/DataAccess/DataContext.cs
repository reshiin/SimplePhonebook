using SimplePhonebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext() : base(ConnectionString) { }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ContactNumber> ContactNumbers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }

        private static string ConnectionString
        {
            get
            {
                return new SqlConnectionStringBuilder()
                {
                    DataSource = SQLServerName,
                    InitialCatalog = SQLDatabase,
                    UserID = SQLUserName,
                    Password = SQLPassword,
                    ConnectTimeout = SQLConnectTimeout
                }.ConnectionString;
            }
        }

        private static string SQLServerName
        {
            get { return ConfigurationManager.AppSettings["SQLServerName"] ?? ""; }
        }

        private static string SQLDatabase
        {
            get { return ConfigurationManager.AppSettings["SQLDatabase"] ?? ""; }
        }

        private static string SQLUserName
        {
            get { return ConfigurationManager.AppSettings["SQLUserName"] ?? ""; }
        }

        private static string SQLPassword
        {
            get { return ConfigurationManager.AppSettings["SQLPassword"] ?? ""; }
        }

        private static int SQLConnectTimeout
        {
            get
            {
                var timeout = ConfigurationManager.AppSettings["SQLConnectTimeout"] ?? "";
                return timeout == "" ? 15 : Convert.ToInt16(timeout);
            }
        }
    }
}
