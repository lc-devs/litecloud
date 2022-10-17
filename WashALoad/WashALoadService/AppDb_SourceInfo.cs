using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService
{
    public class AppDb_SourceInfo
    {
        public MySqlConnection Connection { get; }

        public AppDb_SourceInfo()
        {
            Connection = new MySqlConnection(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["sourceinfo"]);
        }

        public void Dispose() => Connection.Dispose();
    }
}
