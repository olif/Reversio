namespace Reversio.Domain
{
    public class Move
    {
        public Position Position { get; }

        public Move(Position position, Disc color)
        {
            Position = position;
            Color = color;
        }
        public Move(int x, int y, Disc color) : this(new Position(x, y), color)
        {
        }

        public Disc Color { get; }
    }

    public enum Disc
    {
        Dark = -1,
        Light = 1
    }
}
