using Nawigator_SGGW_B34.Models;
using System;

namespace Nawigator_SGGW_B34
{
    interface INotifications
    {
        ISQLite databaseHelper { get; }
        void AddNotification(Room room, DateTime time);
        bool Exist();
        DateTime GetCurrentTime();
        bool IsNotificationAllowed();
        void RemoveExpiredMessage();
        void ShowMessage(Room room);
    }
}
