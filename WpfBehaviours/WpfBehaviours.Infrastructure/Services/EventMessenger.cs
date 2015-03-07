using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace WpfBehaviours.Infrastructure.Services
{
    public sealed class EventMessager : IEventMessager
    {
        private readonly Dictionary<Type, object> subscriberLookup = new Dictionary<Type, object>();

        public IObservable<T> Observe<T>()
        {
            object subject;
            if (!subscriberLookup.TryGetValue(typeof(T), out subject))
            {
                subject = new Subject<T>();
                subscriberLookup.Add(typeof(T), subject);
            }
            return ((ISubject<T>)subject).AsObservable();
        }

        public void Publish<T>(T @event)
        {
            object subject;
            if (subscriberLookup.TryGetValue(@event.GetType(), out subject))
            {
                ((Subject<T>)subject).OnNext(@event);
            }
        }
    }

}
