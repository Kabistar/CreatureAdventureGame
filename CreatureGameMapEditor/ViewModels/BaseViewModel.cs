using System.ComponentModel;

namespace CreatureGameMapEditor.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        protected void ChangeProperty(object sender, string property)
        {
            PropertyChanged(sender, new PropertyChangedEventArgs(property));
        }
    }
}
