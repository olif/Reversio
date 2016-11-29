﻿using System;
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
        public static Message GameStateChangedMessage(GameStateChangedEventArgs eventArgs) => new Message(Server.MessageType.GameStateChanged, eventArgs);
        public static Message GameCreated(GameCreatedEventArgs eventArgs) => new Message(Server.MessageType.NewGameCreated, eventArgs);
        public static Message GameStarted(GameStartedEventArgs eventArgs) => new Message(Server.MessageType.GameStarted, eventArgs);
        public static Message InvitationDeclined => new Message(Server.MessageType.GameInvitationDeclined, string.Empty);

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
