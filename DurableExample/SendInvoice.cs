using System;
using DurableTask.Core;

namespace DurableExample
{
    public class SendInvoice : TaskActivity<string, Invoce>
    {
        protected override Invoce Execute(TaskContext context, string input)
        {    
            //logic here
            return new Invoce()
            {
                InoiceNumber = input,
                Amount = new Random().NextDouble()
            };
        }
    }

    public struct Invoce
    {
        public double Amount { get; set; }
        public string InoiceNumber { get; set; }
    }
}