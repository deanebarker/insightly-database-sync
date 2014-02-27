using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    [InsightlyTableMapping(ObjectName = "Contact", TableName = "Contacts")]
    public class InsightlyContact : InsightlyObject
    {
        public InsightlyContact(XmlElement xml) : base(xml)
        {
        }

        public override string Id
        {
            get { return GetValue(".//insightly:CONTACT_ID"); }
        }

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return String.Concat(GetValue(".//{0}FIRST_NAME"), " ", GetValue(".//{0}LAST_NAME")).Trim();
            }
        }

        [InsightlyColumnMapping]
        public string Email
        {
            get
            {
                return GetValue(".//{0}CONTACTINFOS/{0}ContactInfo[{0}TYPE='EMAIL']/{0}DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string OrgId
        {
            get
            {
                return GetValue(".//{0}DEFAULT_LINKED_ORGANISATION");
            }
        }

        [InsightlyColumnMapping]
        public string Title
        {
            get
            {
                return GetValue(".//{0}Link[{0}ORGANISATION_ID[text()] and {0}ROLE[text()]]/{0}ROLE");
            }
        }

        [InsightlyColumnMapping]
        public string LinkedInUrl
        {
            get
            {
                return GetValue(".//{0}ContactInfo[{0}SUBTYPE = 'LinkedInPublicProfileUrl']/{0}DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string WorkPhone
        {
            get
            {
                return GetValue(".//{0}ContactInfo[{0}TYPE='PHONE' and {0}LABEL='Work']/{0}DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string MobilePhone
        {
            get
            {
                return GetValue(".//{0}ContactInfo[{0}TYPE='PHONE' and {0}LABEL='Mobile']/{0}DETAIL");
            }
        }
    }
}
