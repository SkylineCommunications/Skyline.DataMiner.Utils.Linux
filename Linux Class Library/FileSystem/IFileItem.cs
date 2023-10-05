namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;

	/// <summary>
	/// Represents a file item.
	/// </summary>
	public interface IFileItem
	{
		/// <summary>
		/// The group owner.
		/// </summary>
		string Group { get; }

		/// <summary>
		/// The count of hard links.
		/// </summary>
		int HardLinkCount { get; }

		/// <summary>
		/// The location.
		/// </summary>
		string Location { get; }

		/// <summary>
		/// The name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The full path.
		/// </summary>
		string Path { get; }

		/// <summary>
		/// The permissions.
		/// </summary>
		Permissions Permissions { get; }

		/// <summary>
		/// The size.
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// The user owner.
		/// </summary>
		string User { get; }

		/// <summary>
		/// Update the permissions of the file.
		/// </summary>
		/// <param name="permissions">The new permission levels.</param>
		/// <param name="recursive">Indicates if the permissions need to be changed recursive.</param>
		void Chmod(Permissions permissions, bool recursive = false);
	}
}