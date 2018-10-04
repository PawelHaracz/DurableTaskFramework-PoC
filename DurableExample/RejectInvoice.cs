using System;
using DurableTask.Core;

namespace DurableExample
{
    public class RejectInvoice : TaskActivity<string, bool>
    {
        protected override bool Execute(TaskContext context, string input)
        {
            return true;
        }
    }
}