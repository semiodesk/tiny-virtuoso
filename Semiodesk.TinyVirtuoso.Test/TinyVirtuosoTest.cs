using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semiodesk.TinyVirtuoso;


namespace TinyVirtuosoTest
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TinyTest()
        {
            TinyVirtuoso t = new TinyVirtuoso();

            string instanceName = "Test";

            if( !t.AvailableInstances.Contains(instanceName) )
              t.CreateInstance(instanceName);
            t.Start(instanceName);


            t.Stop(instanceName);
            
        }


    }
}
