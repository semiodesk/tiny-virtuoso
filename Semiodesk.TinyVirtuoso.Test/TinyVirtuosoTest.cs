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
            TinyVirtuoso t = new TinyVirtuoso(defaultDatabase:"Test");
            t.Start();

            



            //IModel m;
            //Uri modelUri = new Uri("http://semiodesk.com/tinyVirtuosTest");
            //if (store.ContainsModel(modelUri))
            //    m = store.GetModel(modelUri);
            //else
            //    m = store.CreateModel(modelUri);


            t.Stop();
            
        }


    }
}
