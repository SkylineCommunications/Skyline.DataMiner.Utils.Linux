// Ignore Spelling: Chmod

namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;
	using Skyline.DataMiner.Utils.Linux.Exceptions;

	internal class BaseFileItem : IFileItem
	{
		internal BaseFileItem(ILinux linux, string lslaLine, string location)
		{
			var cells = lslaLine.Split((char[])null, 9, StringSplitOptions.RemoveEmptyEntries);
			if (cells.Length != 9)
			{
				throw new ArgumentException("Expected 9 items from the output line separated by white space.", "lslaLine");
			}

			var permissionPart = cells[0].TrimEnd('.');
			if (permissionPart.Length != 10)
			{
				throw new ArgumentException($"'{cells[0]}' does not have the expected format (e.g. -rwxr--r--)", "lslaLine");
			}

			FileType = GetFileType(permissionPart[0]);
			Permissions = new Permissions(permissionPart.Substring(1, 9));
			HardLinkCount = Convert.ToInt32(cells[1]);
			User = cells[2];
			Group = cells[3];
			Size = new Size(long.Parse(cells[4]));
			int index = cells[8].LastIndexOf('/');
			if (index > -1 && index + 1 < cells[8].Length)
			{
				Name = cells[8].Substring(index + 1);
			}
			else
			{
				Name = cells[8];
			}

			Location = location;
			Linux = linux;
		}

		public FileTypes FileType { get; private set; }

		public string Group { get; private set; }

		public int HardLinkCount { get; private set; }

		public ILinux Linux { get; private set; }

		public string Location { get; private set; }

		public string Name { get; private set; }

		public string Path
		{
			get
			{
				if (Location == "/")
				{
					return "/" + Name;
				}

				return Location + "/" + Name;
			}
		}

		public Permissions Permissions { get; private set; }

		public Size Size { get; private set; }

		public string User { get; private set; }

		public static BaseFileItem GetFileItemByPath(ILinux linux, string filePath, FileTypes fileType)
		{
			string folderPath = string.Empty;

			var trimmedFilepath = filePath;
			if (filePath.EndsWith("/"))
			{
				trimmedFilepath = filePath.TrimEnd('/');
			}

			int index = trimmedFilepath.LastIndexOf('/');
			if (index > 0)
			{
				folderPath = trimmedFilepath.Substring(0, index);
			}

			if (string.IsNullOrWhiteSpace(folderPath))
			{
				folderPath = "/";
			}
			else if (folderPath.StartsWith("~"))
			{
				string homeLocation = linux.Connection.RunCommand("ls -d ~").Trim().TrimEnd('/');
				folderPath = folderPath.Replace("~", homeLocation);
				trimmedFilepath = trimmedFilepath.Replace("~", homeLocation);
			}

			var file = GetFilesInDirectory(linux, folderPath).FirstOrDefault(f => f.Path.Equals(trimmedFilepath));
			if (file == null || file.FileType != fileType)
			{
				throw new FileItemNotFoundException(folderPath);
			}

			return file;
		}

		public static ICollection<BaseFileItem> GetFilesInDirectory(ILinux linux, string folderPath)
		{
			string command = "ls -laH " + folderPath;
			var result = linux.Connection.RunCommand(command);
			return GetFiles(linux, result, folderPath);
		}

		public void Chmod(Permissions permissions, bool recursive = false)
		{
			string options = recursive ? "-R " : String.Empty;
			Linux.Connection.RunCommand($"sudo {options}chmod {permissions} {Path}");
			Permissions = permissions;
		}

		internal static ICollection<BaseFileItem> GetFiles(ILinux linux, string lslaResult, string location)
		{
			List<BaseFileItem> files = new List<BaseFileItem>();
			var lines = lslaResult.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			int start = 0;

			if (lines.Length < 1)
			{
				throw new ArgumentException($"ls -laH {location} command does not show any information", lslaResult);
			}

			if (lines[0].StartsWith("total"))
			{
				start = 1;
			}

			// Skip first row (total Size)
			for (int i = start; i < lines.Length; i++)
			{
				var file = new BaseFileItem(linux, lines[i], location);
				if (file.Name == "." || file.Name == "..")
				{
					continue;
				}

				files.Add(file);
			}

			return files;
		}

		private static FileTypes GetFileType(char t)
		{
			switch (t)
			{
				case '-':
					return FileTypes.File;

				case 'd':
					return FileTypes.Directory;

				case 'l':
					return FileTypes.Link;

				case 'c':
					return FileTypes.CharacterDeviceFile;

				case 'b':
					return FileTypes.BlockDeviceFile;

				case 's':
					return FileTypes.LocalSocketFile;

				case 'p':
					return FileTypes.NamedPipe;

				default:
					throw new ArgumentException($"Type {t} not known.", "t");
			}
		}
	}
}