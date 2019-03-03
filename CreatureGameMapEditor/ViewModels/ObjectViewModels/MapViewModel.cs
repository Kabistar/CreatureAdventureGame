using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CreatureGameMapEditor.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        #region Private Members
        Map map;

        ObservableCollection<TileViewModel> tiles;
        AtlasViewModel atlas;
        BrushViewModel brush;
        History history;
        #endregion
        #region Public Properties
        public ICommand UndoAction { get; set; }
        public ICommand RedoAction { get; set; }
        public ICommand ChangeAtlas { get; set; }
        public ICommand ClickMap { get; set; }
        public ICommand DragMap { get; set; }

        public AtlasViewModel Atlas
        {
            get { return atlas; }
            private set { atlas = value; ChangeProperty(this, "Atlas"); }
        }

        public BrushViewModel Brush
        {
            get { return brush; }
            private set { brush = value; ChangeProperty(this, "Brush"); }
        }

        public History History
        {
            get { return history; }
            private set { history = value; }
        }

        public ObservableCollection<TileViewModel> Tiles
        {
            get { return tiles; }
        }
        

        public ushort Width
        {
            get { return map.Width; }
            //private set { map.Width = value; ChangeProperty(this, "Width"); }
        }

        public ushort Height
        {
            get { return map.Height; }
            //private set { map.Height = value;  ChangeProperty(this, "Height"); }
        }

        public string Name
        {
            get { return map.MapName; }
            set
            {
                map.MapName = value;
                ChangeProperty(this, "Name");
            }
        }

        #endregion

        public MapViewModel(Map map)
        {
            // Set the model
            this.map = map;

            // Instantiate an atlas from the map.
            Atlas = new AtlasViewModel(map.Atlas);

            //Instantate the initial brush.
            brush = new BrushViewModel(this);

            // Instantiate the Tiles - Future tiles added should be done through the event
            tiles = new ObservableCollection<TileViewModel>();
            //map.Tiles.CollectionChanged += TilesCollectionChanged;

            foreach (Tile t in map.Tiles)
            {
                Tiles.Add(new TileViewModel(t));
                Tiles[Tiles.Count - 1].PropertyChanged += TileViewModel_PropertyChanged;
            }

            history = new History(map);

            // Add commands
            UndoAction = new RelayCommand(Command_UndoTilePlace);
            RedoAction = new RelayCommand(Command_RedoTilePlace);
            ClickMap = new ParameterCommand(Command_ClickMap);
            DragMap = new ParameterCommand(Command_DragMap);
        }
        
        #region Public Functions
        public void SetTile(ushort x, ushort y, byte tileID, byte flags)
        {
            int index = y * Width + x;
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            History.AddEntry(x, y, tileID, flags);
            Tiles[index].TileID = tileID;
            Tiles[index].Flags = flags;
        }

        public void SetMapSize(ushort newWidth, ushort newHeight, Tile fillTile)
        {
            ushort oldWidth = Width;
            ushort oldHeight = Height;
            Debug.WriteLine("Setting Map Size: " + newWidth + ", " + newHeight);

            if (newWidth < 0 || newHeight < 0 || fillTile == null) return;
            // Change Height
            // Adding new items
            if (newHeight > oldHeight)
            {
                for (int j = 0; j < (newHeight - oldHeight); j++)
                {
                    for (int i = 0; i < Width; i++)
                    {
                        map.Tiles.Add(new Tile(fillTile.TileID, fillTile.Flags));
                    }
                }
            }
            // Removing old items
            else if (oldHeight > newHeight)
            {
                for (int j = 0; j < (oldHeight - newHeight); j++)
                {
                    for (int i = 0; i < Width; i++)
                    {
                        Atlas.UnRegisterTile(Tiles[Tiles.Count - 1]);
                        map.Tiles.RemoveAt(Tiles.Count - 1);
                    }
                }
            }
            //Height = newHeight;

            // Change Width
            if (newWidth > oldWidth)
            {
                // Add a tile at the end
                //map.Tiles.Add(new Tile(fillTile.Value, fillTile.Flags));
                // Add the others
                for (int j = 0; j < (newWidth - oldWidth); j++)
                {
                    int col_offset = oldWidth + j; // column to add the tile in
                    for (int i = Height - 1; i >= 0; i--)
                    {
                        int starting_row = i * (col_offset);
                        map.Tiles.Insert(starting_row + col_offset, new Tile(fillTile.TileID, fillTile.Flags));
                    }
                }
            }
            // Removing old items
            else if (oldWidth > newWidth)
            {
                for (int j = (oldWidth - newWidth) - 1; j >= 0; j--)
                {
                    int col_offset = oldWidth + j; // column to add the tile in
                    for (int i = Height - 1; i >= 0; i--)
                    {
                        int starting_row = i * (col_offset);
                        Atlas.UnRegisterTile(Tiles[starting_row + col_offset - 1]);
                        Tiles.RemoveAt(starting_row + col_offset - 1);
                    }
                }
            }

            //Width = newWidth;
            ChangeProperty(this, "Tiles");
        }
        #endregion

        #region Private Functions
        private void TileViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TileID"))
            {
                if (sender.GetType() != typeof(TileViewModel)) throw new ArgumentException("Tile Type: " + sender.GetType() + " is not valid. Expected TileViewModel");
                // Update the Tiles bitmap image.
                TileViewModel tile = sender as TileViewModel;
                if (tile.TileID >= Atlas.Tiles.Count)
                {
                    // Set a null tile?
                }
                else
                {
                    tile.TileImage = Atlas.Tiles[tile.TileID];
                }
            }
        }

        //private void TilesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            if (e.NewStartingIndex != -1)
        //            {
        //                int i = e.NewStartingIndex;
        //                // Insert into the list
        //                foreach (var m in e.NewItems)
        //                {
        //                    Tiles.Insert(i++, new TileViewModel(Atlas, (Tile)m));
        //                }
        //            }
        //            else
        //            {
        //                // Append to end of list
        //                foreach (var m in e.NewItems)
        //                {
        //                    Tiles.Add( new TileViewModel(Atlas, (Tile)m));
        //                }
        //            }

        //            break;
        //        case NotifyCollectionChangedAction.Remove:
        //            if (e.OldStartingIndex != -1)
        //            {
        //                int i = e.OldStartingIndex;
        //                // Insert into the list
        //                foreach (var m in e.OldItems)
        //                {
        //                    Tiles.RemoveAt(i);
        //                }
        //            }
        //            else
        //            {
        //                // Append to end of listOldItems)
        //                foreach (var m in e.OldItems)
        //                {
        //                    Tiles.Remove(Tiles.FirstOrDefault(v => v.Tile == (Tile)m));
        //                }
        //            }
        //            break;
        //        case NotifyCollectionChangedAction.Reset:
        //            Tiles.Clear();
        //            break;
        //        case NotifyCollectionChangedAction.Replace:
        //            int index = e.OldStartingIndex;
        //            for(int i = 0; i < e.NewItems.Count; i++)
        //            {
        //                Tiles.RemoveAt(index);
        //                Tiles.Insert(index, new TileViewModel(Atlas, (Tile)e.NewItems[i]));
        //                index++;
        //            }
        //            break;
        //        case NotifyCollectionChangedAction.Move:
        //            int rIndex = e.OldStartingIndex;
        //            for(int i = 0; i < e.OldItems.Count; i++)
        //            {
        //                Tiles.RemoveAt(rIndex);
        //            }

        //            int aIndex = e.NewStartingIndex;
        //            for (int i = 0; i < e.NewItems.Count; i++)
        //            {
        //                Tiles.Insert(aIndex++, new TileViewModel(Atlas, (Tile) e.NewItems[i]));
        //            }
        //            break;
        //    }
        //    ChangeProperty(this, "Tiles");
        //}
        #endregion

        #region Commands
        private void Command_ClickMap(object coordinates)
        {
            History.AddMarker();
            Command_DragMap(coordinates);
        }

        private void Command_DragMap(object coordinates)
        {
            Point loc = (Point)coordinates;

            int x = (int)loc.X;
            int y = (int)loc.Y;

            Brush.PaintMap(this, loc);
        }

        private void Command_UndoTilePlace()
        {
            history.Undo(false);
        }

        private void Command_RedoTilePlace()
        {
            history.Redo(false);
        }
        #endregion
    }
}
