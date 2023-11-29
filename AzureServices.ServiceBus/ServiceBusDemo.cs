using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureServices.ServiceBus
{
    public class ServiceBusDemo
    {
        // Microsoft.Azure.ServiceBus
        // Azure.Messaging.ServiceBus: Will be used

        static string ConnectionString = "";
        private ServiceBusAdministrationClient serviceBusAdministrationClient;
        private ServiceBusClient serviceBusClient;
        static string QueueName = "demoqueue";
        static string TopicName = "demotopic";
        static string Subscription1 = "firstSubscriber";
        static string Subscription2 = "secondSubscriber";

        public ServiceBusDemo()
        {
            serviceBusAdministrationClient = new ServiceBusAdministrationClient(ConnectionString);
            serviceBusClient = new ServiceBusClient(connectionString: ConnectionString);
        }

        public async Task CreateQueue()
        {
            // default queue
            var defaultQueue = await serviceBusAdministrationClient.CreateQueueAsync(QueueName);

            // create custom queue options
            var options = new CreateQueueOptions(QueueName)
            {
                DefaultMessageTimeToLive = TimeSpan.FromHours(1),
                DeadLetteringOnMessageExpiration = true,
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5),
                LockDuration = TimeSpan.FromMinutes(2),
                RequiresSession = true
            };

            // create a queue using the queue description
            var customQueue = await serviceBusAdministrationClient.CreateQueueAsync(options);

        }

        public async Task CreateTopicAndSubscription()
        {
            // create a default topic
            var defaultTopic = await serviceBusAdministrationClient.CreateTopicAsync(TopicName);


            // create custom topic options
            var options = new CreateTopicOptions(TopicName)
            {
                DefaultMessageTimeToLive = TimeSpan.FromHours(1),
                RequiresDuplicateDetection = true,
                DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(5),
            };

            // create a topic using the topic description
            var customTopic = await serviceBusAdministrationClient.CreateTopicAsync(options);

            // subscribe to the topic
            var subscriber1 = await serviceBusAdministrationClient.CreateSubscriptionAsync(TopicName, Subscription1);

            var subscriptionOption = new CreateSubscriptionOptions(TopicName, Subscription2)
            {
                AutoDeleteOnIdle = TimeSpan.FromMinutes(10),
                DefaultMessageTimeToLive = TimeSpan.FromHours(1)
            };

            var subscriber2 = await serviceBusAdministrationClient.CreateSubscriptionAsync(subscriptionOption);
        }

        public ServiceBusMessage MessageSerialization()
        {
            // create a text message
            var messageText = "Hello, world!";
            var message = new ServiceBusMessage(messageText); // utf-8 serialized

            // deserializing a text message
            messageText = message.Body.ToString();

            // Using the subject property
            message = new ServiceBusMessage() { Subject = "Hello, world" };
            messageText = message.Subject;

            // JSON message serializaiton
            var order = new PizzaOrder()
            {
                CustomerName = "Alan",
                Type = "Kebab",
                Size = "Extra Large"
            };

            // serialize the order object
            string jsonOrderPizza = JsonConvert.SerializeObject(order);

            // create a message
            message = new ServiceBusMessage(jsonOrderPizza)
            {
                Subject = "PizzaOrder",
                ContentType = "application/json"
            };

            // receive the message
            // get the message body as string
            string messageBodyText = message.Body.ToString();

            // deserialize the Pizza Order
            PizzaOrder pizzaOrder = JsonConvert.DeserializeObject<PizzaOrder>(messageBodyText);


            return message;
        }

        public async Task SendMessage()
        {
            // create a message sender
            var sender = serviceBusClient.CreateSender(QueueName);

            // create a message batch
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            List<PizzaOrder> pizzaOrderList = new List<PizzaOrder>();

            foreach(var pizzaOrder in pizzaOrderList)
            {
                // create a pizza order message
                var message = new ServiceBusMessage(JsonConvert.SerializeObject(pizzaOrderList));

                // addind the message to the batch
                if (!messageBatch.TryAddMessage(message))
                {
                    // If it is too large for the batch
                    throw new Exception("The message is too large");
                }
            }

            // send the message to the batch
            await sender.SendMessagesAsync(messageBatch);

            await sender.CloseAsync();
        }


    }

    internal class PizzaOrder
    {
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
