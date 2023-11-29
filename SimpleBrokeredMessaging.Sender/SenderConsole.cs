using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace SimpleBrokeredMessaging.Sender
{
    class SenderConsole
    {
        static string ConnectionString = "";
        static string QueueName = "demoqueue";

        static string Sentence = "Microsoft Azure Service Bus.";
        static async Task Main(string[] args)
        {
            // Create a service bus client
            var client = new ServiceBusClient(ConnectionString);

            // Create a service bus sender
            var sender = client.CreateSender(QueueName);

            // send some messages
            Console.WriteLine("Sending messages...");



            // read each character of the sentence
            foreach (var character in Sentence)
            {
                // a message can be formed only by joining strings not character
                var message = new ServiceBusMessage(character.ToString());

                // send message to queue
                await sender.SendMessageAsync(message);

                Console.WriteLine($" Sent: {character}");
            }

            // As each connection will create an overhead to the service bus
            // closer the sender once message is sent
            await sender.CloseAsync();

            Console.WriteLine("Sent messages.");
            Console.ReadLine();
        }

        //public static void Main()
        //{
        //    Bot.Instance.Display1("Sathish");
        //    Bot.Instance.Display2("Saurabh");
        //}
    }
}
