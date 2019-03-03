using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CreatureGameMapEditor.Controls
{

    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class AtlasDisplayControl : UserControl
    {
        #region Private Fields
        private int DisplaySize
        {
            get { return (int)GetValue(DisplaySizeProperty); }
            set { SetValue(DisplaySizeProperty, value); }
        }
        #endregion

        #region Public Properties
        public float ZoomLevel {get; set;}
        public float MaxZoomLevel { get; set; }
        public float MinZoomLevel { get; set; }
        public float ZoomInterval { get; set; }
        public int TileSize { get; set; }

        public ICommand SelectItem
        {
            get { return (ICommand)GetValue(SelectItemProperty); }
            set { SetValue(SelectItemProperty, value); }
        }
        
        public IList Items
        {
            get { return (IList)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        #endregion

        #region Dependancy Properties
        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(AtlasDisplayControl), new PropertyMetadata(-1));
        
        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IList), typeof(AtlasDisplayControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for DisplaySize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplaySizeProperty =
            DependencyProperty.Register("DisplaySize", typeof(int), typeof(AtlasDisplayControl), new PropertyMetadata(16));

        // Using a DependencyProperty as the backing store for SelectTileCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectItemProperty =
            DependencyProperty.Register("SelectItem", typeof(ICommand), typeof(AtlasDisplayControl), new PropertyMetadata(null));
        #endregion

        public AtlasDisplayControl()
        {
            // Default Zoom properties
            ZoomLevel = 1.5f;
            MaxZoomLevel = 3f;
            MinZoomLevel = 0.75f;
            ZoomInterval = 0.25f;

            TileSize = 16;
            CalculateDisplaySize();
            
            InitializeComponent();
        }

        private void CalculateDisplaySize()
        {
            DisplaySize = (int)(TileSize * ZoomLevel);
        }

        #region Events
        private void Event_PreZoom(object sender, MouseWheelEventArgs e)
        {
            
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                e.Handled = true;
            }
            
            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = ListBox.MouseWheelEvent;
            e2.Source = e.Source;

            AtlasContainer.RaiseEvent(e2);
        }

        private void Event_Zoom(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta < 0 && ZoomLevel >= MinZoomLevel)
                {
                    ZoomLevel -= ZoomInterval;
                }

                if (e.Delta > 0 && ZoomLevel <= MaxZoomLevel)
                {
                    ZoomLevel += ZoomInterval;
                }
                CalculateDisplaySize();
            }
        }

        private int last_index = -1;
        private void Event_SelectTile(object sender, MouseButtonEventArgs e)
        {
            if (SelectItem != null && SelectItem.CanExecute(e.GetPosition(sender as IInputElement)))
            {
                int index = AtlasContainer.SelectedIndex;
                if (index != -1)
                {
                    SelectItem.Execute((byte)index);
                    last_index = index;
                }
                else
                {
                    if (last_index != -1)
                    {
                        SelectItem.Execute((byte)last_index);
                    }
                }                
            }
        }
        #endregion
    }
}
