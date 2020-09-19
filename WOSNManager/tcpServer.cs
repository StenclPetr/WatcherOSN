using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Text;
using System.IO;


namespace WOSNManager
{
    class TcpServer : IDisposable
    {
        private int _port = 17000;
        public TcpListener listener;
        public TcpServer()
        {
            listener = new TcpListener(IPAddress.Parse(Modul.local_address), _port);
            //TcpListener listener = new TcpListener(IPAddress.Parse(Modul.local_address), _port);
            listener.Start();

            do
            {
                Socket sock = listener.AcceptSocket();
                if (sock.Connected == true)
                {
                    //lock (this)
                    //{
                    //    Modul.remoteIP = IPAddress.Parse(((IPEndPoint)sock.RemoteEndPoint).Address.ToString()).ToString();
                    //}
                    MyServer oneSrv = new MyServer();
                    oneSrv.Soket = sock;
                    oneSrv.RemoteIPAddress = IPAddress.Parse(((IPEndPoint)sock.RemoteEndPoint).Address.ToString()).ToString();
                    ThreadStart thStHandler = new ThreadStart(oneSrv.Handler);
                    Thread thHandler = new Thread(thStHandler);
                    thHandler.Start();
                }
            } while (Modul.vypnout);

            listener.Stop();

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
        public void Dispose()
        {
            // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
            Dispose(true);
            // TODO: Zrušte komentář následujícího řádku, pokud se výše přepisuje finalizační metoda.
            GC.SuppressFinalize(this);
        }
        #endregion

    }

    class MyServer : IDisposable
    {
        private Socket myServerSock;
        private string _remote_ip_address;

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
            try
            {
                cmd = null;
                zprava = null;
                retString = null;
                while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                {
                    cmd = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                    break;
                }

                lock (this)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (Modul.DictOfPC[item.Key].AdresaStanice == _remote_ip_address)
                        {
                            Modul.DictOfPC[item.Key].PrijemOzn(int.Parse(cmd));
                        }
                    }
                }
                //prikaz = cmd.Substring(0, 4);
                //if (prikaz == "MESS")
                //{
                //    zprava = cmd.Substring(4, cmd.Length - 4);
                //    cmd = "MESS";
                //}
                //else if (prikaz == "KILL")
                //{
                //    zprava = cmd.Substring(4, cmd.Length - 4);
                //    cmd = "KILL";
                //}

                //switch (cmd)
                //{
                //    case "EXIT":
                //        ukoncit = false;
                //        break;
                //}

                Byte[] res = System.Text.Encoding.UTF8.GetBytes(retString + Environment.NewLine);
                myServerSock.Send(res);

                myServerSock.Close();

            }
            catch (Exception e)
            {
                myServerSock.Close();
            }
        }
        ~MyServer()
        {
            Dispose(false);
        }
        #region IDisposable Support
        private bool disposedValue = false; // Zjištění redundantních volání

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    myServerSock.Dispose();
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //void IDisposable.Dispose()
        //{
        //    // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
        //    Dispose(true);
        //    // TODO: Zrušte komentář následujícího řádku, pokud se výše přepisuje finalizační metoda.
        //    // GC.SuppressFinalize(this);
        //}
        #endregion
    }
}
