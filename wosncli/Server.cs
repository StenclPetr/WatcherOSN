using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace wosncli
{
    public delegate void ZmenaHandler();

    class Server
    {
        private string _adresa_serveru;
        private bool _stav_serveru;
        private bool _spojeni;
        private bool _dataNTFS = false;
        public event ZmenaHandler Zmeneno;
        //private LogEvent logEvent = new LogEvent("Wosncli.Server");

        public Server(string ip_adresa_serveru)
        {
            _adresa_serveru = ip_adresa_serveru;
            _stav_serveru = true;
            _spojeni = false;
            this.Zmeneno += Server_Zmeneno;
        }

        private void Server_Zmeneno()
        {
            if (_stav_serveru == true)
            {
                if (_spojeni == false)
                {
                    try
                    {
                        //logEvent.ZapsatZpravuDoLogu("Zasláno ozn.", System.Diagnostics.EventLogEntryType.Warning);

                        TcpClient clientSocket = new TcpClient(_adresa_serveru, 17000);
                        NetworkStream networkStream = clientSocket.GetStream();
                        StreamReader steamReader = new StreamReader(networkStream);
                        string zprava = Modul.port_klient_tcp_server.ToString();
                        Byte[] pryc = Encoding.UTF8.GetBytes(zprava);

                        networkStream.Write(pryc, 0, pryc.Length);
                        networkStream.Close();
                        clientSocket.Close();
                        _spojeni = true;
                    }
                    catch (Exception e)
                    {

                    }

                }

            }
        }
        public void StavServeru(bool stav)
        {
            _stav_serveru = stav;
            PriZmene();
        }
        protected void PriZmene()
        {
            if(Zmeneno != null)
            {
                Zmeneno();
            }
        }
        public string Adresa_serveru
        {
            get { return _adresa_serveru; }
        }

        public bool Stav_serveru
        {
            get { return _stav_serveru; }
            set { _stav_serveru = value; }
        }
        public bool Spojeni
        {
            get { return _spojeni; }
            set { _spojeni = value; }
        }
        public bool DataNtfs
        {
            get { return _dataNTFS; }
            set { _dataNTFS = value; }
        }
    }
}
