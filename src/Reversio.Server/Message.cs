using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Reversio.Domain;

namespace Reversio.Server
{
    public class Message
    {
        public const string GameStateChangedType = "reversio.gameStateChanged";
        public const string MoveType = "reversio.move";

        public static Message GameStateChangedMessage (GameState state) => new Message(GameStateChangedType, state);

        public string MessageType { get; set; }

        public JObject Payload { get; set; }

        private static JsonSerializer jsonSerializer = new JsonSerializer()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public Message(string type, object payload)
        {
            var serializer = new JsonSerializer(); 
            MessageType = type;
            Payload = JObject.FromObject(payload, jsonSerializer);
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
