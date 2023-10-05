using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skyline.DataMiner.Utils.Linux.Communication.Tests
{
	[TestClass()]
	public class FixedBufferTests
	{
		[TestMethod()]
		public void Full()
		{
			var buffer = new FixedBuffer<int>(3);
			buffer.Add(1);
			buffer.Add(2);
			buffer.Add(3);

			Assert.AreEqual(3, buffer.Values.Length, "Length");
			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(1 + i, buffer.Values[i], $"Value {i}");
			}
		}

		[TestMethod()]
		public void Less()
		{
			var buffer = new FixedBuffer<int>(3);
			buffer.Add(1);
			buffer.Add(2);

			Assert.AreEqual(2, buffer.Values.Length, "Length");
			for (int i = 0; i < 2; i++)
			{
				Assert.AreEqual(1 + i, buffer.Values[i], $"Value {i}");
			}
		}

		[TestMethod()]
		public void Overflow()
		{
			var buffer = new FixedBuffer<int>(3);
			buffer.Add(1);
			buffer.Add(2);
			buffer.Add(3);
			buffer.Add(4);

			Assert.AreEqual(3, buffer.Values.Length, "Length");
			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(2 + i, buffer.Values[i], $"Value {i}");
			}
		}
	}
}