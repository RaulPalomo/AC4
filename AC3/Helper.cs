﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using AC4;
using System.Globalization;
using CsvHelper.Configuration;

namespace GestioAigua
{
    public class Helper
    {
        public static List<Record> Reader()
        {
            var records = new List<Dictionary<string, string>>();

            using (var reader = new StreamReader("../../../Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv"))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = new Dictionary<string, string>();

                    foreach (var header in csv.HeaderRecord)
                    {
                        var cleanedHeader = header.Replace(" ", "");
                        record[cleanedHeader] = csv.GetField(header);
                    }

                    records.Add(record);
                }
            }
            List<Record> recordsList = new List<Record>();
            foreach(var record in records)
            {
                Record r = new Record();
                r.Any = Convert.ToInt32(record["Any"]);
                r.CodiComarca = Convert.ToInt32(record["Codicomarca"]);
                r.Comarca = record["Comarca"];
                r.Poblacio = Convert.ToInt32(record["Població"]);
                r.DomesticXarxa = Convert.ToInt32(record["Domèsticxarxa"]);
                r.ActivitatsEconomiques = Convert.ToInt32(record["Activitatseconòmiquesifontspròpies"]);
                r.Total = Convert.ToInt32(record["Total"]);
                r.ConsumDomesticPerCapita = double.Parse(record["Consumdomèsticpercàpita"])/100;
                recordsList.Add(r);
            }
            return recordsList;
        }
        public static void Append(Record record)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using var stream = File.Open("../../../Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv", FileMode.Append);
            using var writer = new StreamWriter(stream);
            using var csvWriter = new CsvWriter(writer, config);
            var records = new List<Record>
            {
                record
            };
            csvWriter.WriteRecords(records);

        }
        public static void CsvToXml()
        {
            const string ConvertSuccess = "Csv convertido a Xml correctamente";

            var records = new List<Dictionary<string, string>>();

            using (var reader = new StreamReader("../../../Consum_d_aigua_a_Catalunya_per_comarques_20240402.csv"))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = new Dictionary<string, string>();

                    foreach (var header in csv.HeaderRecord)
                    {
                        var cleanedHeader = header.Replace(" ", "");
                        record[cleanedHeader] = csv.GetField(header);
                    }

                    records.Add(record);
                }
            }

            var root = new XElement("data");

            foreach (var record in records)
            {
                var recordElement = new XElement("record");

                foreach (var kvp in record)
                {
                    var elementName = kvp.Key;
                    elementName = Regex.Replace(elementName, @"[^\w.-]", "");


                    recordElement.Add(new XElement(elementName, kvp.Value));
                }

                root.Add(recordElement);
            }

            var xmlDocument = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            xmlDocument.Save("GestioAigua.xml");
            Console.WriteLine(ConvertSuccess);
        }
        public static Dictionary<int,string> Comarques()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GestioAigua.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("//record");
            Dictionary<int,string> list = new Dictionary<int,string>();
            foreach (XmlNode node in nodes)
            {
                foreach (XmlNode singleNode in node)
                {
                    if (singleNode.Name == "Comarca")
                    {

                        if (!list.ContainsKey(Convert.ToInt32(node.SelectSingleNode("Codicomarca").InnerText))) 
                        { 
                            list.Add(Convert.ToInt32(node.SelectSingleNode("Codicomarca").InnerText), singleNode.InnerText); 
                        }

                    }

                }
            }
            return list;
        }

        public static bool Repeticio(int any, int comarca)
        {
            //Comprova si ja existeix un registre amb el mateix any i comarca
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GestioAigua.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("//record");
            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode("Any").InnerText == any.ToString() && node.SelectSingleNode("Codicomarca").InnerText == comarca.ToString())
                {
                    return true;
                }
            }
            return false;
        }
        


        
        public static string CalcConsum(string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GestioAigua.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("//record");
            List<double>avg = new List<double>();
            foreach (XmlNode node in nodes)
            {

                if(node.SelectSingleNode("Comarca").InnerText == name)
                {
                    
                    XmlNode consum = node.SelectSingleNode("Consumdomèsticpercàpita");
                    double cValue = Convert.ToDouble(consum.InnerText);
                    avg.Add(cValue);
                    
                }
               
            }
            double result = avg.Average()/100;
            result=Math.Round(result, 2);
            return result.ToString();

        }
        
        public static string ConsDomesticBaix(string any,string comarca)
        {
            double bot = 600000;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GestioAigua.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("//record");
            
            foreach (XmlNode node in nodes)
            {
                XmlNode comarcaNode = node.SelectSingleNode("Comarca");
                XmlNode yearNode = node.SelectSingleNode("Any");
                XmlNode consum = node.SelectSingleNode("Consumdomèsticpercàpita");
                if (yearNode.InnerText == any)
                {
                    double cValue = Convert.ToDouble(consum.InnerText);
                    if (cValue < bot)
                    {
                        bot = cValue;
                        comarca = comarcaNode.InnerText;

                    }
                    
                        
                }
            }
            return comarca;

        }
        public static string ConsDomesticAlt(string any, string comarca)
        {
            double top = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("GestioAigua.xml");
            XmlNodeList nodes = xmlDoc.SelectNodes("//record");

            foreach (XmlNode node in nodes)
            {
                XmlNode comarcaNode = node.SelectSingleNode("Comarca");
                XmlNode yearNode = node.SelectSingleNode("Any");
                XmlNode consum = node.SelectSingleNode("Consumdomèsticpercàpita");
                if (yearNode.InnerText == any)
                {
                    double cValue = Convert.ToDouble(consum.InnerText);
                    if (cValue > top)
                    {
                        top = cValue;
                        comarca = comarcaNode.InnerText;

                    }


                }
            }
            return comarca;

        }
       
    }
}
