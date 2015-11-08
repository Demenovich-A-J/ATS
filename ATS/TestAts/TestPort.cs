using System;
using ATS.Station_Model.AbstractClasses;

namespace ATS.TestAts
{
    public class TestPort : Port
    {
        public void Test()
        {
            StateChanged +=  (sender, state) => { Console.WriteLine($"Port detect change his State to {State}");};
        }
    }
}