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
                        canvasStart.DrawLine(x, y, x - 10, y + 10, paint);
                        canvasStart.DrawLine(x, y, x + 10, y + 10, paint);
                        break;
                    }
                case EnumPosition.Right:
                    {
                        canvasStart.DrawLine(x, y, x - 10, y - 10, paint);
                        canvasStart.DrawLine(x, y, x - 10, y + 10, paint);
                        break;
                    }
                case EnumPosition.Down:
                    {
                        canvasStart.DrawLine(x, y, x - 10, y - 10, paint);
                        canvasStart.DrawLine(x, y, x + 10, y - 10, paint);
                        break;
                    }
                case EnumPosition.Left:
                    {
                        canvasStart.DrawLine(x, y, x + 10, y - 10, paint);
                        canvasStart.DrawLine(x, y, x + 10, y + 10, paint);
                        break;
                    }
                default:
                    break;
            }
        }

        public void DrawPathBetweenFloors()
        {
            DrawPathFromRoomToStairsAndReverse();

            if ((roomStart.X == 51 && stairsStart.X == 51) ||
                (roomStart.X == 230 && stairsStart.X == 230) ||
                (roomStart.X == 410 && stairsStart.X == 410) ||
                (roomStart.X == 589 && stairsStart.X == 589) ||
                (roomStart.Y == 116 && stairsStart.Y == 116) ||
                (roomStart.Y == 306 && stairsStart.Y == 306))
            {
                //Nic nie trzeba robić :D
            }
            else if (roomStart.Y > 116 && roomStart.Y < 306)
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
            }

            if ((stairsFinish.X == 51 && roomFinish.X == 51) ||
                (stairsFinish.X == 230 && roomFinish.X == 230) ||
                (stairsFinish.X == 410 && roomFinish.X == 410) ||
                (stairsFinish.X == 589 && roomFinish.X == 589) ||
                (stairsFinish.Y == 116 && roomFinish.Y == 116) ||
                (stairsFinish.Y == 306 && roomFinish.Y == 306))
            {
                //Nic nie trzeba robić :D
            }
            else if (stairsFinish.Y > 116 && stairsFinish.Y < 306)
            {
                DrawPathStartHorizontal(stairsFinish, roomFinish, false);
            }
            else
            {
                DrawPathStartVertical(stairsFinish, roomFinish, false);
            }

            using (Canvas tempCanvas = new Canvas(bmpFloorFinish))
            {
                tempCanvas.DrawLine((int)stairsFinish.X, (int)stairsFinish.Y, (int)roomFinish.X, (int)roomFinish.Y, paint);
            }
        }

        public void DrawPathFromRoomToCorridor(Room which, bool finish = false, bool start = true)
        {
            //schody do góry X 347 - rysuj do 1195
            //schody na dół  X 291  - rysuj do 850
            switch ((int)which.X)
            {
                case 35:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 51, (int)which.Y, paint);
                        which.X = 51;
                        break;
                    }
                case 67:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 51, (int)which.Y, paint);
                        which.X = 51;
                        break;
                    }
                case 212:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 230, (int)which.Y, paint);
                        which.X = 230;
                        break;
                    }
                case 250:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 230, (int)which.Y, paint);
                        which.X = 230;
                        break;
                    }
                case 291:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 850, (int)which.Y, paint);
                        which.X = 265;
                        break;
                    }
                case 347:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 1195, (int)which.Y, paint);
                        which.X = 373;
                        break;
                    }
                case 390:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 410, (int)which.Y, paint);
                        which.X = 410;
                        break;
                    }
                case 428:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 410, (int)which.Y, paint);
                        which.X = 410;
                        break;
                    }
                case 573:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Left, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 589, (int)which.Y, paint);
                        which.X = 589;
                        break;
                    }
                case 605:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Right, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, 589, (int)which.Y, paint);
                        which.X = 589;
                        break;
                    }
                default:
                    break;
            }

            //schody u góry  Y 1250
            //schody u dołu  Y 110
            switch ((int)which.Y)
            {
                case 35:
                    {
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 116, paint);
                        which.Y = 116;
                        break;
                    }
                case 93:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Up, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 116, paint);
                        which.Y = 116;
                        break;
                    }
                case 140:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Down, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 116, paint);
                        which.Y = 116;
                        break;
                    }
                case 283:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Up, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 306, paint);
                        which.Y = 306;
                        break;
                    }
                case 330:
                    {
                        if (finish)
                        {
                            DrawArrow((int)which.X, (int)which.Y, EnumPosition.Down, start);
                        }
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 306, paint);
                        which.Y = 306;
                        break;
                    }
                case 390:
                    {
                        canvasStart.DrawLine((int)which.X, (int)which.Y, (int)which.X, 306, paint);
                        which.Y = 306;
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

            //SDD   347    390    schody na dole na dół
            //SDU   291    390    schody na dole do góry
            //SUU   347    35     schody na górze do góry
            //SUD   291    35     schody na górze na dół 

            if (roomStart.Floor - roomFinish.Floor < 0)
            {//w dół
                if (roomStart.Y > 208)
                {
                    stairsStart = new Room(roomStart.Floor, "SDU", 291, 390);
                    stairsFinish = new Room(roomFinish.Floor, "SDD", 347, 390);
                }
                else
                {
                    stairsStart = new Room(roomStart.Floor, "SUU", 347, 35);
                    stairsFinish = new Room(roomFinish.Floor, "SUD", 291, 35);
                }
            }
            else
            {//w górę
                if (roomStart.Y > 208)
                {
                    stairsStart = new Room(roomStart.Floor, "SDD", 347, 390);
                    stairsFinish = new Room(roomFinish.Floor, "SDU", 291, 390);
                }
                else
                {
                    stairsStart = new Room(roomStart.Floor, "SUD", 291, 35);
                    stairsFinish = new Room(roomFinish.Floor, "SUU", 347, 35);
                }
            }
            DrawPathFromRoomToCorridor(stairsStart, true);
            DrawPathFromRoomToCorridor(stairsFinish, false, false);
        }

        public void DrawPathStartHorizontal(Room roomStart, Room roomFinish, bool start = true)
        {
            int newY = 0;
            if (Math.Abs(roomFinish.Y - roomStart.Y) < 200 &&
                Math.Abs(roomFinish.X - roomStart.X) == 0)
            {
                return;
            }
            else if (roomStart.Y <= 208 && roomFinish.Y <= 208)
            {
                newY = 116;
            }
            else if (roomStart.Y > 208 && roomFinish.Y > 208)
            {
                newY = 306;
            }
            else if (roomStart.Y <= 208 && roomFinish.Y > 208)
            {
                newY = 306;
            }
            else if (roomStart.Y > 208 && roomFinish.Y <= 208)
            {
                newY = 116;
            }
            canvasStart.DrawLine((int)roomStart.X, (int)roomStart.Y, (int)roomStart.X, newY, paint);
            canvasStart.DrawLine((int)roomFinish.X, (int)roomFinish.Y, (int)roomFinish.X, newY, paint);
            roomStart.Y = newY;
            roomFinish.Y = newY;

        }
        public void DrawPathStartVertical(Room roomStart, Room roomFinish, bool start = true)
        {
            using (Canvas tempCanvas = start ? canvasStart : canvasFinish)
            {
                int newX = 0;
                //51, 230, 410, 589
                //  135, 315, 506
                //  600,  570,  575
                //A       X <= 135
                //B 135 < X <= 315
                //C 315< X <= 506
                //D 506< X

                if (Math.Abs(roomFinish.X - roomStart.X) < 575 &&
                    Math.Abs(roomFinish.Y - roomStart.Y) == 0)
                {
                    return;
                }
                //↓roomStart jest w przedziale A
                else if (roomStart.X <= 135 &&//AA
                    roomFinish.X <= 135)
                {
                    newX = 51;
                }
                else if (roomStart.X <= 135 &&//AB
                         roomFinish.X > 135 && roomFinish.X <= 315)
                {
                    newX = 230;
                }
                else if (roomStart.X <= 135 &&//AC
                         roomFinish.X > 315 && roomFinish.X <= 506)
                {
                    newX = 410;
                }
                else if (roomStart.X <= 135 &&//AD
                         roomFinish.X > 506)
                {
                    newX = 589;
                }
                //↓roomStart jest w przedziale B
                else if (roomStart.X > 135 && roomStart.X <= 315 &&//BA
                         roomFinish.X <= 135)
                {
                    newX = 51;
                }
                else if (roomStart.X > 135 && roomStart.X <= 315 &&//BB
                        roomFinish.X > 135 && roomFinish.X <= 315)
                {
                    newX = 230;
                }
                else if (roomStart.X > 135 && roomStart.X <= 315 &&//BC
                         roomFinish.X > 315 && roomFinish.X <= 506)
                {
                    newX = 410;
                }
                else if (roomStart.X > 135 && roomStart.X <= 315 &&//BD
                         roomFinish.X > 506)
                {
                    newX = 589;
                }
                //↓roomStart jest w przedziale C
                else if (roomStart.X > 315 && roomStart.X <= 506 &&//CA
                         roomFinish.X <= 135)
                {
                    newX = 51;
                }
                else if (roomStart.X > 315 && roomStart.X <= 506 &&//CB
                         roomFinish.X > 135 && roomFinish.X <= 320)
                {
                    newX = 230;
                }
                else if (roomStart.X > 315 && roomStart.X <= 506 &&//CC
                         roomFinish.X > 320 && roomFinish.X <= 506)
                {
                    newX = 410;
                }
                else if (roomStart.X > 315 && roomStart.X <= 506 &&//CD
                         roomFinish.X > 506)
                {
                    newX = 589;
                }
                //↓roomStart jest w przedziale D
                else if (roomStart.X > 506 &&//DA
                         roomFinish.X <= 135)
                {
                    newX = 51;
                }
                else if (roomStart.X > 506 &&//DB
                         roomFinish.X > 135 && roomFinish.X <= 320)
                {
                    newX = 230;
                }
                else if (roomStart.X > 506 &&//DC
                         roomFinish.X > 320 && roomFinish.X <= 506)
                {
                    newX = 410;
                }
                else if (roomStart.X > 506 &&//DD
                         roomFinish.X > 506)
                {
                    newX = 589;
                }
                tempCanvas.DrawLine((int)roomStart.X, (int)roomStart.Y, newX, (int)roomStart.Y, paint);
                tempCanvas.DrawLine((int)roomFinish.X, (int)roomFinish.Y, newX, (int)roomFinish.Y, paint);
                roomStart.X = newX;
                roomFinish.X = newX;
            }
        }

        public object[] DrawPath(Room start, Room finish)
        {
            roomStart = start;
            roomFinish = finish;

            if (roomStart.Floor != roomFinish.Floor)
            {
                int idOfMapStart = 3, idOfMapFinish = 3;
                switch (finish.Floor)
                {
                    case -1:
                        {
                            idOfMapFinish = Resource.Drawable.floorM1;
                            break;
                        }
                    case 0:
                        {
                            idOfMapFinish = Resource.Drawable.floor0;
                            break;
                        }
                    case 1:
                        {
                            idOfMapFinish = Resource.Drawable.floor1;
                            break;
                        }
                    case 2:
                        {
                            idOfMapFinish = Resource.Drawable.floor2;
                            break;
                        }
                    case 3:
                        {
                            idOfMapFinish = Resource.Drawable.floor3;
                            break;
                        }
                    default:
                        break;
                }
                switch (start.Floor)
                {
                    case -1:
                        {
                            idOfMapStart = Resource.Drawable.floorM1;
                            break;
                        }
                    case 0:
                        {
                            idOfMapStart = Resource.Drawable.floor0;
                            break;
                        }
                    case 1:
                        {
                            idOfMapStart = Resource.Drawable.floor1;
                            break;
                        }
                    case 2:
                        {
                            idOfMapStart = Resource.Drawable.floor2;
                            break;
                        }
                    case 3:
                        {
                            idOfMapStart = Resource.Drawable.floor3;
                            break;
                        }
                    default:
                        break;
                }
                using (Bitmap bitmapStart = BitmapFactory.DecodeResource(context.Resources, idOfMapStart))
                {
                    bmpFloorStart = Bitmap.CreateBitmap(bitmapStart.Width, bitmapStart.Height, Bitmap.Config.Rgb565);

                    using (canvasStart = new Canvas(bmpFloorStart))
                    {
                        canvasStart.DrawBitmap(bitmapStart, new Matrix(), null);
                        using (Bitmap bitmapFinish = BitmapFactory.DecodeResource(context.Resources, idOfMapFinish))
                        {
                            bmpFloorFinish = Bitmap.CreateBitmap(bitmapFinish.Width, bitmapFinish.Height, Bitmap.Config.Rgb565);

                            using (canvasFinish = new Canvas(bmpFloorFinish))
                            {
                                canvasFinish.DrawBitmap(bitmapFinish, new Matrix(), null);

                                DrawPathBetweenFloors();

                                return new object[] { bmpFloorStart, bmpFloorFinish };
                            }
                        }
                    }
                }
            }
            else
            {
                int idOfMap = 3;
                switch (finish.Floor)
                {
                    case -1:
                        {
                            idOfMap = Resource.Drawable.floorM1;
                            break;
                        }
                    case 0:
                        {
                            idOfMap = Resource.Drawable.floor0;
                            break;
                        }
                    case 1:
                        {
                            idOfMap = Resource.Drawable.floor1;
                            break;
                        }
                    case 2:
                        {
                            idOfMap = Resource.Drawable.floor2;
                            break;
                        }
                    case 3:
                        {
                            idOfMap = Resource.Drawable.floor3;
                            break;
                        }
                    default:
                        break;
                }
                using (Bitmap myBitmap = BitmapFactory.DecodeResource(context.Resources, idOfMap))
                {
                    bmpFloorStart = Bitmap.CreateBitmap(myBitmap.Width, myBitmap.Height, Bitmap.Config.Rgb565);
                    using (canvasStart = new Canvas(bmpFloorStart))
                    {
                        canvasStart.DrawBitmap(myBitmap, new Matrix(), null);
                        canvasStart.DrawLine((float)roomStart.X, (float)roomStart.Y, (float)roomFinish.X, (float)roomFinish.Y, paint);

                        DrawPathFromRoomToCorridor(roomStart);
                        DrawPathFromRoomToCorridor(roomFinish, true);

                        if ((roomStart.X == 51 && roomFinish.X == 51) ||
                            (roomStart.X == 230 && roomFinish.X == 230) ||
                            (roomStart.X == 410 && roomFinish.X == 410) ||
                            (roomStart.X == 589 && roomFinish.X == 589) ||
                            (roomStart.Y == 116 && roomFinish.Y == 116) ||
                            (roomStart.Y == 306 && roomFinish.Y == 306))
                        {
                            //Nic nie trzeba robić :D
                        }
                        else if (roomStart.Y > 116 && roomStart.Y < 306)
                        {
                            DrawPathStartHorizontal(roomStart, roomFinish);
                        }
                        else
                        {
                            DrawPathStartVertical(roomStart, roomFinish);
                        }

                        return new object[] { bmpFloorStart };
                    }

                }
            }
        }
    }
}