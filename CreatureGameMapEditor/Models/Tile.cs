using System;
using System.Collections.Generic;
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
    public class Tile : IXmlSerializable
    {
        public enum JumpDirections
        {
            NorthToSouth = 0,
            SouthToNorth = 1,
            EastToWest = 2,
            WestToEast = 3
        }

        public enum TileTypes
        {
            Land = 0,
            Water = 1,
            Wall = 2
        }

        #region Private Members
        byte tileID;
        byte flags;
        #endregion

        #region Public Properties
        public byte TileID
        {
            get
            {
                return tileID;
            }
            set
            {
                tileID = value;
            }
        }

        public TileTypes TileType
        {
            get
            {
                return (TileTypes)((flags & 0xC0) >> 6);
            }
            set
            {
                if (value < 0 || value >= Enum.GetValues(typeof(TileTypes)).Cast<TileTypes>().Max())
                    throw new ArgumentOutOfRangeException(value + " is not within TileType range.");
                Flags = (byte)((flags & 0b00111111) | ((byte)value << 6));
            }
        }

        public byte Encounter
        {
            get
            {
                return (byte)(flags & 0x0F);
            }
            set
            {
                if (value < 0 || value >= 16)
                    throw new ArgumentOutOfRangeException(value + " is not range for an encounter (0-15).");
                Flags = (byte)((flags & 0b11110000) | ((byte)value));
            }
        }

        public byte Portal
        {
            get
            {
                return (byte)(flags & 0x1F);
            }
            set
            {
                if (value < 0 || value >= 32)
                    throw new ArgumentOutOfRangeException(value + " is not range for a portal (0-31).");
                Flags = (byte)((flags & 0b11100000) | ((byte)value));
            }
        }

        public bool Jumpable
        {
            get
            {
                return (flags & 0x88) == 0x08; // Walls cannot be jumpable
            }
            set
            {
                Flags = (byte)((flags & 0b11110111) | (value == true ? 8 : 0));
            }
        }

        public JumpDirections JumpDirection
        {
            get
            {
                return (JumpDirections)((flags & 0x06) >> 1);
            }
            set
            {
                if (value < 0 || value >= Enum.GetValues(typeof(JumpDirections)).Cast<JumpDirections>().Max())
                    throw new ArgumentOutOfRangeException(value + " is not within JumpDirections range.");
                Flags = (byte)((flags & 0b11111001) | ((byte)value << 1));
            }
        }

        /// <summary>
        /// <para>Flags that alter how this tile interacts with players <br />
        ///     0000zyyx : regular land : z determines if its jumpable <br />
        ///          00  : Jumpable from north to south <br />
        ///          01  : Jumpable from south to  north <br />
        ///          10  : Jumpable from east to west <br />
        ///          11  : Jumpable from west to east <br />
        ///     0001yyyy : land with an encounter : yyyy determines the encounter ID <br />
        ///     001yyyyy : land with a portal on it: yyyyy determines the portal ID
        /// </para>
        /// <para>
        ///     0100zyyx : regular water : z determines if its jumpable <br />
        ///          00   : Jumpable from north to south <br />
        ///          01   : Jumpable from south to north <br />
        ///          10   : Jumpable from east to west <br />
        ///          11   : Jumpable from west to east <br />
        ///     0101yyyy : water with an encounter : yyyy determines the encounter ID <br />
        ///     011yyyyy : water with a portal on it: yyyyy determines the portal ID
        /// </para>
        /// <para>
        ///     10xxxxxx : wall
        /// </para>
        /// </summary>
        public byte Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }
        #endregion
        

        public Tile(byte tileID, byte flags)
        {
            this.tileID = tileID;
            this.flags = flags;
        }

        private Tile() { }


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
            writer.WriteAttributeString("TileID", tileID + "");
            writer.WriteAttributeString("Flags", flags + "");
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
