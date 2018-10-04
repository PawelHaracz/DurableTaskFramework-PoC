using System;
using DurableTask.Core;

namespace DurableExample
{
    public class ApproveInvoice : TaskActivity<Invoce, bool>
    {
        protected override bool Execute(TaskContext context, Invoce input)
        {
            //logic here 
            return Math.Abs(input.Amount % 2) < 0.001;
        }
    }
}