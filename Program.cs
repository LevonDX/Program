namespace Program
{
    class Program
    {
        private static Queue<int> stream = new Queue<int>();
        private static Random random = new Random();
        private static readonly Object o = new Object();

        private static void Produce()
        {
            int input;

            while (true)
            {
                input = random.Next(300, 1000);
                Thread.Sleep(input);

                lock (o)
                {
                    if (stream.Count > 10)
                        Monitor.PulseAll(o);

                    if (stream.Count > 100)
                    {
                        Monitor.PulseAll(o);
                        Monitor.Wait(o);
                    }

                    stream.Enqueue(input);
                    Console.WriteLine($"PRODUCE - {stream.Count}");
                }
            }
        }

        private static void Consume()
        {
            while (true)
            {
                int input = random.Next(300, 1000);
                Thread.Sleep(input);

                lock (o)
                {
                    if (stream.Count < 10)
                    {
                        Monitor.PulseAll(o);
                        Monitor.Wait(o);
                    }

                    stream.Dequeue();
                    Console.WriteLine($"CONSUME  - {stream.Count}");
                }
            }
        }

        static void Main()
        {
            Thread t1 = new Thread(Produce);
            t1.Name = "Producer";

            Thread t2 = new Thread(Consume);
            t2.Name = "Consumer";

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();
        }
    }
}