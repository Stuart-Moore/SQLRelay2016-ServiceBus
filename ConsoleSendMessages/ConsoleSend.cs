using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBus_SharedSettings;

namespace ConsoleSend.Sender
{
    class ConsoleSend
    {
        static string ConnectionString = Settings.ConnectionString;
        static string QueueName = Settings.QueueName;
        static string TextString = "SQL Relay what a great conference";
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to startsending");
            Console.ReadLine();

            SendTextString(TextString, Settings.SendSync);

            Console.WriteLine("Test sent, press enter to finish");
            Console.ReadLine();
        }




        static void SendTextString(string text, bool sendSync)
        {
            // Create a client
            var client = QueueClient.CreateFromConnectionString
                (ConnectionString, QueueName);

            Console.Write("Sending: ");

            var taskList = new List<Task>();

            foreach (var letter in text.ToCharArray())
            {
                // Create an empty message and set the lable.
                var message = new BrokeredMessage(letter.ToString());
                message.Label = letter.ToString();
                message.SessionId = "test";
                if (sendSync)
                {
                    // Send the message
                    client.Send(message);
                    Console.Write(message.Label);
                }
                else
                {
                    // Create a task to send the message
                    //var sendTask = new Task(() =>
                    //{
                    //    client.Send(message);
                    //    Console.Write(message.Label);
                    //});
                    //sendTask.Start();
                    taskList.Add(client.SendAsync(message).ContinueWith
                        (t => Console.WriteLine("Sent: " + message.Label)));
                    //Console.Write(message.Label);
                }

            }

            if (!sendSync)
            {
                Console.WriteLine("Waiting...");
                Task.WaitAll(taskList.ToArray());
                Console.WriteLine("Complete!");
            }

            Console.ReadLine();
            Console.WriteLine();

            // Always close the client
            client.Close();
        }

        static void SlideCode()
        {
            var client = TopicClient.CreateFromConnectionString("");
            var message = new BrokeredMessage();

            // Veryfy the size of the message body.
            if (message.Size > 250 * 1024)
            {
                throw new ArgumentException("Message is too large");
            }

            // Send synchronously
            client.Send(message);

            // Always close the client
            client.Close();


            // Send assynchronously
            var sendTask = client.SendAsync(message).ContinueWith
                (t => Console.WriteLine("Sent: " + message.Label));

       


            // Always close the client
            client.Close();



        }

        static void SendControlMessage()
        {

            // Create a message with no body.
            var message = new BrokeredMessage()
            {
                Label = "Control"
            };

            // Add some properties to the property collection
            message.Properties.Add("SystemId", 1462);
            message.Properties.Add("Command", "Pending Restart");
            message.Properties.Add("ActionTime", DateTime.UtcNow.AddHours(2));

            // Send the message
            var client = QueueClient.CreateFromConnectionString
                (ConnectionString, QueueName);
            Console.Write("Sending control message...");
            client.Send(message);
            Console.WriteLine("Done!");


            Console.WriteLine("Send again?");
            var response = Console.ReadLine();

            if (response.ToLower().StartsWith("y"))
            {
                // Try to send the message a second time...
                Console.Write("Sending control message again...");
                message = message.Clone();
                client.Send(message);
                Console.WriteLine("Done!");
            }

            client.Close();

        }

   
    }
}
