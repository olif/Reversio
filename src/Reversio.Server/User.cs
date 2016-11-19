using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reversio.Server
{
    public class User
    {
        public Guid Id { get; }

        public string Name { get; }

        public User(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}
