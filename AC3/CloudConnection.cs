using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace AC4
{
    public class CloudConnection
    {
        private String HOST = "\thorton.db.elephantsql.com:5432"; // Ubicació de la BD.
        private String DB = "njzrzuxw"; // Nom de la BD.
        private String USER = "njzrzuxw";
        private String PASSWORD = "OsfDH82-y_XVVxIekWLXxAT02Hs-KLes";

        public NpgsqlConnection GetConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(
                "Host=" + HOST + ";" + "Username=" + USER + ";" +
                "Password=" + PASSWORD + ";" + "Database=" + DB + ";"
            );
            conn.Open();
            return conn;
        }
    }
}
