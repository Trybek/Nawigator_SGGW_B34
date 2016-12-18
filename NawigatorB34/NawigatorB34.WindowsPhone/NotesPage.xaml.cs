using Nawigator_SGGW_B34.Common;
using Nawigator_SGGW_B34.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Nawigator_SGGW_B34
{
    public sealed partial class NotesPage : Page
    {
        private NotificationsWinPhone notifications;
        private SQLiteWinPhone databaseHelper;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public NotesPage()
        {
            notifications = new NotificationsWinPhone();
            databaseHelper = new SQLiteWinPhone();
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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
            }
            foreach (var item in GridAddNote.Children)
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
            }
            foreach (var item in menu.Items)
            {
                item.FontSize = App.FontSize;
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
            SetFontSize();
            MakeList();
        }
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }
        #endregion

        private void MakeList()
        {
            foreach (var item in ContentRoot.Children)
            {
                if (item is TextBlock)
                {
                    ContentRoot.Children.Remove(item as TextBlock);
                }
            }
            List<Notes> listOfNotes = databaseHelper.ReadNotes();
            if (listOfNotes.Count == 0)
            {
                return;
            }
            else
            {
                foreach (var note in listOfNotes)
                {
                    DateTime date = DateTime.ParseExact(note.TimeOfNote, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    if (date < DateTime.Now)
                    {
                        databaseHelper.DeleteNote(note.ID);
                    }
                    else
                    {
                        TextBlock text = new TextBlock()
                        {
                            FontSize = App.FontSize,
                            Text = $"{note.TextOfNote}\nW dniu: {note.TimeOfNote.Insert(10, " o godzinie ")} w {note.RoomName}\n",
                            Name = "item" + note.ID
                        };
                        text.Tapped += Text_Tapped;
                        ContentRoot.Children.Insert(0, text);
                    }
                }
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            GridAddNote.Visibility = Visibility.Visible;
            if (comboBox1.Items.Count == 0)
            {
                List<Room> listOfRooms = databaseHelper.ReadRooms();
                foreach (var item in listOfRooms)
                {
                    comboBox1.Items.Add(new ComboBoxItem() { Name = item.ID.ToString(), Content = item.Name });
                }
            }
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = string.Empty;
            }
            if (comboBox.SelectedIndex != -1)
            {
                comboBox.SelectedIndex = -1;
            }
        }
        private void b3_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty((comboBox1.SelectedItem as ComboBoxItem).Name))
            {
                notifications.ShowMessage(databaseHelper.FindRoomByID((comboBox1.SelectedItem as ComboBoxItem).Name));
            }
            else
            {
                notifications.ShowMessage(databaseHelper.FindRoomByID("1"));
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Notes note = new Notes();
            note.RoomID = int.Parse((comboBox1.SelectedItem as ComboBoxItem).Name);
            note.RoomName = (comboBox1.SelectedItem as ComboBoxItem).Content.ToString();
            note.TextOfNote = textBox.Text;
            note.TimeOfNote = datePicker.Date.ToString("dd-MM-yyyy") + " " + timePicker.Time.ToString(@"hh\:mm");
            databaseHelper.InsertNote(note);

            TextBlock text = new TextBlock()
            {
                FontSize = App.FontSize,
                Text = $"{note.TextOfNote}\nO godzinie:{note.TimeOfNote} w {note.RoomName}",
                Name = "item" + note.ID
            };
            text.Tapped += Text_Tapped;
            ContentRoot.Children.Insert(0, text);

            notifications.AddNotification(databaseHelper.FindRoomByID(note.RoomID.ToString()), DateTime.Parse(note.TimeOfNote));

            GridAddNote.Visibility = Visibility.Collapsed;
        }

        private void Text_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            int i = 1;
            foreach (var item in menu.Items)
            {
                item.Name = i + element.Name;
                i++;
            }
            menu.ShowAt(element);
        }
        private void MenuFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem items = (MenuFlyoutItem)sender;
            string test = (sender as MenuFlyoutItem).Name;
            int index = int.Parse(test.Remove(0, 5));
            databaseHelper.DeleteNote(index);
            MakeList();
        }
        private void MenuFlyoutEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuFlyoutItem)sender;
            string test = ((MenuFlyoutItem)sender).Name;
            int index = int.Parse(test.Remove(0, 5));
            //textBoxEdit.Text = (element.DataContext as SourceData).Frames.ToString();
            //if (textBoxEdit.Text == "0")
            //{
            //    textBlockEdit.Visibility = Visibility.Collapsed;
            //    textBoxEdit.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    textBlockEdit.Visibility = Visibility.Visible;
            //    textBoxEdit.Visibility = Visibility.Visible;
            //}
            //popUp.IsOpen = true;
        }

    }
}
