using System;

namespace WpfBehaviours.Infrastructure.Services
{
    public interface IEventMessager
    {
        IObservable<T> Observe<T>();
        void Publish<T>(T @event);
    }
}
