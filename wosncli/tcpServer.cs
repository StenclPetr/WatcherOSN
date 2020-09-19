using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace wosncli
{
    class TcpServer : IDisposable
    {
        private int _port = 16000;
        //private LogEvent log = new LogEvent("Wosncli.tcpServer");
        public TcpServer()
        {
            ZjistitVolnyPort();

            TcpListener listener = new TcpListener(IPAddress.Parse(Modul.local_address), _port);
            listener.Start();

            do
            {
                Socket sock = listener.AcceptSocket();
                if (sock.Connected == true)
                {
                    lock (this)
                    {
                        Modul.remoteIP = IPAddress.Parse(((IPEndPoint)sock.RemoteEndPoint).Address.ToString()).ToString();
                    }
                    MyServer oneSrv = new MyServer();
                    oneSrv.Soket = sock;
                    oneSrv.RemoteIPAddress = Modul.remoteIP;
                    ThreadStart thStHandler = new ThreadStart(oneSrv.Handler);
                    Thread thHandler = new Thread(thStHandler);
                    thHandler.Start();
                }
            } while (true);
        }
        private void ZjistitVolnyPort()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(Modul.local_address), _port);
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endPoints = properties.GetActiveTcpListeners();

        znovu:

            IEnumerable<IPEndPoint> porty = from e in endPoints
                                            where e.Port == _port
                                            select e;

            if (porty.Contains<IPEndPoint>(endpoint) == true)
            {
                _port++;
                endpoint.Port = _port;
                goto znovu;
            }

            lock (this)
            {
                Modul.port_klient_tcp_server = _port;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Zjištění redundantních volání

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Uvolněte spravovaný stav (spravované objekty).
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu níže.
                // TODO: Nastavte velká pole na hodnotu null.

                disposedValue = true;
            }
        }

        // TODO: Přepište finalizační metodu, jenom pokud metoda Dispose(bool disposing) výše obsahuje kód pro uvolnění nespravovaných prostředků.
        // ~TcpServer() {
        //   // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Tento kód je přidaný pro správnou implementaci odstranitelného vzoru.
        void IDisposable.Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
            Dispose(true);
            // TODO: Zrušte komentář následujícího řádku, pokud se výše přepisuje finalizační metoda.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }

    class MyServer : IDisposable
    {
        private Socket myServerSock;
        private string _remote_ip_address;
        //private LogEvent log = new LogEvent("Wosncli.myServer");

        public Socket Soket
        {
            get { return myServerSock; }
            set { myServerSock = value; }
        }
        public string RemoteIPAddress
        {
            get { return _remote_ip_address; }
            set { _remote_ip_address = value; }
        }
        public void Handler()
        {
            string zprava;
            string cmd;
            string prikaz;
            string retString;
            bool ukoncit = true;
            int i;

            NetworkStream ns = new NetworkStream(myServerSock);
            Byte[] bytes = new Byte[1024];
            byte[] data = new byte[1024 * 512];

            try
            {
                cmd = null;
                zprava = null;
                retString = null;
                do
                {
                    while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        cmd = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                        break;
                    }

                    prikaz = cmd.Substring(0, 4);
                    if (prikaz == "MESS")
                    {
                        zprava = cmd.Substring(4, cmd.Length - 4);
                        cmd = "MESS";
                    }
                    else if (prikaz == "KILL")
                    {
                        zprava = cmd.Substring(4, cmd.Length - 4);
                        cmd = "KILL";
                    }
                    
                    switch (cmd)
                    {
                        case "EXIT":
                            ukoncit = false;
                            break;
                        case "PCID":
                            retString = Environment.MachineName;
                            break;
                        case "USER":
                            retString = Environment.UserName;
                            break;
                        case "OKNO":
                            retString = Modul.AktivniOkno();
                            break;
                        case "LIST":
                            data = Modul.AktivniProcesy().ToArray();
                            myServerSock.Send(data);
                            Thread.Sleep(67);

                            GC.Collect();
                            goto ven;
                        case "MESS":
                            Thread t1 = new Thread(delegate ()
                                                    {
                                                    MessageBox.Show(zprava, "UPOZORNĚNÍ", MessageBoxButtons.OK,MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button1,MessageBoxOptions.ServiceNotification);
                                                    });
                            t1.Start();
                            break;
                        case "KILL":
                            Modul.ProcesKill(int.Parse(zprava));
                            Thread t2 = new Thread(delegate ()
                                                    {
                                                        MessageBox.Show("Proces byl ukončen správcem!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                                                    });
                                                    t2.Start();
                            break;
                        case "LOGOFF":
                            Modul.LogoffUser();
                            break;
                        case "LOCK":
                            Modul.LockWorkStation();
                            break;
                        case "SCREEN":
                            //Bitmap bitmap = new Bitmap(Modul.PrintScreen());
                            MemoryStream ms = new MemoryStream();
                            Modul.PrintScreen().Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            data = ms.ToArray();
                            myServerSock.Send(data);
                            Thread.Sleep(67);
                            //Modul.PrintScreen();
                            //Byte[] odpo = new Byte[Modul.stream.Length];
                            //Modul.stream.Read(odpo, 0, (int)Modul.stream.Length);
                            //myServerSock.Send(odpo);
                            //myServerSock.SendFile(Environment.CurrentDirectory + @"\printscreen.jpeg");
                            //Modul.stream.Close();
                            //File.Delete(Environment.CurrentDirectory + @"\printscreen.jpeg");
                            GC.Collect();
                            goto ven;
                        //break;

                        case "NTFS":

                            retString = Modul.aktivni_servery[_remote_ip_address].DataNtfs.ToString();
                            lock (this)
                            {
                                Modul.aktivni_servery[_remote_ip_address].DataNtfs = false;
                            }
                            break;

                    }

                    Byte[] res = System.Text.Encoding.UTF8.GetBytes(retString + Environment.NewLine);
                    myServerSock.Send(res);

                    ven:;

                } while (ukoncit);

                lock (this)
                {
                    //Modul.aktivni_servery[_remote_ip_address].Stav_serveru = false;
                    Modul.aktivni_servery[_remote_ip_address].Spojeni = false;
                    Modul.aktivni_servery[_remote_ip_address].StavServeru(false);
                }
                myServerSock.Close();
                //log.ZapsatZpravuDoLogu("Spojeni ukonceno", System.Diagnostics.EventLogEntryType.Error);

            }
            catch (Exception ex)
            {
                lock (this)
                {
                    //Modul.aktivni_servery[_remote_ip_address].Stav_serveru = false;
                    Modul.aktivni_servery[_remote_ip_address].Spojeni = false;
                    Modul.aktivni_servery[_remote_ip_address].StavServeru(false);
                    
                }
                myServerSock.Close();
                //log.ZapsatZpravuDoLogu(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Zjištění redundantních volání

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Uvolněte spravovaný stav (spravované objekty).
                }

                // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu níže.
                // TODO: Nastavte velká pole na hodnotu null.

                disposedValue = true;
            }
        }

        // TODO: Přepište finalizační metodu, jenom pokud metoda Dispose(bool disposing) výše obsahuje kód pro uvolnění nespravovaných prostředků.
        // ~MyServer() {
        //   // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Tento kód je přidaný pro správnou implementaci odstranitelného vzoru.
        void IDisposable.Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
            Dispose(true);
            // TODO: Zrušte komentář následujícího řádku, pokud se výše přepisuje finalizační metoda.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
