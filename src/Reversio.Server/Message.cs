using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Reversio.Server
{
    public class Message
    {
        public Guid ConnectionId { get; }

        public string Type { get; }

        public JObject Payload { get; }

        public Message(Guid connectionId, string type, JObject payload)
        {
            ConnectionId = connectionId;
            Type = type;
            Payload = payload;
        }
    }
}
