using System;

namespace Reversio.Domain
{
    public class Bystander
    {
        public Bystander()
        {
        }

        public Bystander(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public Bystander(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; }

        public Guid Id { get; }
    }
}
