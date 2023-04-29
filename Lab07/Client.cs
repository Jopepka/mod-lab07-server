using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab07
{
    internal class Client
    {
        Server server { get; set; }
        public event EventHandler<procEventArgs> request;
        public Client(Server server)
        {
            this.server = server;
            this.request = server.proc;
        }

        protected virtual void OnProc(procEventArgs e)
        {
            EventHandler<procEventArgs> handler = request;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Start(int id)
        {
            procEventArgs e = new procEventArgs();
            e.ID = id;
            OnProc(e);
        }
    }
}
