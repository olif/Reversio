using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Server
{
    public static class MessageType
    {
        public const string GameStateChanged = "reversio.event.gameStateChanged";
        public const string NewGameCreated = "reversio.event.newGameCreated";
        public const string GameStarted = "reversio.event.gameStarted";

        public const string Move = "reversio.event.move";
        public const string JoinAsObserver = "reversio.event.joinAsObserver";
        public const string StartGameWithRandomPlayer = "reversio.event.startGameWithRandomPlayer";
        public const string GameInvitation = "reversio.event.gameInvitation";
        public const string GameInvitationDeclined = "reversio.event.gameInvitationDeclined";
        public static string GetStats { get; set; }
    }
}
