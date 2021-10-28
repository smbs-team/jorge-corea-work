using ConnectorService.Utilities;
using PTASSyncService.Models;

namespace ConnectorService
{
    public class ConnectorInstances
    {
        private static ConnectorInstances instances = null;
        DataExportUtility exportInstance;
        DataImportUtility importInstance;

        public enum ConnectorStatus
        {
            Ready = 0,
            Running = 1,
            Invalid = 2
        }

        public ConnectorStatus Status
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public static ConnectorInstances Instances
        {
            get
            {
                if (instances == null)
                {
                    instances = new ConnectorInstances();
                }

                return instances;
            }
        }

        public long LoadRootEntities(LoadRootEntityModel loadRootEntityMdl, long assignmentID)
        {
            return importInstance.LoadRootEntities(loadRootEntityMdl, assignmentID);
        }

    }
}
