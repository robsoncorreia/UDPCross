using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UDPCross.Model;
using Xamarin.Forms;

namespace UDPCross
{
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<DeviceUDP> DeviceUDPs = new ObservableCollection<DeviceUDP>();

        public MainPage()
        {
            InitializeComponent();

            DevicesUDP.ItemsSource = DeviceUDPs;

            try
            {
                Comando.Text = Application.Current.Properties["comando_text"].ToString();
                IP.Text = Application.Current.Properties["ip_text"].ToString();
                Porta.Text = Application.Current.Properties["port_text"].ToString();
            }
            catch (Exception e)
            {

                Console.WriteLine(e); ;
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            DeviceUDPs.Add(new DeviceUDP
            {
                Comando = Comando.Text,
                IP = IP.Text,
                Porta = Porta.Text
            });

            Application.Current.Properties["comando_text"] = Comando.Text;
            Application.Current.Properties["ip_text"] = IP.Text;
            Application.Current.Properties["port_text"] = Porta.Text;
            Application.Current.SavePropertiesAsync();

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse(IP.Text);

            byte[] sendbuf = Encoding.ASCII.GetBytes(Comando.Text);

            IPEndPoint ep = new IPEndPoint(broadcast, int.Parse(Porta.Text));

            s.SendTo(sendbuf, ep);

            Console.WriteLine("Message sent to the broadcast address");
        }
    }
}
