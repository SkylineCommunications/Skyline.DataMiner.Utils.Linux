using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Skyline.DataMiner.Utils.Linux.FileSystem.Tests
{
	[TestClass()]
	public class PermissionsTests
	{
		[TestMethod()]
		public void PermissionsTest()
		{
			var permissions = new Permissions("rwxr---w-");
			Assert.AreEqual(new Permissions.Permission("rwx"), permissions.UserPermission, "User");
			Assert.AreEqual(new Permissions.Permission("r--"), permissions.GroupPermission, "Group");
			Assert.AreEqual(new Permissions.Permission("-w-"), permissions.OtherPermission, "Other");
		}

		[TestMethod()]
		public void EqualsTest()
		{
			var permissions1 = new Permissions("rwxr---w-");
			var permissions1Other = new Permissions("rwxr---w-");
			var permissions2 = new Permissions("rwxrwxrwx");
			Assert.IsTrue(permissions1.Equals(permissions1Other), "Same Equals");
			Assert.AreEqual(permissions1, permissions1Other, "Same Equals object");
			Assert.IsTrue(permissions1 == permissions1Other, "Same ==");
			Assert.IsFalse(permissions1.Equals(permissions2), "Different Equals");
			Assert.AreNotEqual(permissions1, permissions2, "Same Equals object");
			Assert.IsFalse(permissions1 == permissions2, "Different");
		}
	}
}