using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Tibb.Util;
using System.Windows.Forms;

namespace Tibb.Clients
{
    public class Player
    {

        public Process process;
        private IntPtr Handle;
        private int AdressOffset;
        private static uint battleListStartAddress = 0x950008; //982
        private static uint stepsAddress = 0xB0;
        private static uint Max = 1300;
        private static uint endsAddress = battleListStartAddress + (stepsAddress * Max);

        private static uint playerIdAddress = 0x987EA4;//982
        private static uint statusAddress = 0x7C3E98;//982

        private static uint levelAddress = 0x7BA0CC;//982
        private static uint passwordAddress = 0x94CEC8;//982
        private static uint accountAddress = 0x94CEAC;//982

        /*
         * Constructor
         */
        public Player(Process process)
        {
            this.process = process;
            this.Handle = process.Handle;
            this.AdressOffset = process.MainModule.BaseAddress.ToInt32() - 0x400000;
        }

        #region PlayerInformation
        public uint GetPlayerAdr()
        {
            for (uint i = battleListStartAddress; i <= endsAddress; i += stepsAddress)
            {
                if (ReadInt(i) == ReadInt(playerIdAddress))
                {
                    return i;
                }
            }
            return 0;
        }
        public bool IsOnline()
        {
            if (ReadByte(statusAddress) == 10)
            {
                return true;
            }
            return false;
        }

        public string Name
        {
            get { return ReadString(GetPlayerAdr() + 4); }
        }

        public string ServerName
        {
            get { return ReadString(GetPlayerAdr() + 30); }
        }

        public int Level
        {
            get { return ReadInt(levelAddress); }
        }

        public string Account
        {
            get { return ReadString(accountAddress); }
        }

        public string Password
        {
            get { return ReadString(passwordAddress); }
        }
        #endregion

        /*
         * Memory helper.
         */
        private string ReadString(uint adr)
        {
            return Memory.ReadString(Handle, AdressOffset + adr, 32);
        }

        private int ReadInt(uint adr)
        {
            return Memory.ReadInt32(Handle, AdressOffset + adr);
        }

        private byte ReadByte(uint adr)
        {
            return Memory.ReadByte(Handle, AdressOffset + adr);
        }

        private void WriteInt(uint adr, int value)
        {
            Memory.WriteInt32(Handle, AdressOffset + adr, value);
        }

        private void WriteString(uint adr, string value)
        {
            Memory.WriteString(Handle, AdressOffset + adr, value);
        }

        private void WriteByte(uint adr, byte value)
        {
            Memory.WriteByte(Handle, (long)AdressOffset + adr, value);
        }
    }
}