namespace Skyline.DataMiner.Utils.Linux.Communication
{
	using Renci.SshNet;

	/// <summary>
	/// Represents the settings to be used to set up a connection.
	/// </summary>
	public class ConnectionSettings
	{
		internal ConnectionInfo _connectionInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionSettings"/> class using the host, name and password.
		/// </summary>
		/// <param name="host">The host to which the connection needs to be created.</param>
		/// <param name="userName">The user name to be used to connect to the host.</param>
		/// <param name="password">The password to be used to connect to the host.</param>
		public ConnectionSettings(string host, string userName, string password)
		{
			Host = host;
			UserName = userName;
			Password = password;
		}

		/// <summary>
		/// The host.
		/// </summary>
		public string Host { get; private set; }

		/// <summary>
		/// The password.
		/// </summary>
		public string Password { get; private set; }

		/// <summary>
		/// The user name.
		/// </summary>
		public string UserName { get; private set; }

		internal ConnectionInfo ConnectionInfo
		{
			get
			{
				if (_connectionInfo == null)
				{
					_connectionInfo = new PasswordConnectionInfo(Host, UserName, Password);
				}

				return _connectionInfo;
			}
		}
	}
}