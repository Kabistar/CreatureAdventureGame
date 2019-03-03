using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CreatureGameMapEditor.ViewModels
{
    public class AtlasViewModel : BaseViewModel
    {

        #region Private Members
        Atlas atlas;
        BitmapSource source;
        ObservableCollection<CroppedBitmap> tiles;
        int selectedTile;
        List<TileViewModel> registeredTiles;
        #endregion

        #region Public Properties
        public string TileSheet
        {
            get { return atlas.AtlasFile; }
            set { SetTileSheet(value); ChangeProperty(this, "TileSheet"); }
        }

        public int SheetWidth
        {
            get { return (int)(source.PixelWidth); }
        }

        public int SheetHeight
        {
            get { return (int)(source.PixelHeight); }
        }

        public int SelectedTile
        {
            get
            {
                return selectedTile;
            }
            set
            {
                selectedTile = value;
                ChangeProperty(this, "SelectedTile");
            }
        }

        public byte TileWidth
        {
            get { return atlas.TileWidth; }
            set
            {
                atlas.TileWidth = value;
                BreakTileSheet();
                ChangeProperty(this, "TileWidth");
            }
        }

        public byte TileHeight
        {
            get { return atlas.TileHeight; }
            set
            {
                atlas.TileHeight = value;
                BreakTileSheet();
                ChangeProperty(this, "TileHeight");
            }
        }

        public ObservableCollection<CroppedBitmap> Tiles
        {
            get { return tiles; }
            private set { tiles = value; ChangeProperty(this, "Tiles"); }
        }
        #endregion

        public AtlasViewModel(Atlas atlas)
        {
            this.atlas = atlas;
            source = null;
            Tiles = new ObservableCollection<CroppedBitmap>();
            SelectedTile = -1;
            registeredTiles = new List<TileViewModel>();
            TileSheet = atlas == null ? "" : atlas.AtlasFile;
            
        }

        #region Public Functions
        public void RegisterTile(TileViewModel tile)
        {
            registeredTiles.Add(tile);
        }

        public void UnRegisterTile(TileViewModel tile)
        {
            registeredTiles.Remove(tile);
        }
        #endregion
        #region Private Functions
        private void SetTileSheet(string sheet)
        {
            if (!File.Exists(sheet)) return;
            atlas.AtlasFile = sheet;
            BitmapImage t = new BitmapImage();
            t.BeginInit();
            t.CacheOption = BitmapCacheOption.OnLoad;
            t.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            t.UriSource = new Uri(sheet);
            t.EndInit();
            source = t;
            
            BreakTileSheet();
        }

        private void BreakTileSheet()
        {
            if (source == null) return;
            int xRun = (int)source.PixelWidth / TileWidth;
            int yRun = (int)source.PixelHeight / TileHeight;
            // This is incase the size is accidentally too large, we don't want to kill the system.
            if (xRun * yRun > 2048) return;
            

            Tiles.Clear();
            for (int y = 0; y < yRun; y++)
            {
                for (int x = 0; x < xRun; x++)
                {
                    CroppedBitmap cbm = new CroppedBitmap(source, new Int32Rect(x * TileWidth, y * TileHeight, TileWidth, TileHeight));

                    Tiles.Add(cbm);
                }
            }

            // Update Every Registered Tile;
            foreach(TileViewModel tile in registeredTiles)
            {
                if (tile.TileID < 0 || tile.TileID >= Tiles.Count) { tile.TileImage = null; continue; }
                tile.TileImage = Tiles[tile.TileID];

            }
            ChangeProperty(this, "Tiles");
        }
        #endregion
    }
}
