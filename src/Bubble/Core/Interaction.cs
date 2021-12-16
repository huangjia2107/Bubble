using System;
using System.Windows;

namespace Bubble.Core
{
    /// <summary>
    /// Static class that owns the Behavior attached properties. Handles propagation of AssociatedObject change notifications.
    /// Reference to https://github.com/Microsoft/XamlBehaviorsWpf
    /// </summary>
    public static class Interaction
    {
        public static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached("Behavior", typeof(Behavior), typeof(Interaction), new FrameworkPropertyMetadata(OnBehaviorChanged));

        public static Behavior GetBehavior(DependencyObject dpo)
        {
            return (Behavior)dpo.GetValue(BehaviorProperty);
        }
        public static void SetBehavior(DependencyObject dpo, Behavior value)
        {
            dpo.SetValue(BehaviorProperty, value);
        }

        private static void OnBehaviorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldBehavior = (Behavior)args.OldValue;
            var newBehavior = (Behavior)args.NewValue;

            if (oldBehavior != newBehavior)
            {
                if (oldBehavior != null && ((IAttachedObject)oldBehavior).AssociatedObject != null)
                {
                    oldBehavior.Detach();
                }

                if (newBehavior != null && obj != null)
                {
                    if (((IAttachedObject)newBehavior).AssociatedObject != null)
                    {
                        throw new InvalidOperationException("Cannot set the same Behavior on multiple objects.");
                    }

                    newBehavior.Attach(obj);
                }
            }
        }
    }
}
