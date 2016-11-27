using System;

namespace Reversio.Domain.Events
{
    public delegate void GameInvitationHandler(object sender, InvitationEventArgs e);

    public class InvitationEventArgs : EventArgs
    {
        public InvitationEventArgs(Player inviter, Player invitee)
        {
            Inviter = inviter;
            Invitee = invitee;
        }

        public Player Inviter { get; }

        public Player Invitee { get; }
    }

    public class ChallangeDeclinedEventArgs : InvitationEventArgs
    {
        public ChallangeDeclinedEventArgs(Player inviter, Player invitee) 
            :base(inviter, invitee)
        {
            
        }

        public bool InvitationDeclined = true;

    }
}
