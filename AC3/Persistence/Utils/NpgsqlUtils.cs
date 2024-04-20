using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace AC4.Persistence.Utils
{
    public class NpgsqlUtils
    {
        public static string OpenConnection()
        {
            // Carregar la cadena de connexió a la base de dades des de l'arxiu de configuració
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return config.GetConnectionString("MyPostgresConn");
        }

        
    }
}
