﻿using Nawigator_SGGW_B34.Common;
using Nawigator_SGGW_B34.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Nawigator_SGGW_B34
{
    public sealed partial class NotesPage : Page
    {
        private INotifications notifications;
        private ISQLite databaseHelper;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private int index;

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
            foreach (var item in gridEdit.Children)
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
            comboBox1.Items.Clear();
            comboBoxEdit1.Items.Clear();
            List<Room> listOfRooms = databaseHelper.ReadRooms();
            foreach (var item in listOfRooms)
            {
                comboBox1.Items.Add(new ComboBoxItem() { Name = item.ID.ToString(), Content = item.Name });
                comboBoxEdit1.Items.Add(new ComboBoxItem() { Name = item.ID.ToString(), Content = item.Name });
            }

            for (int i = ContentRoot.Children.Count - 1; i >= 0; i--)
            {
                if (ContentRoot.Children[i] is TextBlock)
                {
                    ContentRoot.Children.Remove(ContentRoot.Children[i] as TextBlock);
                }
            }

            List<Note> listOfNotes = notifications.GetListOfNotes();
            if (listOfNotes.Count == 0)
            {
                return;
            }

            foreach (var note in listOfNotes)
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

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            GridAddNote.Visibility = Visibility.Visible;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = string.Empty;
            }
        }
        private void b3_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem != null && string.IsNullOrEmpty((comboBox1.SelectedItem as ComboBoxItem).Name))
            {
                notifications.ShowMessage(databaseHelper.FindRoomByID(int.Parse((comboBox1.SelectedItem as ComboBoxItem).Name)));
            }
            else
            {
                notifications.ShowMessage(databaseHelper.FindRoomByID(1));
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Note note = new Note();
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
            notifications.AddNotification(databaseHelper.FindRoomByID(note.RoomID), DateTime.Parse(note.TimeOfNote));
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
            index = int.Parse((sender as TextBlock).Name.Remove(0, 4));
            menu.ShowAt(element);
        }
        private void MenuFlyoutDelete_Click(object sender, RoutedEventArgs e)
        {
            databaseHelper.DeleteNote(index);
            MakeList();
        }
        private void MenuFlyoutEdit_Click(object sender, RoutedEventArgs e)
        {
            Note note = databaseHelper.FindNoteByID(index);
            DateTime date = DateTime.ParseExact(note.TimeOfNote, @"dd-MM-yyyy HH\:mm", CultureInfo.InvariantCulture);
            datePickerEdit.Date = date.Date;
            timePickerEdit.Time = date.TimeOfDay;
            textBoxEdit.Text = note.TextOfNote;

            foreach (var item in comboBoxEdit1.Items)
            {
                if ((item as ComboBoxItem).Name == note.RoomID.ToString())
                {
                    comboBoxEdit1.SelectedItem = item;
                    break;
                }
            }
            popUp.IsOpen = true;
        }

        private void popUp_Opened(object sender, object e)
        {
            if (RequestedTheme == ElementTheme.Dark)
            {
                gridEdit.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
            else if (RequestedTheme == ElementTheme.Light)
            {
                gridEdit.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
        }
        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            Note note = databaseHelper.FindNoteByID(index);
            note.RoomID = int.Parse((comboBoxEdit1.SelectedItem as ComboBoxItem).Name);
            note.RoomName = (comboBoxEdit1.SelectedItem as ComboBoxItem).Content.ToString();
            note.TextOfNote = textBoxEdit.Text;
            note.TimeOfNote = datePickerEdit.Date.ToString("dd-MM-yyyy") + " " + timePickerEdit.Time.ToString(@"hh\:mm");

            databaseHelper.UpdateNote(note);
            MakeList();
            popUp.IsOpen = false;
        }
    }
}
