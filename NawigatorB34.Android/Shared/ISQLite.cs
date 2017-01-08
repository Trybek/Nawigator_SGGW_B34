using NawigatorB34.Android.Models;
using System.Collections.Generic;

namespace NawigatorB34.Android
{
    interface ISQLite
    {
        string DBPath { get; set; }

        List<Room> ReadRooms();
        List<Room> ReadRoomsOnFloor(int floor);
        List<Note> ReadNotes();
        List<int> ReadFloors();
        Room GetStairsByName(string name);
        Room FindRoomByID(int ID);
        Note FindNoteByID(int ID);

        string GetNameRoomByID(string ID);
        bool CheckTableExists();
        void CopyDatabase();
        void InsertNote(Note note);
        void UpdateNote(Note note);
        void DeleteNote(int id);
    }
}
