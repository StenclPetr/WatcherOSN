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
    public partial class frmDialog : Form
    {
        string _adresa;
        public frmDialog()
        {
            InitializeComponent();
        }
        public frmDialog(string adresa)
        {
            InitializeComponent();
            this.Text = "Odhlášení uživatele";
            _adresa = adresa;
            for (int i = 0; i < Modul.DictOfPC[adresa].ListKlientu.Count; i++)
            {
                if (Modul.DictOfPC[adresa].ListKlientu[i].StavSpojeni == true)
                {
                    comboBox1.Items.Add(Modul.DictOfPC[adresa].ListKlientu[i].JmenoUzivatele);
                }
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Vyber položku ze seznamu!", "CHYBA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (comboBox1.SelectedItem.ToString() != "")
            {
                for (int i = 0; i < Modul.DictOfPC[_adresa].ListKlientu.Count; i++)
                {
                    if (Modul.DictOfPC[_adresa].ListKlientu[i].JmenoUzivatele == comboBox1.SelectedItem.ToString())
                    {
                        Modul.DictOfPC[_adresa].ListKlientu[i].Odhlasit();
                        break;
                    }
                }
            }
            comboBox1.Dispose();
            this.Close();
        }

        private void cmdStorno_Click(object sender, EventArgs e)
        {
            comboBox1.Dispose();
            this.Close();
        }
    }
}
