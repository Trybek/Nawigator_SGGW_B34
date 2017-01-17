using Nawigator_SGGW_B34.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace Nawigator_SGGW_B34
{
    class NotificationsWinPhone : INotifications
    {
        public ISQLite DatabaseHelper { get { return new SQLiteWinPhone(); } }
        public bool IsNotificationAllowed { get { return App.ShowNotifications; } } //Sprawdza wg ustawień, systemowo może być wyłączone nie wiem jak to sprawdzić 
        public bool RemoveOldNotes { get { return App.RemoveOldNotes; } }

        public async void AddNotification(Note note)
        {
            if (IsNotificationAllowed)
            {
                DateTime time = DateTime.ParseExact(note.TimeOfNote, @"dd-MM-yyyy HH\:mm", CultureInfo.InvariantCulture);
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                var toastText = toastXml.GetElementsByTagName("text");
                var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
                toastElement.SetAttribute("launch", "toast://" + note.RoomID);
                string nameRoom = note.RoomName.Replace("A", "Aula ")
                                               .Replace("BW", " Łazienka damska")
                                               .Replace("BM", " Łazienka męska")
                                               .Replace("F", " Bufet")
                                               .Replace("S", " Apteczka")
                                               .Replace("CY", "Szatnia żółta")
                                               .Replace("CG", "Szatnia zielona");

                toastText[0].AppendChild(toastXml.CreateTextNode($"{note.TextOfNote} za {App.TimerNotifications} min w {(nameRoom.Contains("Aula") ? "" : "sali")} {nameRoom}"));
                toastText[1].AppendChild(toastXml.CreateTextNode($"Chcesz zobaczyć mapę?"));

                var customAlarmScheduledToast = new ScheduledToastNotification(toastXml, time);
                toastNotifier.AddToSchedule(customAlarmScheduledToast);
            }
            else
            {
                MessageDialog message = new MessageDialog("Powiadomienia w aplikacji są wyłączone, aby je włączyć proszę przejść do ustawień.");
                await message.ShowAsync();
            }
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

        public async void ShowMessage(Room room)
        {
            if (IsNotificationAllowed)
            {
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                var toastText = toastXml.GetElementsByTagName("text");
                var toastElement = ((XmlElement)toastXml.SelectSingleNode("/toast"));
                toastElement.SetAttribute("launch", "toast://" + room.ID);

                string nameRoom = room.Name.Replace("A", "Aula ")
                                           .Replace("BW", " Łazienka damska")
                                           .Replace("BM", " Łazienka męska")
                                           .Replace("F", " Bufet")
                                           .Replace("S", " Apteczka")
                                           .Replace("CY", "Szatnia żółta")
                                           .Replace("CG", "Szatnia zielona");

                toastText[0].AppendChild(toastXml.CreateTextNode($"Zajęcia za {App.TimerNotifications} min w sali {nameRoom}"));       //Heading text of Notification   
                toastText[1].AppendChild(toastXml.CreateTextNode($"Chcesz zobaczyć mapę?"));    //Body text of Notification    

                var toast = new ToastNotification(toastXml);
                toastNotifier.Show(toast);
            }
            else
            {
                MessageDialog message = new MessageDialog("Powiadomienia w aplikacji są wyłączone, aby je włączyć proszę przejść do ustawień.");
                await message.ShowAsync();
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
    }
}
