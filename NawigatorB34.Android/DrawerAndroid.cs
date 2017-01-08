using System;
using Android.Content;
using NawigatorB34.Android.Models;
using Android.Graphics;

namespace NawigatorB34.Android
{
    class DrawerAndroid : IDrawer
    {
        private Context context;
        private Room roomStart;
        private Room roomFinish;
        private Room stairsStart;
        private Room stairsFinish;
        private Bitmap bmpFloorStart;
        private Bitmap bmpFloorFinish;
        private Canvas canvasStart;
        private Canvas canvasFinish;
        private ISQLite databaseHelper;
        private Paint paint = new Paint(PaintFlags.AntiAlias);

        public DrawerAndroid(Context context)
        {
            databaseHelper = new SQLiteAndroid(context.Assets);
            this.context = context;
        }

        public void DrawArrow(int x, int y, EnumPosition position, bool start = true)
        {
            switch (position)
            {
                case EnumPosition.Up:
                    {
                        canvasStart.DrawLine(x, y, x - 30, y + 30, paint);
                        canvasStart.DrawLine(x, y, x + 30, y + 30, paint);
                        break;
                    }
                case EnumPosition.Right:
                    {
                        canvasStart.DrawLine(x, y, x - 30, y - 30, paint);
                        canvasStart.DrawLine(x, y, x - 30, y + 30, paint);
                        break;
                    }
                case EnumPosition.Down:
                    {
                        canvasStart.DrawLine(x, y, x - 30, y - 30, paint);
                        canvasStart.DrawLine(x, y, x + 30, y - 30, paint);
                        break;
                    }
                case EnumPosition.Left:
                    {
                        canvasStart.DrawLine(x, y, x + 30, y - 30, paint);
                        canvasStart.DrawLine(x, y, x + 30, y + 30, paint);
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
            using (Canvas tempCanvas = new Canvas(bmpFloorStart))
            {
                tempCanvas.DrawLine((int)roomStart.X, (int)roomStart.Y, (int)stairsStart.X, (int)stairsStart.Y, paint);

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
            }
            using (Canvas tempCanvas = new Canvas(bmpFloorFinish))
            {
                tempCanvas.DrawLine((int)stairsFinish.X, (int)stairsFinish.Y, (int)roomFinish.X, (int)roomFinish.Y, paint);
            }
        }

        public void DrawPathFromRoomToCorridor(Room which, bool finish = false, bool start = true)
        {
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
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 165, (int)which.Y, paint);
                        which.X = 165;
                        break;
                    }
                case 215:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 165, (int)which.Y, paint);
                        which.X = 165;
                        break;
                    }
                case 675:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 735, (int)which.Y, paint);
                        which.X = 735;
                        break;
                    }
                case 800:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 735, (int)which.Y, paint);
                        which.X = 735;
                        break;
                    }
                case 930:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 850, (int)which.Y, paint);
                        which.X = 850;
                        break;
                    }
                case 1110:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 1195, (int)which.Y, paint);
                        which.X = 1195;
                        break;
                    }
                case 1250:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 1305, (int)which.Y, paint);
                        which.X = 1305;
                        break;
                    }
                case 1370:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 1305, (int)which.Y, paint);
                        which.X = 1305;
                        break;
                    }
                case 1830:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 1880, (int)which.Y, paint);
                        which.X = 1880;
                        break;
                    }
                case 1935:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 1880, (int)which.Y, paint);
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
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 380, paint);
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
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 380, paint);
                        which.Y = 380;
                        break;
                    }
                case 450:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Down, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 380, paint);
                        which.Y = 380;
                        break;
                    }
                case 905:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Up, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 990, paint);
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
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 990, paint);
                        which.Y = 990;
                        break;
                    }
                case 1250:
                    {
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 990, paint);
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

            if (roomStart.Floor - roomFinish.Floor < 0)
            {//w dół
                if (roomStart.Y > 685)
                {
                    stairsStart = databaseHelper.GetStairsByName($"{roomStart.Floor}/SDU");
                    stairsFinish = databaseHelper.GetStairsByName($"{roomFinish.Floor}/SDD");
                }
                else
                {
                    stairsStart = databaseHelper.GetStairsByName($"{roomStart.Floor}/SUU");
                    stairsFinish = databaseHelper.GetStairsByName($"{roomFinish.Floor}/SUD");
                }
            }
            else
            {//w górę
                if (roomStart.Y > 685)
                {
                    stairsStart = databaseHelper.GetStairsByName($"{roomStart.Floor}/SDD");
                    stairsFinish = databaseHelper.GetStairsByName($"{roomFinish.Floor}/SDU");
                }
                else
                {
                    stairsStart = databaseHelper.GetStairsByName($"{roomStart.Floor}/SUD");
                    stairsFinish = databaseHelper.GetStairsByName($"{roomFinish.Floor}/SUU");
                }
            }

            DrawPathFromRoomToCorridor(stairsStart, true);
            DrawPathFromRoomToCorridor(stairsFinish, false, false);
        }

        public void DrawPathStartHorizontal(Room roomStart, Room roomFinish, bool start = true)
        {
            //Canvas tempCanvas;
            //if (start)
            //{
            //    tempCanvas = new Canvas(bmpFloorStart);
            //    tempCanvas.DrawBitmap(bmpFloorStart, new Matrix(), null);
            //}
            //else
            //{
            //    tempCanvas = new Canvas(bmpFloorFinish);
            //    tempCanvas.DrawBitmap(bmpFloorFinish, new Matrix(), null);
            //}

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
            canvasStart.DrawLine((int)roomStart.X, (int)roomStart.Y, (int)roomStart.X, newY, paint);
            canvasStart.DrawLine((int)roomFinish.X, (int)roomFinish.Y, (int)roomFinish.X, newY, paint);
            roomStart.Y = newY;
            roomFinish.Y = newY;

        }
        public void DrawPathStartVertical(Room roomStart, Room roomFinish, bool start = true)
        {
            Canvas tempCanvas;
            if (start)
            {
                tempCanvas = canvasStart;
            }
            else
            {
                tempCanvas = canvasFinish;
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
            tempCanvas.DrawLine((int)roomStart.X, (int)roomStart.Y, newX, (int)roomStart.Y, paint);
            tempCanvas.DrawLine((int)roomFinish.X, (int)roomFinish.Y, newX, (int)roomFinish.Y, paint);
            roomStart.X = newX;
            roomFinish.X = newX;
        }

        public object[] DrawPath(Room start, Room finish)
        {
            roomStart = start;
            roomFinish = finish;

            if (roomStart.Floor != roomFinish.Floor)
            {
                DrawPathBetweenFloors();

                return new object[] { bmpFloorStart, bmpFloorFinish };
            }
            else
            {
                using (Bitmap myBitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.floor3))
                {
                    bmpFloorStart = Bitmap.CreateBitmap(myBitmap.Width, myBitmap.Height, Bitmap.Config.Rgb565);
                    canvasStart = new Canvas(bmpFloorStart);
                    canvasStart.DrawBitmap(myBitmap, new Matrix(), null);
                }
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

                canvasStart.DrawLine((float)roomStart.X, (float)roomStart.Y, (float)roomFinish.X, (float)roomFinish.Y, paint);

                return new object[] { bmpFloorStart };
            }
        }
    }
}