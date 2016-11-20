namespace Reversio.Domain
{
    public class Move
    {
        public Position Position { get; }

        public Move(Position position, DiscColor disc)
        {
            Position = position;
            Disc = disc;
        }
        public Move(int x, int y, DiscColor color) : this(new Position(x, y), color)
        {
        }

        public DiscColor Disc { get; }
    }
}
