using CreatureGameMapEditor.ViewModels;
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
    /// Interaction logic for TilePickerWindow.xaml
    /// </summary>
    public partial class TilePickerWindow : Window
    {
        public TilePickerWindow()
        {
            InitializeComponent();
        }

        private void Event_CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
