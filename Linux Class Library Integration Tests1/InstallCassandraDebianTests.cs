using System;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Skyline.DataMiner.CommunityLibrary.Linux;
using Skyline.DataMiner.CommunityLibrary.Linux.Communication;
using Skyline.DataMiner.CommunityLibrary.Linux.Debian;
using Skyline.DataMiner.CommunityLibrary.Linux.Firewall;

namespace Skyline.DataMiner.CommunityLibrary.Linux.Tests
{
	[TestClass()]
	public class InstallCassandraDebianTests
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
		public void InstallCassandra()
		{
			// Check if Cassandra is not installed
			Assert.IsFalse(linux.PackageManager.IsPackageInstalled("cassandra"), "The 'cassandra' package is already installed. Please restore the VM to a blank setup first.");

			// Install Cassandra online
			OnlineInstallation();
		}

		[TestMethod()]
		public void UpgradeCassandra()
		{
			// Add to source list
			Debian.SourceListItem sourceListItem = new Debian.SourceListItem
			{
				ArchiveType = Debian.SourceListItem.ArchiveTypes.Deb,
				RepositoryUrl = "https://debian.cassandra.apache.org",
				PgpFilePath = "/etc/apt/keyrings/cassandra.gpg",
				Distribution = "41x",
				Components = new[] { "main" },
			};
			linux.AddOrUpdateDebianSourceList(new[] { sourceListItem }, "/etc/apt/sources.list.d/cassandra.sources.list");
			linux.Connection.RunCommand("sudo apt update && sudo apt install -y cassandra", new System.Collections.Generic.Dictionary<string, string> { { "\\[default=N\\] \\?", "N" } });
		}

		private void OnlineInstallation()
		{
			// Open Firewall ports
			int[] portNumbers = new[] { 7000, 7001, 9042 };
			foreach (var port in portNumbers)
			{
				if (!linux.Firewall.IsOpen(port))
				{
					linux.Firewall.Open(port, FirewallNetworkProtocols.TCP);
				}
			}

			// Add to source list
			Debian.SourceListItem sourceListItem = new Debian.SourceListItem
			{
				ArchiveType = Debian.SourceListItem.ArchiveTypes.Deb,
				RepositoryUrl = "https://debian.cassandra.apache.org",
				PgpFilePath = "/etc/apt/keyrings/cassandra.gpg",
				Distribution = "40x",
				Components = new[] { "main" },
			};
			linux.AddOrUpdateDebianSourceList(new[] { sourceListItem }, "/etc/apt/sources.list.d/cassandra.sources.list");
			linux.Connection.RunCommand("sudo curl https://downloads.apache.org/cassandra/KEYS | sudo gpg -o /etc/apt/keyrings/cassandra.gpg --dearmor");
			linux.PackageManager.Install("cassandra");
			var service = linux.GetService("cassandra");
			Assert.IsTrue(SpinWait.SpinUntil(() => { Thread.Sleep(1000); return service.IsRunning(); }, TimeSpan.FromMinutes(2)), "Service running");
			Assert.IsTrue(SpinWait.SpinUntil(() => { return nodetoolRunning("127.0.0.1"); }, TimeSpan.FromMinutes(2)), "Nodetool running");
		}

		private static bool nodetoolRunning(string address)
		{
			Thread.Sleep(1000);
			try
			{
				string response = linux.Connection.RunCommand("nodetool status");
				var lines =
					from line in response.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
					where line.Contains(address)
					where line.Contains("UN")
					select line;

				return lines.Any();
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}