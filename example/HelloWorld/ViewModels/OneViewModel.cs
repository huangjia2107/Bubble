using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

using Bubble.Events;
using Bubble.Mvvm;

using HelloWorld.Models;

namespace HelloWorld.ViewModels
{
    public class OneViewModel : BindableBase
    {
        public Action<object, RoutedEventArgs> HandleRoutedEventAction => HandleRoutedEvent;

        private IEnumerable<RoutedEvent> _myEvents = null;
        public IEnumerable<RoutedEvent> MyEvents => _myEvents ??= new List<RoutedEvent>
        {
            Control.MouseDoubleClickEvent,
        };

        public ObservableCollection<string> EventHistory { get; } = new ObservableCollection<string>();

        public ObservableCollection<ItemViewModel> MyItems { get; } = new ObservableCollection<ItemViewModel>();

        private ItemViewModel _selectedItem;
        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        private void HandleRoutedEvent(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"Sender: {sender}\nSource: {e.Source}\nOriginalSource: {e.OriginalSource}\nRoutedEvent: {e.RoutedEvent}", "Event Info");

            var key = EventParameter.GetKey(e.OriginalSource as DependencyObject);
            if (string.IsNullOrEmpty(key))
                return;

            if (EventHistory.Count > 100)
                EventHistory.RemoveAt(0);

            EventHistory.Add($"OriginalSource: {e.OriginalSource},  RoutedEvent: {e.RoutedEvent}");

            switch (key)
            {
                case RoutedEventKeys.AddItemKey:
                    {
                        MyItems.Add(new ItemViewModel { Name = $"{MyItems.Count + 1}" });
                    }
                    break;

                case RoutedEventKeys.DeleteItemKey:
                    {
                        if (SelectedItem != null)
                            MyItems.Remove(SelectedItem);
                    }
                    break;

                case RoutedEventKeys.ClearItemKey:
                    {
                        MyItems.Clear();
                    }
                    break;

                case RoutedEventKeys.ClearEventHistoryKey:
                    {
                        EventHistory.Clear();
                    }
                    break;

                case RoutedEventKeys.DoubleClickItemKey:
                    {
                        MessageBox.Show($"{SelectedItem.Name}", "Hello World");
                    }
                    break;
            }
        }
    }
}
