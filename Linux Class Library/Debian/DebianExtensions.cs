namespace Skyline.DataMiner.Utils.Linux.Debian
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using Skyline.DataMiner.Utils.Linux.Exceptions;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;

	/// <summary>
	/// Extension methods for Debian based systems.
	/// </summary>
	public static class DebianExtensions
	{
		/// <summary>
		/// Add or update a source list file.
		/// </summary>
		/// <param name="server">The Linux machine.</param>
		/// <param name="sourceListItems">The full list of source list items to have in the file.</param>
		/// <param name="sourceListFilePath">The path of the source list file.</param>
		/// <exception cref="LinuxNotSupportedException">The Linux machine is not Debian based.</exception>
		public static void AddOrUpdateDebianSourceList(this ILinux server, IEnumerable<SourceListItem> sourceListItems, string sourceListFilePath)
		{
			if (server.OsInfo.OsType != OperatingSystemType.Debian)
			{
				throw new LinuxNotSupportedException("This method can only be used for Debian based systems.");
			}

			string content = string.Join("\n", sourceListItems.Select(i => i.ToString()));
			server.CreateFile(sourceListFilePath, content, Encoding.ASCII, true);
		}

		/// <summary>
		/// Add keys to the system
		/// </summary>
		/// <param name="server">The Linux machine.</param>
		/// <param name="keysUrl">The URL to the keys.</param>
		/// <param name="output">The output returned by running the command.</param>
		/// <returns><see langword="true"/> if the keys were added; otherwise <see langword="false"/>.</returns>
		public static bool TryAddKeys(this ILinux server, string keysUrl, out string output)
		{
			output = server.Connection.RunCommand($"curl {keysUrl} | sudo apt-key add -");
			if (output.EndsWith("OK"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}