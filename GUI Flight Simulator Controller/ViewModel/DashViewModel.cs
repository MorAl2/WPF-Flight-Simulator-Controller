using System;
using System.ComponentModel;

namespace GUI_Flight_Simulator_Controller.controlers
{
    class DashViewModel : INotifyPropertyChanged
    {
        private Model model;
        public DashViewModel(Model m)
        {
            this.model = m;
            // joining the observers list.
            m.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
             {
                 OnPropertyChanged(e.PropertyName);
             };
        }

        // updating the data from the model.
        public void UpdateData()
        {
            model.UpdateGaugeScreen();
        }

        // the observers list.
        public event PropertyChangedEventHandler PropertyChanged;

        // notifiyng of change.
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // the fields of the View that bind to this..
        public string Heading { get { return model.Heading; } }
        public System.Windows.Visibility ErrorMsg { get { return model.ErrorMsg; } }
        public string VerticalSpeed { get { return model.VerticalSpeed; } }
        public string GroundSpeed { get { return model.GroundSpeed; } }
        public string AirSpeed { get { return model.AirSpeed; } }
        public string GpsAlt { get { return model.GpsAlt; } }
        public string Roll { get { return model.Roll; } }
        public string Pitch { get { return model.Pitch; } }
        public string Altitude { get { return model.Altitude; } }
    }
}
