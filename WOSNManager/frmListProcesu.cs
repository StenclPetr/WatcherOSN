using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WOSNManager
{
    public partial class frmListProcesu : Form
    {
        string stanice, user;
        string[,] pole;
        public frmListProcesu()
        {
            InitializeComponent();
        }
        public frmListProcesu(string stanice, string user, string[,] pole)
        {
            InitializeComponent();
            this.stanice = stanice;
            this.user = user;
            this.pole = pole;
            NaplnitList();
        }

        private void ukončitProcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in Modul.DictOfPC)
            {
                if (item.Value.JmenoStanice == stanice)
                {
                    foreach (var item2 in item.Value.Klienti)
                    {
                        if (item2.Value.JmenoUzivatele == user)
                        {
                            item2.Value.Kill(listView1.SelectedItems[0].SubItems[1].Text);
                            this.Close();
                        }
                    }
                }
            }
        }

        private void NaplnitList()
        {
            for (int i = 0; i < pole.Length/3; i++)
            {
                ListViewItem item = new ListViewItem(pole[i,0]);
                item.SubItems.Add(pole[i, 1]);
                item.SubItems.Add(pole[i, 2]);
                listView1.Items.Add(item);
            }

        }
    }
}
