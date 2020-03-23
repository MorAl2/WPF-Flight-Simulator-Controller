using GUI_Flight_Simulator_Controller.controlers;
using System;
using System.Configuration;
using System.Windows;

namespace GUI_Flight_Simulator_Controller
{
    public partial class MainWindow : Window
    {
        private StartWindow anscestor;
        public MainWindow(Model m, StartWindow login)
        {           
            InitializeComponent();
            this.anscestor = login;
            // setting the same model for the view models.
            Dash.SetSingelMod(m,this);
            MyMap.SetSingelMod(m);
        }

        public void ReturnToLogin()
        {
            this.Visibility = Visibility.Hidden;
            this.Dash.ErrorTextBox.Visibility = Visibility.Hidden;
            this.Dash.ErrorPic.Visibility = Visibility.Hidden;
            anscestor.LoginAgain(this);
        }
        // closing the program when closed.
        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
