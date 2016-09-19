using System;

namespace Reversio.Domain
{
    public class Bystander
    {
        public Bystander(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public string Name { get; }

        public Guid Id { get; }
    }
}
