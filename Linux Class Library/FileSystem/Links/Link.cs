// Ignore Spelling: Chmod

namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;

	internal class Link : ILink
	{
		private readonly BaseFileItem baseFile;

		internal Link(BaseFileItem baseFileItem)
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
	}
}