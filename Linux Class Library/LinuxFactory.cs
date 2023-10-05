namespace Skyline.DataMiner.Utils.Linux
{
	using Skyline.DataMiner.Utils.Linux.Communication;

	public static class LinuxFactory
	{
		/// <summary>
		/// Create an <see cref="ILinux"/> object by providing a connection.
		/// </summary>
		/// <param name="connection">The connection to be used to connect to the Linux system.</param>
		/// <returns>The Linux object.</returns>
		public static ILinux GetLinux(ISshConnection connection)
		{
			return new Linux(connection);
		}

		/// <summary>
		/// Create an <see cref="ILinux"/> object by providing connection settings.
		/// </summary>
		/// <param name="settings">The connection settings to be used to connect to the Linux system.</param>
		/// <returns>The Linux object.</returns>
		public static ILinux GetLinux(ConnectionSettings settings)
		{
			var connection = SshConnectionFactory.GetSshConnection(settings);
			return new Linux(connection);
		}
	}
}