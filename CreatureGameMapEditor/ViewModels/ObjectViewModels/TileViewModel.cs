using CreatureGameMapEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CreatureGameMapEditor.ViewModels
{
    public class TileViewModel : BaseViewModel
    {
        #region private members
        Tile tile;
        BitmapSource tileImage;
        bool isSelected;
        #endregion
        #region Public Properties
        public Tile Tile { get { return tile; } }

        public BitmapSource TileImage
        {
            get { return tileImage; }
            set { tileImage = value; ChangeProperty(this, "TileImage"); }
        }

        public byte TileID
        {
            get { return tile.TileID; }
            set { tile.TileID = value; ChangeProperty(this, "TileID"); }
        }

        public byte Flags
        {
            get { return tile.Flags; }
            set { tile.Flags = value; ChangeProperty(this, "Flags"); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; ChangeProperty(this, "IsSelected"); }
        }
        #endregion

        public TileViewModel(Tile tile)
        {
            this.tile = tile;
            this.isSelected = false;
        }

        #region Private Functions
        #endregion
    }
}
