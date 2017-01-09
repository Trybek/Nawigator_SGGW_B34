using System;
using System.Collections.Generic;
using NawigatorB34.Android.Models;
using Android.Content.Res;
using Android.Content;
using Android.Preferences;
using Android.App;

namespace NawigatorB34.Android
{
    class NotificationsAndroid : INotifications
    {
        private AssetManager asset;
        private Context context;
        private int timerNotifications;

        public NotificationsAndroid(AssetManager asset, Context context)
        {
            this.asset = asset;
            this.context = context;

            using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context))
            {
                timerNotifications = prefs.GetInt("TimerNotifications", 15);
            }
        }

        public ISQLite DatabaseHelper
        {
            get
            {
                return new SQLiteAndroid(asset);
            }
        }

        public bool IsNotificationAllowed
        {
            get
            {
                using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context))
                {
                    return prefs.GetBoolean("ShowNotifications", true);
                }
            }
        }

        public bool RemoveOldNotes
        {
            get
            {
                using (ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context))
                {
                    return prefs.GetBoolean("removeOldNotes", false);
                }
            }
        }

        public void AddNotification(Room room, DateTime time)
        {
            if (IsNotificationAllowed)
            {
                string nameRoom = room.Name.Replace("A", "Aula ")
                                           .Replace("BW", " �azienka damska").Replace("BM", " �azienka m�ska")
                                           .Replace("SDD", "Schody w d� od zielonej").Replace("SDU", "Schody w g�r� od zielonej")
                                           .Replace("SUD", "Schody w d� od ��tej").Replace("SUU", "Schody w g�r� od ��tej");

                Notification.Builder builder = new Notification.Builder(context)
                       .SetContentTitle($"Zaj�cia za { 51 }min w sali { nameRoom}")
                       .SetContentText("Chcesz zobaczy� map�?")
                       .SetSmallIcon(Resource.Drawable.Icon);

                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);

                //var customAlarmScheduledToast = new ScheduledToastNotification(toastXml, time);
                //toastNotifier.AddToSchedule(customAlarmScheduledToast);
            }
            else
            {
                //MessageDialog message = new MessageDialog("Powiadomienia w aplikacji s� wy��czone, aby je w��czy� prosz� przej�� do ustawie�.");
                //await message.ShowAsync();
            }
        }

        public List<Note> GetListOfNotes()
        {
            List<Note> listOfNotes = DatabaseHelper.ReadNotes();
            if (RemoveOldNotes)
            {
                RemoveExpiredNotes(ref listOfNotes);
            }
            return listOfNotes;
        }

        public void RemoveExpiredNotes(ref List<Note> listOfNotes)
        {
            if (listOfNotes.Count == 0)
            {
                return;
            }
            else
            {
                foreach (var note in listOfNotes)
                {
                    DateTime date = DateTime.ParseExact(note.TimeOfNote, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    if (date < DateTime.Now && RemoveOldNotes)
                    {
                        DatabaseHelper.DeleteNote(note.ID);
                    }
                }
                listOfNotes = DatabaseHelper.ReadNotes();
            }
        }

        public void ShowMessage(Room room)
        {
            if (IsNotificationAllowed)
            {
                string nameRoom = room.Name.Replace("A", "Aula ")
                                           .Replace("BW", " �azienka damska").Replace("BM", " �azienka m�ska")
                                           .Replace("SDD", "Schody w d� od zielonej").Replace("SDU", "Schody w g�r� od zielonej")
                                           .Replace("SUD", "Schody w d� od ��tej").Replace("SUU", "Schody w g�r� od ��tej");

                Notification.Builder builder = new Notification.Builder(context)
                       .SetContentTitle($"Zaj�cia za { timerNotifications } min w sali { nameRoom}")
                       .SetContentText("Chcesz zobaczy� map�?")
                       .SetSmallIcon(Resource.Drawable.Icon);

                // Build the notification:
                Notification notification = builder.Build();

                // Get the notification manager:
                NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

                // Publish the notification:
                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);
            }
            else
            {

            }
        }
    }
}