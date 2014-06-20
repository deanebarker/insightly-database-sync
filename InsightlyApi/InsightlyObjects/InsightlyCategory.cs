using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    [InsightlyTableMapping(ObjectName = "Category", TableName = "Categories")]
    public class InsightlyCategory : InsightlyObject
    {
        public InsightlyCategory(XmlElement xml) : base(xml)
        {

        }

        public override string Id
        {
            get { return GetValue(".//{0}CATEGORY_ID"); }
        }

        [InsightlyColumnMapping]
        public string Name
        {
            get { return GetValue(".//{0}CATEGORY_NAME"); }
        }
    }
}