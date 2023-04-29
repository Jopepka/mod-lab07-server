namespace Lab07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(10);

            Client clients = new Client(server);

            for (int i = 0; i < 1000; i++)
            {
                clients.Start(i);
            }

            while (true)
            {
                if (server.IsNotWork())
                    break;
                //Thread.Sleep(100);
            }

            Console.WriteLine(server.processedCount);
            Console.WriteLine(server.rejectedCount);
        }
    }
}