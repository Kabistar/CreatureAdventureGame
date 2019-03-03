using CreatureGameMapEditor.Models;
using CreatureGameMapEditor.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CreatureGameMapEditor.ViewModels
{
    public class MapEditorViewModel : BaseViewModel
    {
        #region Private members
        MapViewModel map;
        #endregion

        #region Commands
        public ICommand ViewMapSettings { get; set; }
        #endregion

        #region Public members
        public MapViewModel Map
        {
            get { return map; }
            set { map = value; ChangeProperty(this, "Map"); }
        }
        #endregion

        public MapEditorViewModel()
        {
            //Atlas = new AtlasViewModel(this, "C:/Users/nickm/Desktop/gfx/grayscale.png");
            Map = new MapViewModel(new Map(0));

            ViewMapSettings = new RelayCommand(Command_ViewMapSettings);
        }

        private void Command_ViewMapSettings()
        {
            Window w = new MapSettingsWindow();
            w.DataContext = new MapSettingsViewModel(Map);
            w.Show();
        }
    }
}
