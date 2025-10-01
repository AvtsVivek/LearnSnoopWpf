namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Collections.Concurrent;
    using System.Windows;
    using System.Windows.Threading;

    public class EventManagerWrapperNew
    {
        public static readonly EventManagerWrapperNew Instance = new();

        private ConcurrentDictionary<EventRegistrationNew, object?> EventRegistrations { get; } = new();

        public EventRegistrationNew RegisterClassHandler(Dispatcher dispatcher, Type targetType, RoutedEvent routedEvent, RoutedEventHandler routedEventHandler, bool handledEventsToo)
        {
            var eventRegistration = new EventRegistrationNew(dispatcher, routedEvent, routedEventHandler);
            while (this.EventRegistrations.TryAdd(eventRegistration, null) == false)
            {
            }

            EventManager.RegisterClassHandler(targetType, routedEvent, new RoutedEventHandler(this.HandleRoutedEvent), handledEventsToo);

            return eventRegistration;
        }

        public void RemoveClassHandler(EventRegistrationNew eventRegistration)
        {
            if (this.EventRegistrations.ContainsKey(eventRegistration) == false)
            {
                return;
            }

            while (this.EventRegistrations.TryRemove(eventRegistration, out _) == false)
            {
            }
        }

        private void HandleRoutedEvent(object sender, RoutedEventArgs e)
        {
            var handlers = this.EventRegistrations;

            foreach (var item in handlers)
            {
                var handler = item.Key;

                if (handler.RoutedEvent == e.RoutedEvent
                    && handler.Dispatcher == Dispatcher.CurrentDispatcher)
                {
                    handler.RoutedEventHandler.Invoke(sender, e);
                }
            }
        }

        public class EventRegistrationNew
        {
            public Dispatcher Dispatcher { get; }

            public RoutedEvent RoutedEvent { get; }

            public RoutedEventHandler RoutedEventHandler { get; }

            public EventRegistrationNew(Dispatcher dispatcher, RoutedEvent routedEvent, RoutedEventHandler routedEventHandler)
            {
                this.Dispatcher = dispatcher;
                this.RoutedEvent = routedEvent;
                this.RoutedEventHandler = routedEventHandler;
            }
        }
    }
}
