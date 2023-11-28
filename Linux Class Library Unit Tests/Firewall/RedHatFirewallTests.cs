using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Skyline.DataMiner.Utils.Linux;
using Skyline.DataMiner.Utils.Linux.Firewall;

namespace Linux_Class_Library_Unit_Tests.Firewall
{
	[TestClass()]
	public class RedHatFirewallTests
	{
		public static string PortOpenResult = @"7000/tcp";

		[TestMethod()]
		public void IsRunning_FirewallRunning_commandSuccess()
		{
			var connection = new Mock<ISshConnection>();
			connection.Setup(c => c.RunCommand($@"sudo firewall-cmd --state", null)).Returns($"running");

			var linux = new Mock<ILinux>();
			linux.Setup(c => c.Connection).Returns(connection.Object);

			RhelFirewall firewall = new RhelFirewall(linux.Object);
			bool IsRunning = firewall.IsRunning();
			Assert.IsTrue(IsRunning);
		}

		[TestMethod()]
		public void IsRunning_FirewallNotRunning_commandSuccess()
		{
			var connection = new Mock<ISshConnection>();
			connection.Setup(c => c.RunCommand($@"sudo firewall-cmd --state", null)).Returns($"error");

			var linux = new Mock<ILinux>();
			linux.Setup(c => c.Connection).Returns(connection.Object);

			RhelFirewall firewall = new RhelFirewall(linux.Object);
			bool IsRunning = firewall.IsRunning();
			Assert.IsFalse(IsRunning);
		}

		[TestMethod()]
		public void IsRunning_PortOpen_commandSuccess()
		{
			int portNumber = 7000;

			var connection = new Mock<ISshConnection>();
			connection.Setup(c => c.RunCommand($@"sudo firewall-cmd --state", null)).Returns($"running");
			connection.Setup(c => c.RunCommand($@"sudo firewall-cmd --permanent --list-ports", null)).Returns(PortOpenResult);

			var linux = new Mock<ILinux>();
			linux.Setup(c => c.Connection).Returns(connection.Object);

			RhelFirewall firewall = new RhelFirewall(linux.Object);
			bool IsRunning = firewall.IsOpen(portNumber);
			Assert.IsTrue(IsRunning);
		}

		[TestMethod()]
		public void IsRunning_PortClosed_commandSuccess()
		{
			int portNumber = 7000;

			var connection = new Mock<ISshConnection>();
			connection.Setup(c => c.RunCommand($@"sudo firewall-cmd --state", null)).Returns($"running");
			connection.Setup(c => c.RunCommand($@"sudo firewall-cmd --permanent --list-ports", null)).Returns(string.Empty);

			var linux = new Mock<ILinux>();
			linux.Setup(c => c.Connection).Returns(connection.Object);

			RhelFirewall firewall = new RhelFirewall(linux.Object);
			bool IsRunning = firewall.IsOpen(portNumber);
			Assert.IsFalse(IsRunning);
		}
	}
}