using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UDPCross.Model;
using Xamarin.Forms;
using Plugin.Clipboard;

namespace UDPCross
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {

        public ObservableCollection<DeviceUDP> DeviceUDPs = new ObservableCollection<DeviceUDP>();

        private DeviceUDP _DeviceUDP;

        public DeviceUDP DeviceUDP
        {
            get { return _DeviceUDP; }
            set
            {
                _DeviceUDP = value;
                NotifyPropertyChanged();
            }
        }


        public MainPage()
        {
            InitializeComponent();

            DevicesUDP.ItemsSource = DeviceUDPs;

            try
            {
                //Application.Current.Properties.Clear();
                foreach (var item in Application.Current.Properties)
                {
                    DeviceUDP deviceUDP = JsonConvert.DeserializeObject<DeviceUDP>(item.Value.ToString());
                    DeviceUDPs.Add(deviceUDP);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e); ;
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Comando.Text) || string.IsNullOrEmpty(IP.Text) || string.IsNullOrEmpty(Porta.Text))
                {
                    return;
                }

                DeviceUDP deviceUDP = new DeviceUDP
                {
                    ID = Application.Current.Properties.Count.ToString(),
                    Comando = Comando.Text,
                    IP = IP.Text,
                    Porta = Porta.Text
                };

                Application.Current.Properties.Add(deviceUDP.ID, JsonConvert.SerializeObject(deviceUDP));

                DeviceUDPs.Add(deviceUDP);

                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }


            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse(IP.Text);

            byte[] sendbuf = Encoding.ASCII.GetBytes($"@{Comando.Text}#");

            IPEndPoint ep = new IPEndPoint(broadcast, int.Parse(Porta.Text));

            s.SendTo(sendbuf, ep);

            s.Close();

            Console.WriteLine("Message sent to the broadcast address");
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.Properties.Clear();
            DeviceUDPs.Clear();
        }

        private async void DevicesUDP_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            DeviceUDP deviceUDP = (DeviceUDP)listView.SelectedItem;
            CrossClipboard.Current.SetText(deviceUDP.ToString());
            await DisplayAlert("UDP Cross", "Ip, Porta e Comando copiados.", "Ok");
        }
    }
}
