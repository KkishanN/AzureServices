using Azure.Messaging.ServiceBus;
using AzureServices.Common;
using AzureServices.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WorkingWithMessages.Sender
{
    class SenderConsole
    {
        private static ServiceBusClient SBClient = null;
        static async Task Main(string[] args)
        {
            string Sentence = "Let's explore Azure Service Bus messaging service";
            SBClient = new ServiceBusClient(ConfigHelper.SBConnectionString);


            // await SendTextString(Sentence);

            //await SendPizzaOrderAsync();

            //await SendControlMessageAsync();

            //await SendPizzaOrderListAsMessagesAsync();

            //await SendPizzaOrderListAsBatchAsync();
        }

        private static async Task SendPizzaOrderListAsBatchAsync()
        {
            WriteLine("SendPizzaOrderListAsBatchAsync", ConsoleColor.Cyan);

            var pizzaOrderList = GetPizzaOrderList();

            var sender = SBClient.CreateSender(ConfigHelper.QueueName);

            WriteLine("Sending...", ConsoleColor.Yellow);

            var watch = Stopwatch.StartNew();

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            foreach (var pizzaOrder in pizzaOrderList)
            {
                var jsonPizzaOrder = JsonConvert.SerializeObject(pizzaOrder);
                var message = new ServiceBusMessage(jsonPizzaOrder)
                {
                    Subject = "PizzaOrder",
                    ContentType = "application/json"
                };
                if (!messageBatch.TryAddMessage(message))
                {
                    throw new Exception("The message is too large to fit in the batch");
                }
            }

            await sender.SendMessagesAsync(messageBatch);

            WriteLine($"Sent {pizzaOrderList.Count} orders! - Time: {watch.ElapsedMilliseconds} milliseconds");

            Console.WriteLine();
            Console.WriteLine();

            await sender.CloseAsync();
        }

        private static async Task SendPizzaOrderListAsMessagesAsync()
        {
            WriteLine("SendPizzaOrderListAsMessagesAsync", ConsoleColor.Cyan);

            var pizzaOrderList = GetPizzaOrderList();

            var sender = SBClient.CreateSender(ConfigHelper.QueueName);

            WriteLine("Sending...", ConsoleColor.Yellow);
            var watch = Stopwatch.StartNew();

            foreach (var pizzaOrder in pizzaOrderList)
            {
                var jsonPizzaOrder = JsonConvert.SerializeObject(pizzaOrder);
                var message = new ServiceBusMessage(jsonPizzaOrder)
                {
                    Subject = "PizzaOrder",
                    ContentType = "application/json"
                };

                await sender.SendMessageAsync(message);
            }

            WriteLine($"Sent {pizzaOrderList.Count} orders! - Time: {watch.ElapsedMilliseconds}");
            Console.WriteLine();
            Console.WriteLine();

            await sender.CloseAsync();
        }

        private static async Task SendControlMessageAsync()
        {
            WriteLine("SendControlMessageAsync", ConsoleColor.Cyan);

            // create a message with no body.
            var message = new ServiceBusMessage
            {
                Subject = "Control"
            };

            // add some application props to the property collection
            message.ApplicationProperties.Add("SystemId", 1462);
            message.ApplicationProperties.Add("Command", "Pending Restart");
            message.ApplicationProperties.Add("ActionTime", DateTime.UtcNow);

            // send the message
            var sender = SBClient.CreateSender(ConfigHelper.QueueName);
            Write("Sending control message...", ConsoleColor.Green);
            await sender.SendMessageAsync(message);

            WriteLine("Done!", ConsoleColor.Green);
            Console.WriteLine();
            await sender.CloseAsync();

        }

        private static async Task SendPizzaOrderAsync()
        {
            WriteLine("SendPizzaOrderAsync", ConsoleColor.Cyan);

            var order = new PizzaOrder()
            {
                CustomerName = "Saurabh Kumar",
                Type = "Hawaiian",
                Size = "Medium"
            };

            // serialize the order object
            var jsonPizzaOrder = JsonConvert.SerializeObject(order);

            // create a message
            var message = new ServiceBusMessage(jsonPizzaOrder)
            {
                Subject = "PizzaOrder",
                ContentType = "application/json"
            };

            // send the message . .. 
            var sender = SBClient.CreateSender(ConfigHelper.QueueName);
            Write("Sending order . . .", ConsoleColor.Green);

            await sender.SendMessageAsync(message);
            WriteLine("Done!", ConsoleColor.Green);
            Console.WriteLine();
            await sender.CloseAsync();
        }

        private static async Task SendTextString(string sentence)
        {
            WriteLine("SendTextStringAsMessageAsync", ConsoleColor.Cyan);

            // create a service bus sender
            var sender = SBClient.CreateSender(ConfigHelper.QueueName);

            Write("Sending. . .", ConsoleColor.Green);

            // create and send a text message
            var message = new ServiceBusMessage(sentence);
            await sender.SendMessageAsync(message);

            WriteLine("Done!", ConsoleColor.Green);

            //close the sender
            await sender.CloseAsync();
        }

        static List<PizzaOrder> GetPizzaOrderList()
        {
            string[] names = { "Rajveer", "Abhinav", "Aradhya" };
            string[] pizzas = { "Hawaiian", "vegetarian", "Capricciosa", "Napolita" };

            var pizzaOrderList = new List<PizzaOrder>();
            for (int pizza = 0; pizza < pizzas.Length; pizza++)
            {
                for (int name = 0; name < names.Length; name++)
                {
                    PizzaOrder order = new PizzaOrder()
                    {
                        CustomerName = names[name],
                        Type = pizzas[pizza],
                        Size = "Large"
                    };
                    pizzaOrderList.Add(order);
                }
            }

            return pizzaOrderList;
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
