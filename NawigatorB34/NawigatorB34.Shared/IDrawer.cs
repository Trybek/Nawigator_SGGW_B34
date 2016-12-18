using Nawigator_SGGW_B34.Models;
using System.Threading.Tasks;

namespace Nawigator_SGGW_B34
{
    interface IDrawer
    { 
        void DrawPathFromRoomToCorridor(Room which, bool finish = false, bool start = true);
        void DrawPathFromRoomToStairsAndReverse();
        void DrawArrow(int x, int y, EnumPosition position, bool start = true);
        void DrawPathBetweenFloors();
        void DrawPathStartHorizontal(Room roomStart, Room roomFinish, bool start = true);
        void DrawPathStartVertical(Room roomStart, Room roomFinish, bool start = true);
        Task<object[]> DrawPath(Room start, Room finish);
    }
}
