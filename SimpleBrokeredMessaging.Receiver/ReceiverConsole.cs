using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Receiver
{
    class ReceiverConsole
    {
        static string ConnectionString = "";
        static string QueueName = "demoqueue";
        static async Task Main(string[] args)
        {
            try
            {
                // Create a service bus client
                var client = new ServiceBusClient(ConnectionString);

                // create a service bus receiver
                var receiver = client.CreateReceiver(QueueName);

                // receiver the messages
                Console.WriteLine("Receive messages...");

                while (true)
                {
                    var message = await receiver.ReceiveMessageAsync();

                    if (message != null)
                    {
                        Console.Write(message.Body.ToString());

                        // Complete the message received operation
                        await receiver.CompleteMessageAsync(message);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("All messages received");
                        break;
                    }
                }

                // Close the receiver
                await receiver.CloseAsync();
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
