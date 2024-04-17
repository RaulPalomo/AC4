using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AC4;
namespace AC4
{
    public class RecordCRUD
    {
        public static void DropTable()
        {
            using (var conn = new CloudConnection().GetConnection())
            {
                using (var cmd = new Npgsql.NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "DROP TABLE IF EXISTS record";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void CreateTable()
        {
            
            using (var conn = new CloudConnection().GetConnection())
            {
                using (var cmd = new Npgsql.NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS record (" +
                                      "Year NUMERIC(20)," +
                                      "CodiComarca INT," +
                                      "Comarca VARCHAR(50)," +
                                      "Poblacio INT," +
                                      "DomesticXarxa INT," +
                                      "ActivitatsEconomiques INT," +
                                      "Total INT," +
                                      "ConsumDomesticPerCapita FLOAT," +
                                      "CONSTRAINT PK_DatosComarca PRIMARY KEY (Year, CodiComarca)" +
                                      ")";
                    cmd.ExecuteNonQuery();
                }
            }

        }
        /*string query = "INSERT INTO \"Contact\" (\"FirstName\", \"LastName\") VALUES (@FirstName, @LastName)";
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", contact.Name);
                command.Parameters.AddWithValue("@LastName", contact.Surname);
                connection.Open();
                command.ExecuteNonQuery();*/
        public void InsertRecord(Record record)
        {
            
            using (var conn = new CloudConnection().GetConnection())
            {
                using (var cmd = new Npgsql.NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO record (Year, CodiComarca, Comarca, Poblacio, DomesticXarxa, ActivitatsEconomiques, Total, ConsumDomesticPerCapita) VALUES (@Year, @CodiComarca, @Comarca, @Poblacio, @DomesticXarxa, @ActivitatsEconomiques, @Total, @ConsumDomesticPerCapita)";
                    cmd.Parameters.AddWithValue("@Year", record.Any);
                    cmd.Parameters.AddWithValue("@CodiComarca", record.CodiComarca);
                    cmd.Parameters.AddWithValue("@Comarca", record.Comarca);
                    cmd.Parameters.AddWithValue("@Poblacio", record.Poblacio);
                    cmd.Parameters.AddWithValue("@DomesticXarxa", record.DomesticXarxa);
                    cmd.Parameters.AddWithValue("@ActivitatsEconomiques", record.ActivitatsEconomiques);
                    cmd.Parameters.AddWithValue("@Total", record.Total);
                    cmd.Parameters.AddWithValue("@ConsumDomesticPerCapita", record.ConsumDomesticPerCapita);
                    cmd.ExecuteNonQuery();
                }
            }

        }
        public void InsertAllRecords(List<Record> records)
        {
            foreach (var record in records)
            {
                InsertRecord(record);
            }
        }
    }
}
