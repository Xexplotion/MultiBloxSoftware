using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiBlox
{
    public class Tracker
    {
        Dictionary<string, Roblox> instances = new();

        public void Add(Roblox instance)
        {
            instances.Add(instance.accountID, instance);
        }
        public void Close(string accountID)
        {
            if (instances == null) return;
            if (!instances.ContainsKey(accountID)) return;
            instances[accountID].Close();
            instances.Remove(accountID);
        }

        public Dictionary<string, Roblox> GetInstances()
        {
            return instances;
        }
    }

    public class Roblox
    {
        public string accountID;
        Process? roblox;
        public Roblox(Process? proc, string accountID)
        {
            this.accountID = accountID;
            this.roblox = proc;
        }

        internal void Close()
        {
            roblox?.Close();
            roblox?.Dispose();
        }
    }
}
