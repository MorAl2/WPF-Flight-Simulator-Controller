using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace GUI_Flight_Simulator_Controller.controlers
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private Model m = new Model();
        private MainWindow main = null;
        public StartWindow()
        {
            InitializeComponent();
            ConnectWin.SetSingelMod(m);
            ConnectWin.SetAncestor(this);
        }

        public void MoveToMainWIndows()
        {   

            this.main = new MainWindow(this.m, this);
            this.main.Show();
            this.main.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }

        internal void LoginAgain(MainWindow mainWindow)
        {
            this.Visibility = Visibility.Visible;
            ConnectWin = new ConnectView();
            ConnectWin.SetSingelMod(m);
            ConnectWin.SetAncestor(this);
        }
        // creating the view model and biniding data.
        private void StartWin_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
