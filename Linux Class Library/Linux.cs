namespace Skyline.DataMiner.Utils.Linux
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;

	using Renci.SshNet;

	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;
	using Skyline.DataMiner.Utils.Linux.Exceptions;
	using Skyline.DataMiner.Utils.Linux.FileSystem;
	using Skyline.DataMiner.Utils.Linux.Firewall;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;
	using Skyline.DataMiner.Utils.Linux.SoftwareBundleManager;
	using Skyline.DataMiner.Utils.Linux.Services;

	/// <summary>
	/// Represents a Linux system.
	/// </summary>
	internal sealed class Linux : ILinux
	{
		/// <summary>
		/// The object used for SSH communication.
		/// </summary>

		private readonly ISshConnection ConnectionInfo;
		private string _arch;
		private ICollection<IDisk> _disks;
		private IFirewall _firewall;
		private OsReleaseInfo _os;
		private ISoftwareBundleManager _softwareBundleManager;

		public Linux(ISshConnection connection)
		{
			ConnectionInfo = connection;
		}

		public string Arch
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_arch))
				{
					_arch = this.Connection.RunCommand("sudo uname -m");
				}

				return _arch;
			}
		}

		public ISshConnection Connection
		{
			get
			{
				return ConnectionInfo;
			}
		}

		public IEnumerable<IDisk> Disks
		{
			get
			{
				if (_disks == null)
				{
					_disks = GetDisks();
				}

				return _disks;
			}
		}

		public IFirewall Firewall
		{
			get
			{
				if (_firewall == null)
				{
					_firewall = GetFirewall();
				}

				return _firewall;
			}
		}

		public OsReleaseInfo OsInfo
		{
			get
			{
				if (_os == null)
				{
					GetOperatingSystemInfo();
				}

				return _os;
			}
		}

		public ISoftwareBundleManager SoftwareBundleManager
		{
			get
			{
				if (_softwareBundleManager == null)
				{
					_softwareBundleManager = GetSoftwareBundleManager();
				}

				return _softwareBundleManager;
			}
		}

		public IDirectory CreateDirectory(string path)
		{
			if (!DirectoryExists(path))
			{
				Connection.RunCommand($"mkdir {path}; echo done");
			}

			return GetDirectory(path);
		}

		public IFile CreateFile(string path, string content, Encoding encoding, bool overwrite = false)
		{
			IFile file;
			if ((FileExists(path) && !overwrite))
			{
				throw new FileItemAlreadyExistsException(path);
			}
			else
			{
				Connection.RunCommand($"sudo sh -c '> {path}'");
				file = GetFile(path);
				if (!file.Permissions.OtherPermission.Write)
				{
					var newOtherPerm = new Permissions.Permission(file.Permissions.OtherPermission.Read, true, file.Permissions.OtherPermission.Execute);
					var newpermissions = new Permissions(file.Permissions.UserPermission, file.Permissions.GroupPermission, newOtherPerm);
					file.Chmod(newpermissions);
					file = GetFile(path);
				}

				using (var sftp = new SftpClient(Connection.ConnectionSettings.ConnectionInfo))
				{
					sftp.Connect();
					sftp.WriteAllText(path, content, encoding);
				}
			}

			return file;
		}

		public bool DirectoryExists(string path)
		{
			try
			{
				BaseFileItem.GetFileItemByPath(this, path, FileTypes.Directory);
				return true;
			}
			catch (FileItemNotFoundException)
			{
				return false;
			}
		}

		public void Dispose()
		{
			ConnectionInfo.Dispose();
		}

		public bool FileExists(string path)
		{
			try
			{
				BaseFileItem.GetFileItemByPath(this, path, FileTypes.File);
				return true;
			}
			catch (FileItemNotFoundException)
			{
				return false;
			}
		}

		public IDirectory GetDirectory(string path)
		{
			BaseFileItem file = BaseFileItem.GetFileItemByPath(this, path, FileTypes.Directory);
			return new Directory(file);
		}

		public IFile GetFile(string path)
		{
			BaseFileItem file = BaseFileItem.GetFileItemByPath(this, path, FileTypes.File);
			return new File(file);
		}

		public ILink GetLink(string path)
		{
			BaseFileItem file = BaseFileItem.GetFileItemByPath(this, path, FileTypes.Link);
			return new Link(file);
		}

		public IService GetService(string serviceName)
		{
			return new Service(this, serviceName);
		}

		public IFile UploadFile(string localPath, string path)
		{
			IFile file;
			using (var fs = System.IO.File.OpenRead(localPath))
			{
				file = UploadFile(fs, path);
			}

			return file;
		}

		public IFile UploadFile(System.IO.Stream stream, string path, Action<ulong> uploadCallback = null)
		{
			using (var sftpclient = new SftpClient(Connection.ConnectionSettings.ConnectionInfo))
			{
				sftpclient.Connect();
				sftpclient.UploadFile(stream, path, uploadCallback);
			}

			return GetFile(path);
		}

		private ICollection<IDisk> GetDisks()
		{
			var command = "lsblk --bytes -n";

			var disks = new Collection<IDisk>();

			var result = this.Connection.RunCommand(command);

			var devices = result.Split('\n');

			BlockDevice parent = null;

			foreach (var device in devices)
			{
				var items = device.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList();
				if (items.Count < 6)
				{
					// Invalid Fields
					continue;
				}

				if (items.Count == 6)
				{
					items.Add(""); // Add missing mount points
				}

				var name = items[0];

				// Counts the number of white spaces
				var indentationLength = device.TakeWhile(Char.IsWhiteSpace).Count();
				var indentation = new string(' ', indentationLength);

				var isChild = name.Contains("└─") || name.Contains("├─");

				var majorMinorNumbers = items[1];
				var isRemovable = items[2] == "1";
				var bytesSize = long.Parse(items[3]);
				var isReadOnly = items[4] == "1";
				Enum.TryParse(items[5], true, out BlockType type);

				// If its a child, follow the type of the parent.
				type = isChild && parent != null
					? parent.Type
					: type;

				var mountPoint = items[6];

				var blockDevice = new BlockDevice(name, majorMinorNumbers, isRemovable, bytesSize, isReadOnly, type, mountPoint);
				var disk = new Disk(blockDevice);
				disk.Indentation = indentation;

				disks.Add(disk);

				parent = blockDevice;
			}

			return disks.Where(x => x.Type == BlockType.Disk).ToList();
		}

		private IFirewall GetFirewall()
		{
			IFirewall firewall;

			switch (OsInfo.OsType)
			{
				case OperatingSystemType.Debian:
					firewall = new DebianFirewall(this);
					break;

				case OperatingSystemType.RHEL:
					firewall = new RhelFirewall(this);
					break;

				default:
					throw new LinuxNotSupportedException();
			}

			return firewall;
		}

		private void GetOperatingSystemInfo()
		{
			string command = "cat /etc/os-release";
			var result = Connection.RunCommand(command);
			_os = OsReleaseInfo.ParseOsReleaseFile(result);
		}

		private ISoftwareBundleManager GetSoftwareBundleManager()
		{
			ISoftwareBundleManager firewall;

			switch (OsInfo.OsType)
			{
				case OperatingSystemType.Debian:
					firewall = new DebianSoftwareBundleManager(this);
					break;

				case OperatingSystemType.RHEL:
					firewall = new RhelSoftwareBundleManager(this);
					break;

				default:
					throw new LinuxNotSupportedException();
			}

			return firewall;
		}
	}
}