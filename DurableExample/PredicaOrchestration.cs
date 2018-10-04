using System;
using System.Threading.Tasks;
using DurableTask.Core;

namespace DurableExample
{
    public class PredicaOrchestration : TaskOrchestration<PredicaResult, string>
    {
        public override async Task<PredicaResult> RunTask(OrchestrationContext context, string input)
        {            
            var sendInvoice = await context.ScheduleTask<Invoce>(typeof(SendInvoice), input);            
            var isApprove = await context.ScheduleTask<bool>(typeof(ApproveInvoice), sendInvoice);
            
            if (isApprove == false)
            {
                await context.ScheduleTask<bool>(typeof(RejectInvoice), input);
            }

            return new PredicaResult
            {
                IsOk = isApprove
            };
        }
    } 

    public struct PredicaResult
    {
        public bool IsOk { get; set; }
    }
}