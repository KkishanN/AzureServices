using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System;
using System.Threading.Tasks;

namespace ChatApplication
{
    class Program
    {
        static string ConnectionString = "";
        static string TopicName = "groupOne";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter name: ");
            var userName = Console.ReadLine();

            // create an administration client to manage artifacts
            var serviceBusAdministrationClient = new ServiceBusAdministrationClient(ConnectionString);

            // create a topic if it does not exist
            if (!await serviceBusAdministrationClient.TopicExistsAsync(TopicName))
            {
                await serviceBusAdministrationClient.CreateTopicAsync(TopicName);
            }

            // create a temporary subscription for the user if does not exist
            if (!await serviceBusAdministrationClient.SubscriptionExistsAsync(TopicName, userName))
            {
                var options = new CreateSubscriptionOptions(TopicName, userName)
                {
                    AutoDeleteOnIdle = TimeSpan.FromMinutes(5)
                };
                await serviceBusAdministrationClient.CreateSubscriptionAsync(options);
            }

            // create a service bus client
            var serviceBusClient = new ServiceBusClient(ConnectionString);

            // create a service bus sender
            var serviceBusSender = serviceBusClient.CreateSender(TopicName);

            // create a message processor
            var processor = serviceBusClient.CreateProcessor(TopicName, userName);

            // add handler to process messages
            processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start the message processor
            await processor.StartProcessingAsync();

            // send a hello message
            var helloMessage = new ServiceBusMessage($"{userName } has entered the room.");
            await serviceBusSender.SendMessageAsync(helloMessage);

            while(true)
            {
                var text = Console.ReadLine();

                if(text == "exit")
                {
                    break;
                }

                // send a chat message
                var message = new ServiceBusMessage($"{userName}> {text}");
                await serviceBusSender.SendMessageAsync(message);
            }

            // send a goodbye message
            var goodbyeMessage = new ServiceBusMessage($"{userName} has left the room.");
            await serviceBusSender.SendMessageAsync(goodbyeMessage);

            // stop the message processor
            await processor.StopProcessingAsync();

            // close the processor and sender
            await processor.CloseAsync();
            await serviceBusSender.CloseAsync();
            Console.WriteLine("Thank you for now, see you again!");
        }

        private static Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            throw new NotImplementedException();
        }

        private static async Task MessageHandler(ProcessMessageEventArgs arg)
        {
            // retrieve and print the message body
            var test = arg.Message.Body.ToString();
            Console.WriteLine(test);

            // complete the message
            await arg.CompleteMessageAsync(arg.Message);
        }
    }
}
