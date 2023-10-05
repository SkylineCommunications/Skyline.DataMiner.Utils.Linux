namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration
{
	/// <summary>
	/// Specifies the block type.
	/// </summary>
	public enum BlockType
	{
		/// <summary>
		/// Partition (Partition).
		/// </summary>
		Part,

		/// <summary>
		/// Logical Volume Manager (Mapper).
		/// </summary>
		Lvm,

		/// <summary>
		/// Encrypted partition (Mapper).
		/// </summary>
		Crypt,

		/// <summary>
		/// (ATA)RAID (Mapper).
		/// </summary>
		Dmraid,

		/// <summary>
		/// Multi path (Mapper).
		/// </summary>
		Mpath,

		/// <summary>
		/// Path (Mapper).
		/// </summary>
		Path,

		/// <summary>
		/// Device mapper (Mapper).
		/// </summary>
		Dm,

		/// <summary>
		/// Loop (Mapper).
		/// </summary>
		Loop,

		/// <summary>
		/// Multiple devices (Multiple Devices).
		/// </summary>
		Md,

		/// <summary>
		/// Linear (Multiple Devices).
		/// </summary>
		Linear,

		/// <summary>
		/// RAID0 (Multiple Devices).
		/// </summary>
		Raid0,

		/// <summary>
		/// RAID1 (Multiple Devices).
		/// </summary>
		Raid1,

		/// <summary>
		/// RAID4 (Multiple Devices).
		/// </summary>
		Raid4,

		/// <summary>
		/// RAID5 (Multiple Devices).
		/// </summary>
		Raid5,

		/// <summary>
		/// RAID10 (Multiple Devices).
		/// </summary>
		Raid10,

		/// <summary>
		/// Multiple path (Multiple Devices).
		/// </summary>
		Multipath,

		/// <summary>
		/// Disk (SCSI Devices).
		/// </summary>
		Disk,

		/// <summary>
		/// Tape (SCSI Devices).
		/// </summary>
		Tape,

		/// <summary>
		/// Printer (SCSI Devices).
		/// </summary>
		Printer,

		/// <summary>
		/// Processor (SCSI Devices).
		/// </summary>
		Processor,

		/// <summary>
		/// Worm (SCSI Devices).
		/// </summary>
		Worm,

		/// <summary>
		/// ROM (SCSI Devices).
		/// </summary>
		Rom,

		/// <summary>
		/// Scanner (SCSI Devices).
		/// </summary>
		Scanner,

		/// <summary>
		/// Changer (SCSI Devices).
		/// </summary>
		Changer,

		/// <summary>
		/// Communication (SCSI Devices).
		/// </summary>
		Comm,

		/// <summary>
		/// RAID (SCSI Devices).
		/// </summary>
		Raid,

		/// <summary>
		/// Enclosure (SCSI Devices).
		/// </summary>
		Enclosure,

		/// <summary>
		/// RBC (SCSI Devices).
		/// </summary>
		Rbc,

		/// <summary>
		/// OSD (SCSI Devices).
		/// </summary>
		Osd,
	}
}