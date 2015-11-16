using System;
using ATS.BillingSystemModel.Intarfaces;
using ATS.Station_Model.AbstractClasses;
using ATS.Station_Model.States;

namespace ATS.Test
{
    public class TestTerminal : Terminal
    {
        public TestTerminal(PhoneNumber number, ITariffPlan tariffPlan) : base(number, tariffPlan)
        {
            State = TerminalState.Free;
            IncomingRequest += OnIncomingRequest;
            OutgoingCall +=
                (sender, info) =>
                {
                    Console.WriteLine($"Phone : {((Terminal) sender).Number.Number} call to {info.Target.Number}");
                };
            Online += (sender, args) => { Console.WriteLine($"Phone {((Terminal) sender).Number.Number} now Online"); };
            Offline +=
                (sender, args) => { Console.WriteLine($"Phone {((Terminal) sender).Number.Number} now offline"); };
        }

        public TerminalState State { get; set; }

        private void OnIncomingRequest(object sender, PhoneNumber source)
        {
            Console.WriteLine("{0} received request for incoming connection from {1}", Number.Number, source.Number);
        }

        public override void Drop()
        {
            if (State == TerminalState.Free) return;
            base.Drop();
        }

        public override void Answer()
        {
            if (State != TerminalState.IncomingCall) return;
            base.Answer();
        }

        public override void Reject()
        {
            if (State != TerminalState.IncomingCall) return;
            base.Reject();
        }
    }
}