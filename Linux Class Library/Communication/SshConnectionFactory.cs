namespace Skyline.DataMiner.Utils.Linux.Communication
{
	/// <summary>
	/// Provides methods to create factory <see cref="ISshConnection"/> objects.
	/// </summary>
	public static class SshConnectionFactory
	{
		/// <summary>
		/// Creates a factory SSH connection.
		/// </summary>
		/// <param name="connectionSettings">The connection settings to be used in the SSH connection.</param>
		/// <returns>The SSH connection.</returns>
		public static ISshConnection GetSshConnection(ConnectionSettings connectionSettings)
		{
			return new SshConnection(connectionSettings);
		}
	}
}