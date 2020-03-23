using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Flight_Simulator_Controller.controlers
{
    public class ConnectViewModel : INotifyPropertyChanged
    {
        private Model model;
        public ConnectViewModel(Model m)
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

        // updating the observers.
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // telling the model to Connect to the simulator.
        public int VM_Connect(string ip, int port) {
            return (model.Connect(ip, port));
        }

    }
}
