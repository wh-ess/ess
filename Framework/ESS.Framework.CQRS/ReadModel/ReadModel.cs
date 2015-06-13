#region

using System.Collections;
using ESS.Framework.CQRS.Event;

#endregion

namespace ESS.Framework.CQRS.ReadModel
{
    public abstract class ReadModel : IReadModel
    {
        public abstract bool Clear(); 
        public abstract IEnumerable GetAll();

        public void Rebuild(IEnumerable events)
        {
            if (Clear())
            {
                foreach (var e in events)
                {
                    GetType()
                        .GetMethod("ApplyOneEvent")
                        .MakeGenericMethod(e.GetType())
                        .Invoke(this, new[] { e });
                }
            }
        }

        /// <summary>
        ///     Applies a single event to.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="ev"></param>
        public void ApplyOneEvent<TEvent>(TEvent ev)
        {
            var applier = this as ISubscribeTo<TEvent>;
            if (applier != null)
            {
                applier.Handle(ev);
            }
        }
    }
}