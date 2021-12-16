using Bubble.Mvvm;

namespace HelloWorld.ViewModels
{
    public class ItemViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
    }
}
