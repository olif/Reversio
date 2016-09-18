using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class GameEngineSpecs
    {
        [Fact]
        public void Test()
        {
            var engine = new GamesTable();
            var firstPlayer = new Bystander();
            var opponent = new Bystander();
            var game = engine.CreateNewGame(firstPlayer);

            game.JoinOpponent(opponent);
        }
    }
}
