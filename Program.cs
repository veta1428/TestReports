using Azure.Messaging.ServiceBus;

namespace QueueSender
{
    class Program
    {
        static string connectionString = "";

        static string queueName = "reports-requests";

        static ServiceBusClient client;

        static ServiceBusSender sender;

        private static int numOfMessages = 3;

        static async Task Main(string[] args)
        {
            if (args.Length != 0)
            {
                numOfMessages = Convert.ToInt32(args[0]);
            }

            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);

            var sbmessage = new ServiceBusMessage(@"{
   ""$type"":""RNet.Shared.Reporting.Messages.GenerateReportMessage, RNet.Shared.Reporting"",
   ""Key"":"""+ Guid.NewGuid() + @""",
   ""Type"":""TestReport"",
   ""Data"":""{\""SomeText\"":\""Text\""}""
}");
            sbmessage.ApplicationProperties.Add("Tenant", "Cosmetology");

            try
            {
                await sender.SendMessageAsync(sbmessage);
                Console.WriteLine($"Message was send:");
                Console.WriteLine(sbmessage.Body);
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }
    }
}