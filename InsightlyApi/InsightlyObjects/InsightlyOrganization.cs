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

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return GetValue(".//ORGANISATION_NAME");
            }
        }

        [InsightlyColumnMapping]
        public string CityState
        {
            get
            {
                return String.Concat(GetValue(".//ADDRESSES/Address/CITY"), ", ", GetValue(".//ADDRESSES/Address/STATE")).Trim(new char[] { ' ', ',' });
            }
        }

        [InsightlyColumnMapping]
        public string Url
        {
            get
            {
                return GetValue(".//ContactInfo[TYPE='WEBSITE']/DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string ParentId
        {
            get
            {
                // Only send the ID back if it's different -- don't create self-referential links
                var parentId = GetValue(".//OrganisationLink[RELATIONSHIP_ID='7']/SECOND_ORGANISATION_ID");
                if (parentId != Id)
                {
                    return parentId;
                }

                return String.Empty;
            }
        }
    }
}
