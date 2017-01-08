using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Util;
using Android.Widget;
using NawigatorB34.Android.Models;

namespace NawigatorB34.Android
{
    [Activity(Label = "NotesPage")]
    public class NotesPage : Activity
    {
        INotifications notifications;
        ISQLite databaseHelper;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NotesPage);
            notifications = new NotificationsAndroid(Assets, ApplicationContext);
            databaseHelper = new SQLiteAndroid(Assets);

            using (Button show = FindViewById<Button>(Resource.Id.button1))
            {
                show.Click += Show_Click;
            }

            List<Room> rooms = databaseHelper.ReadRoomsOnFloor(3);
            List<int> roomsID = new List<int>();
            List<string> roomsName = new List<string>();
            foreach (var item in rooms)
            {
                roomsName.Add(item.Name);
                roomsID.Add(item.ID);
            }
            rooms.Clear();

            using (Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1))
            {
                using (var ada = new ArrayAdapter(this, Resource.Layout.TextViewItem, roomsName.ToArray()))
                {
                    spinner.Adapter = ada;
                }
                spinner.ItemSelected += SpinnerRoom_ItemSelected;
            }
            using (Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerRoomID))
            {
                using (var ada = new ArrayAdapter(this, Resource.Layout.TextViewItem, roomsID.ToArray()))
                {
                    spinner.Adapter = ada;
                }
            }
            roomsID.Clear();
            roomsName.Clear();

            using (Button button = FindViewById<Button>(Resource.Id.button2))
            {
                button.Click += ButtonAdd_Click;
            }
            MakeList();
            SetFontSize();
        }

        private void SetFontSize()
        {

        }

        private void SpinnerRoom_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            using (Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerRoomID))
            {
                spinner.SetSelection(e.Position);
            }
        }

        private void MakeList()
        {
            using (var layout = Window.DecorView.FindViewById<LinearLayout>(Resource.Id.linearLayout1))
            {
                layout.RemoveViewsInLayout(1, layout.ChildCount - 5);
                foreach (var note in notifications.GetListOfNotes())
                {
                    using (TextView txt = new TextView(this))
                    {
                        txt.ContentDescription = note.ID.ToString(); ;
                        txt.SetTextSize(ComplexUnitType.Mm, 16);
                        txt.SetText($"{note.TextOfNote}\nW dniu: {note.TimeOfNote/*.Insert(10, " o godzinie ")*/} w {note.RoomName}\n", TextView.BufferType.Normal);
                        txt.Click += NoteTextView_Click;
                        layout.AddView(txt, 1);
                    }
                    SetContentView(layout);
                }
            }
        }

        private void NoteTextView_Click(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, (TextView)sender);
            menu.Menu.Add(0, 0, 0, (sender as TextView).ContentDescription);
            menu.Menu.SetGroupVisible(0, false);
            menu.MenuInflater.Inflate(Resource.Layout.PopupMenu, menu.Menu);
            menu.MenuItemClick += Menu_MenuItemClick;
            menu.Show();
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.TitleFormatted.ToString())
            {
                case "Usuñ":
                    {
                        var idOfNote = (sender as PopupMenu).Menu.GetItem(0).TitleFormatted.ToString();
                        databaseHelper.DeleteNote(int.Parse(idOfNote));

                        (sender as PopupMenu).Menu.RemoveItem(0);
                        (sender as PopupMenu).Dispose();

                        MakeList();
                        break;
                    }
                case "Edytuj":
                    {
                        var idOfNote = (sender as PopupMenu).Menu.GetItem(0).TitleFormatted.ToString();
                        Note note = databaseHelper.FindNoteByID(int.Parse(idOfNote));

                        (sender as PopupMenu).Menu.RemoveItem(0);
                        (sender as PopupMenu).Dispose();

                        EditNotesPopup editNote = new EditNotesPopup(Assets, ApplicationContext, int.Parse(idOfNote));
                        editNote.Show(FragmentManager, "dialog test");
                        editNote.EditComplete += EditNote_EditComplete;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void EditNote_EditComplete(object sender, EventArgs e)
        {
            MakeList();
        }

        private void Show_Click(object sender, EventArgs e)
        {
            notifications.AddNotification(databaseHelper.FindRoomByID(1), DateTime.Now);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Note note = new Note();
            using (EditText date = FindViewById<EditText>(Resource.Id.editTextDate))
            {
                using (EditText time = FindViewById<EditText>(Resource.Id.editTextHour))
                {
                    note.TimeOfNote = date.Text + "" + time.Text;//datePicker.ToString("dd-MM-yyyy") + " " + timePicker.CurrentHour + ":" + timePicker.CurrentMinute;
                }
            }
            using (Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerRoomID))
            {
                note.RoomID = int.Parse(spinner.SelectedItem.ToString());
            }

            using (Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner1))
            {
                note.RoomName = spinner.SelectedItem.ToString();
            }
            using (var editText = FindViewById<EditText>(Resource.Id.editTextOfNote))
            {
                note.TextOfNote = editText.Text;
            }

            databaseHelper.InsertNote(note);
            notifications.AddNotification(databaseHelper.FindRoomByID(note.RoomID), DateTime.Parse(note.TimeOfNote));

            using (var layout = Window.DecorView.FindViewById<LinearLayout>(Resource.Id.linearLayout1))
            {
                using (TextView txt = new TextView(this))
                {
                    txt.ContentDescription = note.ID.ToString(); ;
                    txt.SetTextSize(ComplexUnitType.Mm, 16);
                    txt.SetText($"{note.TextOfNote}\nW dniu: {note.TimeOfNote/*.Insert(10, " o godzinie ")*/} w {note.RoomName}\n", TextView.BufferType.Normal);
                    txt.Click += NoteTextView_Click;
                    layout.AddView(txt, 1);
                }
            }
        }
    }
}