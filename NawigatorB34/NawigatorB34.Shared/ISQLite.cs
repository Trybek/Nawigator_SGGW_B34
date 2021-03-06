﻿using Nawigator_SGGW_B34.Models;
using System.Collections.Generic;

namespace Nawigator_SGGW_B34
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
