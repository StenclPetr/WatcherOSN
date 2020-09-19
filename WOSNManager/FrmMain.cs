using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WOSNManager
{
    public delegate void AktualizaceHandler(string adresa,string user,string okno, bool ntfs);
    public delegate void StavPcHandler(string adresa, bool stav);

    public partial class frmMain : Form
    {
        private Thread t1;
        private int pocitadlo = 0;
        public frmMain()
        {
            InitializeComponent();
            Modul.aktu += Modul_aktu;
            Modul.stavPc += Modul_stavPc;

            Modul.ZjistiLocalniIpAdresu();
            Modul.NactiRozsah();
            Modul.NacistNastaveni();

            Modul.datumSpusteni = DateTime.Now.Date;
            Modul.ZalozeniSouboru();
            Timer.Start();

            foreach (var item in Modul.DictOfPC)
            {
                NaplnitList(item.Value.JmenoStanice);
            }

            //Thread t1 = new Thread(new ThreadStart(SpustitTCP));
            t1 = new Thread(new ThreadStart(SpustitTCP));
            t1.Start();

            pingerToolStripMenuItem.Checked = Modul.pinger;

            if (Modul.pinger == true)
            {
                if (backgroundWorker.IsBusy != true)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            else if (Modul.pinger == false)
            {
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].ImageKey == imageList.Images.Keys[0] | listView.Items[i].ImageKey == imageList.Images.Keys[1])
                    {
                        listView.Items[i].ImageKey = imageList.Images.Keys[4];
                    }
                }

            }

            this.Width = Modul.sirkaForm;
            this.Height = Modul.vyskaForm;

        }

        private void Modul_stavPc(string adresa, bool stav)
        {
            int pocet = 0;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (listView.Items[i].Text == Modul.DictOfPC[key: adresa].JmenoStanice)
                {
                    if (pingerToolStripMenuItem.Checked == true)
                    {
                        if (stav == true)
                        {
                            if (listView.Items[i].ImageKey == imageList.Images.Keys[0] | listView.Items[i].ImageKey == imageList.Images.Keys[4])
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[1];
                            }

                            else if (true)
                            {

                            }
                        }
                        else if (stav == false)
                        {
                            if (listView.Items[i].ImageKey != imageList.Images.Keys[0])
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[0];
                            }
                        }

                    }

                }

                if (listView.Items[i].ImageKey == imageList.Images.Keys[1])
                {
                    pocet++;
                }
                else if (listView.Items[i].ImageKey == imageList.Images.Keys[2])
                {
                    pocet++;
                }
                else if (listView.Items[i].ImageKey == imageList.Images.Keys[3])
                {
                    pocet++;
                }

            }
            if (pingerToolStripMenuItem.Checked == true)
            {
                toolStripStatusLabel1.Text = "ONLINE: " + pocet + "/" + listView.Items.Count;
            }

        }

        private void Modul_aktu(string adresa,string user,string okno, bool ntfs)
        {
            int pocetLogin = 0;

            for (int i = 0; i < listView.Items.Count; i++)
            {

                if (listView.Items[i].Text == Modul.DictOfPC[adresa].JmenoStanice)
                {

                    if (Modul.DictOfPC[adresa].ListKlientu.Count > 1)
                    {
                        int pocet = 0;
                        for (int j = 0; j < Modul.DictOfPC[adresa].ListKlientu.Count; j++)
                        {
                            if (Modul.DictOfPC[adresa].ListKlientu[j].StavSpojeni == true)
                            {
                                pocet++;
                            }
                        }

                        if (pocet > 1)
                        {
                            if (listView.Items[i].ImageKey != imageList.Images.Keys[3])
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[3];
                                listView.Items[i].ForeColor = Color.Red;
                            }
                            if (listView.Items[i].SubItems[2].Text != okno)
                            {
                                if (okno.ToString() != "")
                                {
                                    listView.Items[i].SubItems[2].Text = okno;
                                    listView.Items[i].SubItems[1].Text = user;
                                    Modul.dataSetLog.Tables["Z"].Rows.Add(Modul.DictOfPC[adresa].JmenoStanice, user, DateTime.Now.ToLongTimeString(), okno);
                                }
                            }
                            if (ntfs == true)
                            {
                                txtZmenyNTFS.Text = DateTime.Now.ToLongTimeString() + " " + user + " vytvořil nepovolené soubory." + Environment.NewLine + txtZmenyNTFS.Text;
                            }


                        }
                        else if (pocet == 1)
                        {
                            if (listView.Items[i].ImageKey != imageList.Images.Keys[2])
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[2];
                                listView.Items[i].ForeColor = Color.Black;
                            }

                            if (listView.Items[i].SubItems[2].Text != okno)
                            {
                                listView.Items[i].SubItems[2].Text = okno;
                                listView.Items[i].SubItems[1].Text = user;

                                if (okno.ToString() != "")
                                {
                                    Modul.dataSetLog.Tables["Z"].Rows.Add(Modul.DictOfPC[adresa].JmenoStanice, user, DateTime.Now.ToLongTimeString(), okno);
                                }
                            }

                            if (ntfs == true)
                            {
                                txtZmenyNTFS.Text = DateTime.Now.ToLongTimeString() + " " + user + " vytvořil nepovolené soubory." + Environment.NewLine + txtZmenyNTFS.Text;
                            }


                        }
                        else if (pocet == 0)
                        {
                            listView.Items[i].ImageKey = imageList.Images.Keys[1];
                            listView.Items[i].ForeColor = Color.Black;
                            listView.Items[i].SubItems[2].Text = "";
                            listView.Items[i].SubItems[1].Text = "";
                            Modul.dataSetLog.Tables["Z"].Rows.Add(Modul.DictOfPC[adresa].JmenoStanice, user, DateTime.Now.ToLongTimeString(), "Odhlášení");

                        }

                    }
                    else if (Modul.DictOfPC[adresa].ListKlientu.Count == 1)
                    {
                        if (Modul.DictOfPC[adresa].ListKlientu[0].StavSpojeni == true)
                        {
                            if (listView.Items[i].ImageKey != imageList.Images.Keys[2])
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[2];
                                listView.Items[i].ForeColor = Color.Black;
                            }

                            if (listView.Items[i].SubItems[2].Text != okno)
                            {
                                listView.Items[i].SubItems[2].Text = okno;
                                listView.Items[i].SubItems[1].Text = user;

                                if (okno.ToString() != "")
                                {
                                    Modul.dataSetLog.Tables["Z"].Rows.Add(Modul.DictOfPC[adresa].JmenoStanice, user, DateTime.Now.ToLongTimeString(), okno);
                                }
                            }
                            else if (listView.Items[i].SubItems[1].Text == "")
                            {
                                listView.Items[i].SubItems[2].Text = okno;
                                listView.Items[i].SubItems[1].Text = user;

                            }

                            if (ntfs == true)
                            {
                                txtZmenyNTFS.Text = DateTime.Now.ToLongTimeString() + " " + user + " vytvořil nepovolené soubory."  + Environment.NewLine + txtZmenyNTFS.Text; 
                            }

                        }
                        else if (Modul.DictOfPC[adresa].ListKlientu[0].StavSpojeni == false)
                        {
                            if (pingerToolStripMenuItem.Checked == false)
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[4];
                            }
                            else if (pingerToolStripMenuItem.Checked == true)
                            {
                                listView.Items[i].ImageKey = imageList.Images.Keys[1];
                            }
                            
                            listView.Items[i].SubItems[2].Text = "";
                            listView.Items[i].SubItems[1].Text = "";
                            Modul.dataSetLog.Tables["Z"].Rows.Add(Modul.DictOfPC[adresa].JmenoStanice, user, DateTime.Now.ToLongTimeString(), "Odhlášení");
                        }
                    }
                }

                if (listView.Items[i].ImageKey == imageList.Images.Keys[2] | listView.Items[i].ImageKey == imageList.Images.Keys[3])
                {
                    pocetLogin++;
                }

            }
            if (pingerToolStripMenuItem.Checked == false)
            {
                toolStripStatusLabel1.Text = "ONLINE: " + pocetLogin + "/" + listView.Items.Count;
            }
            toolStripStatusLabel2.Text = " | Přihlášeno: " + pocetLogin;
        }

 
        private void NaplnitList(string jmeno)
        {
            ListViewItem item = new ListViewItem(jmeno);
            item.SubItems.Add("");
            item.SubItems.Add("");
            item.ImageKey = imageList.Images.Keys[0];
            listView.Items.Add(item);
        }

        private void SpustitTCP()
        {
            TcpServer tcpServer = new TcpServer();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            pocitadlo++;
            if (Modul.myComputer.Network.IsAvailable == true)
            {
                Modul.Send();
                foreach (var item in Modul.DictOfPC)
                {
                    item.Value.Start();
                }
            }
            else if (Modul.myComputer.Network.IsAvailable == false)
            {
                lock (this)
                {
                    Modul.dataSetLog.WriteXml(Modul.cesta + Modul.soubor);

                }
            }

            if (Modul.datumSpusteni != DateTime.Now.Date)
            {
                Modul.dataSetLog.WriteXml(Modul.cesta + Modul.soubor);
                Modul.dataSetLog.Dispose();
                Modul.ZalozeniSouboru();
                Modul.datumSpusteni = DateTime.Now.Date;
            }

            if (pocitadlo == 200)
            {
                lock (this)
                {
                    Modul.dataSetLog.WriteXml(Modul.cesta + Modul.soubor);
                }
                toolStripStatusLabel3.Text = " | Log uložen: " + DateTime.Now.ToShortTimeString();
                pocitadlo = 0;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Timer.Stop();
            Modul.client.Close();
            Modul.vypnout = false;
            lock (this)
            {
                Modul.dataSetLog.WriteXml(Modul.cesta + Modul.soubor);
            }
            //backgroundWorker.CancelAsync();

            try
            {
                TcpClient clientSocket = new TcpClient(Modul.local_address, 17000);
                NetworkStream networkStream = clientSocket.GetStream();
                StreamReader steamReader = new StreamReader(networkStream);
                string zprava = "ukončit";
                Byte[] pryc = Encoding.UTF8.GetBytes(zprava);

                networkStream.Write(pryc, 0, pryc.Length);
                networkStream.Close();
                clientSocket.Close();
            }
            catch (Exception)
            {

            }
            t1.Abort();
            //Process process =  Process.GetCurrentProcess();
            //process.Kill();

            Modul.point.X = this.Location.X;
            Modul.point.Y = this.Location.Y;
            Modul.sirkaForm = this.Width;
            Modul.vyskaForm = this.Height;
            Modul.pinger = pingerToolStripMenuItem.Checked;
            Modul.UlozitNastaveni();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 0; i < Modul.DictOfPC.Count; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                    uint LongIP;
                    string buffer;
                    UInt32 hIP;
                    uint timeout;
                    buffer = new StringBuilder().Append(' ', 32).ToString();
                    LongIP = convertIPtoLong(Modul.dataSetPc.Tables["PC"].Rows[i]["adresa"].ToString());
                    hIP = PING.IcmpCreateFile();
                    PING.pIPo.TTL = 255;
                    timeout = 500;
                    PING.IcmpSendEcho(hIP, LongIP, buffer, (uint)buffer.Length, ref PING.pIPo, ref PING.pIPe, (uint)Marshal.SizeOf(PING.pIPe) + 8, timeout);
                    //result = Modul.dataSetPc.Tables["PC"].Rows[i]["adresa"].ToString() + "-" + describeResponse(PING.pIPe.Status);
                    //worker.ReportProgress(i * 10);
                    if (PING.pIPe.Status == 0)
                    {
                        Modul.DictOfPC[Modul.dataSetPc.Tables["PC"].Rows[i]["adresa"].ToString()].StavPc(true);
                    }
                    else if (PING.pIPe.Status != 0)
                    {
                        Modul.DictOfPC[Modul.dataSetPc.Tables["PC"].Rows[i]["adresa"].ToString()].StavPc(false);
                    }
                }
            }
        }
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (pingerToolStripMenuItem.Checked == true)
            {
                if (backgroundWorker.IsBusy != true)
                {
                    GC.Collect();
                    backgroundWorker.RunWorkerAsync();
                }
            }
        }

        public UInt32 convertIPtoLong(string ip)
        {
            string[] digits;
            digits = ip.Split(".".ToCharArray());
            return Convert.ToUInt32(
            Convert.ToUInt32(digits[3]) * Math.Pow(2, 24) +
            Convert.ToUInt32(digits[2]) * Math.Pow(2, 16) + Convert.ToUInt32(digits[1]) * Math.Pow(2, 8) + Convert.ToUInt32(digits[0]));
        }

        #region KontextovaNabidka
        private void wakeUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[i].Text)
                        {
                            Modul.WakeUp(item.Value.MacAdresa, item.Value.AdresaStanice, "255.255.255.255");
                        }
                    }
                }
            }
            else if (listView.SelectedItems.Count == 1)
            {
                foreach (var item in Modul.DictOfPC)
                {
                    if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                    {
                        Modul.WakeUp(item.Value.MacAdresa, item.Value.AdresaStanice, "255.255.255.255");
                    }
                }
            }
        }

        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vícenásobný výběr nelze použít pro ScreenShot!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (listView.SelectedItems.Count == 1)
            {
                if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {
                            for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                            {
                                if (item.Value.ListKlientu[i].JmenoUzivatele == listView.SelectedItems[0].SubItems[1].Text)
                                {
                                    item.Value.ListKlientu[i].ScreenShot("SCREEN");
                                    if (Modul._imgScreenShot != null)
                                    {
                                        frmScreenShot frmScreenShot = new frmScreenShot(item.Value.JmenoStanice, item.Value.ListKlientu[i].JmenoUzivatele);
                                        frmScreenShot.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                }
                else 
                {
                    MessageBox.Show("Není přihlášený uživatel!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }


        private void listProcesůToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vícenásobný výběr nelze použít pro výčet procesů!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (listView.SelectedItems.Count == 1)
            {
                if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {
                            for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                            {
                                if (item.Value.ListKlientu[i].JmenoUzivatele == listView.SelectedItems[0].SubItems[1].Text)
                                {
                                    item.Value.ListKlientu[i].ListProcesu("LIST");
                                    frmListProcesu listProcesu = new frmListProcesu(item.Value.JmenoStanice, item.Value.ListKlientu[i].JmenoUzivatele, item.Value.ListKlientu[i].Pole);
                                    listProcesu.ShowDialog();
                                }
                            }
                        }
                    }
                }
                else if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[3])
                {
                    MessageBox.Show("Je přihlášený více než uživatel!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("Není přihlášený uživatel!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void zaslatZprávuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                string zprava = Microsoft.VisualBasic.Interaction.InputBox("Napište text pro hromadnou zprávu.");
                if (zprava == "")
                {
                    goto konec;
                }
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[i].Text)
                        {
                            for (int j = 0; j < item.Value.ListKlientu.Count; j++)
                            {
                                if (item.Value.ListKlientu[j].JmenoUzivatele == listView.SelectedItems[i].SubItems[1].Text)
                                {
                                    item.Value.ListKlientu[j].ZaslatZpravu(zprava);
                                }
                            }
                        }
                    }
                }

            }
            else if (listView.SelectedItems.Count == 1)
            {
                if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {
                            for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                            {
                                if (item.Value.ListKlientu[i].JmenoUzivatele == listView.SelectedItems[0].SubItems[1].Text)
                                {
                                    string zprava = Microsoft.VisualBasic.Interaction.InputBox("Napište text zprávy.");
                                    if (zprava != "")
                                    {
                                        item.Value.ListKlientu[i].ZaslatZpravu(zprava);
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Není přihlášený uživatel!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            konec:;
        }

        private void zamknoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[i].Text)
                        {
                            for (int j = 0; j < item.Value.ListKlientu.Count; j++)
                            {
                                if (item.Value.ListKlientu[j].JmenoUzivatele == listView.SelectedItems[i].SubItems[1].Text)
                                {
                                    item.Value.ListKlientu[j].Zamknout();
                                }
                            }
                        }
                    }
                }

            }
            else if (listView.SelectedItems.Count == 1)
            {
                if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {
                            for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                            {
                                if (item.Value.ListKlientu[i].JmenoUzivatele == listView.SelectedItems[0].SubItems[1].Text)
                                {
                                    item.Value.ListKlientu[i].Zamknout();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Není přihlášený uživatel!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void odhlásitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[i].Text)
                        {
                            if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                            {
                                for (int j = 0; j < item.Value.ListKlientu.Count; j++)
                                {
                                    if (item.Value.ListKlientu[j].JmenoUzivatele == listView.SelectedItems[i].SubItems[1].Text)
                                    {
                                        item.Value.ListKlientu[j].Odhlasit();
                                    }
                                }

                            }
                        }
                    }
                }

            }
            else if (listView.SelectedItems.Count == 1)
            {
                if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {
                            int pocet = 0;
                            for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                            {
                                if (item.Value.ListKlientu[i].StavSpojeni == true)
                                {
                                    pocet++;
                                }
                            }
                            if (pocet > 1)
                            {
                                frmDialog dialog = new frmDialog(item.Value.AdresaStanice);
                                dialog.ShowDialog();
                                
                            }
                            else if (pocet == 1)
                            {
                                for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                                {
                                    if (item.Value.ListKlientu[i].JmenoUzivatele == listView.SelectedItems[0].SubItems[1].Text)
                                    {
                                        item.Value.ListKlientu[i].Odhlasit();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[3])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {
                            int pocet = 0;
                            for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                            {
                                if (item.Value.ListKlientu[i].StavSpojeni == true)
                                {
                                    pocet++;
                                }
                            }
                            if (pocet > 1)
                            {
                                frmDialog dialog = new frmDialog(item.Value.AdresaStanice);
                                dialog.ShowDialog();

                            }
                            else if (pocet == 1)
                            {
                                for (int i = 0; i < item.Value.ListKlientu.Count; i++)
                                {
                                    if (item.Value.ListKlientu[i].JmenoUzivatele == listView.SelectedItems[0].SubItems[1].Text)
                                    {
                                        item.Value.ListKlientu[i].Odhlasit();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Není přihlášený uživatel!", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

        }

        private void restartovatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[i].Text)
                        {
                            if (item.Value.StavStanice == true)
                            {
                                item.Value.Restartovat();
                            }

                        }
                    }
                }

            }
            else if (listView.SelectedItems.Count == 1)
            {
                foreach (var item in Modul.DictOfPC)
                {
                    if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                    {
                        if (item.Value.StavStanice == true)
                        {
                            item.Value.Restartovat();
                        }
                    }
                }

            }
        }

        private void vypnoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                for (int i = 0; i < listView.SelectedItems.Count; i++)
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[i].Text)
                        {
                            if (item.Value.StavStanice == true)
                            {
                                item.Value.Vypnout();
                            }

                        }
                    }
                }

            }
            else if (listView.SelectedItems.Count == 1)
            {
                foreach (var item in Modul.DictOfPC)
                {
                    if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                    {
                        if (item.Value.StavStanice == true)
                        {
                            item.Value.Vypnout();
                        }
                    }
                }

            }
        }
        private void vlastnostiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vyberte pouze jednu položku.", "UPOZORNĚNÍ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (listView.SelectedItems.Count == 1)
            {
                if (listView.SelectedItems[0].ImageKey == imageList.Images.Keys[2])
                {
                    foreach (var item in Modul.DictOfPC)
                    {
                        if (item.Value.JmenoStanice == listView.SelectedItems[0].Text)
                        {

                        }
                    }
                }
            }
        }
        #endregion


        private void konecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pingerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pingerToolStripMenuItem.Checked == true)
            {
                if (backgroundWorker.IsBusy != true)
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            else if (pingerToolStripMenuItem.Checked == false)
            {
                backgroundWorker.CancelAsync();

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    if (listView.Items[i].ImageKey == imageList.Images.Keys[0] | listView.Items[i].ImageKey == imageList.Images.Keys[1])
                    {
                        listView.Items[i].ImageKey = imageList.Images.Keys[4];
                    }
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            Screen[] screens;
            screens = System.Windows.Forms.Screen.AllScreens;
            if (screens.Length > 1)
            {
                this.Left = Modul.point.X;
                this.Top = Modul.point.Y;
            }
            
        }

        private void oProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmApp frmApp = new frmApp();
            frmApp.ShowDialog();
            
        }

        private void listView_ItemMouseHover(object sender, ListViewItemMouseHoverEventArgs e)
        {
            toolTip1.SetToolTip(listView, e.Item.SubItems[2].Text);
        }
    }

    static class Modul
    {
        public static Computer myComputer = new Computer();
        public static string local_address;
        public static IPAddress[] adresa;
        public static Dictionary<string, PC> DictOfPC = new Dictionary<string, PC>();
        public static bool vypnout = true;
        public static System.Drawing.Bitmap _imgScreenShot;
        public static DataSet dataSetPc = new DataSet();
        public static event AktualizaceHandler aktu;
        public static event StavPcHandler stavPc;
        public static string cesta;
        public static string soubor;
        public static DataSet dataSetLog;
        public static DateTime datumSpusteni;

        private static bool s_adresa = true;
        private const int PORT_NUMBER = 15000;
        public static UdpClient client = new UdpClient();
        private static IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
        private static byte[] bytes;

        //settings
        public static DataSet dataSetSet;
        public static Point point;
        public static int sirkaForm, vyskaForm;
        public static bool pinger;

        public static void NacistNastaveni()
        {
            try
            {
                dataSetSet = new DataSet();
                dataSetSet.ReadXml(Environment.CurrentDirectory + @"\settings.xml");
                point = new Point(int.Parse(dataSetSet.Tables["set"].Rows[0]["x"].ToString()), int.Parse(dataSetSet.Tables["set"].Rows[0]["y"].ToString()));
                sirkaForm = int.Parse(dataSetSet.Tables["set"].Rows[0]["form_widht"].ToString());
                vyskaForm = int.Parse(dataSetSet.Tables["set"].Rows[0]["form_height"].ToString());
                pinger = bool.Parse(dataSetSet.Tables["set"].Rows[0]["pinger"].ToString());

            }
            catch (Exception)
            {
            
                dataSetSet = new DataSet();
                DataTable dataTable = new DataTable("set");
                DataColumn col1 = dataTable.Columns.Add("x", typeof(string));
                DataColumn col2 = dataTable.Columns.Add("y", typeof(string));
                DataColumn col3 = dataTable.Columns.Add("form_widht", typeof(string));
                DataColumn col4 = dataTable.Columns.Add("form_height", typeof(string));
                DataColumn col5 = dataTable.Columns.Add("pinger", typeof(string));

                dataSetSet.Tables.Add(dataTable);

                point = new Point(0,0);
                sirkaForm = 450;
                vyskaForm = 805;
                pinger = true;

                dataSetSet.Tables["set"].Rows.Add(point.X.ToString(), point.Y.ToString(), sirkaForm.ToString(), vyskaForm.ToString(), pinger.ToString());

                dataSetSet.WriteXml(Environment.CurrentDirectory + @"\settings.xml");

            }
        }

        public static void UlozitNastaveni()
        {
            dataSetSet.Tables["set"].Rows[0]["x"] = point.X.ToString();
            dataSetSet.Tables["set"].Rows[0]["y"] = point.Y.ToString();
            dataSetSet.Tables["set"].Rows[0]["form_widht"] = sirkaForm.ToString();
            dataSetSet.Tables["set"].Rows[0]["form_height"] = vyskaForm.ToString();
            dataSetSet.Tables["set"].Rows[0]["pinger"] = pinger.ToString();
            dataSetSet.WriteXml(Environment.CurrentDirectory + @"\settings.xml");
        }

        public static void MAktualizace(ref string adresa,ref string user,ref string okno, ref bool ntfs)
        {
            if (aktu != null)
            {
                aktu(adresa,user,okno,ntfs);
            }
        }

        public static void MStavPc(ref string adresa, bool stav)
        {
            if (stavPc != null)
            {
                stavPc(adresa, stav);
            }
        }
        public static void ZjistiLocalniIpAdresu()
        {
            adresa = Dns.GetHostAddresses(Environment.MachineName);
            while (s_adresa)
            {
                foreach (var item in adresa)
                {
                    if (item.ToString().Length <= 15 & item.ToString().Length >= 10 & item.ToString().Substring(0, 3) != "169")
                    {
                        bytes = Encoding.ASCII.GetBytes(item.ToString());
                        local_address = item.ToString();
                        s_adresa = false;
                        break;
                    }
                }
            }
        }
        public static void WakeUp(string macAddress, string ipAddress, string subnetMask)
        {
            UdpClient client = new UdpClient();

            Byte[] datagram = new byte[102];

            for (int i = 0; i <= 5; i++)
            {
                datagram[i] = 0xff;
            }

            string[] macDigits = null;
            if (macAddress.Contains("-"))
            {
                macDigits = macAddress.Split('-');
            }
            else
            {
                macDigits = macAddress.Split(':');
            }

            if (macDigits.Length != 6)
            {
                throw new ArgumentException("Incorrect MAC address supplied!");
            }

            int start = 6;
            for (int i = 0; i < 16; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                }
            }

            IPAddress address = IPAddress.Parse(ipAddress);
            IPAddress mask = IPAddress.Parse(subnetMask);
            //IPAddress broadcastAddress = address.GetBroadcastAddress(mask);

            client.Connect(subnetMask, 9);
            client.Send(datagram, datagram.Length);
        }

        public static void Send()
        {
            client.Send(bytes, bytes.Length, ip);
        }

        public static void NactiRozsah()
        {
            dataSetPc.ReadXml(Environment.CurrentDirectory + @"\rozsahSledovanýchPC.xml");
            for (int i = 0; i < dataSetPc.Tables["PC"].Rows.Count; i++)
            {
                DictOfPC.Add(dataSetPc.Tables["PC"].Rows[i]["adresa"].ToString(),
                    new PC(dataSetPc.Tables["PC"].Rows[i]["jmeno"].ToString(),
                    dataSetPc.Tables["PC"].Rows[i]["adresa"].ToString(),
                    dataSetPc.Tables["PC"].Rows[i]["mac"].ToString()));
            }

        }
        public static void ZalozeniSouboru()
        {
            string mesic;
            string den;
            if (DateTime.Now.Month < 10)
            {
                mesic = "0" + DateTime.Now.Month;
            }
            else
            {
                mesic = DateTime.Now.Month.ToString();
            }
            if (DateTime.Now.Day < 10)
            {
                den = "0" + DateTime.Now.Day;
            }
            else
            {
                den = DateTime.Now.Day.ToString();
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory + @"\data\" + DateTime.Now.Year + @"\" + mesic + @"\");

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            cesta = Environment.CurrentDirectory + @"\data\" + DateTime.Now.Year + @"\" + mesic + @"\";
            soubor = DateTime.Now.Year + "-" + mesic + "-" + den + ".xml";

            FileInfo fileInfo = new FileInfo(cesta + soubor);

            if (!fileInfo.Exists)
            {
                dataSetLog = new DataSet("Log");
                DataTable dataTable = new DataTable("Z");
                DataColumn colJS = dataTable.Columns.Add("S", typeof(string));
                DataColumn colJU = dataTable.Columns.Add("U", typeof(string));
                DataColumn colT = dataTable.Columns.Add("T", typeof(string));
                DataColumn colAO = dataTable.Columns.Add("O", typeof(string));

                dataSetLog.Tables.Add(dataTable);
                dataSetLog.WriteXml(cesta + soubor);
            }
            else
            {
                dataSetLog = new DataSet("Log");
                DataTable dataTable = new DataTable("Z");
                DataColumn colJS = dataTable.Columns.Add("S", typeof(string));
                DataColumn colJU = dataTable.Columns.Add("U",typeof(string));
                DataColumn colT = dataTable.Columns.Add("T", typeof(string));
                DataColumn colAO = dataTable.Columns.Add("O",typeof(string));

                dataSetLog.Tables.Add(dataTable);

                dataSetLog.ReadXml(cesta + soubor);
            }

        }
    }

    public class PING
    {
        public struct IP_OPTION_INFORMATION
        {
            public byte TTL, Tos, Flags, OptionSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string OptionsData;
        }
        public struct ICMP_ECHO_REPLY
        {
            public uint Address, Status, RoundTripTime;
            public ushort DataSize, Reserved;
            public IP_OPTION_INFORMATION Options;
        }
        [DllImport("icmp.dll", SetLastError = true)]
        public static extern uint IcmpSendEcho(
        uint IcmpHandle,
        uint DestAddress,
        string RequestData,
        uint RequestSize,
        ref IP_OPTION_INFORMATION RequestOptns,
        ref ICMP_ECHO_REPLY ReplyBuffer,
        uint ReplySize,
        uint TimeOut);
        [DllImport("icmp.dll", SetLastError = true)]
        public static extern uint IcmpCreateFile();
        public static IP_OPTION_INFORMATION pIPo;
        public static ICMP_ECHO_REPLY pIPe;
    }

}
