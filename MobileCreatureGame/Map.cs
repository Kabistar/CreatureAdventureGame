using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileCreatureGame
{
    public enum JumpDirection
    {
        NorthToSouth    = 0,
        SouthToNorth    = 1,
        EastToWest      = 2,
        WestToEast      = 3
    }

    public enum TileType
    {
        Land            = 0,
        Wall            = 2,
        Water           = 1
    }

    public struct Tile
    {
        /// <summary>
        /// graphic is the index into the texture map. The Map containing this tile determines the graphic.
        /// </summary>
        byte graphic;
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
        byte flags;

        public byte Graphic
        {
            get
            {
                return graphic;
            }
        }

        public TileType TileType
        {
            get
            {
                return (TileType)((flags & 0xC0) >> 6);
            }
        }

        public bool Jumpable
        {
            get
            {
                return (flags & 0x88) == 0x08; // Walls cannot be jumpable
            }
        }

        public JumpDirection JumpDirection
        {
            get
            {
                return (JumpDirection)((flags & 0x06) >> 1);
            }
        }

        public Tile(byte graphic, byte flags)
        {
            this.graphic = graphic;
            this.flags = flags;
        }
    }

    public struct Encounter
    {
        byte chance;
        byte weight_0;
        byte weight_1;
        byte weight_2;
        byte weight_3;
        byte weight_4;
        byte weight_5;
    }

    public class Map
    {
        Tile[,] tiles;

        public Map()
        {
            tiles = new Tile[16, 16];

            for(int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    tiles[x,y] = new Tile(6, 0);
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    batch.Draw(Game1.textureMap, new Vector2(x * 16, y*16), new Rectangle((tiles[x,y].Graphic % 16) * 16, (tiles[x, y].Graphic / 16) * 16, 16,16), Color.White);
                }
            }
        }
    }
}
