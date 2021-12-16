using System;
using System.Windows;

namespace Bubble.Events
{
    public static class EventProvider
    {
        public static readonly RoutedEvent BubbleEvent = EventManager.RegisterRoutedEvent("BubbleEvent", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(EventProvider));

        public static void RaiseBubbleEvent(UIElement element, string key, object instance)
        {
            if (element == null || string.IsNullOrWhiteSpace(key))
                return;

            EventParameter.Reset(element);

            EventParameter.SetKey(element, key);
            EventParameter.SetInstance(element, instance);

            element.RaiseEvent(new RoutedEventArgs(BubbleEvent));
        }
    }
}