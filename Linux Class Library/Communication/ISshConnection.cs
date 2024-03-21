namespace Skyline.DataMiner.Utils.Linux
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Utils.Linux.Communication;

	/// <summary>
	/// SSH connection interface.
	/// </summary>
	public interface ISshConnection : IDisposable
	{
		/// <summary>
		/// Gets whether the SSH connection has been connected.
		/// </summary>
		bool Connected { get; }

		/// <summary>
		/// Gets the output of the commands of the current ssh session
		/// </summary>

		/// <summary>
		/// Gets the settings used for the SSH connection.
		/// </summary>
		ConnectionSettings ConnectionSettings { get; }

		/// <summary>
		/// Gets or sets the number of retries that needs to be done for every command send over the SSH connection (Default: 0)
		/// </summary>
		int Retries { get; set; }

		/// <summary>
		/// Gets the output of the last commands executed (max 50 outputs).
		/// </summary>
		IEnumerable<string> OutputResults { get; }

		/// <summary>
		/// Initializes the connection.
		/// </summary>
		void Connect();

		/// <summary>
		/// Disconnect the SSH connection.
		/// </summary>
		void Disconnect();

		/// <summary>
		/// Send a command through the SSH connection and send a response on every prompt.
		/// </summary>
		/// <param name="command">The command to be send.</param>
		/// <param name="responsesForPatterns">The dictionary containing the response for every pattern (key).</param>
		/// <returns>The response of the command.</returns>
		string RunCommand(string command, Dictionary<string, string> responsesForPatterns = null);
	}
}