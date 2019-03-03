using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CreatureGameMapEditor.ViewModels
{
    public class MapSettingsViewModel : BaseViewModel
    {
        #region Private Members
        // Map we want to edit
        MapViewModel map;

        // Temp variables
        TileViewModel fillTile;
        AtlasViewModel atlas;
        ushort mapWidth;
        ushort mapHeight;
        string mapName;

        #endregion

        #region Public Properties
        public ICommand ApplyChanges { get; set; }
        public ICommand SelectAtlasFile { get; set; }
        public TileViewModel FillTile { get { return fillTile; } private set { fillTile = value; ChangeProperty(this, "FillTile"); } }
        public AtlasViewModel Atlas { get { return atlas; } private set { atlas = value; ChangeProperty(this, "Atlas"); } }
        public string MapName { get { return mapName; } set { mapName = value; ChangeProperty(this, "MapName"); } }
        public ushort MapWidth { get { return mapWidth; } set { mapWidth = value; ChangeProperty(this, "MapWidth"); } }
        public ushort MapHeight { get { return mapHeight; } set { mapHeight = value; ChangeProperty(this, "MapHeight"); } }
        #endregion


        public MapSettingsViewModel(MapViewModel map)
        {
            this.map = map;

            MapName = map.Name;
            MapWidth = map.Width;
            MapHeight = map.Height;

            ApplyChanges = new RelayCommand(Command_ApplyChanges);
            SelectAtlasFile = new ParameterCommand(Command_SelectAtlasFile);

            Atlas = new AtlasViewModel(new Atlas(map.Atlas.TileSheet, map.Atlas.TileWidth, map.Atlas.TileHeight));
            FillTile = new TileViewModel(Atlas, new Tile(0, 0));
        }

        #region Public Functions
        #endregion

        #region Private Functions
        #endregion

        #region Commands
        private void Command_ApplyChanges()
        {
            map.Name = MapName;

            map.SetMapSize(MapWidth, MapHeight, FillTile.Tile);

            map.Atlas.TileWidth = Atlas.TileWidth;
            map.Atlas.TileHeight = Atlas.TileHeight;
            map.Atlas.TileSheet = Atlas.TileSheet;
        }

        private void Command_SelectAtlasFile(object argument)
        {
            if (argument.GetType() != typeof(string)) return;
            Atlas.TileSheet = argument as string;
        }
        #endregion
    }
}
