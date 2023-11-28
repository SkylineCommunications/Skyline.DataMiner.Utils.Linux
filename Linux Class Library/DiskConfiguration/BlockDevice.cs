namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	internal class BlockDevice : IBlockDevice
	{
		private Size _size;

		public BlockDevice(string name, string majorMinorNumbers, bool isRemovable, long bytesSize, bool isReadOnly, BlockType type, string mountPoint)
		{
			Name = name;
			MajorMinorNumbers = majorMinorNumbers;
			IsRemovable = isRemovable;
			BytesSize = bytesSize;
			IsReadOnly = isReadOnly;
			Type = type;
			MountPoint = mountPoint;
		}

		[JsonProperty("size")]
		public long BytesSize { get; set; }

		[JsonIgnore]
		public ICollection<IBlockDevice> Children
		{
			get
			{
				if (RawChildren == null)
				{
					return new IBlockDevice[] { };
				}

				return RawChildren;
			}
		}

		[JsonProperty("ro")]
		public bool IsReadOnly { get; set; }

		[JsonProperty("rm")]
		public bool IsRemovable { get; set; }

		[JsonProperty("maj:min")]
		public string MajorMinorNumbers { get; set; }

		[JsonProperty("mountpoint")]
		public string MountPoint { get; set; }

		/// <summary>
		/// The name of the block device.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Use Children, not rawChildren.
		/// </summary>
		[JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
		public BlockDevice[] RawChildren { get; private set; }

		[JsonIgnore]
		public Size Size
		{
			get
			{
				if (_size == null)
				{
					_size = new Size(this.BytesSize);
				}

				return _size;
			}
		}

		[JsonProperty("type")]
		public BlockType Type { get; set; }
	}

	internal class BlockDevices
	{
		[JsonProperty("blockdevices")]
		public ICollection<BlockDevice> Children { get; set; }
	}
}