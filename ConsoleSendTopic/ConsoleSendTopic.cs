using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceBus_SharedSettings;
using Microsoft.ServiceBus.Messaging;

namespace ConsoleSendTopic
{
    class ConsoleSendTopic
    {
        static string ConnectionString = Settings.ConnectionString;
        static string TopicName = Settings.TopicName;
        static string TextString = "SQL Relay what a great conference";
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start sending to topic");
            Console.ReadLine();

            SendTextString(TextString, Settings.SendSync);

            Console.WriteLine("Test sent, press enter to finish");
            Console.ReadLine();
        }




        static void SendTextString(string text, bool sendSync)
        {
            // Create a client

            
            TopicClient client =  TopicClient.CreateFromConnectionString(ConnectionString, TopicName);


            Console.Write("Sending: ");

            var taskList = new List<Task>();
            int i = 0;
            foreach (var letter in text.ToCharArray())
            {
                // Create an empty message and set the lable.
                var message = new BrokeredMessage();
                message.Label = letter.ToString();
                //This will give us
                message.Properties["FilterID"] = ((i++)%2);
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




    }
}
