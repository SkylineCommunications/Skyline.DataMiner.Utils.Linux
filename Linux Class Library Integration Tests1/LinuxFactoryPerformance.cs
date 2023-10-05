using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Skyline.DataMiner.CommunityLibrary.Linux;
using Skyline.DataMiner.CommunityLibrary.Linux.Communication;

namespace Skyline.DataMiner.CommunityLibrary.Linux.Tests
{
	[TestClass()]
	public class LinuxFactoryPerformance
	{
		private static ILinux linux;

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			linux = LinuxFactory.GetLinux(new ConnectionSettings("10.11.13.6", "skyline", "***"));
			linux.Connection.Connect();
		}

		[ClassCleanup] 
		public static void ClassCleanup()
		{
			linux.Dispose();
		}

		[TestMethod()]
		public void GetCassandraVersion()
		{
			var version = linux.PackageManager.GetInstalledVersion("cassandra");
			Assert.AreEqual("4.0.5", version);
		}

		[TestMethod()]
		public void GetUpgradeVersion()
		{
			var version = linux.PackageManager.GetUpgradeVersion("cassandra");
			Assert.AreEqual("4.0.11", version);
		}

		[TestMethod()]
		public void GetUpgradeablePackages()
		{
			var packageNames = linux.PackageManager.GetUpgradeablePackages();
			Assert.IsTrue(packageNames.Any(p => p.Equals("cassandra")));
		}

		[TestMethod()]
		public void GetInstalledPackages()
		{
			var packageNames = linux.PackageManager.GetInstalledPackages();
			Assert.IsTrue(packageNames.Any(p => p.Equals("cassandra")));
		}

		[TestMethod()]
		public void IsPackageInstalledTest()
		{
			Assert.IsTrue(linux.PackageManager.IsPackageInstalled("cassandra"));
		}

		[TestMethod()]
		public void GetUpgradeVersionTest()
		{
			var version = linux.PackageManager.GetUpgradeVersion("cassandra");
			Console.WriteLine(version);
			Assert.IsTrue(string.IsNullOrEmpty(version));
		}

		[TestMethod()]
		public void IsServiceRunning()
		{
			var service = linux.GetService("cassandra");
			Assert.IsTrue(service.IsRunning());
		}
	}
}