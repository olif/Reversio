using System;

namespace Reversio.Domain
{
    public class Participant
    {
        public Participant(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public Participant(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; }

        public Guid Id { get; }
    }
}
