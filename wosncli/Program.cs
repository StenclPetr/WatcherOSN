using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using Microsoft.VisualBasic.Devices;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;

namespace wosncli
{

    class Program
    {
        [STAThread]
        static void Main()
        {
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            //LogEvent logEvent = new LogEvent("Wosncli.Program");

            if (Modul.myComputer.Network.IsAvailable == true)
            {
                Modul.adresa = System.Net.Dns.GetHostAddresses(Environment.MachineName);
                while (Modul.s_adresa)
                {
                    foreach (var item in Modul.adresa)
                    {
                        if (item.ToString().Length <= 15 & item.ToString().Length >= 10 & item.ToString().Substring(0, 3) != "169")
                        {
                            Modul.local_address = item.ToString();
                            //logEvent.ZapsatZpravuDoLogu("Adresa síťového rozhraní: " + Modul.local_address, EventLogEntryType.Information);
                            Modul.s_adresa = false;
                            break;
                        }
                    }
                }

                Thread t1 = new Thread(new ThreadStart(SpustinMmf));
                t1.Start();
                FileSystemWatcherExam.WatchFile(@"C:\users\" + Environment.UserName + @"\");

                do
                {
                    Thread.Sleep(250);
                } while (true);



            }
            else if (Modul.myComputer.Network.IsAvailable == false)
            {
                //logEvent.ZapsatZpravuDoLogu("Problém se síťovým připojením! Zkontroluj kabel.", EventLogEntryType.Error);
            }

        }

        private static void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            //Poslední vysílání?
            //Modul.ZapsatZpravuDoLogu("Vypínám", EventLogEntryType.Warning);
            
        }


        public static void SpustinMmf()
        {
            Mmf mmmf = new Mmf();
        }

    }
    public class FileSystemWatcherExam
    {

        public static void WatchFile(string path)
        {
            FileSystemWatcher lWatcher = new FileSystemWatcher();
            //nastavime adresar, ktery ma byt sledovan
            lWatcher.Path = path;
            //vytvorime instance delegatu
            lWatcher.Created += new FileSystemEventHandler(FSCreated);
            //nastavime, ze maji byt sledovany i zmeny ve vnorenych adresarich
            lWatcher.IncludeSubdirectories = true;
            //spustime sledovani
            lWatcher.EnableRaisingEvents = true;


        }
        private static void FSCreated(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.Contains(".mp3") == true | e.FullPath.Contains(".wma") == true | e.FullPath.Contains(".flac") == true)
            {
                foreach (var item in Modul.aktivni_servery)
                {
                    if (item.Value.Stav_serveru == true)
                    {
                        item.Value.DataNtfs = true;
                    }
                }
            }
        }

    }

    static class Modul
    {

        public static Computer myComputer = new Computer();
        public static IPAddress[] adresa;
        public static bool s_adresa = true;
        public static string local_address;
        //public static EventLog objLog = new EventLog();
        public static StreamReader reader;
        public static StringBuilder prevod_Adresy = new StringBuilder();
        public static Dictionary<string, Server> aktivni_servery = new Dictionary<string, Server>();
        public static ArrayList alSockets = new ArrayList();
        public static string remoteIP;
        public static int port_klient_tcp_server;

        private static IntPtr hWnd;
        private static StringBuilder caption = new StringBuilder(256);
        const int nchar = 256;


        //public static void ZapsatZpravuDoLogu(string zprava, EventLogEntryType typ)
        //{
        //    objLog.WriteEntry(zprava, typ);
        //}

        [DllImport("user32.dll")]
        public static extern int LockWorkStation();

        [DllImport("user32.dll", EntryPoint = "GetWindowTextW", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int GetActiveWindowText(IntPtr hWnd, StringBuilder lbString, int cch);


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr ExitWindowsEx(IntPtr uFlags, IntPtr dwReserved);

        const int EWX_REBOOT = 2;
        const int EWX_FORCE = 4;
        const int EWX_SHUTDOWN = 1;
        const int EWX_LOGOFF = 0;

        public static string AktivniOkno()
        {
            hWnd = GetForegroundWindow();
            GetActiveWindowText(hWnd, caption, nchar);
            return caption.ToString();
        }
        public static void LogoffUser()
        {
            long ret;
            ret = (long)ExitWindowsEx((IntPtr)EWX_FORCE, (IntPtr)0);

        }
        public static MemoryStream AktivniProcesy()
        {
            ListProcesu list = new ListProcesu();

            MemoryStream stream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, list.ListyProcesu);

            return stream;
            
        }
        public static void ProcesKill(int id_procesu)
        {
            Process[] lokalni_procesy;
            lokalni_procesy = Process.GetProcesses();

            foreach (var item in lokalni_procesy)
            {
                if (item.Id == id_procesu)
                {
                    item.Kill();
                    break;
                }
            }

        }
        public static Bitmap PrintScreen()
        {
            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

            Graphics graphics = Graphics.FromImage(printscreen as Image);

            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            return printscreen;

        }
    }
    class ListProcesu
    {
        private System.Diagnostics.Process[] aaa;
        int veliskost;
        string[,] pole;

        public ListProcesu()
        {
            aaa = System.Diagnostics.Process.GetProcesses();
            long handle_procesu;
            int pocitadlo = 0;
            foreach (var item in aaa)
            {
                handle_procesu = (long)item.MainWindowHandle;
                if (handle_procesu != 0)
                {
                    veliskost++;
                }

            }
            pole = new string[veliskost, 3];

            foreach (var item in aaa)
            {
                handle_procesu = (long)item.MainWindowHandle;
                if (handle_procesu != 0)
                {
                    pole[pocitadlo, 0] = item.ProcessName;
                    pole[pocitadlo, 1] = item.Id.ToString();
                    pole[pocitadlo, 2] = item.MainWindowTitle;

                    pocitadlo++;
                }

            }

        }

        public string[,] ListyProcesu
        {
            get { return pole; }
        }
    }
    class LogEvent : IDisposable
    {
        private EventLog objLog = new EventLog();


        public LogEvent(string zdroj)
        {
            objLog.Source = zdroj;
            objLog.Log = "Application";
        }
        public string ZdrojZpravy
        {
            get { return objLog.Source; }
            set { objLog.Source = value; }
        }
        public string TypLogu
        {
            get { return objLog.Log; }
            set { objLog.Log = value; }
        }

        public void ZapsatZpravuDoLogu(string zprava,EventLogEntryType typ)
        {
            objLog.WriteEntry(zprava, typ);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    objLog.Dispose();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~eventLog() {
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
