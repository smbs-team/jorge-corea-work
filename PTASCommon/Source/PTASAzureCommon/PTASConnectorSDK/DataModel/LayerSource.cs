using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTASConnectorSDK.DataModel
{
    public class LayerSource : INotifyPropertyChanged
    {
        private Guid m_identifier;
        private Int16 m_fileType;
        private bool m_isLinked;
        private string m_name;
        private string m_path;
        private double m_size;
        protected string m_rowStatus;

        public Guid Identifier
        {
            get
            {
                return m_identifier;
            }
        }

        public Int16 FileType
        {
            get
            {
                return m_fileType;
            }
            set
            {
                m_fileType = value;
                OnNotifyPropertyChange("FileType");
            }
        }

        public bool IsLinked
        {
            get
            {
                return m_isLinked;
            }
            set
            {
                m_isLinked = value;
                OnNotifyPropertyChange("IsLinked");
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

        public string Path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
                OnNotifyPropertyChange("Path");
            }
        }

        public double Size
        {
            get
            {
                return m_size;
            }
            set
            {
                m_size = value;
                OnNotifyPropertyChange("Size");
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

        public LayerSource(Int16 fileType, bool isLinked, string name, string path, double size)
        {
            m_identifier = Guid.NewGuid();
            m_fileType = fileType;
            m_isLinked = isLinked;
            m_name = name;
            m_path = path;
            m_size = size;
            m_rowStatus = "i";
        }

        internal LayerSource(Guid identifier, Int16 fileType, bool isLinked, string name, string path, double size)
        {
            m_identifier = identifier;
            m_fileType = fileType;
            m_isLinked = isLinked;
            m_name = name;
            m_path = path;
            m_size = size;
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
