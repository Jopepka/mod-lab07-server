using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab07
{
    internal class Server
    {
        private object threadLock = new object();

        public int requestCount { get; private set; } = 0;
        public int processedCount { get; private set; } = 0;
        public int rejectedCount { get; private set; } = 0;

        PoolRecord[] pool;
        public int n { get; }
        public int ExecutionTime { get; } = 100;

        public bool IsNotWork()
        {
            for (int i = 0; i < pool.Length; i++)
                if (pool[i].in_use)
                    return false;
            return true;
        }

        public Server(int countServers)
        {
            n = countServers;

            pool = new PoolRecord[n];
        }
        struct PoolRecord
        {
            public Thread thread; // объект потока
            public bool in_use; // флаг занятости
        }

        public void proc(object sender, procEventArgs e)
        {
            lock (threadLock)
            {
                Console.WriteLine("Заявка с номером: {0}", e.ID);
                requestCount++;
                for (int i = 0; i < n; i++)
                {
                    if (!pool[i].in_use)
                    {
                        pool[i].in_use = true;
                        pool[i].thread = new Thread(new ParameterizedThreadStart(Answer));
                        pool[i].thread.Start();
                        processedCount++;
                        return;
                    }
                }
                rejectedCount++;
            }
        }

        public void Answer(object? obj)
        {
            Thread.Sleep(ExecutionTime);
            pool[Convert.ToInt32(obj)].in_use = false;
        }
    }
}
