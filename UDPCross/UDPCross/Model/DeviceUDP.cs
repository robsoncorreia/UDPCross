using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace UDPCross.Model
{
    public class DeviceUDP : INotifyPropertyChanged
    {
        private string _id;

        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }
        private string _ip;

        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                NotifyPropertyChanged();
            }
        }
        private string _porta;

        public string Porta
        {
            get { return _porta; }
            set
            {
                _porta = value;
                NotifyPropertyChanged();
            }
        }

        private string _comando;

        public string Comando
        {
            get { return _comando; }
            set
            {
                _comando = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override string ToString()
        {
            return $"IP: {IP} Porta: {Porta} Comando: {Comando}";
        }
    }
}
