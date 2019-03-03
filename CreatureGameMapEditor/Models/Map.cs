using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CreatureGameMapEditor.Models
{
    [Serializable]
    public class Map : IXmlSerializable
    {
        #region Private Members
        private ushort width;
        private ushort height;
        private ushort mapId;
        private string mapName;
        private List<Tile> tiles;
        private List<Encounter> encounters;
        private Atlas atlas;
        #endregion

        #region Public Properties
        public Atlas Atlas
        {
            get { return atlas; }
            set { atlas = value; }
        }

        public List<Encounter> Encounters
        {
            get { return encounters; }
            set { encounters = value; }
        }

        public string MapName
        {
            get { return mapName; }
            set { mapName = value; }
        }

        public ushort MapID
        {
            get { return mapId; }
            set { mapId = value; }
        }

        public List<Tile> Tiles
        {
            get { return tiles; }
        }

        public ushort Width
        {
            get { return width; }
        }

        public ushort Height
        {
            get { return height; }
        }
        #endregion


        private Map() { }
        public Map(ushort mapId, ushort width = 0, ushort height = 0)
        {
            this.width = width;
            this.height = height;
            this.mapId = mapId;

            Atlas = new Atlas("", 16, 16);
            MapName = "New Map #" + mapId.ToString("D5");

            //
            // Init Tiles - Width and Height cannot change.
            //
            tiles = new List<Tile>(width * height);
            
            for (int i = 0; i < height * width; i++)
            { 
                tiles.Add(new Tile(0, 0));
            }

            //
            // Init Encounters
            //
            encounters = new List<Encounter>(6);

            for (int i = 0; i < 6; i++)
            {
                encounters.Add(new Encounter(0));
            }
        }

        #region Public Functions
        public Tile GetTile(ushort x, ushort y)
        {
            if (x >= Width || y >= Height) return null;
            return Tiles[y * Width + x];
        }

        public void SetTile(ushort x, ushort y, byte tileID, byte flags)
        {
            if (x >= Width || y >= Height) return;
            Tiles[y * Width + x].TileID = tileID;
            Tiles[y * Width + x].Flags = flags;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        
        public void ReadXml(XmlReader reader)
        {
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Metadata");
            writer.WriteElementString("MapId", mapId + "");
            writer.WriteElementString("MapName", mapName);
            writer.WriteElementString("Width", width + "");
            writer.WriteElementString("Height", height + "");
            writer.WriteEndElement();
            writer.WriteStartElement("Encounters");
            for (int i = 0; i < Encounter.NumberOfEncounters; i++)
            {
                writer.WriteStartElement("Encounter");
                writer.WriteAttributeString("ID", i + "");
                encounters[i].WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteStartElement("Atlas");
            atlas.WriteXml(writer);
            writer.WriteEndElement();
            writer.WriteStartElement("Tiles");
            writer.WriteAttributeString("Count", tiles.Count + "");
            for (int i = 0; i < tiles.Count; i++)
            {
                writer.WriteStartElement("Tile");
                writer.WriteAttributeString("Index", i + "");
                tiles[i].WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        #endregion

        #region Private Functions
        #endregion
    }
}
