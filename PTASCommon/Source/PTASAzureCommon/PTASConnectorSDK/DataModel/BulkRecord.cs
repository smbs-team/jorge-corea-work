using System;
using System.Collections.Generic;
using System.Text;

namespace PTASConnectorSDK.DataModel
{
    /// <summary>
    /// Class to handle bulk inserts.
    /// </summary>
    public class BulkRecord
    {
        public long id_upl;
        public long changeSetID_upl;
        public string entityKind_upl;
        public string strData_upl;
        public long dataOrder_upl;

        public BulkRecord()
        { }
        public BulkRecord(long ChangeSetID_upl, string EntityKind_upl, string StrData_upl, long DataOrder_upl)
        {
            id_upl = 0;
            changeSetID_upl = ChangeSetID_upl;
            entityKind_upl = EntityKind_upl;
            strData_upl = StrData_upl;
            dataOrder_upl = DataOrder_upl;
        }
    }
}
