using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semiodesk.TinyVirtuoso;
using System.Reflection;
using System.IO;
using System.Threading;

namespace TinyVirtuosoTest
{
    [TestFixture]
    public class Test
    {
        //[Test]
        public void TinyTest()
        {
            //Environment.CurrentDirectory = Path.Combine(Environment.CurrentDirectory, "blub");
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
            //DirectoryInfo deployDir = new DirectoryInfo("C:\\Projects\\Unity3d\\example1\\TinyVirtuoso Example\\Output\\win\\game_Data\\StreamingAssets");
            TinyVirtuoso t = new TinyVirtuoso(targetDir);

            string instanceName = "Test";

            Virtuoso virt = t.GetOrCreateInstance(instanceName);
            virt.Start();
            Console.WriteLine("Started...");
            Console.WriteLine("..Waiting...");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine("Trying to stop");
            t.Stop(instanceName);
            Console.WriteLine("Stopped");

        }


    }
}
