using System;
using System.Collections.Generic;
using System.Linq;
using Nawigator_SGGW_B34.Models;
using SQLite;
using Windows.Storage;
using Windows.ApplicationModel;
using System.IO;

namespace Nawigator_SGGW_B34
{
    class SQLiteWindows : ISQLite
    {
        public string DBPath { get; set; }

        public SQLiteWindows()
        {
            DBPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "NawigatorDB.db3");
            if (!CheckTableExists())
            {
                CopyDatabase();
            }
        }

        #region Check if tables exists
        public bool CheckTableExists()
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                try
                {
                    List<Room> roomList = dbConn.Table<Room>().ToList();
                    List<Note> notesList = dbConn.Table<Note>().ToList();
                    return true;
                }
                catch (SQLiteException)
                {
                    return false;
                }
            }
        }
        public async void CopyDatabase()
        {
            StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("NawigatorDB.db3");
            StorageFile database = await ApplicationData.Current.LocalFolder.GetFileAsync("NawigatorDB.db3");
            await databaseFile.CopyAndReplaceAsync(database);
        }
        #endregion
        #region Find sth and return 
        public Room GetStairsByName(string name)
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                Room room = dbConn.Table<Room>().First(r => r.Name == name);
                return room;
            }
        }
        public Room FindRoomByID(string ID)
        {
            int id = int.Parse(ID);
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                Room room = dbConn.Table<Room>().First(r => r.ID == id);
                return room;
            }
        }
        public string GetNameRoomByID(string ID)
        {
            int id = int.Parse(ID);
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                Room room = dbConn.Table<Room>().First(r => r.ID == id);
                return room.Name;
            }

        }
        #endregion
        #region Read Sth from database
        public List<Room> ReadRooms()
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                List<Room> roomlist = dbConn.Table<Room>().ToList();
                return roomlist;
            }
        }
        public List<Room> ReadRoomsOnFloor(int floor)
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                List<Room> roomList = dbConn.Table<Room>().Where(r => r.Floor == floor).ToList();
                return roomList;
            }
        }
        public List<Note> ReadNotes()
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                if (dbConn.Table<Note>().Count() == 0)
                {
                    return new List<Note>();
                }
                List<Note> notes = dbConn.Table<Note>().ToList();
                List<Room> rooms = dbConn.Table<Room>().ToList();
                foreach (var item in notes)
                {
                    item.RoomName = rooms.First(r => r.ID == item.RoomID).Name;
                }
                return notes;
            }
        }
        public List<int> ReadFloors()
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                List<int> floors = new List<int>();
                List<Room> roomList = dbConn.Table<Room>().ToList();
                foreach (var room in roomList)
                {
                    if (!floors.Contains(room.Floor))
                    {
                        floors.Add(room.Floor);
                    }
                }
                return floors;
            }
        }
        #endregion
        #region Insert sth to database
        public void InsertNote(Note note)
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                dbConn.Insert(note);
            }
        }
        #endregion
        #region Delete sth from database
        public void DeleteNote(int id)
        {
            using (var dbConn = new SQLiteConnection(DBPath))
            {
                var note = dbConn.Table<Note>().First(n => n.ID == id);
                dbConn.Delete(note);
            }
        }
        #endregion
    }
}
