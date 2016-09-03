namespace Reversio.Domain
{
    public class Player
    {
        private readonly Board _board;
        private readonly Disc _disc;

        public Player(User user, Disc disc, Board board)
        {
            _board = board;
            _disc = disc;
        }

        public object Id { get; internal set; }

        public void PlacesDiscAt(Position position)
        {
            var disc = new Move(position.X, position.Y, _disc);
            _board.Place(disc);
        }
    }
}
