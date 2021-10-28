using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTASConnectorSDK.DataModel
{
    public class Layer : INotifyPropertyChanged
    {
        Guid m_identifier;
        string m_details;
        string m_entityRootKeyField;
        string m_geoColumnName;
        bool m_isOn;
        bool m_isRootEntityLayer;
        string m_layerKeyFieldName;
        double m_maximumScale;
        double m_minimumScale;
        string m_name;
        int m_order;
        string m_shapeType;
        string m_spatialReference;
        bool m_supportClipping;
        string m_tableName;
        Int16 m_type;
        string m_uniqueName;
        Guid m_layerSourceIdentifier;
        Guid m_styleIdentifier;
        protected string m_rowStatus;

        public Guid Identifier
        {
            get
            {
                return m_identifier;
            }
        }

        public string Details
        {
            get
            {
                return m_details;
            }
            set
            {
                m_details = value;
                OnNotifyPropertyChange("Details");
            }
        }

        public string EntityRootKeyField
        {
            get
            {
                return m_entityRootKeyField;
            }
            set
            {
                m_entityRootKeyField = value;
                OnNotifyPropertyChange("EntityRootKeyField");
            }
        }

        public string GeoColumnName
        {
            get
            {
                return m_geoColumnName;
            }
            set
            {
                m_geoColumnName = value;
                OnNotifyPropertyChange("GeoColumnName");
            }
        }

        public bool IsOn
        {
            get
            {
                return m_isOn;
            }
            set
            {
                m_isOn = value;
                OnNotifyPropertyChange("IdentIsOnifier");
            }
        }

        public bool IsRootEntityLayer
        {
            get
            {
                return m_isRootEntityLayer;
            }
            set
            {
                m_isRootEntityLayer = value;
                OnNotifyPropertyChange("IsRootEntityLayer");
            }
        }

        public string LayerKeyFieldName
        {
            get
            {
                return m_layerKeyFieldName;
            }
            set
            {
                m_layerKeyFieldName = value;
                OnNotifyPropertyChange("LayerKeyFieldName");
            }
        }

        public double MaximumScale
        {
            get
            {
                return m_maximumScale;
            }
            set
            {
                m_maximumScale = value;
                OnNotifyPropertyChange("MaximumScale");
            }
        }

        public double MinimumScale
        {
            get
            {
                return m_minimumScale;
            }
            set
            {
                m_minimumScale = value;
                OnNotifyPropertyChange("MinimumScale");
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
                OnNotifyPropertyChange("Name");
            }
        }

        public int Order
        {
            get
            {
                return m_order;
            }
            set
            {
                m_order = value;
                OnNotifyPropertyChange("Order");
            }
        }

        public string ShapeType
        {
            get
            {
                return m_shapeType;
            }
            set
            {
                m_shapeType = value;
                OnNotifyPropertyChange("ShapeType");
            }
        }

        public string SpatialReference
        {
            get
            {
                return m_spatialReference;
            }
            set
            {
                m_spatialReference = value;
                OnNotifyPropertyChange("SpatialReference");
            }
        }

        public bool SupportClipping
        {
            get
            {
                return m_supportClipping;
            }
            set
            {
                m_supportClipping = value;
                OnNotifyPropertyChange("SupportClipping");
            }
        }

        public string TableName
        {
            get
            {
                return m_tableName;
            }
            set
            {
                m_tableName = value;
                OnNotifyPropertyChange("TableName");
            }
        }

        public Int16 Type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
                OnNotifyPropertyChange("Type");
            }
        }

        public string UniqueName
        {
            get
            {
                return m_uniqueName;
            }
            set
            {
                m_uniqueName = value;
                OnNotifyPropertyChange("UniqueName");
            }
        }

        public Guid LayerSourceIdentifier
        {
            get
            {
                return m_layerSourceIdentifier;
            }
            set
            {
                m_layerSourceIdentifier = value;
                OnNotifyPropertyChange("LayerSourceIdentifier");
            }
        }

        public Guid StyleIdentifier
        {
            get
            {
                return m_styleIdentifier;
            }
            set
            {
                m_styleIdentifier = value;
                OnNotifyPropertyChange("StyleIdentifier");
            }
        }

        public string RowStatus
        {
            get
            {
                return m_rowStatus;
            }
        }

        protected void OnNotifyPropertyChange(string propertyname)
        {
            if (string.IsNullOrEmpty(m_rowStatus))
            {
                m_rowStatus = "u";
            }

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Layer(string details, string entityRootKeyField, string geoColumnName, bool isOn, bool isRootEntityLayer, string layerKeyFieldName, double maximumScale, double minimumScale, string name, int order, string shapeType, string spatialReference, bool supportClipping, string tableName, Int16 type, string uniqueName, Guid layerSourceIdentifier, Guid styleIdentifier)
        {
            m_identifier = Guid.NewGuid();
            m_details = details;
            m_entityRootKeyField = entityRootKeyField;
            m_geoColumnName = geoColumnName;
            m_isOn = isOn;
            m_isRootEntityLayer = isRootEntityLayer;
            m_layerKeyFieldName = layerKeyFieldName;
            m_maximumScale = maximumScale;
            m_minimumScale = minimumScale;
            m_name = name;
            m_order = order;
            m_shapeType = shapeType;
            m_spatialReference = spatialReference;
            m_supportClipping = supportClipping;
            m_tableName = tableName;
            m_type = type;
            m_uniqueName = uniqueName;
            m_layerSourceIdentifier = layerSourceIdentifier;
            m_styleIdentifier = styleIdentifier;
            m_rowStatus = "i";
        }

        internal Layer(Guid identifier, string details, string entityRootKeyField, string geoColumnName, bool isOn, bool isRootEntityLayer, string layerKeyFieldName, double maximumScale, double minimumScale, string name, int order, string shapeType, string spatialReference, bool supportClipping, string tableName, Int16 type, string uniqueName, Guid layerSourceIdentifier, Guid styleIdentifier)
        {
            m_identifier = identifier;
            m_details = details;
            m_entityRootKeyField = entityRootKeyField;
            m_geoColumnName = geoColumnName;
            m_isOn = isOn;
            m_isRootEntityLayer = isRootEntityLayer;
            m_layerKeyFieldName = layerKeyFieldName;
            m_maximumScale = maximumScale;
            m_minimumScale = minimumScale;
            m_name = name;
            m_order = order;
            m_shapeType = shapeType;
            m_spatialReference = spatialReference;
            m_supportClipping = supportClipping;
            m_tableName = tableName;
            m_type = type;
            m_uniqueName = uniqueName;
            m_layerSourceIdentifier = layerSourceIdentifier;
            m_styleIdentifier = styleIdentifier;
            m_rowStatus = string.Empty;
        }

        public void Delete()
        {
            if (m_rowStatus == "i" || m_rowStatus == "pd")
                m_rowStatus = "pd";
            else
                m_rowStatus = "d";
        }
    }
}
