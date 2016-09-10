using System;

namespace Reversio.Domain
{
    public class Bystander
    {
        public Bystander()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
    }
}
