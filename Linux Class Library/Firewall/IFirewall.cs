namespace Skyline.DataMiner.Utils.Linux.Firewall
{
	/// <summary>
	/// Represents a firewall running on the Linux server.
	/// </summary>
	public interface IFirewall
	{
		/// <summary>
		/// Closes a specified port.
		/// </summary>
		/// <param name="portNumber">The port number to be closed.</param>
		/// <param name="protocol">The protocol to be used.</param>
		void Close(int portNumber, FirewallNetworkProtocols protocol);

		/// <summary>
		/// Checks if a specified port is open. If the firewall is disabled, it is closed by default.
		/// </summary>
		/// <param name="portNumber">The port number to be checked.</param>
		/// <returns><c>true</c> if the port is open; otherwise <c>false</c>.</returns>
		bool IsOpen(int portNumber);

		/// <summary>
		/// Checks if the firewall is currently running.
		/// </summary>
		/// <returns><c>true</c> if the firewall is running; otherwise <c>false</c>.</returns>
		bool IsRunning();

		/// <summary>
		/// Opens a specified port with a specified protocol.
		/// </summary>
		/// <param name="portNumber">The port number to be opened.</param>
		/// <param name="protocol">The protocol to be used.</param>
		void Open(int portNumber, FirewallNetworkProtocols protocol);

		/// <summary>
		/// Starts the firewall.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops the firewall
		/// </summary>
		void Stop();
	}
}