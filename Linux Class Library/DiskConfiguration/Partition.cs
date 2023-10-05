namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration
{
	using System.Collections.Generic;

	internal class Partition : IPartition
	{
		private readonly IBlockDevice blockDevice;

		internal Partition(IBlockDevice blockDevice)
		{
			this.blockDevice = blockDevice;
		}

		public ICollection<IBlockDevice> Children => blockDevice.Children;

		public bool IsReadOnly => blockDevice.IsReadOnly;

		public bool IsRemovable => blockDevice.IsRemovable;

		public string MountPoint => blockDevice.MountPoint;

		public string Name => blockDevice.Name;

		public Size Size => blockDevice.Size;

		public BlockType Type => blockDevice.Type;
	}
}