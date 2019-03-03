using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CreatureGameMapEditor.Controls
{
    /// <summary>
    /// Interaction logic for TileFlagsControl.xaml
    /// </summary>
    public partial class TileFlagsControl : UserControl
    {
        #region Private Properties

        private bool DisplayFeatures
        {
            get { return (bool)GetValue(DisplayFeaturesProperty); }
            set { SetValue(DisplayFeaturesProperty, value); }
        }

        private bool DisplayJumpDirection
        {
            get { return (bool)GetValue(DisplayJumpDirectionProperty); }
            set { SetValue(DisplayJumpDirectionProperty, value); }
        }

        private bool DisplayEncounterNumber
        {
            get { return (bool)GetValue(DisplayEncounterNumberProperty); }
            set { SetValue(DisplayEncounterNumberProperty, value); }
        }

        private bool DisplayPortalNumber
        {
            get { return (bool)GetValue(DisplayPortalNumberProperty); }
            set { SetValue(DisplayPortalNumberProperty, value); }
        }

        #endregion
        #region Public Properties
        public byte Flags
        {
            get { return (byte)GetValue(FlagsProperty); }
            set { SetValue(FlagsProperty, value); }
        }
        #endregion
        #region Dependency Properties
        // Using a DependencyProperty as the backing store for DisplayFeatures.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayFeaturesProperty =
            DependencyProperty.Register("DisplayFeatures", typeof(bool), typeof(TileFlagsControl), new PropertyMetadata(false));
        // Using a DependencyProperty as the backing store for DisplayPortalNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayPortalNumberProperty =
            DependencyProperty.Register("DisplayPortalNumber", typeof(bool), typeof(TileFlagsControl), new PropertyMetadata(false));
        // Using a DependencyProperty as the backing store for DisplayEncounterNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayEncounterNumberProperty =
            DependencyProperty.Register("DisplayEncounterNumber", typeof(bool), typeof(TileFlagsControl), new PropertyMetadata(false));
        // Using a DependencyProperty as the backing store for DisplayJumpDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayJumpDirectionProperty =
            DependencyProperty.Register("DisplayJumpDirection", typeof(bool), typeof(TileFlagsControl), new PropertyMetadata(false));
        // Using a DependencyProperty as the backing store for Flags.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlagsProperty =
            DependencyProperty.Register("Flags", typeof(byte), typeof(TileFlagsControl), new PropertyMetadata(OnFlagsChangedCallBack));
        #endregion

        private static void OnFlagsChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TileFlagsControl c = sender as TileFlagsControl;
            if (c != null)
                c.DetermineAvailableSettings();
        }

        public TileFlagsControl()
        {
            DisplayEncounterNumber = false;
            DisplayPortalNumber = false;
            DisplayJumpDirection = false;
            DisplayFeatures = false;

            InitializeComponent(); 
        }

        private void DetermineAvailableSettings()
        {
            DisplayEncounterNumber = false;
            DisplayPortalNumber = false;
            DisplayJumpDirection = false;
            DisplayFeatures = false;

            if (DataContext == null)
            {
                return;
            }

            byte flag = Flags;
            switch ((flag >> 6) & 3)
            {
                // Ground
                case 0:
                    TileTypeSelection.SelectedIndex = 0;
                    DisplayFeatures = true;
                    break;
                // Water
                case 1:
                    TileTypeSelection.SelectedIndex = 1;
                    DisplayFeatures = true;
                    break;
                // Wall
                case 2:
                    TileTypeSelection.SelectedIndex = 2;
                    break;
            }

            if (DisplayFeatures == true)
            {
                if (((flag >> 5) & 1) == 1)
                {
                    TileFeatureSelection.SelectedIndex = 3;
                    PortalID.Value = (flag & 31);
                    DisplayPortalNumber = true;
                }
                else if(((flag >> 4) & 1) == 1)
                {
                    TileFeatureSelection.SelectedIndex = 2;
                    EncounterID.Value = (flag & 15);
                    DisplayEncounterNumber = true;
                }
                else if (((flag >> 3) & 1) == 1)
                {
                    TileFeatureSelection.SelectedIndex = 1;
                    JumpDirectionSelection.SelectedIndex = ((flag >> 1) & 3);
                    DisplayJumpDirection = true;
                }
            }
        }

        private void ModifyFlag()
        {
            byte flag = 0;
            if (TileTypeSelection == null || TileFeatureSelection == null || JumpDirectionSelection == null || EncounterID == null || PortalID == null)
            {
                return;
            }

            // Ground Tiles
            if (TileTypeSelection.SelectedIndex == 0)
            {
                flag = 0b00000000;
                if (TileFeatureSelection.SelectedIndex == 1)
                {
                    flag |= 0b00001000;
                    flag |= (byte)(JumpDirectionSelection.SelectedIndex << 1);
                }
                else if (TileFeatureSelection.SelectedIndex == 2)
                {
                    flag |= 0b00010000;
                    flag |= (byte)EncounterID.Value;
                }
                else if (TileFeatureSelection.SelectedIndex == 3)
                {
                    flag |= 0b00100000;
                    flag |= (byte)PortalID.Value;
                }
            }
            // Water
            else if (TileTypeSelection.SelectedIndex == 1)
            {
                flag = 0b01000000;
                if (TileFeatureSelection.SelectedIndex == 1)
                {
                    flag |= 0b00001000;
                    flag |= (byte)(JumpDirectionSelection.SelectedIndex << 1);
                }
                else if (TileFeatureSelection.SelectedIndex == 2)
                {
                    flag |= 0b00010000;
                    flag |= (byte)EncounterID.Value;
                }
                else if (TileFeatureSelection.SelectedIndex == 3)
                {
                    flag |= 0b00100000;
                    flag |= (byte)PortalID.Value;
                }
            }
            // Wall
            else if (TileTypeSelection.SelectedIndex == 2)
            {
                flag = 0b10000000;
            }
            Flags = flag;
        }

        #region Events
        private void Event_ChangeDataContext(object sender, DependencyPropertyChangedEventArgs e)
        {
            DetermineAvailableSettings();
        }

        private void Event_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModifyFlag();
        }

        private void Event_SliderValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ModifyFlag();
        }
        #endregion

    }
}
