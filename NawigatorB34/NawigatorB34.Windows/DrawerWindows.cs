using System;
using System.Threading.Tasks;
using Nawigator_SGGW_B34.Models;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Nawigator_SGGW_B34
{
    class DrawerWindows : IDrawer
    {
        private SQLiteWindows databaseHelper = new SQLiteWindows();
        private WriteableBitmap writeableBmpFloorStart;
        private WriteableBitmap writeableBmpFloorFinish;
        private Room roomStart;
        private Room roomFinish;
        private Room stairsStart;
        private Room stairsFinish;

        #region Draw Methods
        public void DrawArrow(int x, int y, EnumPosition position, bool start = true)
        {
            WriteableBitmap floorMap;
            if (start)
            {
                floorMap = writeableBmpFloorStart;
            }
            else
            {
                floorMap = writeableBmpFloorFinish;
            }

            switch (position)
            {
                case EnumPosition.Up:
                    {
                        floorMap.DrawLineAa(x, y, x - 30, y + 30, Color.FromArgb(255, 0, 0, 0), 10);
                        floorMap.DrawLineAa(x, y, x + 30, y + 30, Color.FromArgb(255, 0, 0, 0), 10);
                        break;
                    }
                case EnumPosition.Right:
                    {
                        floorMap.DrawLineAa(x, y, x - 30, y - 30, Color.FromArgb(255, 0, 0, 0), 10);
                        floorMap.DrawLineAa(x, y, x - 30, y + 30, Color.FromArgb(255, 0, 0, 0), 10);
                        break;
                    }
                case EnumPosition.Down:
                    {
                        floorMap.DrawLineAa(x, y, x - 30, y - 30, Color.FromArgb(255, 0, 0, 0), 10);
                        floorMap.DrawLineAa(x, y, x + 30, y - 30, Color.FromArgb(255, 0, 0, 0), 10);
                        break;
                    }
                case EnumPosition.Left:
                    {
                        floorMap.DrawLineAa(x, y, x + 30, y - 30, Color.FromArgb(255, 0, 0, 0), 10);
                        floorMap.DrawLineAa(x, y, x + 30, y + 30, Color.FromArgb(255, 0, 0, 0), 10);
                        break;
                    }
                default:
                    break;
            }
        }

        public void DrawPathBetweenFloors()
        {
            DrawPathFromRoomToStairsAndReverse();

            if ((roomStart.X == 165 && stairsStart.X == 165) ||
                (roomStart.X == 735 && stairsStart.X == 735) ||
                (roomStart.X == 1305 && stairsStart.X == 1305) ||
                (roomStart.X == 1880 && stairsStart.X == 1880) ||
                (roomStart.Y == 380 && stairsStart.Y == 380) ||
                (roomStart.Y == 990 && stairsStart.Y == 990))
            {
                //Nic nie trzeba robić :D
            }
            else if (roomStart.Y > 380 && roomStart.Y < 990)
            {
                DrawPathStartHorizontal(roomStart, stairsStart);
            }
            else
            {
                DrawPathStartVertical(roomStart, stairsStart);
            }
            writeableBmpFloorStart.DrawLineAa((int)roomStart.X, (int)roomStart.Y, (int)stairsStart.X, (int)stairsStart.Y, Color.FromArgb(255, 0, 0, 0), 10);

            if ((stairsFinish.X == 165 && roomFinish.X == 165) ||
                (stairsFinish.X == 735 && roomFinish.X == 735) ||
                (stairsFinish.X == 1305 && roomFinish.X == 1305) ||
                (stairsFinish.X == 1880 && roomFinish.X == 1880) ||
                (stairsFinish.Y == 380 && roomFinish.Y == 380) ||
                (stairsFinish.Y == 990 && roomFinish.Y == 990))
            {
                //Nic nie trzeba robić :D
            }
            else if (stairsFinish.Y > 380 && stairsFinish.Y < 990)
            {
                DrawPathStartHorizontal(stairsFinish, roomFinish, false);
            }
            else
            {
                DrawPathStartVertical(stairsFinish, roomFinish, false);
            }
            writeableBmpFloorFinish.DrawLineAa((int)stairsFinish.X, (int)stairsFinish.Y, (int)roomFinish.X, (int)roomFinish.Y, Color.FromArgb(255, 0, 0, 0), 10);
        }

        public void DrawPathFromRoomToCorridor(Room which, bool finish = false, bool start = true)
        {
            WriteableBitmap floorMap;
            if (start)
            {
                floorMap = writeableBmpFloorStart;
            }
            else
            {
                floorMap = writeableBmpFloorFinish;
            }
            //schody do góry X 1110 - rysuj do 1195
            //schody na dół  X 930  - rysuj do 850
            switch ((int)which.X)
            {
                case 100:
                case 110:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 165, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 165;
                        break;
                    }
                case 215:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 165, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 165;
                        break;
                    }
                case 675:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 735, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 735;
                        break;
                    }
                case 800:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 735, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 735;
                        break;
                    }
                case 930:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 850, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 850;
                        break;
                    }
                case 1110:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 1195, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 1195;
                        break;
                    }
                case 1250:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 1305, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 1305;
                        break;
                    }
                case 1370:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 1305, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 1305;
                        break;
                    }
                case 1830:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 1880, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 1880;
                        break;
                    }
                case 1935:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, 1880, (int)which.Y, Color.FromArgb(255, 0, 0, 0), 10);
                        which.X = 1880;
                        break;
                    }
                default:
                    break;
            }

            //schody u góry  Y 1250
            //schody u dołu  Y 110
            switch ((int)which.Y)
            {
                case 110:
                    {
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, (int)which.X, 380, Color.FromArgb(255, 0, 0, 0), 10);
                        which.Y = 380;
                        break;
                    }
                case 295:
                case 300:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Up, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, (int)which.X, 380, Color.FromArgb(255, 0, 0, 0), 10);
                        which.Y = 380;
                        break;
                    }
                case 450:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Down, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, (int)which.X, 380, Color.FromArgb(255, 0, 0, 0), 10);
                        which.Y = 380;
                        break;
                    }
                case 905:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Up, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, (int)which.X, 990, Color.FromArgb(255, 0, 0, 0), 10);
                        which.Y = 990;
                        break;
                    }
                case 1055:
                case 1060:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Down, start);
                        }
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, (int)which.X, 990, Color.FromArgb(255, 0, 0, 0), 10);
                        which.Y = 990;
                        break;
                    }
                case 1250:
                    {
                        floorMap.DrawLineAa((int)which.X, (int)which.Y, (int)which.X, 990, Color.FromArgb(255, 0, 0, 0), 10);
                        which.Y = 990;
                        break;
                    }
                default:
                    break;
            }

        }

        public void DrawPathFromRoomToStairsAndReverse()
        {
            DrawPathFromRoomToCorridor(roomStart);
            DrawPathFromRoomToCorridor(roomFinish, true, false);

            //SDD   1110    1250    schody na dole na dół
            //SDU   930     1250    schody na dole do góry
            //SUU   1110    110     schody na górze do góry
            //SUD   930     110     schody na górze na dół 

            if (roomStart.Floor - roomFinish.Floor < 0)
            {//w dół
                if (roomStart.Y > 685)
                {//wprowadzić sztywno schody
                    stairsStart = new Room(roomStart.Floor, "SDU", 930, 1250);
                    stairsFinish = new Room(roomFinish.Floor, "SDD", 1110, 1250);
                }
                else
                {
                    stairsStart = new Room(roomStart.Floor, "SUU", 1110, 110);
                    stairsFinish = new Room(roomFinish.Floor, "SUD", 930, 110);
                }
            }
            else
            {//w górę
                if (roomStart.Y > 685)
                {
                    stairsStart = new Room(roomStart.Floor, "SDD", 1110, 1250);
                    stairsFinish = new Room(roomFinish.Floor, "SDU", 930, 1250);
                }
                else
                {
                    stairsStart = new Room(roomStart.Floor, "SUD", 930, 110);
                    stairsFinish = new Room(roomFinish.Floor, "SUU", 1110, 110);
                }
            }
            DrawPathFromRoomToCorridor(stairsStart, true);
            DrawPathFromRoomToCorridor(stairsFinish, false, false);
        }

        public void DrawPathStartHorizontal(Room roomStart, Room roomFinish, bool start = true)
        {
            WriteableBitmap floorMap;
            if (start)
            {
                floorMap = writeableBmpFloorStart;
            }
            else
            {
                floorMap = writeableBmpFloorFinish;
            }
            int newY = 0;
            if (Math.Abs(roomFinish.Y - roomStart.Y) < 610 &&
                Math.Abs(roomFinish.X - roomStart.X) == 0)
            {
                return;
            }
            else if (roomStart.Y <= 685 && roomFinish.Y <= 685)
            {
                newY = 380;
            }
            else if (roomStart.Y > 685 && roomFinish.Y > 685)
            {
                newY = 990;
            }
            else if (roomStart.Y <= 685 && roomFinish.Y > 685)
            {
                newY = 990;
            }
            else if (roomStart.Y > 685 && roomFinish.Y <= 685)
            {
                newY = 380;
            }
            floorMap.DrawLineAa((int)roomStart.X, (int)roomStart.Y, (int)roomStart.X, newY, Color.FromArgb(255, 0, 0, 0), 10);
            floorMap.DrawLineAa((int)roomFinish.X, (int)roomFinish.Y, (int)roomFinish.X, newY, Color.FromArgb(255, 0, 0, 0), 10);
            roomStart.Y = newY;
            roomFinish.Y = newY;

        }
        public void DrawPathStartVertical(Room roomStart, Room roomFinish, bool start = true)
        {
            WriteableBitmap floorMap;
            if (start)
            {
                floorMap = writeableBmpFloorStart;
            }
            else
            {
                floorMap = writeableBmpFloorFinish;
            }

            int newX = 0;
            //165, 735, 1305, 1880
            //  450, 1020, 1590
            //  600,  570,  575
            //A       X <= 450
            //B 450 < X <= 1020
            //C 1020< X <= 1590
            //D 1590< X

            if (Math.Abs(roomFinish.X - roomStart.X) < 575 &&
                Math.Abs(roomFinish.Y - roomStart.Y) == 0)
            {
                return;
            }
            //↓roomStart jest w przedziale A
            else if (roomStart.X <= 450 &&//AA
                roomFinish.X <= 450)
            {
                newX = 165;
            }
            else if (roomStart.X <= 450 &&//AB
                     roomFinish.X > 450 && roomFinish.X <= 1020)
            {
                newX = 735;
            }
            else if (roomStart.X <= 450 &&//AC
                     roomFinish.X > 1020 && roomFinish.X <= 1590)
            {
                newX = 1305;
            }
            else if (roomStart.X <= 450 &&//AD
                     roomFinish.X > 1590)
            {
                newX = 1880;
            }
            //↓roomStart jest w przedziale B
            else if (roomStart.X > 450 && roomStart.X <= 1020 &&//BA
                     roomFinish.X <= 450)
            {
                newX = 165;
            }
            else if (roomStart.X > 450 && roomStart.X <= 1020 &&//BB
                    roomFinish.X > 450 && roomFinish.X <= 1020)
            {
                newX = 735;
            }
            else if (roomStart.X > 450 && roomStart.X <= 1020 &&//BC
                     roomFinish.X > 1020 && roomFinish.X <= 1590)
            {
                newX = 1305;
            }
            else if (roomStart.X > 450 && roomStart.X <= 1020 &&//BD
                     roomFinish.X > 1590)
            {
                newX = 1880;
            }
            //↓roomStart jest w przedziale C
            else if (roomStart.X > 1020 && roomStart.X <= 1590 &&//CA
                     roomFinish.X <= 450)
            {
                newX = 165;
            }
            else if (roomStart.X > 1020 && roomStart.X <= 1590 &&//CB
                     roomFinish.X > 450 && roomFinish.X <= 1000)
            {
                newX = 735;
            }
            else if (roomStart.X > 1020 && roomStart.X <= 1590 &&//CC
                     roomFinish.X > 1000 && roomFinish.X <= 1590)
            {
                newX = 1305;
            }
            else if (roomStart.X > 1020 && roomStart.X <= 1590 &&//CD
                     roomFinish.X > 1590)
            {
                newX = 1880;
            }
            //↓roomStart jest w przedziale D
            else if (roomStart.X > 1590 &&//DA
                     roomFinish.X <= 450)
            {
                newX = 165;
            }
            else if (roomStart.X > 1590 &&//DB
                     roomFinish.X > 450 && roomFinish.X <= 1000)
            {
                newX = 735;
            }
            else if (roomStart.X > 1590 &&//DC
                     roomFinish.X > 1000 && roomFinish.X <= 1590)
            {
                newX = 1305;
            }
            else if (roomStart.X > 1590 &&//DD
                     roomFinish.X > 1590)
            {
                newX = 1880;
            }
            floorMap.DrawLineAa((int)roomStart.X, (int)roomStart.Y, newX, (int)roomStart.Y, Color.FromArgb(255, 0, 0, 0), 10);
            floorMap.DrawLineAa((int)roomFinish.X, (int)roomFinish.Y, newX, (int)roomFinish.Y, Color.FromArgb(255, 0, 0, 0), 10);
            roomStart.X = newX;
            roomFinish.X = newX;
        }

        public async Task<object[]> DrawPath(Room start, Room finish)
        {
            roomStart = start;
            roomFinish = finish;

            if (roomStart.Floor != roomFinish.Floor)
            {
                var task = Render($"{roomStart.Floor}.png");
                writeableBmpFloorStart = await task;
                task.Wait();

                task = Render($"{roomFinish.Floor}.png");
                writeableBmpFloorFinish = await task;
                task.Wait();

                DrawPathBetweenFloors();
                return new object[] { writeableBmpFloorStart, writeableBmpFloorFinish };
            }
            else
            {
                var task = Render($"{roomStart.Floor}.png");
                writeableBmpFloorStart = await task;
                task.Wait();

                DrawPathFromRoomToCorridor(roomStart);
                DrawPathFromRoomToCorridor(roomFinish, true);

                if ((roomStart.X == 165 && roomFinish.X == 165) ||
                    (roomStart.X == 735 && roomFinish.X == 735) ||
                    (roomStart.X == 1305 && roomFinish.X == 1305) ||
                    (roomStart.X == 1880 && roomFinish.X == 1880) ||
                    (roomStart.Y == 380 && roomFinish.Y == 380) ||
                    (roomStart.Y == 990 && roomFinish.Y == 990))
                {
                    //Nic nie trzeba robić :D
                }
                else if (roomStart.Y > 380 && roomStart.Y < 990)
                {
                    DrawPathStartHorizontal(roomStart, roomFinish);
                }
                else
                {
                    DrawPathStartVertical(roomStart, roomFinish);
                }

                writeableBmpFloorStart.DrawLineAa((int)roomStart.X, (int)roomStart.Y, (int)roomFinish.X, (int)roomFinish.Y, Color.FromArgb(255, 0, 0, 0), 10);

                return new object[] { writeableBmpFloorStart };
            }
        }
        #endregion
        #region Render Bitmap
        private async Task<WriteableBitmap> Render(string file)
        {
            var Assets = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Floors");
            StorageFile file1 = await Assets.GetFileAsync(file);

            BitmapImage i1 = new BitmapImage();
            using (IRandomAccessStream strm = await file1.OpenReadAsync())
            {
                i1.SetSource(strm);
            }

            WriteableBitmap img1 = new WriteableBitmap(i1.PixelWidth, i1.PixelHeight);
            using (IRandomAccessStream strm = await file1.OpenReadAsync())
            {
                img1.SetSource(strm);
            }
            return img1;
        }
        #endregion
    }
}
