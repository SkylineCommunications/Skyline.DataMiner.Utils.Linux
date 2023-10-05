namespace Skyline.DataMiner.Utils.Linux.RHEL
{
	using System.Text;

	using Skyline.DataMiner.Utils.Linux.Exceptions;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;

	/// <summary>
	/// Extension methods for RHEL based systems.
	/// </summary>
	public static class RhelExtensions
	{
		/// <summary>
		/// Add or update a repo file.
		/// </summary>
		/// <param name="server">The Linux machine.</param>
		/// <param name="repoOptions">The repo options to use.</param>
		/// <exception cref="LinuxNotSupportedException">The Linux machine is not RHEL based.</exception>
		public static void AddOrUpdateRhelRepoOptions(this ILinux server, RhelRepoOptions repoOptions)
		{
			if (server.OsInfo.OsType != OperatingSystemType.RHEL)
			{
				throw new LinuxNotSupportedException("This method can only be used for RHEL based systems.");
			}

			server.CreateFile(repoOptions.RepoFilePath, repoOptions.ToString(), Encoding.UTF8, true);
		}
	}
}