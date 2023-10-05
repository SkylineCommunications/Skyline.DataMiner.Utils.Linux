namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using System.Collections.Generic;

	/// <summary>
	/// Represents a directory.
	/// </summary>
	public interface IDirectory : IFileItem
	{
		/// <summary>
		/// Gets the directories present in the directory.
		/// </summary>
		IEnumerable<IDirectory> Directories { get; }

		/// <summary>
		/// Gets the files present in the directory.
		/// </summary>
		IEnumerable<IFile> Files { get; }

		/// <summary>
		/// Gets the links present in the directory.
		/// </summary>
		IEnumerable<ILink> Links { get; }
	}
}