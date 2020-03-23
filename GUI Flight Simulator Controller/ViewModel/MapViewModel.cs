using System;
using System.ComponentModel;

namespace GUI_Flight_Simulator_Controller.controlers
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private Model model;

        // the field of the view is bind to this.
        public Microsoft.Maps.MapControl.WPF.Location Location
        {
            get { return model.Location; }
        }


        public MapViewModel(Model m)
        {
            this.model = m;
            // joining the observers list.
            m.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e.PropertyName);
            };
        }

        // the observers list.
        public event PropertyChangedEventHandler PropertyChanged;

        // notifiying of change.
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // updating the plane location from the model.
        public void UpdateData()
        {
            model.UpdateMapScreen();
        }

    }
}
