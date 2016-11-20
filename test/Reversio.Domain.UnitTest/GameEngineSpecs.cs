using FluentAssertions;
using Reversio.Domain.Events;
using Xunit;

namespace Reversio.Domain.UnitTest
{
    public class GameEngineSpecs
    {
        private GameEngine _sut;

        public GameEngineSpecs()
        {
            _sut = new GameEngine();
        }

        [Fact]
        public void Creates_new_game_when_two_participants_has_joined_the_waiting_queue()
        {
            bool partipant1JoinedGame = false;
            bool participant2JoinedGame = false;
            var participant1 = new Player("p1");
            var participant2 = new Player("p1");
            _sut.GameStarted += (object sender, Player participant, GameStartedEventArgs args) =>
            {
                if (participant1 == participant)
                {
                    partipant1JoinedGame = true;
                }

                if (participant2 == participant)
                {
                    participant2JoinedGame = true;
                }
            };

            _sut.PutPlayerInQueue(participant1);
            _sut.PutPlayerInQueue(participant2);

            partipant1JoinedGame.Should().BeTrue();
            participant2JoinedGame.Should().BeTrue();
        }
    }
}
