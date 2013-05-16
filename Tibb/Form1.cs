using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tibb.Clients;
using System.Diagnostics;
using System.IO;

namespace Tibb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitTibbs init = new InitTibbs();
            //init.initialize();
            init.scanning();

            /*if (ClientMC.isInTibiaFolder())
            {
                ClientMC.OpenMC();
                Process[] p = Process.GetProcessesByName("Tibia MC 9.80");
                System.Threading.Thread.Sleep(500);
                if (p.Length > 1)
                {
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    InitTibbs init = new InitTibbs();
                    //init.initialize();
                    init.scanning();
                }
            }
            else
            {
                string newPath = Path.Combine(ClientMC.getCurrentDirectory(), "Tibia.dat");
                MessageBox.Show("Cannot find file " + newPath + ". (Error Code 1)\n\n\nPlease re-install the program.", "Tibia Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(1);
            }*/
        }
    }
}