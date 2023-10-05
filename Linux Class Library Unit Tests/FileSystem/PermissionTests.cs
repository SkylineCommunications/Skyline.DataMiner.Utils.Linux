using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Skyline.DataMiner.Utils.Linux.FileSystem.Tests
{
    [TestClass()]
    public class PermissionTests
    {
        [TestMethod()]
        public void Permission_Constructor_level_Test()
        {
            var permission0 = new Permissions.Permission(0);
            Assert.IsFalse(permission0.Read, "0 Read");
            Assert.IsFalse(permission0.Write, "0 Write");
            Assert.IsFalse(permission0.Execute, "0 Execute");
            var permission1 = new Permissions.Permission(1);
            Assert.IsFalse(permission1.Read, "1 Read");
            Assert.IsFalse(permission1.Write, "1 Write");
            Assert.IsTrue(permission1.Execute, "1 Execute");
            var permission2 = new Permissions.Permission(2);
            Assert.IsFalse(permission2.Read, "2 Read");
            Assert.IsTrue(permission2.Write, "2 Write");
            Assert.IsFalse(permission2.Execute, "2 Execute");
            var permission3 = new Permissions.Permission(3);
            Assert.IsFalse(permission3.Read, "3 Read");
            Assert.IsTrue(permission3.Write, "3 Write");
            Assert.IsTrue(permission3.Execute, "3 Execute");
            var permission4 = new Permissions.Permission(4);
            Assert.IsTrue(permission4.Read, "4 Read");
            Assert.IsFalse(permission4.Write, "4 Write");
            Assert.IsFalse(permission4.Execute, "4 Execute");
            var permission5 = new Permissions.Permission(5);
            Assert.IsTrue(permission5.Read, "5 Read");
            Assert.IsFalse(permission5.Write, "5 Write");
            Assert.IsTrue(permission5.Execute, "5 Execute");
            var permission6 = new Permissions.Permission(6);
            Assert.IsTrue(permission6.Read, "6 Read");
            Assert.IsTrue(permission6.Write, "6 Write");
            Assert.IsFalse(permission6.Execute, "6 Execute");
            var permission7 = new Permissions.Permission(7);
            Assert.IsTrue(permission7.Read, "7 Read");
            Assert.IsTrue(permission7.Write, "7 Write");
            Assert.IsTrue(permission7.Execute, "7 Execute");
        }

        [TestMethod()]
        public void Permission_Constructor_bools_Test()
        {
            var permission0 = new Permissions.Permission(false, false, false);
            Assert.AreEqual(0, permission0.PermissionLevel, "0 Read");
            var permission1 = new Permissions.Permission(false, false, true);
            Assert.AreEqual(1, permission1.PermissionLevel, "1 Read");
            var permission2 = new Permissions.Permission(false, true, false);
            Assert.AreEqual(2, permission2.PermissionLevel, "2 Read");
            var permission3 = new Permissions.Permission(false, true, true);
            Assert.AreEqual(3, permission3.PermissionLevel, "3 Read");
            var permission4 = new Permissions.Permission(true, false, false);
            Assert.AreEqual(4, permission4.PermissionLevel, "4 Read");
            var permission5 = new Permissions.Permission(true, false, true);
            Assert.AreEqual(5, permission5.PermissionLevel, "5 Read");
            var permission6 = new Permissions.Permission(true, true, false);
            Assert.AreEqual(6, permission6.PermissionLevel, "6 Read");
            var permission7 = new Permissions.Permission(true, true, true);
            Assert.AreEqual(7, permission7.PermissionLevel, "7 Read");
        }

        [TestMethod()]
        public void Permission_Constructor_string_Test()
        {
            var permission0 = new Permissions.Permission("---");
            Assert.AreEqual(0, permission0.PermissionLevel, "0 Read");
            var permission1 = new Permissions.Permission("--x");
            Assert.AreEqual(1, permission1.PermissionLevel, "1 Read");
            var permission2 = new Permissions.Permission("-w-");
            Assert.AreEqual(2, permission2.PermissionLevel, "2 Read");
            var permission3 = new Permissions.Permission("-wx");
            Assert.AreEqual(3, permission3.PermissionLevel, "3 Read");
            var permission4 = new Permissions.Permission("r--");
            Assert.AreEqual(4, permission4.PermissionLevel, "4 Read");
            var permission5 = new Permissions.Permission("r-x");
            Assert.AreEqual(5, permission5.PermissionLevel, "5 Read");
            var permission6 = new Permissions.Permission("rw-");
            Assert.AreEqual(6, permission6.PermissionLevel, "6 Read");
            var permission7 = new Permissions.Permission("rwx");
            Assert.AreEqual(7, permission7.PermissionLevel, "7 Read");
        }

        [TestMethod()]
        public void Permission_Out_OfRange_low_Test()
        {
            bool callFailed = false;
            try
            {
                var permission = new Permissions.Permission(-1);
            }
            catch (ArgumentException)
            {
                callFailed = true;
            }

            Assert.IsTrue(callFailed, "Expected call to fail with ArgumentException");
        }

        [TestMethod()]
        public void Permission_Out_OfRange_high_Test()
        {
            bool callFailed = false;
            try
            {
                var permission = new Permissions.Permission(8);
            }
            catch (ArgumentException)
            {
                callFailed = true;
            }

            Assert.IsTrue(callFailed, "Expected call to fail with ArgumentException");
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var permission0 = new Permissions.Permission("---");
            var permission5 = new Permissions.Permission("r-x");
            var permission5other = new Permissions.Permission(5);
            Assert.IsTrue(permission5.Equals(permission5other), "Same Equals");
            Assert.AreEqual(permission5, permission5other, "Same Equals object");
            Assert.IsTrue(permission5 == permission5other, "Same ==");
            Assert.IsFalse(permission0.Equals(permission5), "Different Equals");
            Assert.AreNotEqual(permission0, permission5other, "Same Equals object");
            Assert.IsFalse(permission0 == permission5, "Different");
        }
    }
}