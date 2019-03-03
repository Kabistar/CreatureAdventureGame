using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CreatureGameMapEditor.Models
{
    [Serializable]
    public class Atlas : BaseModel, IXmlSerializable
    {
        #region Private Members
        private string fileName;
        private byte tileWidth;
        private byte tileHeight;
        #endregion

        #region Public Properties
        public byte TileWidth
        {
            get { return tileWidth; }
            set { tileWidth = (value < (byte)4 ? (byte)4 : value); ChangeProperty(this, "TileWidth"); }
        }

        public byte TileHeight
        {
            get { return tileHeight; }
            set { tileHeight = (value < (byte)4 ? (byte)4 : value); ChangeProperty(this, "TileHeight"); }
        }

        public string AtlasFile
        {
            get { return fileName; }
            set { fileName = value; ChangeProperty(this, "AtlasFile"); }
        }
        #endregion

        public Atlas(string atlasFileLocation, byte tileWidth, byte tileHeight)
        {
            AtlasFile = atlasFileLocation;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        private Atlas() { }

        #region Public Functions
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("FileName", fileName);
            writer.WriteStartElement("Dimensions");
            writer.WriteAttributeString("TileWidth", tileWidth + "");
            writer.WriteAttributeString("TileHeight", tileHeight + "");
            writer.WriteEndElement();
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
