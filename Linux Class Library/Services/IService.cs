namespace Skyline.DataMiner.Utils.Linux.Services
{
	/// <summary>
	/// Represents a service running on a linux server.
	/// </summary>

	public interface IService
	{
		/// <summary>
		/// Checks if the service is installed.
		/// </summary>
		/// <returns><c>true</c> if the service is installed; otherwise <c>false</c>.</returns>
		bool IsInstalled();

		/// <summary>
		/// Checks if the service is enabled.
		/// </summary>
		/// <returns<c>true</c> if the service is enabled; otherwise <c>false</c>.></returns>
		bool IsEnabled();

		/// <summary>
		/// Checks if the service is running.
		/// </summary>
		/// <returns><c>true</c> if service is running; otherwise <c>false</c>.</returns>
		bool IsRunning();

		/// <summary>
		/// Refreshes systemctl.
		/// </summary>
		void Refresh();

		/// <summary>
		/// Restarts the service.
		/// </summary>
		void Restart();

		/// <summary>
		/// Starts the service.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops the service.
		/// </summary>
		void Stop();
	}
}