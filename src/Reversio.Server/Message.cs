using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Reversio.Domain;
using Reversio.Domain.Events;

namespace Reversio.Server
{
    public class Message
    {
        public static Message GameStateChangedMessage(GameStateChangedEventArgs e) => new Message(Server.MessageType.GameStateChanged, e);
        public static Message GameCreated(GameCreatedEventArgs e) => new Message(Server.MessageType.NewGameCreated, e);
        public static Message GameStarted(GameStartedEventArgs e) => new Message(Server.MessageType.GameStarted, e);
        public static Message InvitationDeclined(InvitationEventArgs e) => new Message(Server.MessageType.GameInvitationDeclined, e);
        public static Message InvitePlayerToGame(InvitationEventArgs e) => new Message(Server.MessageType.GameInvitation, e);

        public string MessageType { get; set; }

        public JObject Payload { get; set; }

        private static readonly JsonSerializer JsonSerializer = new JsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private Message()
        {
        }

        public Message(string type)
            :this(type, new object())
        {
            
        }

        public Message(string type, object payload)
        {
            MessageType = type;
            Payload = JObject.FromObject(payload, JsonSerializer);
        }

        public string ToJson()
        {
            var json = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return json;
        }

        public T Deserialize<T>()
        {
            return Payload.ToObject<T>();
        }
    }
}
