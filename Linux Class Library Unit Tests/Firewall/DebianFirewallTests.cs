using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Skyline.DataMiner.Utils.Linux;
using Skyline.DataMiner.Utils.Linux.Firewall;

namespace Linux_Class_Library_Unit_Tests.Firewall
{
	[TestClass()]
    public class DebianFirewallTests
    {
        public static string PortOpenResult = @"
7000/tcp                   ALLOW       Anywhere
7000/tcp (v6)              ALLOW       Anywhere (v6)";

        [TestMethod()]
        public void IsRunning_FirewallRunning_commandSuccess()
        {
            var connection = new Mock<ISshConnection>();
            connection.Setup(c => c.RunCommand($@"sudo ufw status | grep 'Status:'", null)).Returns($"active");

            var linux = new Mock<ILinux>();
            linux.Setup(c => c.Connection).Returns(connection.Object);

            DebianFirewall firewall = new DebianFirewall(linux.Object);
            bool IsRunning = firewall.IsRunning();
            Assert.IsTrue(IsRunning);
        }

        [TestMethod()]
        public void IsRunning_FirewallNotRunning_commandSuccess()
        {
            var connection = new Mock<ISshConnection>();
            connection.Setup(c => c.RunCommand($@"sudo ufw status | grep 'Status:'", null)).Returns($"error");

            var linux = new Mock<ILinux>();
            linux.Setup(c => c.Connection).Returns(connection.Object);

            DebianFirewall firewall = new DebianFirewall(linux.Object);
            bool IsRunning = firewall.IsRunning();
            Assert.IsFalse(IsRunning);
        }

        [TestMethod()]
        public void IsRunning_PortOpen_commandSuccess()
        {
            int portNumber = 7000;

            var connection = new Mock<ISshConnection>();
            connection.Setup(c => c.RunCommand($@"sudo ufw status | grep 'Status:'", null)).Returns($"active");
            connection.Setup(c => c.RunCommand($@"sudo ufw status | grep '{portNumber}'", null)).Returns(PortOpenResult);

            var linux = new Mock<ILinux>();
            linux.Setup(c => c.Connection).Returns(connection.Object);

            DebianFirewall firewall = new DebianFirewall(linux.Object);
            bool IsRunning = firewall.IsOpen(portNumber);
            Assert.IsTrue(IsRunning);
        }

        [TestMethod()]
        public void IsRunning_PortClosed_commandSuccess()
        {
            int portNumber = 7000;

            var connection = new Mock<ISshConnection>();
            connection.Setup(c => c.RunCommand($@"sudo ufw status | grep 'Status:'", null)).Returns($"active");
            connection.Setup(c => c.RunCommand($@"sudo ufw status | grep '{portNumber}'", null)).Returns(string.Empty);

            var linux = new Mock<ILinux>();
            linux.Setup(c => c.Connection).Returns(connection.Object);

            DebianFirewall firewall = new DebianFirewall(linux.Object);
            bool IsRunning = firewall.IsOpen(portNumber);
            Assert.IsFalse(IsRunning);
        }
    }
}
