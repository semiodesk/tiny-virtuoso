using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Semiodesk.Director.Test
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Test1()
        {
            Virtuoso v = new Virtuoso(new FileInfo("..\\..\\..\\virtuoso-opensource\\bin\\virtuoso-t.exe"), new FileInfo("..\\..\\..\\virtuoso-opensource\\database\\virtuoso.ini"));
            v.RemoveLock();
            v.Start();

            while (!v.IsOnline)
            {
                Thread.Sleep(10);
            }
            v.Dispose();
        }


    }
}
