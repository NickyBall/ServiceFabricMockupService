using SharedService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfCommunicationBindingService;

namespace QueueClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Endpoint: ");
            string Endpoint = Console.ReadLine();
            var Service = WcfService.GetNetTcpDuplexService<IQueue>(Endpoint, new QueueClient());
            while (true)
            {
                Console.WriteLine("1 => Enqueue");
                Console.WriteLine("2 => Dequeue");
                Console.Write("Command: ");
                string Command = Console.ReadLine();
                if (Command.Equals("1"))
                {
                    Console.Write("Enter Message: ");
                    string Message = Console.ReadLine();
                    Service.EnQueueAsync(Message).GetAwaiter().GetResult();
                }
                else if (Command.Equals("2"))
                {
                    Service.DeQueueAsync().GetAwaiter().GetResult();
                }
                else Console.WriteLine("Command Invalid");
            }
        }
    }

    public class QueueClient : IQueueClient
    {
        public Task ResponseAsync(string Message) => Task.Factory.StartNew(() => Console.WriteLine(Message));
    }
}
