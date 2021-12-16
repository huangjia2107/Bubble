using System.Windows;

namespace Bubble.Events
{
    public static class EventParameter
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached("Key", typeof(string), typeof(EventParameter));
        public static string GetKey(DependencyObject obj)
        {
            return (string)obj.GetValue(KeyProperty);
        }
        public static void SetKey(DependencyObject obj, string value)
        {
            obj.SetValue(KeyProperty, value);
        }

        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance", typeof(object), typeof(EventParameter));
        public static object GetInstance(DependencyObject obj)
        {
            return obj.GetValue(InstanceProperty);
        }
        public static void SetInstance(DependencyObject obj, object value)
        {
            obj.SetValue(InstanceProperty, value);
        }

        public static void Reset(UIElement element)
        {
            SetKey(element, null);
            SetInstance(element, null);
        }
    }
}
