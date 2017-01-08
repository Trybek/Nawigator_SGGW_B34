using NawigatorB34.Android.Models;

namespace NawigatorB34.Android
{
    interface IDrawer
    {
        void DrawPathFromRoomToCorridor(Room which, bool finish = false, bool start = true);
        void DrawPathFromRoomToStairsAndReverse();
        void DrawArrow(int x, int y, EnumPosition position, bool start = true);
        void DrawPathBetweenFloors();
        void DrawPathStartHorizontal(Room roomStart, Room roomFinish, bool start = true);
        void DrawPathStartVertical(Room roomStart, Room roomFinish, bool start = true);
        //Task<object[]> DrawPath(Room start, Room finish);//Dla windowsa i windowsa phone musi być async
        object[] DrawPath(Room start, Room finish);//Dla Androida nie musi być
    }
}
