using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace GUI_Flight_Simulator_Controller.controlers
{
    public class Model : INotifyPropertyChanged
    {
        // socket for connecting to the simulator.
        TcpClient tcpClient = null;
        // socket stream
        NetworkStream networkStream = null;
        // the observers list.
        public event PropertyChangedEventHandler PropertyChanged;
        
        // the method that will run when notification need to be sent.
        public void onPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // for stooping the threads.
        volatile Boolean stop = false;

        // all the plane fields
        private string heading = "Uninitialized";
        private string verticalSpeed = "Uninitialized";
        private string groundSpeed = "Uninitialized";
        private string airSpeed = "Uninitialized";
        private string gpsAlt = "Uninitialized";
        private string roll = "Uninitialized";
        private string pitch = "Uninitialized";
        private string altitude = "Uninitialized";
        public double len = 32.5110762;
        public double lon = 34.9187708;
        private System.Windows.Visibility errorMsg = System.Windows.Visibility.Hidden;
        private Microsoft.Maps.MapControl.WPF.Location location = new Microsoft.Maps.MapControl.WPF.Location(20.1, 23.9);
        private Microsoft.Maps.MapControl.WPF.Location maplocation = new Microsoft.Maps.MapControl.WPF.Location(20.1, 23.9);

        // properties of the simulator fields.
        public Microsoft.Maps.MapControl.WPF.Location Location { get { return this.location; } set { if (value != this.location) { this.location = value; onPropertyChanged("Location"); } } }
        public Microsoft.Maps.MapControl.WPF.Location MapLocation { get { return this.maplocation; } set { if (value != this.maplocation) { this.maplocation = value; onPropertyChanged("MapLocation"); } } }
        public string Heading { get { return this.heading; } set { if (value != this.heading) { this.heading = value; onPropertyChanged("Heading"); } } }
        public string VerticalSpeed { get { return this.verticalSpeed; } set { if (value != this.verticalSpeed) { this.verticalSpeed = value; onPropertyChanged("VerticalSpeed"); } } }
        public string GroundSpeed { get { return this.groundSpeed; } set { if (value != this.groundSpeed) { this.groundSpeed = value; onPropertyChanged("GroundSpeed"); } } }
        public string AirSpeed { get { return this.airSpeed; } set { if (value != this.airSpeed) { this.airSpeed = value; onPropertyChanged("AirSpeed"); } } }
        public string GpsAlt { get { return this.gpsAlt; } set { if (value != this.gpsAlt) { this.gpsAlt = value; onPropertyChanged("GpsAlt"); } } }
        public string Roll { get { return this.roll; } set { if (value != this.roll) { this.roll = value; onPropertyChanged("Roll"); } } }
        public string Pitch { get { return this.pitch; } set { if (value != this.pitch) { this.pitch = value; onPropertyChanged("Pitch"); } } }
        public string Altitude { get { return this.altitude; } set { if (value != this.altitude) { this.altitude = value; onPropertyChanged("Altitude"); } } }
        public System.Windows.Visibility ErrorMsg { get { return this.errorMsg; } set { if (value != this.errorMsg) { this.errorMsg = value; onPropertyChanged("ErrorMsg"); } } }

        // connecting to the simulator.
        public int Connect(string ip, int port)
        {
            this.stop = false;
            TcpClient tcpclnt = new TcpClient();
            Console.WriteLine("Trying To Connect..");
            try
            {
                tcpclnt.Connect(ip, port);
                if (tcpclnt.Connected)
                {
                    Console.WriteLine("Connected");
                    this.tcpClient = tcpclnt;
                    this.networkStream = tcpclnt.GetStream();
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
            return 0;
        }

        // updating the gauge board.
        public void UpdateGaugeScreen()
        {
            try
            {
                // checking if the connection still exist.
                if (tcpClient == null || networkStream == null)
                {
                    return;
                }
                if(tcpClient.Connected == false)
                {
                    return;
                }
                // running the thread.
                Thread thr = new Thread(GaugeUpdateThreaded);
                thr.Start();
            }
            catch
            {
                // letting the user know of the problem.
                ErrorSignal();
            }
        }
        // updating the plane location on the map.
        public void UpdateMapScreen()
        {
            try
            {
                // checking if the connection still exist.
                if (tcpClient == null || networkStream == null)
                {
                    return;
                }
                // running the thread.
                Thread thr = new Thread(MapUpdateThreaded);
                thr.Start();
            }
            catch (Exception e)
            {
                // letting the user know of the problem.
                ErrorSignal();
            }

        }
        // the functuon that will run in the background and update the gauge board.
        public void GaugeUpdateThreaded()
        {
            while (!stop)
            {
                try
                {
                    // getting the information from the simulator and updating the values..
                    this.Heading = SendAndRcv("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                    this.VerticalSpeed = SendAndRcv("get /instrumentation/gps/indicated-vertical-speed\n");
                    this.GroundSpeed = SendAndRcv("get /instrumentation/gps/indicated-ground-speed-kt\n");
                    this.AirSpeed = SendAndRcv("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                    this.GpsAlt = SendAndRcv("get /instrumentation/gps/indicated-altitude-ft\n");
                    this.Roll = SendAndRcv("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                    this.Pitch = SendAndRcv("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                    this.Altitude = SendAndRcv("get /instrumentation/altimeter/indicated-altitude-ft\n");
                    Thread.Sleep(250);
                }

                catch (Exception e)
                {
                    // letting the user know of the problem.
                    ErrorSignal();
                }
            }
        }
        // the functuon that will run in the background and update the gauge board.
        public void MapUpdateThreaded()
        {
            bool isFirst = true;
            while (!stop)
            {
                try
                {
                    string lon, len;
                    // geting the location points
                    lon = SendAndRcv("get /position/longitude-deg\n");
                    len = SendAndRcv("get /position/latitude-deg\n");
                    double lend = double.Parse(len);
                    double lond = double.Parse(lon);
                    // setting the new location.
                    this.Location = new Microsoft.Maps.MapControl.WPF.Location(lend, lond);
                    if (isFirst)
                    {
                        // in the first time putting the map where the plane is.
                        isFirst = false;
                        this.MapLocation = new Microsoft.Maps.MapControl.WPF.Location(lend, lond);
                    }
                    Thread.Sleep(250);
                }
                catch (Exception e)
                {
                    // letting the user know of the problem.
                    ErrorSignal();
                }

            }
        }

        // method for sending and rcving data from the simulator
        public string SendAndRcv(string sendMsg)
        {
            // making sure just 1 thread will use this each time.
            lock (this)
            {
                try
                {
                    // sending the msg in bytes.
                    Byte[] msg = System.Text.Encoding.ASCII.GetBytes(sendMsg);
                    this.networkStream.Write(msg, 0, msg.Length);

                    // getting the response.
                    msg = new Byte[1024];
                    int res = this.networkStream.Read(msg, 0, 1024);
                    String responseMsg = String.Empty;
                    responseMsg = System.Text.Encoding.ASCII.GetString(msg, 0, res);
                    // checking for bad response.
                    if (responseMsg.Equals("ERR"))
                    {
                        throw new Exception();
                    }
                    Console.WriteLine(responseMsg);
                    return responseMsg;
                }
                catch (Exception e)
                {
                    // letting the user know of the problem.
                    ErrorSignal();
                    return "Error Occurd";
                }
            }

        }

        // setting the msg to tell the user problem happend.
        public void ErrorSignal()
        {
            this.Heading = "Error Occurd";
            this.VerticalSpeed = "Error Occurd";
            this.GroundSpeed = "Error Occurd";
            this.AirSpeed = "Error Occurd";
            this.GpsAlt = "Error Occurd";
            this.Roll = "Error Occurd";
            this.Pitch = "Error Occurd";
            this.Altitude = "Error Occurd";
            this.stop = true;
        }

        // finishing the connection.
        public void Disconnect()
        {
            this.stop = true;
            this.tcpClient.Close();
            this.tcpClient = null;
            this.networkStream = null;
        }
    }
}
