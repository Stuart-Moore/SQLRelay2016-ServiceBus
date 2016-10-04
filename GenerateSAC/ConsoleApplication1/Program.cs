using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Security.Cryptography;
using System.Security.Policy;
using Microsoft.ServiceBus;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Provide the SB endpoint please:");

            string serviceBusConnectionString = Console.ReadLine();
                //"Endpoint=sb://stuarttest1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HNim/ZqAu1Np/dr8G8ZwWUMMl72Ig7BGcyUjI30fPZA=";
            ServiceBusConnectionStringBuilder connectionString = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

            string ServiceBusNamespace = connectionString.Endpoints.First().Host;
            string namespaceKeyName = connectionString.SharedAccessKeyName;
            string namespaceKey = connectionString.SharedAccessKey;

            // Create a token valid for 45mins
            string token = SharedAccessSignatureTokenProvider.GetSharedAccessSignature(namespaceKeyName, namespaceKey, ServiceBusNamespace, TimeSpan.FromMinutes(240));

            Console.WriteLine("Here you go:");
            Console.WriteLine(token);
            Console.ReadLine();
        }
    }
}
