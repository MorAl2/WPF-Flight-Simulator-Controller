using System.Windows.Controls;
using System.Windows.Input;

namespace GUI_Flight_Simulator_Controller.controlers
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        private MapViewModel mapViewModel;

        public MapView()
        {
            InitializeComponent();
            mapViewModel = new MapViewModel(new Model());
            DataContext = mapViewModel;
        }

        // creating the view model and biniding data.
        public void SetSingelMod(Model m)
        {
            mapViewModel = new MapViewModel(m);
            DataContext = mapViewModel;
            // updating the plane location.
            mapViewModel.UpdateData();
        }
    }
}
