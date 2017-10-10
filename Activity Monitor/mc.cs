using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Activity_Monitor
{
    public partial class mc : Form
    {
        public mc()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  textBox1.Text = "";
            this.Hide();
        }

        private void mc_Shown(object sender, EventArgs e)
        {
            //timer1.Enabled = false;
            //timer1.Stop();
            textBox1.Text = textBox1.Text+"\n\n================\n\n" + (string)this.Tag;
            //timer1.Start();
            //timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           // timer1.Enabled = false;
            //textBox1.Text = "";
           // this.Hide();
        }
    }
}
