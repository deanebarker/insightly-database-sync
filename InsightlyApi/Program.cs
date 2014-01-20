using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections;
using System.Configuration;
using InsightlyApi.InsightlyObjects;
using log4net;

namespace InsightlyApi
{
    internal class Program
    {
        public static SqlConnection Connection;
        public static ILog Log;
        private static Dictionary<string, int> Counter = new Dictionary<string,int>();

        private static void Main(string[] args)
        {
            try
            {
                int startTime = Environment.TickCount;
                Log = LogManager.GetLogger("root");
                Log.Debug("Start");

                // Get some basic config settings
                var apiKey = ConfigurationManager.AppSettings["InsightlyApiKey"];
                var url = ConfigurationManager.AppSettings["InsightlyUrl"];
                var toDownload = ConfigurationManager.AppSettings["ToDownload"].Split(',').Select(s => s.Trim()).ToArray();
                var resetFile = "reset.sql";

                // Open a Connection (all database interaction runs off this same Connection...)
                try
                {
                    Connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString);
                    Connection.Open();
                }
                catch (Exception e)
                {
                    Log.Error("Error opening database connection; " + e.Message + "; Connection string: " + ConfigurationManager.ConnectionStrings[1].ConnectionString);
                    throw;
                }

                // Load the reset file.
                var resetSql = String.Empty;
                try
                {
                    resetSql = File.ReadAllText(resetFile);
                    Log.Debug("Read " + resetSql.Length + " byte(s) from \"" + resetFile + "\"");
                }
                catch (Exception e)
                {
                    Log.Error("Error reading reset file: " + resetFile + "; " + e.Message);
                    throw;
                }

                // Run the reset SQL
                try
                {
                    var resetDatabase = new SqlCommand(resetSql);
                    resetDatabase.Connection = Connection;
                    resetDatabase.ExecuteNonQuery();
                    Log.Debug("Executed reset SQL.");
                }
                catch (Exception e)
                {
                    Log.Error("Error executing reset SQL; " + e.Message);
                    throw;
                }

                // Set up the client
                var encodedKey = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(apiKey));
                var client = new WebClient();
                client.Headers.Add("Authorization", "Basic " + encodedKey);
                client.Credentials = new NetworkCredential(encodedKey, String.Empty);

                // Loop through each object type we want to download
                foreach (var download in toDownload)
                {
                    // This header resets after every call, so it has to be reset each time...
                    client.Headers.Add("content-type", "text/xml");

                    // Get the XML
                    var xmlString = String.Empty;
                    try
                    {
                        xmlString = client.DownloadString(String.Concat(url, download));
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error downloading XML from " + String.Concat(url, download) + "; " + e.Message);
                    }

                    File.WriteAllText("data/" + download + ".xml", xmlString);

                    // Parse the XML
                    var xml = new XmlDocument();
                    try
                    {
                        xml.LoadXml(xmlString);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error parsing returned XML; " + e.Message);
                    }

                    // Loop through every top-level node in the XML, which each represents a single object
                    foreach (XmlElement objectNode in xml.SelectNodes("/*/*"))
                    {
                        if (Counter.ContainsKey(objectNode.Name))
                        {
                            Counter[objectNode.Name]++;
                        }
                        else
                        {
                            Counter.Add(objectNode.Name, 1);
                        }

                        // Do we have a custom type for this object?
                        var mappingType = Assembly.GetExecutingAssembly().GetTypes().Where(
                            t => t.GetCustomAttributes(typeof(InsightlyTableMappingAttribute), true).Where(
                                a => ((InsightlyTableMappingAttribute)a).ObjectName.ToLower() == objectNode.Name.ToLower()).Any()).
                                    FirstOrDefault();

                        InsightlyObject mappingObject;
                        if (mappingType != null)
                        {
                            // We have a custom type, instantiate it...
                            mappingObject = Activator.CreateInstance(mappingType, new object[] { objectNode }) as InsightlyObject;
                        }
                        else
                        {
                            // Just use the default object
                            mappingObject = new InsightlyObject(objectNode);
                        }

                        Log.Debug("Created object for " + objectNode.Name + "; ID: " + mappingObject.Id + "; Object Type: " + mappingObject.GetType().ToString());

                        // Save it
                        mappingObject.Save();
                    }
                }

                // Clean up
                Connection.Close();

                Log.Debug("End. " + Math.Floor((double)(Environment.TickCount - startTime) / 1000) + "s");

                foreach (var counter in Counter)
                {
                    Program.Log.Debug(counter.Key + ": " + counter.Value);
                }
            }
            catch (Exception e)
            {
                Program.Log.Equals("Fatal Exception: " + e.Message);
                Console.WriteLine("Fatal Exception: " + e.Message);
            }
        }
    }
}
