using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureGameMapEditor.DataAccess
{
    public static class MapProvider
    {
        public static Map LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new ArgumentException("Maps filepath does not exist.");

            Map ret = new Map(0);

            return ret;

        }

        public static Map ResizeMap(Map map, ushort newWidth, ushort newHeight)
        {
            Map ret = new Map(map.MapID, newWidth, newHeight);

            ushort maxWidth = Math.Max(newWidth, map.Width);
            ushort maxHeight = Math.Max(newHeight, map.Height);


            // Copy over the tiles
            for(ushort y = 0; y < maxHeight; y++)
            {
                for (ushort x = 0; y < maxWidth; y++)
                {
                    Tile oldTile = map.GetTile(x, y);
                    ret.SetTile(x, y, oldTile.TileID, oldTile.Flags);
                }
            }

            ret.Encounters = map.Encounters;
            ret.Atlas = map.Atlas;

            return ret;
        }
    }
}
