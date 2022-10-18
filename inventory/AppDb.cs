using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace inventory
{
    public class AppDb
    {
        public MySqlConnection Connection { get; }

        public AppDb()
        {
            Connection = new MySqlConnection(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["Db"]);
        }

        public void Dispose() => Connection.Dispose();
    }
}
