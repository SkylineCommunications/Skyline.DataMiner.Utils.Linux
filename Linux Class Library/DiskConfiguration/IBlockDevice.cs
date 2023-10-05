namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration
{
	using System.Collections.Generic;

	/// <summary>
	/// Block Device interface.
	/// </summary>
	public interface IBlockDevice
	{
		/// <summary>
		/// Gets or sets the child block devices of the block device.
		/// </summary>
		ICollection<IBlockDevice> Children { get; }

		/// <summary>
		/// Gets whether the block device is read only.
		/// </summary>
		bool IsReadOnly { get; }

		/// <summary>
		/// Gets whether the block device is removable.
		/// </summary>
		bool IsRemovable { get; }

		/// <summary>
		/// Gets the mount point of the block device.
		/// </summary>
		string MountPoint { get; }

		/// <summary>
		/// Gets or sets the name of the block device.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets or sets the size of the block device in bytes.
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// The type of block device.
		/// </summary>
		BlockType Type { get; }
	}
}