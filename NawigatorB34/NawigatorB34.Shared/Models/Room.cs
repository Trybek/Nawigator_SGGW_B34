using SQLite;

namespace Nawigator_SGGW_B34.Models
{
    public class Room
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }

        [NotNull]
        public int Floor { get; set; }

        [MaxLength(5), NotNull]//Aula 4 == A4, łazienka damska == BW, łazienka męska == BM, sala 3/84 == 3/84
        public string Name { get; set; }

        [NotNull]
        public double X { get; set; }

        [NotNull]
        public double Y { get; set; }

        public Room()
        {

        }
        public Room(int floor, string name, double x, double y)
        {
            Floor = floor;
            Name = name;
            X = x;
            Y = y;
        }
    }
}
