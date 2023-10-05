namespace Skyline.DataMiner.Utils.Linux.OperatingSystems
{
	/// <summary>
	/// The type op operating system.
	/// </summary>
	public enum OperatingSystemType
	{
		/// <summary>
		/// Indicates that the oprating system is of the type Debian.
		/// </summary>
		Debian,

		/// <summary>
		/// Indicates that the operating system is of the type RedHat Enterprise Linux.
		/// </summary>
		RHEL,

		/// <summary>
		/// Indicates that the library was unable to identify the type of linux distribution.
		/// </summary>
		Unknown,
	}
}