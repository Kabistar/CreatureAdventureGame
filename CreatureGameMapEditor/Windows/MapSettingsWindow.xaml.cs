using CreatureGameMapEditor.Models;
using CreatureGameMapEditor.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CreatureGameMapEditor.Windows
{
    /// <summary>
    /// Interaction logic for MapSettingsWindow.xaml
    /// </summary>
    public partial class MapSettingsWindow : Window
    {
        public MapSettingsWindow()
        {
            InitializeComponent();
        }

        #region Events

        private void Event_SelectAtlasFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            dlg.Multiselect = false;
            dlg.Title = "Select an atlas image";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                
                if (DataContext != null && DataContext.GetType() == typeof(MapSettingsViewModel)) {
                    (DataContext as MapSettingsViewModel).SelectAtlasFile.Execute(filename);
                }
            }
        }

        private void Event_SelectFillTile(object sender, RoutedEventArgs e)
        {
            if (DataContext != null && DataContext.GetType() == typeof(MapSettingsViewModel))
            {
                MapSettingsViewModel map = (DataContext as MapSettingsViewModel);
                Window w = new TilePickerWindow();
                w.DataContext = new TilePickerViewModel(map.Atlas, map.FillTile);
                w.Show();
            }
        }
        #endregion

        private void Event_ExitWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Event_ApplySettings(object sender, RoutedEventArgs e)
        {
            if (DataContext == null) return;
            
            MapSettingsViewModel vm = DataContext as MapSettingsViewModel;
            if (vm.ApplyChanges == null) return;
            vm.ApplyChanges.Execute(null);


            this.Close();
        }
    }
}
