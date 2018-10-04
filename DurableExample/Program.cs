using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using DurableTask.AzureStorage;
using DurableTask.Core;
using DurableTask.Emulator;
using DurableTask.ServiceBus;
using DurableTask.ServiceBus.Tracking;

namespace DurableExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Start");
            string serviceBusConnectionString = "";
            string storageConnectionString = "";
            string taskHubName = "devstoreaccount";

            var instanceStore = new AzureTableInstanceStore(taskHubName, storageConnectionString);

            var orchestrationServiceAndClient = new ServiceBusOrchestrationService(serviceBusConnectionString, taskHubName, instanceStore, null, null);

            await orchestrationServiceAndClient.CreateIfNotExistsAsync();

            var taskHubWorker = await new TaskHubWorker(orchestrationServiceAndClient)
                .AddTaskOrchestrations(typeof(PredicaOrchestration))
                .AddTaskActivities(new ApproveInvoice())
                .AddTaskActivities(new RejectInvoice())
                .AddTaskActivities(new SendInvoice())
                .StartAsync();


            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);

            var instanceId = Guid.NewGuid().ToString();
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(PredicaOrchestration), instanceId, $"Predica-Inv-{DateTime.UtcNow.Ticks}");
            await taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromMinutes(15), CancellationToken.None);

            await taskHubWorker.StopAsync(false);

        }
    }
}
