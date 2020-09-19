using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace WOSNManager
{
    public delegate void StartovaciUdalostHandler();
    class PC
    {
        private string _jmenoStanice;
        private string _ipAdresaStanice;
        private string _macAdresaStanice;
        private bool _stavStanice = false;
        private TcpClient tcpClient;
        private NetworkStream ns;
        private System.IO.StreamReader rs;
        private byte[] bytes;

        public event StartovaciUdalostHandler StartovaciUdalost;

        private Dictionary<int, Klient> _klienti = new Dictionary<int, Klient>();
        private List<Klient> _listKlientu = new List<Klient>();

        public PC()
        {

        }

        public PC(string jmeno, string adresa, string mac)
        {
            _jmenoStanice = jmeno;
            _ipAdresaStanice = adresa;
            _macAdresaStanice = mac;

        }
        protected void MStartovaciUdalost()
        {
            if (StartovaciUdalost != null)
            {
                StartovaciUdalost();
            }
        }
        public void Start()
        {
            if (_klienti.Count != 0)
            {
                MStartovaciUdalost();
            }
        }
        public string JmenoStanice
        {
            get { return _jmenoStanice; }
        }

        public string AdresaStanice
        {
            get { return _ipAdresaStanice; }
        }

        public string MacAdresa
        {
            get { return _macAdresaStanice; }
        }
        public bool StavStanice
        {
            get { return _stavStanice; }
            set { _stavStanice = value; }
        }
        public void Restartovat()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(_ipAdresaStanice), 15500);
                ns = tcpClient.GetStream();
                rs = new System.IO.StreamReader(ns);
                bytes = System.Text.Encoding.UTF8.GetBytes("REBOOT");
                ns.Write(bytes, 0, bytes.Length);
                tcpClient.Close();
            }
            catch (Exception)
            {
                tcpClient.Close();
            }
        }
        public void Vypnout()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Parse(_ipAdresaStanice), 15500);
                ns = tcpClient.GetStream();
                rs = new System.IO.StreamReader(ns);
                bytes = System.Text.Encoding.UTF8.GetBytes("SHUTDOWN");
                ns.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                tcpClient.Close();
            }

        }
        public void StavPc(bool stav)
        {
            if (stav == true)
            {
                _stavStanice = true;
                Modul.MStavPc(ref _ipAdresaStanice, true);
            }
            else if (stav == false)
            {
                _stavStanice = false;
                Modul.MStavPc(ref _ipAdresaStanice, false);
            }
        }

        internal Dictionary<int, Klient> Klienti { get => _klienti; set => _klienti = value; }
        internal List<Klient> ListKlientu { get => _listKlientu; set => _listKlientu = value; }

        public void PrijemOzn(int port)
        {
            if (Klienti.ContainsKey(port) == true)
            {
                Klienti[port].ZnovuZalozitKlienta();
                Klienti[port].SpojeniSTCPKlientem();
            }
            else if (Klienti.ContainsKey(port)== false)
            {
                Klient klient = new Klient(port, _ipAdresaStanice);

                Klienti.Add(port, klient);
                _listKlientu.Add(klient);
            }
        }
        public void OdebratKlienta(int port)
        {
            _klienti.Remove(port);

        }

        public class Klient
        {
            private int _port;
            private string _ipAdresaStanice;
            private TcpClient tcpClient;
            private NetworkStream ns;
            private NetworkStream nsNTFS;

            private System.IO.StreamReader rs;
            private System.IO.StreamReader rsNTFS;
            private byte[] bytes;
            private byte[] bytesNTFS;
            private string _jmenoUzivatele;
            private string _aktivniOkno;
            private bool _spojeno;
            private bool _prvniLogin = false;
            private bool _dataNTFS = false;

            int dataSize;
            byte[] b = new byte[1024 * 512];
            byte[] bNTFS = new byte[1024 * 512];
            MemoryStream ms;
            string[,] pole_procesu;
            string[,] pole_NTFS = new string[0,0];

            public Klient()
            {

            }
            public Klient(int port, string adresa)
            {
                _port = port;
                _ipAdresaStanice = adresa;
                tcpClient = new TcpClient();
                SpojeniSTCPKlientem();
                Modul.DictOfPC[_ipAdresaStanice].StartovaciUdalost += Klient_StartovaciUdalost;
            }

            private void Klient_StartovaciUdalost()
            {
                if (_spojeno != false)
                {
                    ZjistitJmenoUzivatele();
                    if (_spojeno != false)
                    {
                        if (_prvniLogin == false)
                        {
                            _prvniLogin = true;
                            _aktivniOkno = "Přihlášení";
                            _dataNTFS = false;
                            Modul.MAktualizace(ref _ipAdresaStanice, ref _jmenoUzivatele, ref _aktivniOkno, ref _dataNTFS);
                        }
                    }
                    if (_spojeno != false)
                    {
                        ZjistitAktivniOkno();
                        NTFS();
                        if (_spojeno != false)
                        {
                            Modul.MAktualizace(ref _ipAdresaStanice, ref _jmenoUzivatele, ref _aktivniOkno, ref _dataNTFS);
                        }
                    }
                }
            }

            public void ZnovuZalozitKlienta()
            {
                try
                {
                    tcpClient.Close();
                    tcpClient = new TcpClient();
                }
                catch (Exception)
                {
                    tcpClient = new TcpClient();
                }
                
            }
            private string OdeslatPrikaz(string prikaz)
            {
                try
                {
                    ns = tcpClient.GetStream();
                    rs = new System.IO.StreamReader(ns);
                    bytes = System.Text.Encoding.UTF8.GetBytes(prikaz);
                    ns.Write(bytes, 0, bytes.Length);

                    return rs.ReadLine();
                }
                catch (Exception)
                {
                    tcpClient.Close();
                    _spojeno = false;
                    _prvniLogin = false;
                    try
                    {
                        int pocet = 0;
                        for (int j = 0; j < Modul.DictOfPC[_ipAdresaStanice].ListKlientu.Count; j++)
                        {
                            if (Modul.DictOfPC[_ipAdresaStanice].ListKlientu[j].StavSpojeni == true)
                            {
                                pocet++;
                            }
                        }
                        if (pocet > 1)
                        {
                            string al_user = "Více uživatelů";
                            string al_okno = "prozatím neaktivní";
                            Modul.MAktualizace(ref _ipAdresaStanice, ref al_user, ref al_okno, ref _dataNTFS);
                        }
                        else if (pocet == 1)
                        {
                            string al_user = string.Empty;
                            string al_okno = "prozatím neaktivní";

                            foreach (var item in Modul.DictOfPC[_ipAdresaStanice].ListKlientu)
                            {
                                if (item.StavSpojeni == true)
                                {
                                    al_user = item._jmenoUzivatele;
                                }
                            }
                            Modul.MAktualizace(ref _ipAdresaStanice, ref al_user, ref al_okno, ref _dataNTFS);
                        }
                        else if (pocet == 0)
                        {

                            string al_user = "";
                            string al_okno = "";
                            Modul.MAktualizace(ref _ipAdresaStanice, ref _jmenoUzivatele, ref al_okno, ref _dataNTFS);
                        }
                    }
                    catch (Exception)
                    {


                    }
                    return string.Empty;
                }
            }
            public void NTFS()
            {
                string dataNTFS = OdeslatPrikaz("NTFS");
                _dataNTFS = bool.Parse(dataNTFS);
            }
            public void ListProcesu(string prikaz)
            {
                try
                {
                    ns = tcpClient.GetStream();
                    rs = new System.IO.StreamReader(ns);
                    bytes = System.Text.Encoding.UTF8.GetBytes(prikaz);
                    ns.Write(bytes, 0, bytes.Length);

                    do
                    {
                        dataSize = ns.Read(b, 0, b.Length);
                        ms = new MemoryStream(b, 0, dataSize, true);
                        
                    } while (ns.DataAvailable);
                    pole_procesu = null;
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    pole_procesu = (string[,])binaryFormatter.Deserialize(ms);
                    ms.Dispose();
                    GC.Collect();
                }
                catch (Exception)
                {
                    tcpClient.Close();
                }

            }
            public void ScreenShot(string prikaz)
            {
                try
                {
                    ns = tcpClient.GetStream();
                    rs = new System.IO.StreamReader(ns);
                    bytes = System.Text.Encoding.UTF8.GetBytes(prikaz);
                    ns.Write(bytes, 0, bytes.Length);

                    do
                    {
                        dataSize = ns.Read(b, 0, b.Length);
                        ms = new MemoryStream(b, 0, dataSize, true);
                        Modul._imgScreenShot = new Bitmap(ms);
                    } while (ns.DataAvailable);

                    ms.Dispose();
                    GC.Collect();
                }
                catch (Exception)
                {
                    tcpClient.Close();
                }

            }
            public void ZjistitJmenoUzivatele()
            {
                _jmenoUzivatele = OdeslatPrikaz("USER");
            }
            public void ZjistitAktivniOkno()
            {
                _aktivniOkno = OdeslatPrikaz("OKNO");
            }
            public void ZaslatZpravu(string zprava)
            {
                OdeslatPrikaz("MESS" + zprava);
            }
            public void Zamknout()
            {
                OdeslatPrikaz("LOCK");
            }
            public void Odhlasit()
            {
                OdeslatPrikaz("LOGOFF");
            }
            public void Kill(string id)
            {
                OdeslatPrikaz("KILL" + id);
            }
            public void UkoncitSpojeni()
            {
                OdeslatPrikaz("EXIT");
            }
            public string JmenoUzivatele
            {
                get { return _jmenoUzivatele; }
            }
            public bool StavSpojeni
            {
                get { return _spojeno; }
            }
            public string[,] Pole
            {
                get { return pole_procesu; }
            }
            public string[,] PoleNTFS
            {
                get { return pole_NTFS; }
            }
            public void SpojeniSTCPKlientem()
            {
                try
                {
                    tcpClient.Connect(IPAddress.Parse(_ipAdresaStanice), _port);
                    _spojeno = true;

                }
                catch (Exception)
                {
                    _spojeno = false;
                }

            }

        }
    }
}
