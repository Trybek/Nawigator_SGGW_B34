using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NawigatorB34.Android.Models;
using Android.Content.Res;
using Android.Preferences;

namespace NawigatorB34.Android
{
    class EditNotesPopup : DialogFragment
    {
        private ISQLite databaseHelper;

        private ArrayAdapter adapterRoomName;
        private ArrayAdapter adapterRoomID;

        private View view;
        private Note note;

        public event EventHandler EditComplete;

        public EditNotesPopup(AssetManager asset, Context context, int idNote) : base()
        {
            databaseHelper = new SQLiteAndroid(asset);
            note = databaseHelper.FindNoteByID(idNote);

            string readRoomsOnFloor = string.Empty;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context))
            {
                readRoomsOnFloor = prefs.GetString("ReadRoomsOnFloor", "-1;0;1;2;3");
            }

            List<Room> rooms = databaseHelper.ReadRoomsOnFloor(3);
            List<int> roomsID = new List<int>();
            List<string> roomsName = new List<string>();
            foreach (var item in rooms)
            {
                if (readRoomsOnFloor.Contains(item.Floor.ToString()))
                {
                    roomsName.Add(item.Name);
                    roomsID.Add(item.ID);
                }
            }
            rooms.Clear();

            adapterRoomName = new ArrayAdapter(context, Resource.Layout.TextViewItem, roomsName.ToArray());
            adapterRoomID = new ArrayAdapter(context, Resource.Layout.TextViewItem, roomsID.ToArray());
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PopupWindow, container, false);
            using (EditText editTextTime = view.FindViewById<EditText>(Resource.Id.editTextTime))
            {
                editTextTime.Text = note.TimeOfNote.Split(' ')[1];
            }
            using (EditText editTextDay = view.FindViewById<EditText>(Resource.Id.editTextDate))
            {
                editTextDay.Text = note.TimeOfNote.Split(' ')[0];
            }
            using (EditText editTextNote = view.FindViewById<EditText>(Resource.Id.editTextOfNote))
            {
                editTextNote.Text = note.TextOfNote;
            }
            using (Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinner1))
            {
                spinner.ItemSelected += (s, e) =>
                {
                    using (Spinner spinnerID = view.FindViewById<Spinner>(Resource.Id.spinnerRoomID))
                    {
                        spinnerID.SetSelection(e.Position);
                    }
                };
                spinner.Adapter = adapterRoomName;
                int a = adapterRoomName.GetPosition(note.RoomName);
                spinner.SetSelection(adapterRoomName.GetPosition(note.RoomName));
                adapterRoomName.Dispose();
            }
            using (Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerRoomID))
            {
                spinner.Adapter = adapterRoomID;
                //spinner.SetSelection(adapterRoomID.GetPosition(room.RoomID));
                adapterRoomID.Dispose();
            }

            using (Button btn = view.FindViewById<Button>(Resource.Id.button2))
            {
                btn.Click += BtnSaveChanges_Click;
            }

            this.view = view;
            return view;
        }

        private void BtnSaveChanges_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        public override void Dismiss()
        {
            using (EditText editTextTime = view.FindViewById<EditText>(Resource.Id.editTextTime))
            {
                using (EditText editTextDay = view.FindViewById<EditText>(Resource.Id.editTextDate))
                {
                    note.TimeOfNote = editTextDay.Text + " " + editTextTime.Text;
                }
            }
            using (EditText editTextNote = view.FindViewById<EditText>(Resource.Id.editTextOfNote))
            {
                note.TextOfNote=editTextNote.Text ;
            }
            using (Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinner1))
            {
                note.RoomName = spinner.SelectedItem.ToString();
            }
            using (Spinner spinner = view.FindViewById<Spinner>(Resource.Id.spinnerRoomID))
            {
                note.RoomID = int.Parse(spinner.SelectedItem.ToString());
            }
            databaseHelper.UpdateNote(note);
            view.Dispose();
            EditComplete.Invoke(this, null);
            base.Dismiss();
        }
    }
}