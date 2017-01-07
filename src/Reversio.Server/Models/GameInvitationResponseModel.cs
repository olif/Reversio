using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reversio.Domain;

namespace Reversio.Server.Models
{
    public class GameInvitationResponseModel
    {
        public Player Invitee { get; set; }

        public bool AcceptChallange { get; set; }
    }
}
