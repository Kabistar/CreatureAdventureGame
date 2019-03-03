using CreatureGameMapEditor.ViewModels;
using System;
using System.Collections;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CreatureGameMapEditor.Controls
{
    /// <summary>
    /// Interaction logic for MaxViewControl.xaml
    /// </summary>
    public partial class MapViewControl : UserControl
    {
        #region Public Properties
        public float MaxZoomLevel { get; set; }
        public float MinZoomLevel { get; set; }
        public float ZoomInterval { get; set; }
        public int TileSize { get; set; }

        public float ZoomLevel
        {
            get { return (float)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public ICommand ClickTileCommand
        {
            get { return (ICommand)GetValue(ClickTileCommandProperty); }
            set { SetValue(ClickTileCommandProperty, value); }
        }
        
        public ICommand DragTileCommand
        {
            get { return (ICommand)GetValue(DragTileCommandProperty); }
            set { SetValue(DragTileCommandProperty, value); }
        }

        public IList Items
        {
            get { return (IList)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public short Columns
        {
            get { return (short)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        
        public short Rows
        {
            get { return (short)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        #endregion

        #region Dependency Properties
        // Using a DependencyProperty as the backing store for DragTileCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragTileCommandProperty =
            DependencyProperty.Register("DragTileCommand", typeof(ICommand), typeof(MapViewControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(short), typeof(MapViewControl), new PropertyMetadata((short)0));
        
        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(short), typeof(MapViewControl), new PropertyMetadata((short)0));

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IList), typeof(MapViewControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ClickTileCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickTileCommandProperty =
            DependencyProperty.Register("ClickTileCommand", typeof(ICommand), typeof(MapViewControl), new PropertyMetadata(null));
        
        // Using a DependencyProperty as the backing store for ZoomLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(float), typeof(MapViewControl), new PropertyMetadata(1.5f));

        #endregion

        public MapViewControl()
        {
            ZoomLevel = 1.5f;
            MaxZoomLevel = 3f;
            MinZoomLevel = 0.75f;
            ZoomInterval = 0.25f;
            

            InitializeComponent();
        }

        #region Commands

        private void Command_IncreaseSize()
        {
            ushort width = (DataContext as MapViewModel).Width;
            ushort height = (DataContext as MapViewModel).Height;
            (DataContext as MapViewModel).SetMapSize((ushort)(width + 1), (ushort)(height + 1), new Models.Tile(0, 0));
        }

        #endregion

        #region Events
        private void Event_PreZoom(object sender, MouseWheelEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                e.Handled = true;

                var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                e2.RoutedEvent = ListBox.MouseWheelEvent;
                e2.Source = e.Source;

                MapContainer.RaiseEvent(e2);
            }
            else
            {
                e.Handled = true;

                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    Scroller.ScrollToHorizontalOffset(Scroller.HorizontalOffset - e.Delta);
                }
                else
                {
                    Scroller.ScrollToVerticalOffset(Scroller.VerticalOffset - e.Delta);
                }
                e.Handled = true;
            }
        }

        private void Event_Zoom(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0 && ZoomLevel >= MinZoomLevel)
            {
                ZoomLevel -= ZoomInterval;
            }

            if (e.Delta > 0 && ZoomLevel <= MaxZoomLevel)
            {
                ZoomLevel += ZoomInterval;
            }

            e.Handled = true;
        }

        private int lastPaintx = -1;
        private int lastPainty = -1;

        private void Event_StartPainting(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(sender as IInputElement);
            int x = (int)pos.X / 16;
            int y = (int)pos.Y / 16;

            lastPaintx = x;
            lastPainty = y;

            Point send = new Point(x, y);

            if (ClickTileCommand != null && ClickTileCommand.CanExecute(send))
            {
                ClickTileCommand.Execute(send);
            }
        }

        private void Event_ContinuePainting(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(sender as IInputElement);
                int x = (int)pos.X / 16;
                int y = (int)pos.Y / 16;

                if (x == lastPaintx && y == lastPainty) return;

                lastPaintx = x;
                lastPainty = y;

                Point send = new Point(x, y);

                if (DragTileCommand != null && DragTileCommand.CanExecute(send))
                {
                    DragTileCommand.Execute(send);
                }
            }
        }

        private void Event_StopPainting(object sender, MouseButtonEventArgs e)
        {
            lastPaintx = -1;
            lastPainty = -1;
        }
        #endregion
    }
}
