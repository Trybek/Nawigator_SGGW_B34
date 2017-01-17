using System;
using System.Collections.Generic;
using NawigatorB34.Android.Models;
using Android.Content.Res;
using Android.Content;
using Android.Preferences;
using Android.App;
using Android.OS;

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
        public void AddNotification(Note note)
        {//nie chce mi si� tego robi�
        }
        public void AddNotification(Room room, DateTime time)
        {
            if (IsNotificationAllowed)
            {
                string nameRoom = room.Name.Replace("A", "Aula ")
                                           .Replace("BW", " �azienka damska").Replace("BM", " �azienka m�ska")
                                           .Replace("SDD", "Schody w d� od zielonej").Replace("SDU", "Schody w g�r� od zielonej")
                                           .Replace("SUD", "Schody w d� od ��tej").Replace("SUU", "Schody w g�r� od ��tej");

                Intent intent = new Intent(context, typeof(MainActivity));
                intent.PutExtra("RoomId", room.ID);
                const int pendingIntentId = 0;
                PendingIntent pendingIntent = PendingIntent.GetActivity(context, pendingIntentId, intent, PendingIntentFlags.OneShot);

                Notification.Builder builder = new Notification.Builder(context)
                        .SetAutoCancel(true)
                        .SetContentIntent(pendingIntent)
                        .SetContentTitle($"Zaj�cia za { timerNotifications } min w sali { nameRoom }")
                        .SetContentText("Chcesz zobaczy� map�?")
                        .SetSmallIcon(Resource.Drawable.Icon)
                        .SetWhen((DateTime.Now - time).Ticks);

                Notification notification = builder.Build();
                NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

                const int notificationId = 0;
                notificationManager.Notify(notificationId, notification);

            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(context);
                alert.SetMessage("Powiadomienia w aplikacji s� wy��czone, aby je w��czy� prosz� przej�� do ustawie�.");
                Dialog dialog = alert.Create();
                dialog.Show();
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
    }
}