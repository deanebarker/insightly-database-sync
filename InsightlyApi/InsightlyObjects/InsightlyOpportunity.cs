using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    [InsightlyTableMapping(ObjectName = "Opportunity", TableName = "Opportunities")]
    class InsightlyOpportunity : InsightlyObject
    {
        public InsightlyOpportunity(XmlElement xml)
            : base(xml)
        {

        }

        public override string Id
        {
            get { return GetValue(".//{0}OPPORTUNITY_ID"); }
        }

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return GetValue(".//{0}OPPORTUNITY_NAME");
            }
        }

        [InsightlyColumnMapping]
        public string State
        {
            get
            {
                return GetValue(".//{0}OPPORTUNITY_STATE").ToLower();
            }
        }

        [InsightlyColumnMapping]
        public string Details
        {
            get
            {
                return GetValue(".//{0}OPPORTUNITY_DETAILS");
            }
        }

        [InsightlyColumnMapping]
        public string Created
        {
            get
            {
                return GetValue(".//{0}DATE_CREATED_UTC");
            }
        }

        [InsightlyColumnMapping]
        public string Visibility
        {
            get
            {
                return GetValue(".//{0}VISIBLE_TO").ToLower();
            }
        }

    }
}
