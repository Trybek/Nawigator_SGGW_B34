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
using Android.Preferences;
using Android.Util;

namespace NawigatorB34.Android
{
    [Activity(Label = "SettingsPage")]
    public class SettingsPage : Activity
    {
        bool showNotifications;
        bool removeOldNotes;
        int fontSize;
        int timerNotifications;
        string readRoomsOnFloor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingsPage);

            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                showNotifications = prefs.GetBoolean("ShowNotifications", true);
                removeOldNotes = prefs.GetBoolean("RemoveOldNotes", false);
                fontSize = prefs.GetInt("FontSize", 16);
                timerNotifications = prefs.GetInt("TimerNotifications", 15);
                readRoomsOnFloor = prefs.GetString("ReadRoomsOnFloor", "-1;0;2;3");
            }

            var tabFloors = readRoomsOnFloor.Split(';');
            using (CheckBox checkM1 = FindViewById<CheckBox>(Resource.Id.checkBox1))
            {
                if (tabFloors.Contains("-1"))
                {
                    checkM1.Checked = true;
                }
                checkM1.CheckedChange += CheckFloor_CheckedChange;
            }
            using (CheckBox check0 = FindViewById<CheckBox>(Resource.Id.checkBox2))
            {
                if (tabFloors.Contains("0"))
                {
                    check0.Checked = true;
                }
                check0.CheckedChange += CheckFloor_CheckedChange;
            }
            using (CheckBox check1 = FindViewById<CheckBox>(Resource.Id.checkBox3))
            {
                if (tabFloors.Contains("1"))
                {
                    check1.Checked = true;
                }
                check1.CheckedChange += CheckFloor_CheckedChange;
            }
            using (CheckBox check2 = FindViewById<CheckBox>(Resource.Id.checkBox4))
            {
                if (tabFloors.Contains("2"))
                {
                    check2.Checked = true;
                }
                check2.CheckedChange += CheckFloor_CheckedChange;
            }
            using (CheckBox check3 = FindViewById<CheckBox>(Resource.Id.checkBox5))
            {
                if (tabFloors.Contains("3"))
                {
                    check3.Checked = true;
                }
                check3.CheckedChange += CheckFloor_CheckedChange;
            }

            using (SeekBar seekBarFontSize = FindViewById<SeekBar>(Resource.Id.seekBarFontSize))
            {
                seekBarFontSize.Progress = fontSize;
                seekBarFontSize.StopTrackingTouch += SeekBarFontSize_StopTrackingTouch;
            }
            using (SeekBar seekBarTimerNotifications = FindViewById<SeekBar>(Resource.Id.seekBarTimer))
            {
                seekBarTimerNotifications.Progress = timerNotifications;
                seekBarTimerNotifications.StopTrackingTouch += SeekBarTimerNotifications_StopTrackingTouch;
            }

            using (Switch switchNotifications = FindViewById<Switch>(Resource.Id.switch1))
            {
                switchNotifications.Checked = showNotifications;
                switchNotifications.Click += SwitchNotifications_Click;
            }
            using (Switch switchOldNotes = FindViewById<Switch>(Resource.Id.switch2))
            {
                switchOldNotes.Checked = removeOldNotes;
                switchOldNotes.Click += SwitchOldNotes_Click;
            }
            SetFontSize();
        }

        private void SetFontSize()
        {
            using (LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.linearLayout1))
            {
                for (int i = 0; i < layout.ChildCount; i++)
                {
                    var element = layout.GetChildAt(i);
                    if (element is Button)
                    {
                        (element as Button).SetTextSize(ComplexUnitType.Sp, fontSize);
                    }
                    else if (element is TextView)
                    {
                        (element as TextView).SetTextSize(ComplexUnitType.Sp, fontSize);
                    }
                    else if (element is EditText)
                    {
                        (element as TextView).SetTextSize(ComplexUnitType.Sp, fontSize);
                    }
                    else if (element is CheckBox)
                    {
                        (element as CheckBox).SetTextSize(ComplexUnitType.Sp, fontSize);
                    }
                    else if (element is Switch)
                    {
                        (element as Switch).SetTextSize(ComplexUnitType.Sp, fontSize);
                    }
                }
            }
        }

        private void CheckFloor_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            string floorChecked = string.Empty;
            using (CheckBox checkM1 = FindViewById<CheckBox>(Resource.Id.checkBox1))
            {
                if (checkM1.Checked)
                {
                    floorChecked += "-1;";
                }
            }
            using (CheckBox check0 = FindViewById<CheckBox>(Resource.Id.checkBox2))
            {
                if (check0.Checked)
                {
                    floorChecked += "0;";
                }
            }
            using (CheckBox check1 = FindViewById<CheckBox>(Resource.Id.checkBox3))
            {
                if (check1.Checked)
                {
                    floorChecked += "1;";
                }
            }
            using (CheckBox check2 = FindViewById<CheckBox>(Resource.Id.checkBox4))
            {
                if (check2.Checked)
                {
                    floorChecked += "2;";
                }
            }
            using (CheckBox check3 = FindViewById<CheckBox>(Resource.Id.checkBox5))
            {
                if (check3.Checked)
                {
                    floorChecked += "3;";
                }
            }
            if (!string.IsNullOrEmpty(floorChecked))
            {
                floorChecked = floorChecked.Remove(floorChecked.Length - 1);
            }
            readRoomsOnFloor = floorChecked;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                using (ISharedPreferencesEditor editor = prefs.Edit())
                {
                    editor.PutString("ReadRoomsOnFloor", readRoomsOnFloor);
                    editor.Apply();
                }
            }
        }

        private void SeekBarFontSize_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            fontSize = e.SeekBar.Progress;
            if(fontSize < 10)
            {
                fontSize = 10;
                e.SeekBar.Progress = 10;
            }
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                using (ISharedPreferencesEditor editor = prefs.Edit())
                {
                    editor.PutInt("FontSize", fontSize);
                    editor.Apply();
                }
            }
            SetFontSize();
        }

        private void SeekBarTimerNotifications_StopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            timerNotifications = e.SeekBar.Progress;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                using (ISharedPreferencesEditor editor = prefs.Edit())
                {
                    editor.PutInt("TimerNotifications", timerNotifications);
                    editor.Apply();
                }
            }
        }

        private void SwitchOldNotes_Click(object sender, EventArgs e)
        {
            removeOldNotes = ((Switch)sender).Checked;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                using (ISharedPreferencesEditor editor = prefs.Edit())
                {
                    editor.PutBoolean("RemoveOldNotes", removeOldNotes);
                    editor.Apply();
                }
            }
        }

        private void SwitchNotifications_Click(object sender, EventArgs e)
        {
            showNotifications = ((Switch)sender).Checked;
            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext))
            {
                using (ISharedPreferencesEditor editor = prefs.Edit())
                {
                    editor.PutBoolean("ShowNotifications", showNotifications);
                    editor.Apply();
                }
            }
        }
    }
}