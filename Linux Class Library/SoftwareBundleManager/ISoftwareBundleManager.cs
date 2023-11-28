namespace Skyline.DataMiner.Utils.Linux.SoftwareBundleManager
{
	using System.Collections.Generic;

	using Skyline.DataMiner.Utils.SoftwareBundle;

	/// <summary>
	/// Represents the software bundle manager of the server.
	/// </summary>
	public interface ISoftwareBundleManager
	{
		/// <summary>
		/// Remove unused dependencies.
		/// </summary>
		void AutoRemove();

		/// <summary>
		/// Get the software bundles installed.
		/// </summary>
		/// <returns>The name of the software bundles installed.</returns>
		IEnumerable<string> GetInstalledSoftwareBundles();

		/// <summary>
		/// Get the software bundles that are upgradeable.
		/// </summary>
		/// <returns>The name of the software bundles that are upgradeable.</returns>
		IEnumerable<string> GetUpgradeableSoftwareBundles();

		/// <summary>
		/// Get the installed version of the software bundle.
		/// </summary>
		/// <param name="softwareBundle">The name of the software bundle.</param>
		/// <returns>The version installed.</returns>
		string GetInstalledVersion(string softwareBundle);

		/// <summary>
		/// Get the version of the upgrade that is available for the software bundle.
		/// </summary>
		/// <param name="softwareBundle">The name of the software bundle.</param>
		/// <returns>The version available.</returns>
		string GetUpgradeVersion(string softwareBundle);

		/// <summary>
		/// Install a specific software bundle.
		/// </summary>
		/// <param name="softwareBundle">The name of the software bundle.</param>
		void Install(string softwareBundle);

		/// <summary>
		/// Remove an installed software bundle.
		/// </summary>
		/// <param name="softwareBundle">The name of the software bundle.</param>
		void Remove(string softwareBundle);

		/// <summary>
		/// Updates the local repositories with the latest versions available.
		/// </summary>
		void Update();

		/// <summary>
		/// Updates a specific local repository with the latest versions available.
		/// </summary>
		/// <param name="softwareBundle">The name of the software bundle.</param>
		void Update(string softwareBundle);

		/// <summary>
		/// Verify if the software bundle is installed.
		/// </summary>
		/// <param name="softwareBundle">The name of the software bundle.</param>
		/// <returns><see langword="true"/> if the software bundle is installed; otherwise <see langword="false"/>.</returns>
		bool IsSoftwareBundleInstalled(string softwareBundle);

		/// <summary>
		/// Try to install a custom software bundle towards the Linux machine. This method should be used when the software bundle can't be retrieved from the software bundle manager.
		/// </summary>
		/// <param name="softwareBundle">The software bundle that needs to be installed.</param>
		/// <param name="isUpgrade">Indicates if the software bundle will upgrade an existing software bundle or install a new one.</param>
		/// <param name="responsesForPatterns">The dictionary containing the response for every pattern (key).</param>
		/// <returns><see langword="true"/> if the installation was successful; otherwise <see langword="false"/>.</returns>
		bool TryInstallSoftwareBundle(IUnZippedSoftwareBundle softwareBundle, bool isUpgrade, Dictionary<string, string> responsesForPatterns = null);
	}
}