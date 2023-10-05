// Ignore Spelling: Chmod

namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using System.IO;
	using System.Text;

	using Renci.SshNet;

	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;

	internal class File : IFile
	{
		private readonly BaseFileItem baseFile;

		internal File(BaseFileItem baseFileItem)
		{
			baseFile = baseFileItem;
		}

		public string Group => baseFile.Group;

		public int HardLinkCount => baseFile.HardLinkCount;

		public string Location => baseFile.Location;

		public string Name => baseFile.Name;

		public string Path => baseFile.Path;

		public Permissions Permissions => baseFile.Permissions;

		public Size Size => baseFile.Size;

		public string User => baseFile.User;

		public void Chmod(Permissions permissions, bool recursive = false)
		{
			baseFile.Chmod(permissions, recursive);
		}

		public void Delete()
		{
			baseFile.Linux.Connection.RunCommand($"sudo rm {Path}");
		}

		public void Download(Stream stream, System.Action<ulong> downloadCallback = null)
		{
			using (var sftpclient = new SftpClient(baseFile.Linux.Connection.ConnectionSettings.ConnectionInfo))
			{
				sftpclient.Connect();
				sftpclient.DownloadFile(Path, stream, downloadCallback);
			}
		}

		public void Download(string localPath)
		{
			using (var fs = System.IO.File.Create(localPath))
			{
				Download(fs);
			}
		}

		public byte[] ReadAllBytesFile()
		{
			using (var sftp = new SftpClient(baseFile.Linux.Connection.ConnectionSettings.ConnectionInfo))
			{
				sftp.Connect();
				return sftp.ReadAllBytes(Path);
			}
		}

		public string ReadAllTextFile()
		{
			using (var sftp = new SftpClient(baseFile.Linux.Connection.ConnectionSettings.ConnectionInfo))
			{
				sftp.Connect();
				return sftp.ReadAllText(Path);
			}
		}

		public void WriteAllText(string content)
		{
			WriteAllText(content, Encoding.UTF8);
		}

		public void WriteAllText(string content, Encoding encoding)
		{
			using (var sftp = new SftpClient(baseFile.Linux.Connection.ConnectionSettings.ConnectionInfo))
			{
				sftp.Connect();
				sftp.WriteAllText(Path, content, encoding);
			}
		}
	}
}