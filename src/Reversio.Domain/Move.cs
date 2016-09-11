namespace Reversio.Domain
{
    public class Move
    {
        public Position Position { get; }

        public Move(Position position, Disc disc)
        {
            Position = position;
            Disc = disc;
        }
        public Move(int x, int y, Disc color) : this(new Position(x, y), color)
        {
        }

        public Disc Disc { get; }
    }
}
