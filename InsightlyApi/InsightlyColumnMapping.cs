using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InsightlyApi
{
    // Maps a property to a column in a SQL table
    [AttributeUsage(AttributeTargets.Property)]
    public class InsightlyColumnMappingAttribute : Attribute
    {
        // This is optional. If they don't set a column name, it just uses the name of the property itself
        public string ColumnName { get; set; }
    }
}
