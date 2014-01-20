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

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return GetValue(".//PROJECT_NAME");
            }
        }

        [InsightlyColumnMapping]
        public string OrgId
        {
            get
            {
                return GetValue(".//Link[ORGANISATION_ID[text()]]/ORGANISATION_ID");
            }
        }
    }
}
