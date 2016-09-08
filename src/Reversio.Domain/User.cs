using System;

namespace Reversio.Domain
{
    public class Participant
    {
        public Participant()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}
