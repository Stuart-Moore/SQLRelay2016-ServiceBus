using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using ServiceBus_SharedSettings;


namespace ConsoleRecieveEven
{
    class ConsoleRecieveEven
    {
        //Get Config settings
        static string ConnectionString = Settings.ConnectionString;
        static string TopicName = Settings.TopicName;

        private static string SubName = "relay2016topicEvens";
        private static SubscriptionClient client;

        static void Main(string[] args)
        {
            //Once running, hit enter to start
            Console.WriteLine("Enter to start polling ");
            Console.ReadLine();

            //Check we've got a queue and connect
            var manager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!manager.TopicExists(TopicName))
            {
                Console.Write("Topic " + TopicName + " doesn't exist, fix!");
            }

            //Actually pull the messages
            RecieveAndProcess();



            Console.WriteLine("Finished, hit enter to exit");
            Console.ReadLine();

        }



   


        static void RecieveAndProcess()
        {

            //create cliens and session to pull messages from queue
            SubscriptionClient client = SubscriptionClient.CreateFromConnectionString(ConnectionString, TopicName ,SubName);
            MessageSession orderSession = null;
            orderSession = client.AcceptMessageSession("test");

            //process what's in the queue
            while (true)
            {
                //Setting a 5 second timeout on waiting for a message.
                BrokeredMessage RecieveMessage = orderSession.Receive(TimeSpan.FromSeconds(5));

                //if we've a valid message do something
                if (RecieveMessage != null)
                {
                    try
                    {
                        Console.WriteLine("Recieved {0}", RecieveMessage.Label);
                        // Mark the message as complete
                        RecieveMessage.Complete();
                    }
                    catch
                    {
                        RecieveMessage.Abandon();
                        Console.WriteLine("Something went wrong, abandon read and leave message in queue");
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            client.Close();
        }

        static void WriteDatabase(string inputData)
        {
            string strconn = "Data Source=localhost\\sqlexpress2016;Initial Catalog=QueueDemos;Integrated Security=True";
            SqlConnection sqlconn = new SqlConnection(strconn);
            SqlCommand cmdInsertData = new SqlCommand("dbo.DemoWrite1", sqlconn);
            cmdInsertData.CommandType = CommandType.StoredProcedure;
            cmdInsertData.Parameters.Add(new SqlParameter("@InputData", SqlDbType.NVarChar, 50));
            cmdInsertData.Parameters["@InputData"].Value = inputData;
      //      try
        //    {
                sqlconn.Open();
                cmdInsertData.ExecuteNonQuery();
                Console.WriteLine("Written to DB");              
          /*  }
            catch
            {
                 
                //Console.Beep();
                Console.WriteLine("DB insert Failed");
                
            }
            finally
            {
            */    cmdInsertData.Dispose();
                sqlconn.Close();
            //}

        }
    static void StopReceiving()
        {
            client.Close();
        }


        static void GetQueue()
        {
            var manager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!manager.TopicExists(TopicName))
            {
                Console.Write("Topic " + TopicName + " doesn't exist, fix!");
            }
        }
    }
}
