using NawigatorB34.Android.Models;
using System;
using System.Collections.Generic;

namespace NawigatorB34.Android
{
    interface INotifications
    {
        ISQLite DatabaseHelper { get; }
        bool IsNotificationAllowed { get; }
        bool RemoveOldNotes { get; }

        void AddNotification(Room room, DateTime time);
        void RemoveExpiredNotes(ref List<Note> listOfNotes);
        void ShowMessage(Room room);//zbędne jest tylko do pokazania przykładowego powiadomienia

        List<Note> GetListOfNotes();
    }
}
