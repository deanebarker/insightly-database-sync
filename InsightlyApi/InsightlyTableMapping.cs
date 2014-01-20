using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InsightlyApi
{
    // Maps a class to an Insightly object, so as to insert data in a SQL table
    [AttributeUsage(AttributeTargets.Class)]
    public class InsightlyTableMappingAttribute : Attribute
    {
        // The name of the Insightly object type this class is for
        public string ObjectName { get; set; }

        // The name of the SQL table we're inserting into
        public string TableName { get; set; }
    }
}
