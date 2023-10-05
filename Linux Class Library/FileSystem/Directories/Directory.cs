// Ignore Spelling: Chmod

namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;

	internal class Directory : IDirectory
	{
		private readonly BaseFileItem baseFile;
		private IEnumerable<IDirectory> _directories;
		private IEnumerable<IFile> _files;
		private IEnumerable<ILink> _links;
		private bool retrievedContent;

		internal Directory(BaseFileItem baseFileItem)
		{
			baseFile = baseFileItem;
		}

		public IEnumerable<IDirectory> Directories
		{
			get
			{
				RetrieveContentIfNeeded();
				return _directories;
			}
		}

		public IEnumerable<IFile> Files
		{
			get
			{
				RetrieveContentIfNeeded();
				return _files;
			}
		}

		public string Group => baseFile.Group;

		public int HardLinkCount => baseFile.HardLinkCount;

		public IEnumerable<ILink> Links
		{
			get
			{
				RetrieveContentIfNeeded();
				return _links;
			}
		}

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

		private void RetrieveContentIfNeeded()
		{
			if (retrievedContent)
			{
				return;
			}

			retrievedContent = true;
			var files = BaseFileItem.GetFilesInDirectory(baseFile.Linux, Path);
			_links = files.Where(f => f.FileType == FileTypes.Link).Select(f => new Link(f)).ToArray();
			_directories = files.Where(f => f.FileType == FileTypes.Directory).Select(f => new Directory(f)).ToArray();
			_files = files.Where(f => f.FileType == FileTypes.File).Select(f => new File(f)).ToArray();
		}
	}
}