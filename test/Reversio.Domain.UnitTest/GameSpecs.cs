using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class GameSpecs
    {
        Game target;

        [Fact]
        public void Can_Place_Brick()
        {
            var user1 = new User();
            var game = new Game(user1);
            var user2 = new User();

            game.JoinOpponent(user2);

            var position = new Position(1, 2);

            var player = game.GetPlayer(user1);
            player.PlacesDiscAt(position);  
        }
    }
}
