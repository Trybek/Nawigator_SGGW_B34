﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.Collections.Generic;
using NawigatorB34.Android.Models;
using Android.Content;
using Android.Preferences;
using Android.Util;

namespace NawigatorB34.Android
{
    [Activity(Label = "NawigatorB34.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private IDrawer drawer;
        private ISQLite databaseHelper;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            databaseHelper = new SQLiteAndroid(Assets);
            drawer = new DrawerAndroid(ApplicationContext);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            using (Spinner spinner = FindViewById<Spinner>(Resource.Id.spinnerFloor))
            {
                spinner.ItemSelected += floorSpinner_ItemSelected;
                spinner.Adapter = new ArrayAdapter(this, Resource.Layout.TextViewItem,
                                  new string[] { "Wybierz piętro", "Przyziemie", "Parter", "1 piętro", "2 piętro", "3 piętro" });
            }
            using (Spinner spinnerStart = FindViewById<Spinner>(Resource.Id.spinnerStart))
            {
                using (Spinner spinnerFinish = FindViewById<Spinner>(Resource.Id.spinnerFinish))
                {
                    spinnerStart.ItemSelected += spinnerStart_ItemSelected;
                    spinnerFinish.ItemSelected += spinnerFinish_ItemSelected;
                }
            }

            using (Button settings = FindViewById<Button>(Resource.Id.buttonSettings))
            {
                settings.Click += (sender, e) =>
                {
                    var settingsPage = new Intent(this, typeof(SettingsPage));
                    StartActivity(settingsPage);
                };
            }
            using (Button notes = FindViewById<Button>(Resource.Id.buttonNotes))
            {
                notes.Click += (sender, e) =>
                {
                    var notesPage = new Intent(this, typeof(NotesPage));
                    StartActivity(notesPage);
                };
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            SetFontSize();
            MakeSpinners();
        }

        private void SetFontSize()
        {
            int fontSize;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                fontSize = prefs.GetInt("FontSize", 20);
            }

            using (TextView textViewTitle = FindViewById<TextView>(Resource.Id.textView5)) //jestem przy
            {
                textViewTitle.SetTextSize(ComplexUnitType.Mm, fontSize);
            }
            using (TextView textViewFrom = FindViewById<TextView>(Resource.Id.textView3)) //jestem przy
            {
                textViewFrom.SetTextSize(ComplexUnitType.Mm, fontSize);
            }
            using (TextView textViewTo = FindViewById<TextView>(Resource.Id.textView4)) //chce isc do
            {
                textViewTo.SetTextSize(ComplexUnitType.Mm, fontSize);
            }

            using (Button settings = FindViewById<Button>(Resource.Id.buttonNotes)) //ustawienia
            {
                settings.SetTextSize(ComplexUnitType.Mm, fontSize);
            }
            using (Button notes = FindViewById<Button>(Resource.Id.buttonSettings)) //notatki
            {
                notes.SetTextSize(ComplexUnitType.Mm, fontSize);
            }
        }
        private void MakeSpinners()
        {
            string readRoomsOnFloor = string.Empty;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                readRoomsOnFloor = prefs.GetString("ReadRoomsOnFloor", "-1;0;1;2;3");
            }

            List<Room> rooms = databaseHelper.ReadRooms();
            List<string> roomsName = new List<string>();
            List<int> roomsID = new List<int>();
            foreach (var item in rooms)
            {
                if (readRoomsOnFloor.Contains(item.Floor.ToString()))
                {
                    roomsName.Add(item.Name);
                    roomsID.Add(item.ID);
                }
            }
            rooms.Clear();

            using (Spinner spinnerID = FindViewById<Spinner>(Resource.Id.spinnerRoomID))
            {
                using (var ada = new ArrayAdapter(this, Resource.Layout.TextViewItem, roomsID.ToArray()))
                {
                    spinnerID.Adapter = ada;
                }
            }
            using (Spinner spinnerStart = FindViewById<Spinner>(Resource.Id.spinnerStart))
            {
                using (Spinner spinnerFinish = FindViewById<Spinner>(Resource.Id.spinnerFinish))
                {
                    spinnerStart.ItemSelected += spinnerStart_ItemSelected;
                    spinnerFinish.ItemSelected += spinnerFinish_ItemSelected;
                    using (var ada = new ArrayAdapter(this, Resource.Layout.TextViewItem, roomsName.ToArray()))
                    {
                        spinnerStart.Adapter = ada;
                        spinnerFinish.Adapter = ada;
                    }
                    roomsName.Clear();
                    roomsID.Clear();
                }
            }
        }

        private void spinnerFinish_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            using (Spinner spinnerStart = FindViewById<Spinner>(Resource.Id.spinnerStart))
            {
                if (e.Position != 0 && spinnerStart.SelectedItemPosition != 0)
                {
                    using (Spinner roomID = FindViewById<Spinner>(Resource.Id.spinnerRoomID))
                    {
                        Room start = databaseHelper.FindRoomByID(int.Parse(roomID.GetItemAtPosition(e.Position).ToString()));
                        Room finish = databaseHelper.FindRoomByID(int.Parse(roomID.GetItemAtPosition(e.Position).ToString()));
                        using (Bitmap map = drawer.DrawPath(start, finish)[0] as Bitmap)
                        {
                            using (ImageView image = FindViewById<ImageView>(Resource.Id.imageView1))
                            {
                                image.SetImageResource(Color.Transparent);
                                image.SetImageBitmap(map);
                            }
                        }
                    }
                }
            }
        }
        private void spinnerStart_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            using (Spinner spinnerFinish = FindViewById<Spinner>(Resource.Id.spinnerFinish))
            {
                if (e.Position != 0 && spinnerFinish.SelectedItemPosition != 0)
                {
                    using (Spinner roomID = FindViewById<Spinner>(Resource.Id.spinnerRoomID))
                    {
                        Room start = databaseHelper.FindRoomByID(int.Parse(roomID.GetItemAtPosition(e.Position).ToString()));
                        Room finish = databaseHelper.FindRoomByID(int.Parse(roomID.GetItemAtPosition(e.Position).ToString()));
                        using (Bitmap map = drawer.DrawPath(start, finish)[0] as Bitmap)
                        {
                            using (ImageView image = FindViewById<ImageView>(Resource.Id.imageView1))
                            {
                                image.SetImageResource(Color.Transparent);
                                image.SetImageBitmap(map);
                            }
                        }
                    }
                }
            }
        }

        private void floorSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            using (ImageView image = FindViewById<ImageView>(Resource.Id.imageView1))
            {
                switch (e.Position)
                {
                    case 1:
                        {
                            image.SetImageResource(Resource.Drawable.floorM1);
                            break;
                        }
                    case 2:
                        {
                            image.SetImageResource(Resource.Drawable.floor0);
                            break;
                        }
                    case 3:
                        {
                            image.SetImageResource(Resource.Drawable.floor1);
                            break;
                        }
                    case 4:
                        {
                            image.SetImageResource(Resource.Drawable.floor2);
                            break;
                        }
                    case 5:
                        {
                            image.SetImageResource(Resource.Drawable.floor3);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
    }
}

