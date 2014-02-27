using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    [InsightlyTableMapping(ObjectName="Organisation", TableName="Organizations")]
    public class InsightlyOrganization : InsightlyObject
    {
        public InsightlyOrganization(XmlElement xml) : base(xml)
        {

        }

        public override string Id
        {
            get { return GetValue(".//{0}ORGANISATION_ID"); }
        }

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return GetValue(".//{0}ORGANISATION_NAME");
            }
        }

        [InsightlyColumnMapping]
        public string CityState
        {
            get
            {
                return String.Concat(GetValue(".//{0}ADDRESSES/{0}Address/{0}CITY"), ", ", GetValue(".//{0}ADDRESSES/{0}Address/{0}STATE")).Trim(new char[] { ' ', ',' });
            }
        }

        [InsightlyColumnMapping]
        public string Url
        {
            get
            {
                return GetValue(".//{0}ContactInfo[{0}TYPE='WEBSITE']/{0}DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string ParentId
        {
            get
            {
                // Only send the ID back if it's different -- don't create self-referential links
                var parentId = GetValue(".//{0}OrganisationLink[{0}RELATIONSHIP_ID='7']/{0}SECOND_ORGANISATION_ID");
                if (parentId != Id)
                {
                    return parentId;
                }

                return String.Empty;
            }
        }
    }
}
