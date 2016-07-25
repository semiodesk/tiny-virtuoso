using System;
using NUnit.Framework;
using System.Net.NetworkInformation;
using Semiodesk.TinyVirtuoso.Utils;

namespace Semiodesk
{
	[TestFixture]
	public class PortTest
	{
		[Test]
		public void PortTest1()
		{
            // (Moritz Eberl) At least for my machine
            Assert.IsFalse(PortUtils.IsPortFree(1115));

            Assert.IsTrue(PortUtils.IsPortFree(11150));

		}


	}
}

