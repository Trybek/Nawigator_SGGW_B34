using Nawigator_SGGW_B34.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Nawigator_SGGW_B34
{
    public sealed partial class SettingsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public SettingsPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            toggleSwitch.IsOn = App.ShowNotifications;
            toggleSwitch1.IsOn = App.RemoveOldNotes;
            sliderFontSize.Value = App.FontSize;
            sliderTimerNotifications.Value = App.TimerNotifications;
            textBlock2.Text = App.FontSize.ToString();
            textBlock3.Text = App.TimerNotifications + " min";
            foreach (var item in comboBox.Items)
            {
                if (item is CheckBox)
                {
                    if (App.ReadRoomsOnFloor.Contains((item as CheckBox).Name.Remove(0, 8).Replace("11", "-1") + ";"))
                    {
                        (item as CheckBox).IsChecked = true;
                    }
                }
            }
        }
        private void SetFontSize()
        {
            foreach (var item in ContentRoot.Children)
            {
                if (item is TextBlock)
                {
                    (item as TextBlock).FontSize = App.FontSize;
                }
                else if (item is TextBox)
                {
                    (item as TextBox).FontSize = App.FontSize;
                }
                else if (item is ComboBox)
                {
                    (item as ComboBox).FontSize = App.FontSize;
                }
                else if (item is Button)
                {
                    (item as Button).FontSize = App.FontSize;
                }
                else if (item is Slider)
                {
                    (item as Slider).FontSize = App.FontSize;
                }
                else if(item is ToggleButton)
                {
                    (item as ToggleButton).FontSize = App.FontSize;
                }
            }
        }

        #region Default Methods or Variables
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SetFontSize();
            foreach (var item in comboBox.Items)
            {
                if (item is CheckBox)
                {
                    if (App.ReadRoomsOnFloor.Contains((item as CheckBox).Name.Remove(0, 8).Replace("11", "-1")))
                    {
                        (item as CheckBox).IsChecked = true;
                    }
                }
            }
            comboBox.Items.Add(App.ReadRoomsOnFloor);
            comboBox.SelectedItem = App.ReadRoomsOnFloor;
            this.navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }


        #endregion

        #region Sliders Value Changed
        private void sliderFontSize_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderFontSize != null)
            {
                int fontSize = (int)sliderFontSize.Value;
                var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

                if (settings.Values.ContainsKey("FontSize"))
                {
                    settings.Values.Remove("FontSize");
                }
                settings.Values.Add("FontSize", fontSize);

                App.FontSize = fontSize;
                textBlock2.Text = fontSize.ToString();

                SetFontSize();
            }
        }
        private void sliderTimerNotifications_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderTimerNotifications != null)
            {
                int timer = (int)sliderTimerNotifications.Value;
                var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

                if (settings.Values.ContainsKey("TimerNotifications"))
                {
                    settings.Values.Remove("TimerNotifications");
                }
                settings.Values.Add("TimerNotifications", timer);

                App.TimerNotifications = timer;
                //if (timer == 0)
                //{
                //    textBlock3.Text = "wyłączone";
                //}
                //else
                textBlock3.Text = timer + " min";
            }
        }
        #endregion
        #region Buttons Clicks
        private void buttonSelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in comboBox.Items)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = true;
                }
            }
        }
        private void buttonUnselectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in comboBox.Items)
            {
                if (item is CheckBox)
                {
                    (item as CheckBox).IsChecked = false;
                }
            }
        }
        #endregion
        #region Combobox Methods
        private void comboBox_DropDownClosed(object sender, object e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                buttonSelectAll_Click(comboBox.SelectedItem, null);
            }
            else if (comboBox.SelectedIndex == 1)
            {
                buttonUnselectAll_Click(comboBox.SelectedItem, null);
                comboBox.SelectedIndex = -1;
                return;
            }
            string names = string.Empty;

            CheckBox selected = null;
            if (comboBox.SelectedItem is CheckBox)
            {
                selected = comboBox.SelectedItem as CheckBox;
            }
            foreach (var item in comboBox.Items)
            {
                if (item is CheckBox)
                {
                    if ((item as CheckBox).IsChecked.Value || (selected != null && (item as CheckBox).Name == selected.Name))
                    {
                        names += (item as CheckBox).Name.Remove(0, 8).Replace("11", "-1") + ";";
                    }
                }
            }
            if (string.IsNullOrEmpty(names))
            {
                return;
            }
            names = names.Remove(names.Length - 1);
            comboBox.Items.Add(names);
            comboBox.SelectedItem = names;

            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (settings.Values.ContainsKey("ReadRoomsOnFloor"))
            {
                settings.Values.Remove("ReadRoomsOnFloor");
            }
            settings.Values.Add("ReadRoomsOnFloor", names);
            App.ReadRoomsOnFloor = names;
        }
        private void comboBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            comboBox.SelectedIndex = -1;
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex != -1 && comboBox.SelectedIndex != comboBox.Items.Count)
            {
                return;
            }
            foreach (var item in comboBox.Items)
            {
                if ((sender as ComboBox).SelectedIndex == -1)
                {
                    if (!(item is Button || item is CheckBox || item is ComboBoxItem))
                    {
                        comboBox.Items.Remove(item);
                    }
                }
            }

        }
        #endregion

        private void toggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (settings.Values.ContainsKey("ShowNotifications"))
            {
                settings.Values.Remove("ShowNotifications");
            }
            settings.Values.Add("ShowNotifications", toggleSwitch.IsOn);

            App.ShowNotifications = toggleSwitch.IsOn;
        }
        private void toggleSwitch1_Toggled(object sender, RoutedEventArgs e)
        {
            var settings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (settings.Values.ContainsKey("RemoveOldNotes"))
            {
                settings.Values.Remove("RemoveOldNotes");
            }
            settings.Values.Add("RemoveOldNotes", toggleSwitch.IsOn);

            App.RemoveOldNotes = toggleSwitch1.IsOn;
        }
    }
}