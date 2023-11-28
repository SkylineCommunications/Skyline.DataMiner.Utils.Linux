using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration.Tests
{
	[TestClass()]
	public class SizeTests
	{
		[TestMethod()]
		public void HumanReadableByteCountBinTest()
		{
			long[] testValues = new[]
			{
				0L,
				27L,
				999L,
				1000L,
				1023L,
				1024L,
				1728L,
				110592L,
				7077888L,
				452984832L,
				28991029248L,
				1855425871872L,
				9223372036854775807L,
			};
			string[] expectedValues = new[]
			{
				"0 B",
				"27 B",
				"999 B",
				"1000 B",
				"1023 B",
				"1.0 KiB",
				"1.7 KiB",
				"108.0 KiB",
				"6.8 MiB",
				"432.0 MiB",
				"27.0 GiB",
				"1.7 TiB",
				"8.0 EiB",
			};

			for (int i = 0; i < testValues.Length; i++)
			{
				Assert.AreEqual(expectedValues[i], Size.HumanReadableByteCountBin(testValues[i]));
			}
		}

		[TestMethod()]
		public void HumanReadableByteCountSITest()
		{
			long[] testValues = new[]
			{
				0L,
				27L,
				999L,
				1000L,
				1023L,
				1024L,
				1728L,
				110592L,
				7077888L,
				452984832L,
				28991029248L,
				1855425871872L,
				9223372036854775807L,
			};
			string[] expectedValues = new[]
			{
				"0 B",
				"27 B",
				"999 B",
				"1.0 kB",
				"1.0 kB",
				"1.0 kB",
				"1.7 kB",
				"110.6 kB",
				"7.1 MB",
				"453.0 MB",
				"29.0 GB",
				"1.9 TB",
				"9.2 EB",
			};

			for (int i = 0; i < testValues.Length; i++)
			{
				Assert.AreEqual(expectedValues[i], Size.HumanReadableByteCountSI(testValues[i]));
			}
		}

		[TestMethod()]
		public void Size_Constructor_Test()
		{
			var size = new Size(220L);
			Assert.AreEqual(220L, size.Bytes);
		}

		[TestMethod()]
		public void EqualsTest()
		{
			var size1 = new Size(220L);
			var size1Other = new Size(220L);
			var size2 = new Size(221L);
			Assert.IsTrue(size1.Equals(size1Other), "Same Equals");
			Assert.AreEqual(size1, size1Other, "Same Equals object");
			Assert.IsTrue(size1 == size1Other, "Same ==");
			Assert.IsFalse(size1.Equals(size2), "Different Equals");
			Assert.AreNotEqual(size1, size2, "Same Equals object");
			Assert.IsFalse(size1 == size2, "Different");
		}
	}
}