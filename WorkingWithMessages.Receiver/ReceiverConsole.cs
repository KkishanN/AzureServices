using Azure.Messaging.ServiceBus;
using AzureServices.Common;
using AzureServices.Common.Models;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkingWithMessages.Receiver
{
    class ReceiverConsole
    {
        private static ServiceBusClient SBClient;
        static async Task Main(string[] args)
        {
            SBClient = new ServiceBusClient(ConfigHelper.SBConnectionString);

            // await ReceiveAndProcessText(1);

            await ReceiveAndProcessPizzaOrders(1);
            // await ReceiveAndProcessPizzaOrders(5);
            // await ReceiveAndProcessPizzaOrders(100);

            // await ReceiveAndProcessControlMessage(1);

            // await ReceiveAndProcessCharacters(1);

            // await ReceiveAndProcessCharacters(16); 
        }

        private static async Task ReceiveAndProcessPizzaOrders(int threads)
        {
            WriteLine($"ReceiveAndProcessPizzaOrders({threads})", ConsoleColor.Cyan);

            // set the options for processing messages
            var options = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = threads,
                MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(10)
            };

            // create a message processor
            var processor = SBClient.CreateProcessor(ConfigHelper.QueueName, options);

            // add handler to process messages
            processor.ProcessMessageAsync += ProcessPizzaMessageAsync;

            // add handler to process any errors
            processor.ProcessErrorAsync += ErrorHandler;

            // start the message processor
            await processor.StartProcessingAsync();

            WriteLine("Receiving, hit enter to exit", ConsoleColor.White);
            Console.ReadLine();

            // stop and close the message processor
            await processor.StopProcessingAsync();
            await processor.CloseAsync();
        }

        private static Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            throw new NotImplementedException();
        }

        private static async Task ProcessPizzaMessageAsync(ProcessMessageEventArgs arg)
        {
            // deserialize the message body
            var pizzaOrder = JsonConvert.DeserializeObject<PizzaOrder>(arg.Message.Body.ToString());

            // process the message
            CookPizza(pizzaOrder);

            // complete the message receive operation
            await arg.CompleteMessageAsync(arg.Message);
        }

        private static void CookPizza(PizzaOrder pizzaOrder)
        {
            WriteLine($"Cooking {pizzaOrder.Type} for {pizzaOrder.CustomerName}.", ConsoleColor.Yellow);
            Thread.Sleep(5000);
            WriteLine($"    {pizzaOrder.Type} pizza for {pizzaOrder.CustomerName} is ready!", ConsoleColor.Green);
        }

        private static void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }

        private static void Write(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
        }
    }
}
