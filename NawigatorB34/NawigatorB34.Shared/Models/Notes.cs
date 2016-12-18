using SQLite;
using System;

namespace Nawigator_SGGW_B34.Models
{
    public class Notes
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }

        [NotNull]
        public string TimeOfNote { get; set; }

        [NotNull]
        public int RoomID { get; set; }

        [Ignore]
        public string RoomName { get; set; }

        [NotNull]
        public string TextOfNote { get; set; }

        public Notes()
        {

        }
        public Notes(DateTime timeOfNote, int roomID, string textOfNote)
        {
            TimeOfNote = timeOfNote.ToString("hh:mm:ss");
            RoomID = roomID;
            TextOfNote = textOfNote;
        }
        public Notes(DateTime timeOfNote, int roomID, string roomName, string textOfNote)
        {
            TimeOfNote = timeOfNote.ToString("hh:mm:ss");
            RoomID = roomID;
            RoomName = roomName;
            TextOfNote = textOfNote;
        }
    }
}
