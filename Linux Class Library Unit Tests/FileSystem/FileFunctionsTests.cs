using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Utils.Linux.DiskConfiguration;
using System.Linq;

namespace Skyline.DataMiner.Utils.Linux.FileSystem.Tests
{
    [TestClass()]
    public class FileFunctionsTests
    {
        [TestMethod()]
        public void GetFilesTest()
        {
            var linux = new Mock<ILinux>();
            var fileList = System.IO.File.ReadAllText(@"FileSystem\ls_la_multiple_files.txt");
            var files = BaseFileItem.GetFiles(linux.Object, fileList, "/TestFolder");
            Assert.AreEqual(25, files.Count(), "total file count");
            Assert.AreEqual(1, files.Count(f => f.FileType == FileTypes.File), "file count");
            Assert.AreEqual(6, files.Count(f => f.FileType == FileTypes.Link), "link count");
            Assert.AreEqual(18, files.Count(f => f.FileType == FileTypes.Directory), "directory count");
        }

        [TestMethod()]
        public void GetFileTest()
        {
            var linux = new Mock<ILinux>();
            var fileOutput = System.IO.File.ReadAllText(@"FileSystem\ls_la_single_file.txt");
            linux.Setup(l => l.Connection.RunCommand(It.IsAny<string>(), null)).Returns(fileOutput); 
            BaseFileItem basefile = BaseFileItem.GetFileItemByPath(linux.Object, "/etc/cassandra/cassandra.yaml", FileTypes.File);
            var file = new File(basefile);

            Assert.AreEqual(new Permissions("rw-r--r--"), file.Permissions, "Permissions");
            Assert.AreEqual(1, file.HardLinkCount, "HardLinkCount");
            Assert.AreEqual("cassandra", file.User, "User");
            Assert.AreEqual("cassandra", file.Group, "Group");
            Assert.AreEqual(new Size(60706L), file.Size, "Size");
            Assert.AreEqual("/etc/cassandra", file.Location, "Location");
            Assert.AreEqual("/etc/cassandra/cassandra.yaml", file.Path, "Path");
            Assert.AreEqual("cassandra.yaml", file.Name, "Name");
        }
    }
}