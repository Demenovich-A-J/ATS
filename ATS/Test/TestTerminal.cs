using System;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.AbstractClasses;

namespace ATS.Test
{
    public class TestTerminal : Terminal
    {
        

        private void OnIncomingRequest(object sender, PhoneNumber source)
        {
            Console.WriteLine("{0} received request for incoming connection from {1}", Number.Number, source.Number);
        }

        public TestTerminal(PhoneNumber number, ITariffPlan tariffPlan) : base(number, tariffPlan)
        {
            IncomingRequest += OnIncomingRequest;
            Online += (sender, args) => { Console.WriteLine($"Phone {((Terminal)sender).Number.Number} now Online"); };
            Offline += (sender, args) => { Console.WriteLine($"Phone {((Terminal)sender).Number.Number} now offline"); };
        }
    }
}