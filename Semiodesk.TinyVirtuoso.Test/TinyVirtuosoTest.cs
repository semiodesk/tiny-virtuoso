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
            TinyVirtuoso t = new TinyVirtuoso();

            string instanceName = "Test";

            Virtuoso v = t.GetOrCreateInstance(instanceName);
            v.Start();


            v.Stop();
            
        }

        [Test]
        public void DirectoryTest()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            DirectoryInfo dir = new FileInfo(asm.Location).Directory;

            DirectoryInfo targetDir = new DirectoryInfo(Path.Combine(dir.FullName, "Testington"));
            if (targetDir.Exists)
                targetDir.Delete(true);

            TinyVirtuoso t = new TinyVirtuoso(targetDir);

            string instanceName = "Test";

            Virtuoso virt = t.GetOrCreateInstance(instanceName);
            virt.Start();


            t.Stop(instanceName);

        }


    }
}
