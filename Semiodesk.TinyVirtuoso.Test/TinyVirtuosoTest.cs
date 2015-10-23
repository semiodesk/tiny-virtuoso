using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semiodesk.TinyVirtuoso;
using System.Reflection;
using System.IO;


namespace TinyVirtuosoTest
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TinyTest()
        {
            Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, "blub");
            TinyVirtuoso t = new TinyVirtuoso("Data");

            string instanceName = "Test";

            Virtuoso v = t.GetOrCreateInstance(instanceName);
            v.Start();

            v.GetAdoNetConnectionString();

            v.Stop();
            
        }

        [Test]
        public void DirectoryTest()
        {
            
            DirectoryInfo targetDir = new DirectoryInfo("Data");
            //if (targetDir.Exists)
                //targetDir.Delete(true);

            TinyVirtuoso t = new TinyVirtuoso(targetDir);

            string instanceName = "Test";

            Virtuoso virt = t.GetOrCreateInstance(instanceName);
            virt.Start();


            t.Stop(instanceName);

        }


    }
}
