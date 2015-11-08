using System;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.Intarfaces;

namespace ATS.TestAts
{
    public class TestTerminal : Terminal
    {
        public TestTerminal(PhoneNumber number) : base(number)
        {
            IncomingRequest += OnIncomingRequest;
            Online += (sender, args) => { Console.WriteLine($"Phone {((Terminal)sender).Number} now Online"); };
            Offline += (sender, args) => { Console.WriteLine($"Phone {((Terminal)sender).Number} now offline"); };
        }

        public void OnIncomingRequest(object sender, PhoneNumber source)
        {
            Console.WriteLine("{0} received request for incoming connection from {1}", Number.Number, source.Number);
        }
    }
}