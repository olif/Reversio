namespace Reversio.Domain
{
    public class Player
    {
        private readonly Board _board;
        public Disc  Disc { get; }

        public Player(User user, Disc disc, Board board)
        {
            _board = board;
            Disc = disc;
        }

        public object Id { get; internal set; }
    }
}
