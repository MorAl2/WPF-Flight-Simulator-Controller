using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI_Flight_Simulator_Controller.controlers
{
    public partial class DashBoard : UserControl
    {
        private DashViewModel dashViewModel;
        private MainWindow ancestor;
        public DashBoard()
        {
            InitializeComponent();
            dashViewModel = new DashViewModel(new Model());
        }
        // creating the view model and biniding data.
        public void SetSingelMod(Model m, MainWindow main)
        {
            dashViewModel = new DashViewModel(m);
            DataContext = dashViewModel;
            // updating the board.
            dashViewModel.UpdateData();
            this.ancestor = main;
        }
    }
}
