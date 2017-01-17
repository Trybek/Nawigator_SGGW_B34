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

        void AddNotification(Note note);
        void RemoveExpiredNotes(ref List<Note> listOfNotes);

        List<Note> GetListOfNotes();
    }
}
