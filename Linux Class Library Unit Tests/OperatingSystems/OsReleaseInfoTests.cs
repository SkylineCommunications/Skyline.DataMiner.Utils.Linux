namespace Skyline.DataMiner.Utils.Linux.OperatingSystems.Tests
{
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;

	[TestClass()]
	public class OsReleaseInfoTests
	{
		[TestMethod()]
		public void ParseOsReleaseFileTest()
		{
			var outputText = System.IO.File.ReadAllText(@"OperatingSystems\Ubuntu.txt");
			OsReleaseInfo os_info = OsReleaseInfo.ParseOsReleaseFile(outputText);
			Assert.IsNotNull(os_info, "os_info IsNotNull");
			Assert.AreEqual("Ubuntu", os_info.Name, "os_info.Name AreEqual");
			Assert.AreEqual("20.04.4 LTS (Focal Fossa)", os_info.Version, "os_info.Version AreEqual");
			Assert.AreEqual("ubuntu", os_info.Id, "os_info.Id AreEqual");
			Assert.IsTrue(os_info.Id_Like.AreEqual(new[] { "debian" }), "os_info.Id_Like AreEqual");
			Assert.AreEqual("Ubuntu 20.04.4 LTS", os_info.PrettyName, "os_info.PrettyName AreEqual");
			Assert.AreEqual("20.04", os_info.VersionId, "os_info.VersionId AreEqual");
			Assert.AreEqual("https://www.ubuntu.com/", os_info.HomeUrl, "os_info.VersionId AreEqual");
			Assert.AreEqual("https://help.ubuntu.com/", os_info.SupportUrl, "os_info.VersionId AreEqual");
			Assert.AreEqual("https://bugs.launchpad.net/ubuntu/", os_info.BugReportUrl, "os_info.VersionId AreEqual");
		}

		[TestMethod()]
		public void GetOperatingSystemTest()
		{
			var connection = new Mock<ISshConnection>();
			var outputText = System.IO.File.ReadAllText(@"OperatingSystems\Ubuntu.txt");
			connection.Setup(c => c.RunCommand(It.IsAny<string>(), null)).Returns(outputText);
			var linux = new Linux(connection.Object);
			var os_info = linux.OsInfo;
			Assert.IsNotNull(os_info, "os_info IsNotNull");
			Assert.AreEqual("Ubuntu", os_info.Name, "os_info.Name AreEqual");
			Assert.AreEqual("20.04.4 LTS (Focal Fossa)", os_info.Version, "os_info.Version AreEqual");
			Assert.AreEqual("ubuntu", os_info.Id, "os_info.Id AreEqual");
			Assert.IsTrue(os_info.Id_Like.AreEqual(new[] { "debian" }), "os_info.Id_Like AreEqual");
			Assert.AreEqual("Ubuntu 20.04.4 LTS", os_info.PrettyName, "os_info.PrettyName AreEqual");
			Assert.AreEqual("20.04", os_info.VersionId, "os_info.VersionId AreEqual");
			Assert.AreEqual("https://www.ubuntu.com/", os_info.HomeUrl, "os_info.VersionId AreEqual");
			Assert.AreEqual("https://help.ubuntu.com/", os_info.SupportUrl, "os_info.VersionId AreEqual");
			Assert.AreEqual("https://bugs.launchpad.net/ubuntu/", os_info.BugReportUrl, "os_info.VersionId AreEqual");
		}
	}

	public static class MyExtensions
	{
		public static bool AreEqual(this IEnumerable<string> first, IEnumerable<string> second)
		{
			if (!first.Except(second).Any() && !second.Except(first).Any())
			{
				return true;
			}

			return false;
		}
	}
}