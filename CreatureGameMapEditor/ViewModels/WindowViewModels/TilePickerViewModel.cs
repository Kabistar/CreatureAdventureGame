using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CreatureGameMapEditor.ViewModels
{
    public class TilePickerViewModel : BaseViewModel
    {
        public AtlasViewModel Atlas { get; set; }
        public TileViewModel Tile { get; set; }

        public TilePickerViewModel(AtlasViewModel atlas, TileViewModel tile)
        {
            Atlas = atlas;
            Tile = tile;
        }
    }
}
