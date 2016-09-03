namespace Reversio.Domain
{
    public class Move
    {
        public Position Position { get; }

        public Move(int x, int y, Disc color)
        {
            Position = new Position(x, y);
            Color = color;
        }

        public Disc Color { get; }
    }

    public enum Disc
    {
        Dark = -1,
        Light = 1
    }
}
