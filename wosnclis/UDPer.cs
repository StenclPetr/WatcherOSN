using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace wosnclis
{
    class UDPer : IDisposable
    {
        // konstanta s číslem portu
        const int PORT_NUMBER = 15000;
        // nový UDP klient na portu - viz proměnná const int PORT_NUMBER
        private readonly UdpClient udp = new UdpClient(PORT_NUMBER);

        private IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);

        // inicializace pole pro výsledek
        IAsyncResult ar_ = null;

        // inicializace treadu
        Thread t = null;


        public void Start()
        {
            // spuštění
            // kontrola zda není již spuštěno
            if (t != null)
            {
                // již běží, vyvolání vyjímky, konec pokusu o start
                throw new Exception("Already started, stop first");
            }
            // neběží, možno spustit
            StartListening();
        }


        public void Stop()
        {
            // ukončení
            try
            {
                // pokus o ukončení
                udp.Close();
            }
            // případné zachycení vyjímky
            catch
            {
            }
        }



        private void StartListening()
        {
            // zahájení příjmu
            GC.Collect();
            ar_ = udp.BeginReceive(Receive, new object());
        }


        private void Receive(IAsyncResult ar) // vlastní příjem
        {
                // inicializace EndPointu se zadanou IP adresou a na daném portu
                ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
                // pole byte s přijatými daty
                byte[] bytes = udp.EndReceive(ar, ref ip);
                // převod pole na string
                string message = Encoding.UTF8.GetString(bytes);

            // cyklus s aktualizací
            foreach (var item in Modul.servery)
            {
                // pokud IP serveru odpovídá IP z přijatých dat pak
                if (item.Adresa_serveru == ip.Address.ToString())
                {
                    // nastav Stav_serveru na "true"
                    item.Stav_serveru = true;
                    // nastav Cas_aktualizace na aktuální hodnotu
                    item.Cas_aktualizace = DateTime.Now;
                    // běž na blok konec
                    goto konec;
                }
            }

            // přidání nového server se získanou adresou, převedenou na string
            Modul.servery.Add(new Server(ip.Address.ToString()));


        konec:

            // zahájení poslouchání
            StartListening();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    udp.Close();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UDPer() {
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
