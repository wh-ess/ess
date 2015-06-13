#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ESS.Framework.Common.Components;
using ESS.Framework.CQRS.Command;
using ESS.Framework.CQRS.Domain;
using ESS.Framework.CQRS.Event;
using ESS.Framework.CQRS.ReadModel;

#endregion

namespace ESS.Framework.CQRS
{
    /// <summary>
    ///     This implements a basic message dispatcher, driving the overall command handling
    ///     and event application/distribution process. It is suitable for a simple, single
    ///     node application that can safely build its subscriber list at startup and keep
    ///     it in memory. Depends on some kind of event storage mechanism.
    /// </summary>
    public class MessageDispatcher
    {
        private readonly Dictionary<Type, Action<object>> _commandHandlers = new Dictionary<Type, Action<object>>();
        private readonly IEventStore _eventStore;

        private readonly Dictionary<Type, List<Action<object>>> _eventSubscribers =
            new Dictionary<Type, List<Action<object>>>();

        /// <summary>
        ///     Initializes a message dispatcher, which will use the specified event store
        ///     implementation.
        /// </summary>
        /// <param name="es"></param>
        public MessageDispatcher(IEventStore es)
        {
            _eventStore = es;
        }

        /// <summary>
        ///     Tries to send the specified command to its handler. Throws an exception
        ///     if there is no handler registered for the command.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="c"></param>
        public void SendCommand<TCommand>(TCommand c)
        {
            if (_commandHandlers.ContainsKey(typeof(TCommand)))
            {
                _commandHandlers[typeof(TCommand)](c);
            }
            else
            {
                throw new Exception("No command handler registered for " + typeof(TCommand).Name);
            }
        }

        /// <summary>
        ///     Publishes the specified event to all of its subscribers.
        /// </summary>
        /// <param name="e"></param>
        private void PublishEvent(object e)
        {
            var eventType = e.GetType();
            if (_eventSubscribers.ContainsKey(eventType))
            {
                foreach (var sub in _eventSubscribers[eventType])
                    sub(e);
            }
        }

        /// <summary>
        ///     Registers an aggregate as being the handler for a particular
        ///     command.
        /// </summary>
        /// <typeparam name="TAggregate"></typeparam>
        /// <typeparam name="TCommand"></typeparam>
        public void AddHandlerFor<TCommand, TAggregate>() where TAggregate : IAggregate, new()
        {
            if (_commandHandlers.ContainsKey(typeof(TCommand)))
            {
                throw new Exception("Command handler already registered for " + typeof(TCommand).Name);
            }

            _commandHandlers.Add(typeof(TCommand), c =>
            {
                // Create an empty aggregate.
                var agg = new TAggregate();

                // Load the aggregate with events.
                agg.Id = ((dynamic)c).Id;
                agg.ApplyEvents(_eventStore.LoadEventsFor<TAggregate>(agg.Id));

                // With everything set up, we invoke the command handler, collecting the
                // events that it produces.
                var resultEvents = new ArrayList();
                var handleCommand = agg as IHandleCommand<TCommand>;
                if (handleCommand != null)
                {
                    foreach (var e in handleCommand.Handle((TCommand)c))
                        resultEvents.Add(e);
                }

                // Store the events in the event store.
                if (resultEvents.Count > 0)
                {
                    _eventStore.SaveEventsFor<TAggregate>(agg.Id, agg.Version, resultEvents);
                }

                // Publish them to all subscribers.
                foreach (var e in resultEvents)
                    PublishEvent(e);
            });
        }

        /// <summary>
        ///     Adds an object that subscribes to the specified event, by virtue of implementing
        ///     the ISubscribeTo interface.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="subscriber"></param>
        public void AddSubscriberFor<TEvent>(ISubscribeTo<TEvent> subscriber)
        {
            if (!_eventSubscribers.ContainsKey(typeof(TEvent)))
            {
                _eventSubscribers.Add(typeof(TEvent), new List<Action<object>>());
            }
            _eventSubscribers[typeof(TEvent)].Add(e => subscriber.Handle((TEvent)e));
        }

        /// <summary>
        ///     Looks thorugh the specified assembly for all public types that implement
        ///     the IHandleCommand or ISubscribeTo generic interfaces. Registers each of
        ///     the implementations as a command handler or event subscriber.
        /// </summary>
        /// <param name="ass"></param>
        public void ScanAssembly(Assembly[] ass)
        {
            // Scan for and register handlers.
            var handlers = from a in ass
                           from t in a.GetTypes()
                           from i in t.GetInterfaces()
                           where i.IsGenericType
                           where i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                           let args = i.GetGenericArguments()
                           select new { CommandType = args[0], AggregateType = t };
            foreach (var h in handlers)
            {
                GetType()
                    .GetMethod("AddHandlerFor")
                    .MakeGenericMethod(h.CommandType, h.AggregateType)
                    .Invoke(this, new object[] { });
            }

            // Scan for and register subscribers.
            var subscriber = from a in ass
                             from t in a.GetTypes()
                             from i in t.GetInterfaces()
                             where i.IsGenericType
                             where i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)
                             select new { Type = t, EventType = i.GetGenericArguments()[0] };
            foreach (var s in subscriber)
            {
                GetType()
                    .GetMethod("AddSubscriberFor")
                    .MakeGenericMethod(s.EventType)
                    .Invoke(this, new[] { CreateInstanceOf(s.Type) });
            }
        }

        /// <summary>
        ///     Looks at the specified object instance, examples what commands it handles
        ///     or events it subscribes to, and registers it as a receiver/subscriber.
        /// </summary>
        /// <param name="instance"></param>
        public void ScanInstance(object instance)
        {
            // Scan for and register handlers.
            var handlers = from i in instance.GetType()
                .GetInterfaces()
                           where i.IsGenericType
                           where i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                           let args = i.GetGenericArguments()
                           select new { CommandType = args[0], AggregateType = instance.GetType() };
            foreach (var h in handlers)
            {
                GetType()
                    .GetMethod("AddHandlerFor")
                    .MakeGenericMethod(h.CommandType, h.AggregateType)
                    .Invoke(this, new object[] { });
            }

            // Scan for and register subscribers.
            var subscriber = from i in instance.GetType()
                .GetInterfaces()
                             where i.IsGenericType
                             where i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)
                             select i.GetGenericArguments()[0];
            foreach (var s in subscriber)
            {
                GetType()
                    .GetMethod("AddSubscriberFor")
                    .MakeGenericMethod(s)
                    .Invoke(this, new[] { instance });
            }
        }

        /// <summary>
        ///     Creates an instance of the specified type. If you are using some kind
        ///     of DI container, and want to use it to create instances of the handler
        ///     or subscriber, you can plug it in here.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private object CreateInstanceOf(Type t)
        {
            return ObjectContainer.Resolve(t);
        }

        /// <summary>
        ///     对readmodel进行重建
        /// </summary>
        /// <param name="readModel"></param>
        public void Repaly(IReadModel readModel)
        {
            var events = _eventStore.LoadEventsAll();
            readModel.Rebuild(events);
        }
    }
}