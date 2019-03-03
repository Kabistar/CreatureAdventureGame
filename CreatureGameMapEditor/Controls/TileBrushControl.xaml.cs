using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace CreatureGameMapEditor.Controls
{
    /// <summary>
    /// Interaction logic for TileBrushControl.xaml
    /// </summary>
    public partial class TileBrushControl : UserControl
    {
        #region Public Properties
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value);}
        }
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value);}
        }  
        public ICommand ChangeRows
        {
            get { return (ICommand)GetValue(ChangeRowsProperty); }
            set { SetValue(ChangeRowsProperty, value); }
        }
        public ICommand ChangeColumns
        {
            get { return (ICommand)GetValue(ChangeColumnsProperty); }
            set { SetValue(ChangeColumnsProperty, value); }
        }  

        public IList Items
        {
            get { return (IList)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        #endregion

        #region Dependency Properties
        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IList), typeof(TileBrushControl), new PropertyMetadata(CallBack_ItemsChanged));

        private static void CallBack_ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TileBrushControl brush = d as TileBrushControl;
            IList newList = e.NewValue as IList;

            if (newList != null && newList.Count > 0 && (brush.BrushTiles.SelectedIndex < 0 || brush.BrushTiles.SelectedIndex >= newList.Count))
            {
                brush.BrushTiles.SelectedIndex = 0;
            }
        }

        // Using a DependencyProperty as the backing store for ChangeColumns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangeColumnsProperty =
            DependencyProperty.Register("ChangeColumns", typeof(ICommand), typeof(TileBrushControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for ChangeRows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangeRowsProperty =
            DependencyProperty.Register("ChangeRows", typeof(ICommand), typeof(TileBrushControl), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(int), typeof(TileBrushControl), new PropertyMetadata(1));

        // Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(int), typeof(TileBrushControl), new PropertyMetadata(1));
        #endregion

        public TileBrushControl()
        {
            InitializeComponent();
        }


        private void Event_ChangeColumns(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Tuple<int, int> sliderValues = new Tuple<int, int>((int)e.OldValue, (int)e.NewValue);

            if (ChangeColumns != null && ChangeColumns.CanExecute(sliderValues))
            {
                ChangeColumns.Execute(sliderValues);
            }
        }

        private void Event_ChangeRows(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Tuple<int, int> sliderValues = new Tuple<int, int>((int)e.OldValue, (int)e.NewValue);

            if (ChangeRows != null && ChangeRows.CanExecute(sliderValues))
            {
                ChangeRows.Execute(sliderValues);
            }
        }
    }
}
