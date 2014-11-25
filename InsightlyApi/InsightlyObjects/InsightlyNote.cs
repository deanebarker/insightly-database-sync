using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InsightlyApi.InsightlyObjects
{
    [InsightlyTableMapping(ObjectName = "Note", TableName = "Notes")]
    public class InsightlyNote : InsightlyObject
    {
        public InsightlyNote(XmlElement xml)
            : base(xml)
        {

        }

        public override string Id
        {
            get { return GetValue(".//{0}NOTE_ID"); }
        }

        [InsightlyColumnMapping]
        public string Title
        {
            get { return GetValue(".//{0}TITLE"); }
        }


        [InsightlyColumnMapping]
        public string Body
        {
            get { return GetValue(".//{0}BODY"); }
        }

        [InsightlyColumnMapping]
        public string OpportunityId
        {
            get { return GetValue(".//{0}OPPORTUNITY_ID"); }
        }

        [InsightlyColumnMapping]
        public string ContactId
        {
            get { return GetValue(".//{0}CONTACT_ID"); }
        }

        [InsightlyColumnMapping]
        public string OwnerId
        {
            get { return GetValue(".//{0}OWNER_USER_ID"); }
        }

        [InsightlyColumnMapping]
        public string Created
        {
            get { return GetValue(".//{0}DATE_CREATED_UTC"); }
        }
    }
}