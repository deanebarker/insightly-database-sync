using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    public abstract class InsightlyObject
    {
        protected XmlDocument xml { get; set; }
        protected XmlNamespaceManager nsm { get; set; }

        public abstract string Id { get; }

        public InsightlyObject(XmlElement incomingXml)
        {
            xml = new XmlDocument();
            xml.LoadXml(incomingXml.OuterXml);

            nsm = new XmlNamespaceManager(xml.NameTable);
            nsm.AddNamespace("insightly", "http://schemas.datacontract.org/2004/07/Insightly.API");
        }

        protected string GetValue(string xPath)
        {
            xPath = String.Format(xPath, "insightly:");

            try
            {
                if (xml.SelectSingleNode(xPath, nsm) != null)
                {
                    return xml.SelectSingleNode(xPath, nsm).InnerText;
                }
            }
            catch (Exception e)
            {
                Program.Log.Error("Error attempting to execute xpath \"" + xPath + "\" on ID: " + Id + "; " + e.Message);
            }

            return String.Empty;
        }

        public void Save()
        {
            Program.Log.Debug("Beginning save of ID: " + Id);

            // Insert the ID and the raw XML in the master table
            try
            {
                // Insert the object record if it doesn't exist
                var checkForObjectRecord = new SqlCommand();
                checkForObjectRecord.Connection = Program.Connection;
                checkForObjectRecord.CommandText = "SELECT COUNT(*) FROM Objects WHERE Id = @Id";
                checkForObjectRecord.Parameters.AddWithValue("Id", Id);
                if (!Convert.ToBoolean(checkForObjectRecord.ExecuteScalar()))
                {
                    var insertMasterObjectRecord = new SqlCommand();
                    insertMasterObjectRecord.Connection = Program.Connection;
                    insertMasterObjectRecord.CommandText = "INSERT INTO Objects (Id, Type) VALUES (@Id, @Type)";
                    insertMasterObjectRecord.Parameters.AddWithValue("Id", Id);
                    insertMasterObjectRecord.Parameters.AddWithValue("Type", xml.Name);
                    insertMasterObjectRecord.ExecuteNonQuery();
                    Program.Log.Debug("Inserted master record for ID: " + Id);
                }
                else
                {
                    Program.Log.Debug("Master record already exists for ID: " + Id);
                }
            }
            catch (Exception e)
            {
                Program.Log.Error("Error inserting master record for ID: " + Id + "; " + e.Message);
            }

            // Insert the tags
            foreach (XmlNode tagNode in xml.SelectNodes(".//insightly:TAGS/insightly:Tag/insightly:TAG_NAME", nsm))
            {
                try
                {
                    var insertTagRecord = new SqlCommand();
                    insertTagRecord.Connection = Program.Connection;
                    insertTagRecord.CommandText = "INSERT INTO Tags (ObjectId, Tag) VALUES (@Id, @Tag)";
                    insertTagRecord.Parameters.AddWithValue("Id", Id);
                    insertTagRecord.Parameters.AddWithValue("Tag", tagNode.InnerText);
                    insertTagRecord.ExecuteNonQuery();

                    Program.Log.Debug("Inserted tag record for ID: " + Id + "; Tag: " + tagNode.InnerText);
                }
                catch (Exception e)
                {
                    Program.Log.Error("Error inserting tag record for ID: " + Id + "; " + e.Message);
                }
            }

            // Is this class mapped to a table?
            if (GetType().GetCustomAttributes(typeof(InsightlyTableMappingAttribute), true).Any())
            {
                // Get the name of the table we're mapping to off the attribute
                var tableName = ((InsightlyTableMappingAttribute)GetType().GetCustomAttributes(typeof(InsightlyTableMappingAttribute), true).FirstOrDefault()).TableName;

                // Insert just the ID, leaving all other columns null
                try
                {
                    var insertIndividualObjectRecord = new SqlCommand();
                    insertIndividualObjectRecord.Connection = Program.Connection;
                    insertIndividualObjectRecord.CommandText = "INSERT INTO " + tableName + " (Id) VALUES (@Id)";
                    insertIndividualObjectRecord.Parameters.AddWithValue("Id", Id);
                    insertIndividualObjectRecord.ExecuteNonQuery();

                    Program.Log.Debug("Inserted individual record into \"" + tableName + "\" for ID: " + Id);
                }
                catch (Exception e)
                {
                    Program.Log.Error("Error inserting individual record into \"" + tableName + "\" for ID: " + Id + "; " + e.Message);
                }

                // Get any properties off this object that are mapped to columns
                foreach (var property in GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(InsightlyColumnMappingAttribute), true).Any()))
                {
                    // Get the column name off the attribute, or -- if it's not set -- just use the property name itself as the column name
                    var columnName = ((InsightlyColumnMappingAttribute)property.GetCustomAttributes(typeof(InsightlyColumnMappingAttribute), true).First()).ColumnName ?? property.Name;

                    // Update the record
                    try
                    {
                        // Reflect the value off this property
                        var fieldValue = (string)property.GetValue(this, null);

                        // If it has no value, leave it a null...
                        if (String.IsNullOrWhiteSpace(fieldValue))
                        {
                            Program.Log.Debug("Property had no value for ID: " + Id + "; " + property.Name);
                            continue;
                        }

                        var updateIndividualObjectRecord = new SqlCommand();
                        updateIndividualObjectRecord.Connection = Program.Connection;
                        updateIndividualObjectRecord.CommandText = "UPDATE " + tableName + " SET " + columnName + " = @FieldValue WHERE Id = @Id";
                        updateIndividualObjectRecord.Parameters.AddWithValue("Id", Id);
                        updateIndividualObjectRecord.Parameters.AddWithValue("FieldValue", fieldValue);
                        updateIndividualObjectRecord.ExecuteNonQuery();

                        Program.Log.Debug("Updated individual field for ID: " + Id + "; Property Name: " + property.Name);
                    }
                    catch (Exception e)
                    {
                        Program.Log.Error("Error updating individual field for ID: " + Id + "; Property Name: " + property.Name + "; " + e.Message);
                    }

                }

            }

            Program.Log.Debug("Ending save of ID: " + Id);
        }
    }
}
