using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using Bubble.Core;
using Bubble.Events;

namespace Bubble.Behaviors
{
    public class RoutedEventBehavior : Behavior<UIElement>, IDisposable
    {
        private bool _disposedValue = false;
        private MethodInfo _routedEventHandlerMethodInfo = null;
        private RoutedEventHandler _routedEventHandler = null;
        private SelectionChangedEventHandler _selectionChangedEventHandler = null;
        
        private readonly Dictionary<Type, Delegate> _typeToHandlerMap = null;
        private readonly Dictionary<RoutedEvent, Delegate> _eventToHandlerMap = new Dictionary<RoutedEvent, Delegate>();

        public RoutedEventBehavior()
        {
            _routedEventHandlerMethodInfo = GetType().GetMethod(nameof(InternalRoutedEventHandler), BindingFlags.Instance | BindingFlags.NonPublic);

            _routedEventHandler = new RoutedEventHandler(InternalRoutedEventHandler);
            _selectionChangedEventHandler = new SelectionChangedEventHandler(InternalRoutedEventHandler);

            _typeToHandlerMap = new Dictionary<Type, Delegate>
            {
                [typeof(RoutedEventHandler)] = _routedEventHandler,
                [typeof(SelectionChangedEventHandler)] = _selectionChangedEventHandler
            };
        }

        #region Property

        public static readonly DependencyProperty HandledEventsTooProperty =
            DependencyProperty.Register("HandledEventsToo", typeof(bool), typeof(RoutedEventBehavior), new PropertyMetadata(false));
        public bool HandledEventsToo
        {
            get { return (bool)GetValue(HandledEventsTooProperty); }
            set { SetValue(HandledEventsTooProperty, value); }
        }

        public static readonly DependencyProperty RoutedEventsProperty =
           DependencyProperty.Register("RoutedEvents", typeof(IEnumerable<RoutedEvent>), typeof(RoutedEventBehavior), new PropertyMetadata(OnRoutedEventsPropertyChanged));
        public IEnumerable<RoutedEvent> RoutedEvents
        {
            get { return (IEnumerable<RoutedEvent>)GetValue(RoutedEventsProperty); }
            set { SetValue(RoutedEventsProperty, value); }
        }

        static void OnRoutedEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = sender as RoutedEventBehavior;
            ctrl.AddHandler();
        }

        public static readonly DependencyProperty HandleRoutedEventActionProperty =
            DependencyProperty.Register("HandleRoutedEventAction", typeof(Action<object, RoutedEventArgs>), typeof(RoutedEventBehavior));
        public Action<object, RoutedEventArgs> HandleRoutedEventAction
        {
            get { return (Action<object, RoutedEventArgs>)GetValue(HandleRoutedEventActionProperty); }
            set { SetValue(HandleRoutedEventActionProperty, value); }
        }

        #endregion

        #region Override

        protected override void OnAttached()
        {
            AssociatedObject.AddHandler(EventProvider.BubbleEvent, _routedEventHandler, HandledEventsToo);
            AssociatedObject.AddHandler(ButtonBase.ClickEvent, _routedEventHandler, HandledEventsToo);

            AssociatedObject.AddHandler(Selector.SelectionChangedEvent, _selectionChangedEventHandler, HandledEventsToo);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.RemoveHandler(EventProvider.BubbleEvent, _routedEventHandler);
            AssociatedObject.RemoveHandler(ButtonBase.ClickEvent, _routedEventHandler);

            AssociatedObject.RemoveHandler(Selector.SelectionChangedEvent, _selectionChangedEventHandler);

            RemoveHandler();
        }

        #endregion

        private void AddHandler()
        {
            RemoveHandler();

            if (RoutedEvents == null)
                return;

            foreach (var group in RoutedEvents.GroupBy(h => h.HandlerType))
            {
                var handlerType = group.Key;

                if (!_typeToHandlerMap.ContainsKey(handlerType))
                    _typeToHandlerMap.Add(handlerType, Delegate.CreateDelegate(handlerType, this, _routedEventHandlerMethodInfo));

                var handler = _typeToHandlerMap[handlerType];

                foreach (var eve in group)
                {
                    _eventToHandlerMap.Add(eve, handler);
                    AssociatedObject.AddHandler(eve, handler, HandledEventsToo);
                }
            }
        }

        private void RemoveHandler()
        {
            foreach (var kvp in _eventToHandlerMap)
            {
                AssociatedObject.RemoveHandler(kvp.Key, kvp.Value);
            }

            _eventToHandlerMap.Clear();
        }

        private void InternalRoutedEventHandler(object sender, RoutedEventArgs e)
        {
            HandleRoutedEventAction?.Invoke(sender, e);
        }

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    OnDetaching();
                }

                _typeToHandlerMap.Clear();

                _routedEventHandler = null;
                _selectionChangedEventHandler = null;
                _routedEventHandlerMethodInfo = null;

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
