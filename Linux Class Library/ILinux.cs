namespace Skyline.DataMiner.Utils.Linux
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;

	using Skyline.DataMiner.Utils.Linux.DiskConfiguration;
	using Skyline.DataMiner.Utils.Linux.Exceptions;
	using Skyline.DataMiner.Utils.Linux.FileSystem;
	using Skyline.DataMiner.Utils.Linux.Firewall;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;
	using Skyline.DataMiner.Utils.Linux.Services;
	using Skyline.DataMiner.Utils.Linux.SoftwareBundleManager;

	/// <summary>
	/// Represents a Linux System.
	/// </summary>
	public interface ILinux : IDisposable
	{
		/// <summary>
		/// Get the architecture of the server.
		/// </summary>
		string Arch { get; }

		/// <summary>
		/// Get the SSH connection used to communicate with the Linux server.
		/// </summary>
		ISshConnection Connection { get; }

		/// <summary>
		/// Get the disks in the Linux server.
		/// </summary>
		IEnumerable<IDisk> Disks { get; }

		/// <summary>
		/// Get the Firewall used for the OS.
		/// </summary>
		IFirewall Firewall { get; }

		/// <summary>
		/// Get the Operating System information that the Linux server is using.
		/// </summary>
		OsReleaseInfo OsInfo { get; }

		/// <summary>
		/// Gets the software bundle manager.
		/// </summary>
		ISoftwareBundleManager SoftwareBundleManager { get; }

		/// <summary>
		/// Creates a new directory if it does not yet exist on the Linux server.
		/// </summary>
		/// <param name="path">The path of the directory.</param>
		/// <returns>The directory that is created.</returns>
		IDirectory CreateDirectory(string path);

		/// <summary>
		/// Creates a new file on the Linux server.
		/// </summary>
		/// <param name="path">The target path of the file.</param>
		/// <param name="content">The content of the file.</param>
		/// <param name="encoding">The encoding of the file to be used.</param>
		/// <param name="overwrite">Indicates if a file exists if it can be overwritten.</param>
		/// <returns>The file that was uploaded.</returns>
		IFile CreateFile(string path, string content, Encoding encoding, bool overwrite = false);

		/// <summary>
		/// Checks whether the target directory exists.
		/// </summary>
		/// <param name="path">The full path of the directory.</param>
		/// <returns>Returns <see langword="true"/> if the directory exists; otherwise <see langword="false"/>.</returns>
		bool DirectoryExists(string path);

		/// <summary>
		/// Checks whether the target file exists
		/// </summary>
		/// <param name="path">The full path of the file.</param>
		/// <returns>Returns <see langword="true"/> if the file exists; otherwise <see langword="false"/>.</returns>
		bool FileExists(string path);

		/// <summary>
		/// Gets a specific directory on the Linux server.
		/// </summary>
		/// <param name="path"> The full path of the directory.</param>
		/// <returns>The directory.</returns>
		/// <exception cref="FileItemNotFoundException">The directory was not found.</exception>
		IDirectory GetDirectory(string path);

		/// <summary>
		/// Gets a specific file on the Linux server.
		/// </summary>
		/// <param name="path">The full path of the file.</param>
		/// <returns>The file.</returns>
		/// <exception cref="FileItemNotFoundException">The File was not found.</exception>
		IFile GetFile(string path);

		/// <summary>
		/// Gets a specific link on the Linux server.
		/// </summary>
		/// <param name="path">The full path of the link.</param>
		/// <returns>The link object.</returns>
		/// <exception cref="FileItemNotFoundException">The File was not found.</exception>
		ILink GetLink(string path);

		/// <summary>
		/// Get the service on the Linux system.
		/// </summary>
		/// <param name="serviceName">The name of the service.</param>
		/// <returns>The service.</returns>
		IService GetService(string serviceName);

		/// <summary>
		/// Uploads a file from a local path to a target path on the Linux server.
		/// </summary>
		/// <param name="localPath">The local path of the file to be uploaded.</param>
		/// <param name="path">The target path of the file.</param>
		/// <returns>The file that was uploaded.</returns>
		IFile UploadFile(string localPath, string path);

		/// <summary>
		/// Uploads a file from a stream to a target path on the Linux server.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <param name="path">The target path of the file.</param>
		/// <param name="uploadCallback">The upload callback.</param>
		/// <returns>The file that was uploaded.</returns>
		IFile UploadFile(Stream stream, string path, Action<ulong> uploadCallback = null);
	}
}