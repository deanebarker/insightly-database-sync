using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    [InsightlyTableMapping(ObjectName="Project", TableName="Projects")]
    class InsightlyProject : InsightlyObject
    {
        public InsightlyProject(XmlElement xml)
            : base(xml)
        {

        }

        public override string Id
        {
            get { return GetValue(".//{0}PROJECT_ID"); }
        }

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return GetValue(".//{0}PROJECT_NAME");
            }
        }

        [InsightlyColumnMapping]
        public string OrgId
        {
            get
            {
                return GetValue(".//{0}Link[{0}ORGANISATION_ID[text()]]/{0}ORGANISATION_ID");
            }
        }
    }
}
