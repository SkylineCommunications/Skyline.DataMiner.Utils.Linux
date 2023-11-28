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
		/// <returns><see langword="true"/> if the service is installed; otherwise <see langword="false"/>.</returns>
		bool IsInstalled();

		/// <summary>
		/// Checks if the service is enabled.
		/// </summary>
		/// <returns><see langword="true"/> if the service is enabled; otherwise <see langword="false"/>.></returns>
		bool IsEnabled();

		/// <summary>
		/// Checks if the service is running.
		/// </summary>
		/// <returns><see langword="true"/> if service is running; otherwise <see langword="false"/>.</returns>
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

		/// <summary>
		/// Enables the service.
		/// </summary>
		void Enable();
	}
}