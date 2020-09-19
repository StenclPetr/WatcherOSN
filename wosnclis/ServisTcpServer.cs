using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace wosnclis
{
    class ServisTcpServer
    {
        public ServisTcpServer()
        {

        }
        public void ListenerThread()
        {
            TcpListener servisTcpListener = new TcpListener(IPAddress.Parse(Modul.local_address), 15500);
            servisTcpListener.Start();

            do
            {
                Socket sock = servisTcpListener.AcceptSocket();
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
                byte[] data = new byte[1024 * 512];

                try
                {
                    cmd = null;
                    retString = null;
                    do
                    {
                        while ((i = ns.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            cmd = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                            break;
                        }

                        switch (cmd)
                        {
                            case "EXIT":
                                ukoncit = false;
                                break;
                            case "REBOOT":
                                try
                                {
                                    Modul.ZapisDoEventlogu(cmd, System.Diagnostics.EventLogEntryType.Information);

                                    ProcessStartInfo psi = new ProcessStartInfo();
                                    psi.Verb = "runas";
                                    psi.UseShellExecute = true;
                                    psi.FileName = Environment.CurrentDirectory + @"\reboot.bat";

                                    Process pro = new Process();
                                    pro = Process.Start(psi);
                                    pro.WaitForExit();

                                }
                                catch (Exception ex)
                                {
                                    Modul.ZapisDoEventlogu(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
                                }
                                break;

                            case "SHUTDOWN":
                                try
                                {
                                    Modul.ZapisDoEventlogu(cmd, System.Diagnostics.EventLogEntryType.Information);

                                    ProcessStartInfo psi = new ProcessStartInfo();
                                    psi.Verb = "runas";
                                    psi.UseShellExecute = true;
                                    psi.FileName = Environment.CurrentDirectory + @"\shutdown.bat";

                                    Process pro = new Process();
                                    pro = Process.Start(psi);
                                    pro.WaitForExit();

                                }
                                catch (Exception ex)
                                {
                                    Modul.ZapisDoEventlogu(ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
                                }

                                break;
                        }

                        Byte[] res = System.Text.Encoding.UTF8.GetBytes(retString + Environment.NewLine);
                        myServerSock.Send(res);

                    } while (ukoncit);

                    myServerSock.Close();
                    Modul.remoteIP = null;
                    GC.Collect();

                    //log.ZapsatZpravuDoLogu("Spojeni ukonceno", System.Diagnostics.EventLogEntryType.Error);
                }
                catch (Exception ex)
                {
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


}
