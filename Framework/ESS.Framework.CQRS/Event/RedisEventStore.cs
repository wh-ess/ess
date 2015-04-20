#region

using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using StackExchange.Redis;

#endregion

namespace ESS.Framework.CQRS.Event
{
    /// <summary>
    ///     This is a simple example implementation of an event store, using a Redis
    ///     to provide the storage. Tested and known to work with Redis.
    /// </summary>
    public class RedisEventStore : IEventStore
    {
        private const string _key = "EventStore";
        private readonly IDatabase _redis;

        public RedisEventStore(string host, string port)
        {
            _redis = ConnectionMultiplexer.Connect(host + ":" + port)
                .GetDatabase();
            ;
        }

        public IEnumerable LoadEventsFor<TAggregate>(Guid id)
        {
            var k = _redis.ListRange(id.ToString());
            return _redis.HashGet(_key, k)
                .Select(c => DeserializeEvent(typeof(EventStream).AssemblyQualifiedName, c))
                .OfType<EventStream>()
                .Select(evt => DeserializeEvent(evt.Type.AssemblyQualifiedName, evt.Body));
        }

        public IEnumerable LoadEventsAll()
        {
            return _redis.HashValues(_key)
                .Select(c => DeserializeEvent(typeof(EventStream).AssemblyQualifiedName, c))
                .OfType<EventStream>()
                .Select(evt => DeserializeEvent(evt.Type.AssemblyQualifiedName, evt.Body));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TAggregate"></typeparam>
        /// <param name="aggregateId"></param>
        /// <param name="eventsLoaded"></param>
        /// <param name="newEvents">一个只有一个,多个会覆盖</param>
        public void SaveEventsFor<TAggregate>(Guid aggregateId, int eventsLoaded, ArrayList newEvents)
        {
            foreach (var e in newEvents)
            {
                var timeTicks = DateTime.Now.Ticks;
                var eventStream = new EventStream(aggregateId, e.GetType(), eventsLoaded, SerializeEvent(e), timeTicks);
                _redis.HashSet(_key, aggregateId + "|" + timeTicks, SerializeEvent(eventStream));
                _redis.ListRightPush(aggregateId.ToString(), aggregateId + "|" + timeTicks);
            }
        }

        private static object DeserializeEvent(string typeName, string data)
        {
            return JsonConvert.DeserializeObject(data, Type.GetType(typeName));
        }

        private static string SerializeEvent(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        private class EventStream
        {
            public EventStream(Guid aggregateId, Type type, int version, string body, long timeTicks)
            {
                AggregateId = aggregateId;
                Type = type;
                Body = body;
                TimeTicks = timeTicks;
                Version = version;
            }

            public Guid AggregateId { get; set; }
            public Type Type { get; set; }
            public string Body { get; set; }
            public int Version { get; set; }
            public long TimeTicks { get; set; }
        }
    }
}