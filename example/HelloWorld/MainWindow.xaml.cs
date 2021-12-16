using System.Windows;
using System.Windows.Controls;

using Bubble.Events;
using HelloWorld.ViewModels;

namespace HelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new OneViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).RaiseEvent(new RoutedEventArgs(EventProvider.BubbleEvent, sender as Button));
        }
    }
}
