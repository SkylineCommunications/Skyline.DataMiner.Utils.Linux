namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	/// <summary>
	/// The different file types in Linux.
	/// </summary>
	internal enum FileTypes
	{
		/// <summary>
		/// Contain data of various content types such as text, script, image, videos, etc.
		/// </summary>
		File,

		/// <summary>
		/// Contain the name and address of other files.
		/// </summary>
		Directory,

		/// <summary>
		/// Point or mirror other files.
		/// </summary>
		Link,

		/// <summary>
		/// Represent device files such as hard drives, monitors, etc.
		/// </summary>
		CharacterDeviceFile,

		/// <summary>
		/// Represent device files such as hard drives, monitors, etc.
		/// </summary>
		BlockDeviceFile,

		/// <summary>
		/// Provide inter-process communication.
		/// </summary>
		LocalSocketFile,

		/// <summary>
		/// Allow processes to send data to other processes or receive data from other processes.
		/// </summary>
		NamedPipe,
	}
}