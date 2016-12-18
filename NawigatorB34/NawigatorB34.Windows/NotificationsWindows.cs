using System;
using System.Collections.Generic;
using Nawigator_SGGW_B34.Models;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace Nawigator_SGGW_B34
{
    class NotificationsWindows : INotifications
    {
        public ISQLite databaseHelper { get { return new SQLiteWindows(); } }

        public void AddNotification(Room room, DateTime time)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastText = toastXml.GetElementsByTagName("text");
            var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
            toastElement.SetAttribute("launch", "toast://" + room.ID);
            string nameRoom = room.Name.Replace("A", "Aula ")
                                       .Replace("BW", " Łazienka damska").Replace("BM", " Łazienka męska")
                                       .Replace("SDD", "Schody w dół od zielonej").Replace("SDU", "Schody w górę od zielonej")
                                       .Replace("SUD", "Schody w dół od żółtej").Replace("SUU", "Schody w górę od żółtej");

            toastText[0].AppendChild(toastXml.CreateTextNode($"Zajęcia za {App.TimerNotifications} min w sali {nameRoom}"));       //Heading text of Notification   
            toastText[1].AppendChild(toastXml.CreateTextNode($"Chcesz zobaczyć mapę?"));    //Body text of Notification    

            var customAlarmScheduledToast = new ScheduledToastNotification(toastXml, time);
            toastNotifier.AddToSchedule(customAlarmScheduledToast);
        }

        public bool Exist()
        {
            throw new NotImplementedException();
        }

        public DateTime GetCurrentTime()
        {//to już w ogóle nie ma sensu :P
            return DateTime.Now;
        }

        public bool IsNotificationAllowed()
        {//W windows nie ma sensu 
            return true;
        }

        public void RemoveExpiredMessage()
        {
            List<Notes> listOfNotes = databaseHelper.ReadNotes();
            if (listOfNotes.Count == 0)
            {
                return;
            }
            else
            {
                foreach (var note in listOfNotes)
                {
                    DateTime date = DateTime.ParseExact(note.TimeOfNote, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    if (date < DateTime.Now)
                    {
                        databaseHelper.DeleteNote(note.ID);
                    }
                }
            }
        }

        public void ShowMessage(Room room)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var toastText = toastXml.GetElementsByTagName("text");
            var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
            toastElement.SetAttribute("launch", "toast://" + room.ID);

            string nameRoom = room.Name.Replace("A", "Aula ")
                                       .Replace("BW", " Łazienka damska").Replace("BM", " Łazienka męska")
                                       .Replace("SDD", "Schody w dół od zielonej").Replace("SDU", "Schody w górę od zielonej")
                                       .Replace("SUD", "Schody w dół od żółtej").Replace("SUU", "Schody w górę od żółtej");


            toastText[0].AppendChild(toastXml.CreateTextNode($"Zajęcia za {App.TimerNotifications} min w sali {nameRoom}"));       //Heading text of Notification   
            toastText[1].AppendChild(toastXml.CreateTextNode($"Chcesz zobaczyć mapę?"));    //Body text of Notification    

            var toast = new ToastNotification(toastXml);
            toastNotifier.Show(toast);
        }
    }
}
