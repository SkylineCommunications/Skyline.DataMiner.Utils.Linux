namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration
{
	using System.Collections.Generic;
	using System.Linq;

	internal class Disk : IDisk
	{
		private readonly IBlockDevice _blockDevice;
		private IPartition[] _partitions;

		internal Disk(IBlockDevice blockDevice)
		{
			this._blockDevice = blockDevice;
		}

		public ICollection<IBlockDevice> Children => _blockDevice.Children;

		public string Indentation { get; set; }

		public bool IsReadOnly => _blockDevice.IsReadOnly;

		public bool IsRemovable => _blockDevice.IsRemovable;

		public string MountPoint => _blockDevice.MountPoint;

		public string Name => _blockDevice.Name;

		/// <summary>
		/// Gets or sets the partitions of the disk.
		/// </summary>
		public ICollection<IPartition> Partitions
		{
			get
			{
				if (_partitions == null)
				{
					_partitions = Children.Where(b => b.Type == BlockType.Part).Select(p => new Partition(p)).ToArray();
				}

				return _partitions;
			}
		}

		public Size Size => _blockDevice.Size;

		public BlockType Type => _blockDevice.Type;

		public override string ToString()
		{
			return $"{Indentation}{Name} ({Size})";
		}
	}
}