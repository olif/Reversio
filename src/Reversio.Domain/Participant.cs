using System;

namespace Reversio.Domain
{
    public class Player : ValueObject<Player>
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }

        protected override bool EqualsCore(Player other)
        {
            return Name == other.Name;
        }

        protected override int GetHashCodeCore()
        {
            return Name.GetHashCode();
        }
    }
}
