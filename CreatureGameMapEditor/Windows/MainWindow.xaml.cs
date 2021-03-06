﻿using CreatureGameMapEditor.Models;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CreatureGameMapEditor.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MapEditorViewModel model = new MapEditorViewModel();
            this.DataContext = model;

            InitializeComponent();
        }

        private void Event_MenuNew(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Create a new map.");
        }
    }
}
