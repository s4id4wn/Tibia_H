using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Tibb.Clients
{
    class Cliente
    {
        public Player player;
        private Process p;

        public Cliente(Process p)
        {
            this.p = p;
            this.player = new Player(p);
        }

        public string version()
        {
            return p.MainModule.FileVersionInfo.FileVersion;
        }

        public int id()
        {
            return p.Id;
        }
    }
}
