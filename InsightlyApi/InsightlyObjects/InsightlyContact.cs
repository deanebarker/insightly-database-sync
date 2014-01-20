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

        [InsightlyColumnMapping]
        public string Name
        {
            get
            {
                return String.Concat(GetValue(".//FIRST_NAME"), " ", GetValue(".//LAST_NAME")).Trim();
            }
        }

        [InsightlyColumnMapping]
        public string Email
        {
            get
            {
                return GetValue(".//CONTACTINFOS/ContactInfo[TYPE='EMAIL']/DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string OrgId
        {
            get
            {
                return GetValue(".//DEFAULT_LINKED_ORGANISATION");
            }
        }

        [InsightlyColumnMapping]
        public string Title
        {
            get
            {
                return GetValue(".//Link[ORGANISATION_ID[text()] and ROLE[text()]]/ROLE");
            }
        }

        [InsightlyColumnMapping]
        public string LinkedInUrl
        {
            get
            {
                return GetValue(".//ContactInfo[SUBTYPE = 'LinkedInPublicProfileUrl']/DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string WorkPhone
        {
            get
            {
                return GetValue(".//ContactInfo[TYPE='PHONE' and LABEL='Work']/DETAIL");
            }
        }

        [InsightlyColumnMapping]
        public string MobilePhone
        {
            get
            {
                return GetValue(".//ContactInfo[TYPE='PHONE' and LABEL='Mobile']/DETAIL");
            }
        }
    }
}
