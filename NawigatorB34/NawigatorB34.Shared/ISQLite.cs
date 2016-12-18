using Nawigator_SGGW_B34.Models;
using System.Collections.Generic;

namespace Nawigator_SGGW_B34
{
    interface ISQLite
    {
        string DBPath { get; set; }
        List<Room> ReadRooms();
        List<Room> ReadRoomsOnFloor(int floor);
        List<Notes> ReadNotes();
        List<int> ReadFloors();
        Room GetStairsByName(string name);
        Room FindRoomByID(string ID);

        string GetNameRoomByID(string ID);
        bool CheckTableExists();
        void CopyDatabase();
        void InsertNote(Notes note);
        void DeleteNote(int id);
    }
}
