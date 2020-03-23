using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI_Flight_Simulator_Controller.controlers
{

    public partial class ConnectView : UserControl
    {
        // the Control view model.
        private ConnectViewModel connectViewModel;
        private StartWindow win = null;
        //CTOR
        public ConnectView()
        {
            InitializeComponent();
            IPTextBox.Text = ConfigurationManager.AppSettings["IP"];
            PortTextBox.Text = ConfigurationManager.AppSettings["Port"];
            connectViewModel = new ConnectViewModel(new Model());
            DataContext = connectViewModel;
        }

        public void SetAncestor(StartWindow wind)
        {
            this.win = wind;
        }

// creating the view model and biniding data.
        public void SetSingelMod(Model m)
        {
            connectViewModel = new ConnectViewModel(m);
            DataContext = connectViewModel;
        }
        // connecting to the simulator.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadSign.Visibility = Visibility.Visible;
            String text = loadSign.Content.ToString();
            // trying to Connect.
            int result = connectViewModel.VM_Connect(IPTextBox.Text, int.Parse(PortTextBox.Text));
            // if secessful move to the main screen, if failed letting the user know.
            if(result == 1)
            {
                if (this.win != null)
                {
                    win.MoveToMainWIndows();
                }
            }
            else if(result == 0)
            {
                loadSign.Content = "Problem Occured while Connecting\nIs the ip and port active?";
                loadSign.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}
