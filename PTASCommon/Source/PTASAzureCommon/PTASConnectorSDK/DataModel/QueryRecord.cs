using System;
using System.Collections.Generic;
using System.Text;

namespace PTASConnectorSDK.DataModel
{
    /// <summary>
    /// Class to handle bulk inserts.
    /// </summary>
    public class QueryRecord
    {
        public Guid guidSetID_gq;
        public string entityKind_gq;
        public string strData_gq;
        public long dataOrder_gq;

        public QueryRecord()
        { }
        public QueryRecord(Guid GuidSetID_gq, string EntityKind_gq, string StrData_gq, long DataOrder_gq)
        {
            guidSetID_gq = GuidSetID_gq;
            entityKind_gq = EntityKind_gq;
            strData_gq = StrData_gq;
            dataOrder_gq = DataOrder_gq;
        }
    }
}
