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

        public int RequestCount { get; private set; } = 0;
        public int ProcessedCount { get; private set; } = 0;
        public int RejectedCount { get; private set; } = 0;

        PoolRecord[] pool;
        public int CountPool { get; }
        public int ExecutuionTime { get; } = 100;

        public bool IsNotWork()
        {
            for (int i = 0; i < pool.Length; i++)
                if (pool[i].in_use)
                    return false;
            return true;
        }

        public Server(int countPool, double IntensityRequirements_Mu)
        {
            CountPool = countPool;

            pool = new PoolRecord[CountPool];

            ExecutuionTime = Convert.ToInt32(1 / IntensityRequirements_Mu * 1000);
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
                RequestCount++;
                for (int i = 0; i < CountPool; i++)
                {
                    if (!pool[i].in_use)
                    {
                        pool[i].in_use = true;
                        pool[i].thread = new Thread(new ParameterizedThreadStart(Answer));
                        pool[i].thread.Start(i);
                        ProcessedCount++;
                        return;
                    }
                }
                RejectedCount++;
            }
        }

        public void Answer(object? obj)
        {
            Thread.Sleep(ExecutuionTime);
            pool[Convert.ToInt32(obj)].in_use = false;
        }
    }
}
