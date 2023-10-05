using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Skyline.DataMiner.Utils.Linux.SoftwareBundleManager;

namespace Skyline.DataMiner.Utils.Linux.SoftwareBundleManager.Tests
{
	[TestClass()]
	public class DebianSoftwareBundleManagerTests
	{
		[TestMethod()]
		public void GetInstalledDebVersionTest()
		{
			string packageName = "cassandra";
			var linux = new Mock<ILinux>();
			var response = System.IO.File.ReadAllText(@"PackageManager\UpgradableDebCassandraResponse.txt");
			linux.Setup(l => l.Connection.RunCommand(It.IsAny<string>(), null)).Returns(response);
			var packageManager = new DebianSoftwareBundleManager(linux.Object);
			Assert.AreEqual("4.0.11", packageManager.GetUpgradeVersion(packageName));
		}

		[TestMethod()]
		public void GetDebVersionTest()
		{
			string packageName = "cassandra";
			var linux = new Mock<ILinux>();
			var response = System.IO.File.ReadAllText(@"PackageManager\InstalledDebCassandraResponse.txt");
			linux.Setup(l => l.Connection.RunCommand(It.IsAny<string>(), null)).Returns(response);
			var packageManager = new DebianSoftwareBundleManager(linux.Object);
			Assert.AreEqual("4.0.5", packageManager.GetInstalledVersion(packageName));
		}

		[TestMethod()]
		public void GetInstalledRhelVersionTest()
		{
			string packageName = "rpm";
			var linux = new Mock<ILinux>();
			var response = System.IO.File.ReadAllText(@"PackageManager\InstalledRhelRpmResponse.txt");
			linux.Setup(l => l.Connection.RunCommand(It.IsAny<string>(), null)).Returns(response);
			var packageManager = new RhelSoftwareBundleManager(linux.Object);
			Assert.AreEqual("4.11.3-45.el7", packageManager.GetInstalledVersion(packageName));
		}

		[TestMethod()]
		public void GetRhelVersionTest()
		{
			string packageName = "rpm";
			var linux = new Mock<ILinux>();
			var response = System.IO.File.ReadAllText(@"PackageManager\UpgradableRhelRpmResponse.txt");
			linux.Setup(l => l.Connection.RunCommand(It.IsAny<string>(), null)).Returns(response);
			var packageManager = new RhelSoftwareBundleManager(linux.Object);
			Assert.AreEqual("4.11.3-48.el7_9", packageManager.GetUpgradeVersion(packageName));
		}
	}
}