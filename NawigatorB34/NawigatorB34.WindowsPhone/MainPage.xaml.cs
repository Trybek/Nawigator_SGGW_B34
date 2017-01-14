using Nawigator_SGGW_B34.Models;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Nawigator_SGGW_B34
{
    public sealed partial class MainPage : Page
    {
        private ISQLite databaseHelper;
        private IDrawer drawer;
        private Room roomStart;
        private Room roomFinish;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            databaseHelper = new SQLiteWinPhone();
            drawer = new DrawerWinPhone();

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            comboBox.Items.Add(new ComboBoxItem()
            {
                Content = "Przyziemie",
                Name = "floor11",
                FontSize = App.FontSize
            });
            comboBox.Items.Add(new ComboBoxItem()
            {
                Content = "Parter",
                Name = "floor0",
                FontSize = App.FontSize
            });
            comboBox.Items.Add(new ComboBoxItem()
            {
                Content = "1 piętro",
                Name = "floor1",
                FontSize = App.FontSize
            });
            comboBox.Items.Add(new ComboBoxItem()
            {
                Content = "2 piętro",
                Name = "floor2",
                FontSize = App.FontSize
            });
            comboBox.Items.Add(new ComboBoxItem()
            {
                Content = "3 piętro",
                Name = "floor3",
                FontSize = App.FontSize
            });
        }

        private void MakeMainPage()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            var list = databaseHelper.ReadRooms();
            string[] tab = App.ReadRoomsOnFloor.Split(';');
            foreach (Room room in list)
            {
                if (tab.Contains(room.Floor.ToString()))
                {
                    comboBox1.Items.Add(new ComboBoxItem()
                    {
                        Name = room.ID.ToString(),
                        Content = room.Name.Replace("A", "Aula ")
                                            .Replace("BW", " Łazienka damska").Replace("BM", " Łazienka męska")
                                            .Replace("SDD", "Schody w dół od zielonej").Replace("SDU", "Schody w górę od zielonej")
                                            .Replace("SUD", "Schody w dół od żółtej").Replace("SUU", "Schody w górę od żółtej"),
                        FontSize = App.FontSize
                    });
                    comboBox2.Items.Add(new ComboBoxItem()
                    {
                        Name = room.ID.ToString(),
                        Content = room.Name.Replace("A", "Aula ")
                                            .Replace("BW", " Łazienka damska").Replace("BM", " Łazienka męska")
                                            .Replace("SDD", "Schody w dół od zielonej").Replace("SDU", "Schody w górę od zielonej")
                                            .Replace("SUD", "Schody w dół od żółtej").Replace("SUU", "Schody w górę od żółtej"),
                        FontSize = App.FontSize
                    });
                }
            }
            if (App.IDRoomNotification != -1)
            {
                foreach (var item in comboBox2.Items)
                {
                    if (item is ComboBoxItem)
                    {
                        if ((item as ComboBoxItem).Name == App.IDRoomNotification.ToString())
                        {
                            comboBox2.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (!e.Handled && Frame.CurrentSourcePageType.FullName == "Nawigator_SGGW_B34.MainPage")
            {
                Application.Current.Exit();
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MakeMainPage();
        }

        #region ComboBox Events
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == -1)
            {
                return;
            }
            scrollViewer.Visibility = Visibility.Visible;
            scrollViewer2.Visibility = Visibility.Collapsed;

            progressRing.Visibility = Visibility.Visible;
            progressRing.IsActive = true;

            BitmapImage bm = new BitmapImage(new Uri($"ms-appx:///Floors/{(comboBox.SelectedItem as ComboBoxItem).Name.Remove(0, 5).Replace("11", "-1")}.png", UriKind.Absolute));
            image.Source = bm;

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
        }
        private async void comboBoxRooms_DropDownClosed(object sender, object e)
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                progressRing.Visibility = Visibility.Visible;
                progressRing.IsActive = true;

                comboBox.SelectedIndex = -1;

                roomStart = databaseHelper.FindRoomByID(int.Parse((comboBox1.SelectedItem as ComboBoxItem).Name));
                roomFinish = databaseHelper.FindRoomByID(int.Parse((comboBox2.SelectedItem as ComboBoxItem).Name));

                object[] images = await drawer.DrawPath(roomStart, roomFinish);
                if (images.Length == 1)
                {
                    scrollViewer.Visibility = Visibility.Visible;
                    scrollViewer2.Visibility = Visibility.Collapsed;
                    image.Source = images[0] as WriteableBitmap;
                    textBlock3.Visibility = Visibility.Collapsed;
                }
                else
                {
                    scrollViewer.Visibility = Visibility.Visible;
                    scrollViewer2.Visibility = Visibility.Visible;
                    image.Source = images[0] as WriteableBitmap;
                    image2.Source = images[1] as WriteableBitmap;

                    int floorDifference = roomStart.Floor - roomFinish.Floor;
                    if (floorDifference == -1)
                    {
                        textBlock3.Text = "Piętro wyżej";
                    }
                    else if (floorDifference < 0)
                    {
                        textBlock3.Text = $"{-floorDifference} pięter wyżej";
                    }
                    else if (floorDifference == 1)
                    {
                        textBlock3.Text = "Piętro niżej";
                    }
                    else
                    {
                        textBlock3.Text = $"{floorDifference} pięter niżej";
                    }
                    textBlock3.Visibility = Visibility.Visible;
                }

                progressRing.IsActive = false;
                progressRing.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Navigate Buttons
        private void ButtonNotes_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NotesPage));
        }
        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }
        #endregion
    }
}
