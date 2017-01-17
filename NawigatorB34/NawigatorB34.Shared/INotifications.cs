using Nawigator_SGGW_B34.Models;
using System;
using System.Collections.Generic;

namespace Nawigator_SGGW_B34
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
