using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CreatureGameMapEditor.ViewModels
{
    public class BrushViewModel : BaseViewModel
    {
        #region Private Members
        private int rows;
        private int columns;
        private ObservableCollection<TileViewModel> tiles;
        private MapViewModel map;
        #endregion
        #region Public Properties
        public ICommand SelectTile { get; set; }
        public ICommand ChangeRows { get; set; }
        public ICommand ChangeColumns { get; set; }

        public int Rows
        {
            get { return rows; }
            set { rows = value; ChangeProperty(this, "Rows");}
        }

        public int Columns
        {
            get { return columns; }
            set { columns = value; ChangeProperty(this, "Columns"); }
        }
        
        public ObservableCollection<TileViewModel> Tiles
        {
            get { return tiles; }
            set { tiles = value; ChangeProperty(this, "Tiles"); }
        }
        #endregion

        public BrushViewModel(MapViewModel parentMap)
        {
            // Fields
            map = parentMap;
            Rows = 1;
            Columns = 1;
            
            // Tile Maps
            Tiles = new ObservableCollection<TileViewModel>();
            // Map has one tile by default
            TileViewModel newTile = new TileViewModel(map.Atlas, new Tile(0, 0));
            Tiles.Add(newTile);
            newTile.IsSelected = true;

            // Commands
            ChangeRows = new ParameterCommand(Command_ChangeRows);
            ChangeColumns = new ParameterCommand(Command_ChangeColumns);
            SelectTile = new ParameterCommand(Command_SelectTile);
        }

        #region Public Functions
        public void SetTile(byte id)
        {
            /// Make modifications to the selection
            /// Select flag is set based on the keys currently down
            /// 0x0001 = Control
            /// 0x0010 = Shift
            /// 0x0100 = Alt
            byte selectFlag = 0;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) selectFlag |= 1;
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) selectFlag |= 2;
            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt)) selectFlag |= 4;

            /// Key Patterns and behaviors
            /// 
            /// C     : Change the selected tiles by shifting it to the next non-occupied index.
            /// C + S : Change the selected tiles by shifting it to the previous non-occupied index.
            /// A     : Reset the size of the brush to 1x1.
            /// 
            /// NOTE  : Behaviors that change the size of the brush should be done prior to settings the tiles.
            /// 

            if (selectFlag == 4)
            {
                Tuple<int, int> deltaCols = new Tuple<int, int>(Columns, 1);
                Tuple<int, int> deltaRows = new Tuple<int, int>(Rows, 1);

                Columns = 1;
                Rows = 1;
                Tiles[0].IsSelected = true;
            }

            // Set the selected tiles
            foreach (TileViewModel tile in Tiles)
            {
                if (tile.IsSelected)
                    tile.TileID = id;
            }

            if (selectFlag == 1) // Control Only
            {
                List<int> indices = new List<int>();
                foreach (TileViewModel tile in Tiles)
                {
                    if (tile.IsSelected)
                    {
                        int i = Tiles.IndexOf(tile);
                        indices.Add(i);
                    }
                }
                for (int i = 0; i < indices.Count; i++)
                {
                    int index = indices[i];
                    Tiles[index].IsSelected = false;

                    for (int j = 0; j < Tiles.Count; j++)
                    {
                        int t = index + j + 1;
                        if (t >= Tiles.Count) t -= Tiles.Count;
                        if (!Tiles[t].IsSelected)
                        {
                            Tiles[t].IsSelected = true;
                            break;
                        }
                    }
                }
            }
            else if (selectFlag == 3) // Control + Shift
            {
                List<int> indices = new List<int>();
                foreach (TileViewModel tile in Tiles)
                {
                    if (tile.IsSelected)
                    {
                        int i = Tiles.IndexOf(tile);
                        indices.Add(i);
                    }
                }
                for (int i = indices.Count - 1; i >= 0; i--)
                {
                    int index = indices[i];
                    Tiles[index].IsSelected = false;

                    for (int j = 0; j < Tiles.Count; j++)
                    {
                        int t = index - j - 1;
                        if (t < 0) t += Tiles.Count;
                        if (!Tiles[t].IsSelected)
                        {
                            Tiles[t].IsSelected = true;
                            break;
                        }
                    }
                }
            }
        }

        public void PaintMap(MapViewModel map, Point paintLocation)
        {
            //map.History.AddMarker();
            for(int y = 0; y < Rows; y++)
            {
                for(int x = 0; x < Columns; x++)
                {
                    map.SetTile((ushort)(paintLocation.X + x), (ushort)(paintLocation.Y + y), Tiles[x + (Columns * y)].TileID, Tiles[x + (Columns * y)].Flags);
                }
            }
        }
        #endregion
        #region Private Functions
        #endregion
        #region Commands
        private void Command_SelectTile(object tileId)
        {
            byte id = (byte)tileId;
            SetTile(id);
        }

        private void Command_ChangeRows(object sliderValues)
        {
            Tuple<int, int> delta = (Tuple<int, int>)sliderValues;
            TileViewModel newTile;
            // Adding new items
            if (delta.Item2 > delta.Item1)
            {
                for (int j = 0; j < (delta.Item2 - delta.Item1); j++)
                {
                    for (int i = 0; i < Columns; i++)
                    {
                        newTile = new TileViewModel(map.Atlas, new Tile(0, 0));
                        Tiles.Add(newTile);
                    }
                }
            }
            // Removing old items
            else if (delta.Item1 > delta.Item2)
            {
                for (int y = delta.Item1 - 1; y >= delta.Item2; y--)
                {
                    for (int x = Columns - 1; x >= 0; x--)
                    {
                        map.Atlas.UnRegisterTile(Tiles[Tiles.Count - 1]);
                        Tiles.RemoveAt(Tiles.Count-1);
                    }
                }
            }
        }

        private void Command_ChangeColumns(object sliderValues)
        {
            Tuple<int, int> delta = (Tuple<int, int>)sliderValues;
            TileViewModel newTile;
            // Adding new items
            if (delta.Item2 > delta.Item1)
            {
                for (int j = 0; j < (delta.Item2 - delta.Item1); j++)
                {
                    int col_offset = delta.Item1 + j; // column to add the tile in
                    for (int i = Rows-1; i >= 0; i--)
                    {
                        int starting_row = i * (col_offset);
                        newTile = new TileViewModel(map.Atlas, new Tile(0, 0));
                        Tiles.Insert(starting_row + col_offset, newTile);
                    }
                }
            }
            // Removing old items
            else if (delta.Item1 > delta.Item2)
            {
                // Iterate over rows starting at bottom
                for (int y = Rows-1; y >= 0; y--)
                {
                    // Iterate over columns starting at right
                    for(int x = delta.Item1 - 1; x >= 0; x--)
                    {
                        int index = y * delta.Item1 + x;
                        
                        // If the current item is one of the last tiles in the row, then we remove it
                        if (x >= delta.Item2)
                        {
                            map.Atlas.UnRegisterTile(Tiles[index]);
                            Tiles.RemoveAt(index);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
