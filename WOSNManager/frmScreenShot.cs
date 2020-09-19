using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace WOSNManager
{
    public partial class frmScreenShot : Form
    {
        string stanice, user;
        public frmScreenShot()
        {
            InitializeComponent();
        }
        public frmScreenShot(string stanice,string user)
        {
            InitializeComponent();
            this.stanice = stanice;
            this.user = user;
            this.Text +=" - " + stanice + " - " + user + " - " + DateTime.Now;
            this.Width = Modul._imgScreenShot.Width;
            this.Height = Modul._imgScreenShot.Height;
            pictureBox1.Image = Modul._imgScreenShot;

        }
        private void frmScreenShot_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Dispose();
        }

        private void uložitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap aaa = new Bitmap(Modul._imgScreenShot);
            aaa.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + stanice + "-" + user + "-" +
               DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".jpg", ImageFormat.Jpeg);
            aaa.Dispose();
        }

    }
}
