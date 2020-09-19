using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;

namespace wosnclis
{
    public partial class Wosnclis : ServiceBase
    {
        System.Timers.Timer casovac = new System.Timers.Timer();

        public string adresa_serveru;
        public Wosnclis()
        {
            InitializeComponent();

            Modul.adresa = System.Net.Dns.GetHostAddresses(Environment.MachineName);
            while (Modul.s_adresa)
            {
                foreach (var item in Modul.adresa)
                {
                    if (item.ToString().Length <= 15 & item.ToString().Length >= 10 & item.ToString().Substring(0, 3) != "169")
                    {
                        Modul.local_address = item.ToString();
                        Modul.ZapisDoEventlogu("Přidělená adresa serveru: " + Modul.local_address, EventLogEntryType.Information);
                        Modul.s_adresa = false;
                        break;
                    }
                }
            }

        }

        protected override void OnStart(string[] args)
        {

            Modul.udper = new UDPer();
            Modul.sts = new ServisTcpServer();

            casovac.Elapsed += Casovac_Elapsed;
            casovac.Interval = 3000;

            casovac.Start();
            Modul.udper.Start();

            Modul.thdListener = new Thread(new ThreadStart(Modul.sts.ListenerThread));
            Modul.thdListener.Start();
        }

        protected override void OnStop()
        {
            casovac.Stop();
            //Modul.udper.Stop();
            Modul.thdListener.Suspend();
        }

        private void Casovac_Elapsed(object sender, ElapsedEventArgs e)
        {
            Modul.pocitadlo++;

            foreach (var item in Modul.servery)
            {
                if (item.Stav_serveru == true)
                {
                    Modul.aktu_cas = DateTime.Now;
                    if (Modul.aktu_cas - item.Cas_aktualizace > new TimeSpan(0, 0, 5))
                    {
                        item.Stav_serveru = false;
                    }
                }
            }

            Modul.mmf_text = null;

            foreach (var item in Modul.servery)
            {
                if (item.Stav_serveru == true)
                {
                    Modul.mmf_text = Modul.mmf_text + item.Adresa_serveru.ToString() + ";";
                }
            }


            if (Modul.mmf_text != null)
            {
                if (Modul.last_write_mmf_text != Modul.mmf_text)
                {
                    ZapisDoTmp();
                }
            }
            else if (Modul.mmf_text == null)
            {
                Modul.mmf_text = "empty";
                ZapisDoTmp();
            }


        }
        private static void ZapisDoTmp()
        {
            if (Modul.mmf_text != null)
            {
                using (Modul.mmf = new StreamWriter(Environment.CurrentDirectory + @"\mmf.data"))
                {
                    lock ("myLock")
                    {
                        Modul.mmf.WriteLine(Modul.mmf_text);
                        Modul.last_write_mmf_text = Modul.mmf_text;
                    }
                }
                if (Modul.pocitadlo == 10)
                {
                    Modul.pocitadlo = 0;
                    GC.Collect();
                }
            }
        }


    }
    static class Modul
    {
        public static List<Server> servery = new List<Server>();
        public static ArrayList alSockets = new ArrayList();
        public static DateTime aktu_cas;
        public static string mmf_text;
        public static string last_write_mmf_text;
        public static StreamWriter mmf;
        public static int pocitadlo = 0;

        public static IPAddress[] adresa;
        public static bool s_adresa = true;
        public static string local_address;
        public static string remoteIP;
        public static EventLog objLog = new EventLog();


        public static UDPer udper;
        public static ServisTcpServer sts;
        public static Thread thdListener;

        public static void ZapisDoEventlogu(string zprava, EventLogEntryType typ)
        {

                Modul.objLog.Source = "Wosnclis";
                Modul.objLog.Log = "Application";
                Modul.objLog.WriteEntry(zprava, typ);
        }

    }
}
