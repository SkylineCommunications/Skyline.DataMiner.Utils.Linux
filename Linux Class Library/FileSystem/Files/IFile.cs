namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using System.IO;
	using System.Text;

	/// <summary>
	/// Represents a file.
	/// </summary>
	public interface IFile : IFileItem
	{
		/// <summary>
		/// Delete the file.
		/// </summary>
		void Delete();

		/// <summary>
		/// Downloads the file from target path on the Linux server to a local path.
		/// </summary>
		/// <param name="localPath">The local path of the file to be downloaded to.</param>
		void Download(string localPath);

		/// <summary>
		/// Downloads the file from target path on the Linux server to a local path.
		/// </summary>
		/// <param name="stream">The stream to write the file into.</param>
		/// <param name="downloadCallback">The download callback.</param>
		void Download(Stream stream, System.Action<ulong> downloadCallback = null);

		/// <summary>
		/// Read all the bytes of the file.
		/// </summary>
		/// <returns>The content of the file.</returns>
		byte[] ReadAllBytesFile();

		/// <summary>
		/// Read all the text of the file.
		/// </summary>
		/// <returns>The content of the file.</returns>
		string ReadAllTextFile();

		/// <summary>
		/// Write content to the file.
		/// </summary>
		/// <param name="content">The content that will be written to the file.</param>
		void WriteAllText(string content);

		/// <summary>
		/// Write content to the file with a specific encoding.
		/// </summary>
		/// <param name="content">The content that will be written to the file.</param>
		/// <param name="encoding">The encoding that needs to be used.</param>
		void WriteAllText(string content, Encoding encoding);
	}
}