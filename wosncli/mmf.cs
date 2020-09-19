using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace wosncli
{
    class Mmf : IDisposable 
    {
        private System.Timers.Timer casovac = new System.Timers.Timer();
        private int pocitadlo = 0;
        private StringBuilder ziskanyText = new StringBuilder();
        private const char oddelovac = ';';
        private int poradi_oddelovace = 0;
        private int pocatek = 0;
        private char[] adresy;
        private string adresa_serveru;
        //private LogEvent logEvent = new LogEvent("Wosncli.mmf");
        private object zamek = new object();


        public Mmf()
        {
            casovac.Elapsed += Casovac_Elapsed;
            casovac.Interval = 3000;
            casovac.Start();

            ThreadStart threadDelegate = new ThreadStart(ZalozitSpojeni);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();

        }

        private void Casovac_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pocitadlo++;
            try
            {
                //Získání adresy ze souboru
                    using (Modul.reader = new StreamReader(@"C:\Windows\System32\mmf.data"))
                    {
                        lock (zamek)
                        {
                            ziskanyText.Append(Modul.reader.ReadLine());
                        }
                
                    }

                if (ziskanyText.Length == 5)
                {
                    ziskanyText.Clear();
                }
                else if (ziskanyText.Length > 5)
                {
                    adresy = new char[ziskanyText.Length];
                    adresy = ziskanyText.ToString().ToCharArray();
                    RozdelAdresy(adresy);
                    ziskanyText.Clear();
                }

                //Ověření stavu serverů založených na získaných adresách
                //lock (zamek)
                //{
                //    foreach (var item in Modul.aktivni_servery)
                //    {
                //        if (Modul.aktivni_servery[item.Key].Stav_serveru == true)
                //        {
                //            if (Modul.aktivni_servery[item.Key].Spojeni == false)
                //            {
                //                    Modul.aktivni_servery[item.Key].Spojeni = true;
                //                    //ZaslatOzn(Modul.aktivni_servery[item.Key].Adresa_serveru, Modul.port_klient_tcp_server);
                //            }
                //        }
                //    }
                //}

                if (pocitadlo == 10)
                {
                    pocitadlo = 0;
                    GC.Collect();
                }
            }
            catch (Exception exp)
            {
                //logEvent.ZapsatZpravuDoLogu(exp.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }

        }

        private void RozdelAdresy(char[] predane_adresy)
        {
            for (int i = 0; i < predane_adresy.Length; i++)
            {
                if (predane_adresy[i] == oddelovac)
                {
                    poradi_oddelovace = i;
                    for (int j = pocatek; j < poradi_oddelovace; j++)
                    {
                        Modul.prevod_Adresy.Append(predane_adresy[j]);
                    }
                    pocatek = poradi_oddelovace + 1;
                    adresa_serveru = Modul.prevod_Adresy.ToString();


                    if (Modul.aktivni_servery.Count == 0)
                    {
                        Server akt_server = new Server(adresa_serveru);

                        lock (zamek)
                        {
                            Modul.aktivni_servery.Add(adresa_serveru, akt_server);
                            Modul.aktivni_servery[adresa_serveru].StavServeru(true);
                        }

                    }
                    else if (Modul.aktivni_servery.Count != 0)
                    {
                        if (Modul.aktivni_servery.ContainsKey(adresa_serveru) == false)
                        {
                            Server akt_server = new Server(adresa_serveru);
                            //akt_server.Stav_serveru = true;
                            akt_server.Spojeni = false;

                            lock (zamek)
                            {
                                Modul.aktivni_servery.Add(adresa_serveru, akt_server);
                                Modul.aktivni_servery[adresa_serveru].StavServeru(true);
                            }

                        }
                        else if (Modul.aktivni_servery.ContainsKey(adresa_serveru) == true)
                        {
                            lock (zamek)
                            {
                                //Modul.aktivni_servery[adresa_serveru].Stav_serveru = true;
                                
                                Modul.aktivni_servery[adresa_serveru].StavServeru(true);
                            }
                        }
                    }
                    Modul.prevod_Adresy.Clear();
                    adresa_serveru = null;
                }
            }

            poradi_oddelovace = 0;
            pocatek = 0;
        }


        //private void ZaslatOzn(string adresaProOzn, int port)
        //{
        //    try
        //    {
        //        logEvent.ZapsatZpravuDoLogu("Zasláno ozn.", System.Diagnostics.EventLogEntryType.Warning);

        //        TcpClient clientSocket = new TcpClient(adresaProOzn, 17000);
        //        NetworkStream networkStream = clientSocket.GetStream();
        //        StreamReader steamReader = new StreamReader(networkStream);
        //        string zprava = port.ToString();
        //        Byte[] pryc = Encoding.UTF8.GetBytes(zprava);

        //        networkStream.Write(pryc, 0, pryc.Length);
        //        networkStream.Close();
        //        clientSocket.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        logEvent.ZapsatZpravuDoLogu(e.ToString(), System.Diagnostics.EventLogEntryType.Error);
        //        Modul.aktivni_servery[adresaProOzn].Stav_serveru = false;
        //        Modul.aktivni_servery[adresaProOzn].Spojeni = false;

        //    }

        //}
        private void ZalozitSpojeni()
        {
            TcpServer klient = new TcpServer();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    casovac.Dispose();
                    
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~mmf() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
