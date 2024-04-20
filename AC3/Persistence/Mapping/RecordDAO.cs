using AC4.Persistence.Utils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AC4.Persistence.Mapping.RecordDAO;
using AC4.Persistence.DAO;

namespace AC4.Persistence.Mapping
{
    public class RecordDAO : IRecordDAO
    {
       
            private readonly string connectionString;

            public RecordDAO(string connectionString)
            {
                this.connectionString = connectionString;
            }


            public void AddRecord(Record record)
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    string query = "INSERT INTO \"record\" (\"year\", \"codicomarca\", \"comarca\", \"poblacio\", \"domesticxarxa\", \"activitatseconomiques\", \"total\", \"consumdomesticpercapita\") VALUES (@year, @codi, @comarca, @poblacio, @domestic,@act,@total,@consum)";
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@year", record.Any);
                    command.Parameters.AddWithValue("@codi", record.CodiComarca);
                    
                    command.Parameters.AddWithValue("@comarca", record.Comarca);
                    command.Parameters.AddWithValue("@poblacio", record.Poblacio);
                    command.Parameters.AddWithValue("@domestic", record.DomesticXarxa);
                    command.Parameters.AddWithValue("@act", record.ActivitatsEconomiques);
                    command.Parameters.AddWithValue("@total", record.Total);
                    command.Parameters.AddWithValue("@consum", record.ConsumDomesticPerCapita);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }  

        
    }
}
