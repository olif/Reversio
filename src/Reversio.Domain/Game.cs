using System;

namespace Reversio.Domain
{
    /// <summary>
    /// Represents an ongoing game
    /// </summary>
    public class Game
    {
        private readonly Player _whitePlayer;
        private Player _blackPlayer;
        private readonly Board _board;

        public Game(User firstPlayer)
        {
            _board = new Board(new [,]
            {    
                //0  1  2  3  4  5  6  7
                { 0, 0, 0, 0, 0, 0, 0, 0},  //0  
                { 0, 0, 0, 0, 0, 0, 0, 0},  //1
                { 0, 0, 0, 0, 0, 0, 0, 0},  //2
                { 0, 0, 0, 1, -1, 0, 0, 0}, //3
                { 0, 0, 0, -1, 1, 0, 0, 0}, //4
                { 0, 0, 0, 0, 0, 0, 0, 0},  //5
                { 0, 0, 0, 0, 0, 0, 0, 0},  //6 
                { 0, 0, 0, 0, 0, 0, 0, 0},  //7
            });
            _whitePlayer = new Player(firstPlayer, Disc.Dark, _board);
        }

        public void JoinOpponent(User joiningUser)
        {
            _blackPlayer = new Player(joiningUser, Disc.Light, _board);
        }

        private Player GetPlayer(User user)
        {
            if (user.Id == _whitePlayer.Id) return _whitePlayer;
            if(user.Id == _blackPlayer.Id) return _blackPlayer;
            throw new ArgumentException("Invalid argument");
        }

        public void UserMakesMove(User user1, Position position)
        {
            throw new NotImplementedException();
        }
    }
}
