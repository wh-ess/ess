using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JZT.Frame.CQRS
{
    /// <summary>
    ///     This a basic message dispatcher
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        ///     Tries to send the specified command to its handler. Throws an exception
        ///     if there is no handler registered for the command.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="c"></param>
        void SendCommand<TCommand>(TCommand c);

        /// <summary>
        ///     Publishes the specified event to all of its subscribers.
        /// </summary>
        /// <param name="e"></param>
        void PublishEvent(object e);

        /// <summary>
        ///     Looks thorugh the specified assembly for all public types that implement
        ///     the IHandleCommand or ISubscribeTo generic interfaces. Registers each of
        ///     the implementations as a command handler or event subscriber.
        /// </summary>
        /// <param name="ass"></param>
        void ScanAssembly(Assembly[] ass);
    }
}
