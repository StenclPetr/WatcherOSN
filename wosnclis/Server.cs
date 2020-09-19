using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wosnclis
{
    class Server
    {
        private string _adresa_serveru;
        private bool _stav_serveru;
        private DateTime _cas_posledni_aktualizace;

        public Server(string ip_adresa_serveru)
        {
            _adresa_serveru = ip_adresa_serveru;
            _stav_serveru = false;
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

        public DateTime Cas_aktualizace
        {
            get { return _cas_posledni_aktualizace; }
            set { _cas_posledni_aktualizace = value; }
        }
    }
}
