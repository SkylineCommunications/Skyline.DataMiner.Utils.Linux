namespace Skyline.DataMiner.Utils.Linux.SoftwareBundleManager
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	using Skyline.DataMiner.Utils.Linux.Exceptions;
	using Skyline.DataMiner.Utils.Linux.FileSystem;
	using Skyline.DataMiner.Utils.Linux.OperatingSystems;
	using Skyline.DataMiner.Utils.SoftwareBundle;

	internal class DebianSoftwareBundleManager : ISoftwareBundleManager
	{
		private readonly ILinux linux;

		public DebianSoftwareBundleManager(ILinux linux)
		{
			this.linux = linux;
		}

		public void AutoRemove()
		{
			linux.Connection.RunCommand("sudo apt autoremove -y");
		}

		public void Download(string softwareBundle, string path)
		{
			linux.Connection.RunCommand($"sudo apt-get --download-only -o Dir::Cache=\"{path}\" -o Dir::Cache::archives=\"./\" install {softwareBundle}");
		}

		public IEnumerable<string> GetInstalledSoftwareBundles()
		{
			var response = linux.Connection.RunCommand($"sudo apt list --installed | cat");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> softwareBundles = new List<string>();
			foreach (var line in lines)
			{
				softwareBundles.Add(line.Split('/')[0]);
			}

			return softwareBundles;
		}

		public IEnumerable<string> GetUpgradeableSoftwareBundles()
		{
			var response = linux.Connection.RunCommand($"sudo apt list --upgradable | cat");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> softwareBundles = new List<string>();
			foreach (var line in lines)
			{
				if (!line.Contains('/'))
				{
					continue;
				}

				softwareBundles.Add(line.Split('/')[0]);
			}

			return softwareBundles;
		}

		public void Install(string softwareBundle)
		{
			linux.Connection.RunCommand($"sudo apt update && sudo apt install -y {softwareBundle}");
		}

		public void Remove(string softwareBundle)
		{
			linux.Connection.RunCommand($"sudo apt remove -y {softwareBundle}");
		}

		public void Update()
		{
			linux.Connection.RunCommand("sudo apt update");
		}

		public void Update(string softwareBundle)
		{
			linux.Connection.RunCommand($"sudo apt update && sudo apt-get --only-upgrade install {softwareBundle}");
		}

		public bool TryInstallSoftwareBundle(IUnZippedSoftwareBundle softwareBundle, bool isUpgrade, Dictionary<string, string> responsesForPatterns = null)
		{
			// Check if the package is supported
			if (!SoftwareBundlesFunctions.PackageSupported(linux, softwareBundle.SoftwareBundleInfo))
			{
				throw new IncompatibleSoftwareBundleException($"The operating {softwareBundle.SoftwareBundleInfo.OS} system or architecture {softwareBundle.SoftwareBundleInfo.Arch} in the package does not match with that of the server");
			}

			// Copy the package to the Linux machine
			string folderLocation = $"/home/{linux.Connection.ConnectionSettings.UserName}/{softwareBundle.SoftwareBundleInfo.Name}_{softwareBundle.SoftwareBundleInfo.Version}";
			var directory = SoftwareBundlesFunctions.UploadFilesFromSoftwareBundle(linux, softwareBundle, folderLocation);

			// Install the package
			if (!directory.Files.Any(f => f.Name.EndsWith(".deb")))
			{
				throw new IncompatibleSoftwareBundleException("The package does not include any *.deb files");
			}

			linux.Connection.RunCommand($@"sudo dpkg --force-depends -i {folderLocation}/*.deb", responsesForPatterns);
			linux.Connection.RunCommand($"sudo rm -rf {folderLocation}; echo 'done'");
			return true;
		}

		public string GetInstalledVersion(string softwareBundle)
		{
			string version = string.Empty;
			var response = linux.Connection.RunCommand($"sudo apt list --installed {softwareBundle} | cat");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Regex regex = new Regex("(?<name>\\S+)/(\\S+) (?<version>\\S+) (\\S+)");
			foreach (var line in lines)
			{
				if (!line.Contains('/'))
				{
					continue;
				}

				var match = regex.Match(line);
				if (!match.Success || !match.Groups["name"].Value.Equals(softwareBundle, StringComparison.InvariantCultureIgnoreCase))
				{
					continue;
				}

				version = match.Groups["version"].Value;
			}

			return version;
		}

		public string GetUpgradeVersion(string softwareBundle)
		{
			string version = string.Empty;
			var response = linux.Connection.RunCommand($"sudo apt update && sudo apt list --upgradable {softwareBundle} | cat");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Regex regex = new Regex("(?<name>\\S+)/(\\S+) (?<version>\\S+) (\\S+)");
			foreach (var line in lines)
			{
				if (!line.Contains('/'))
				{
					continue;
				}

				var match = regex.Match(line);
				if (!match.Success || !match.Groups["name"].Value.Equals(softwareBundle, StringComparison.InvariantCultureIgnoreCase))
				{
					continue;
				}

				version = match.Groups["version"].Value;
			}

			return version;
		}

		public bool IsSoftwareBundleInstalled(string softwareBundle)
		{
			return !string.IsNullOrWhiteSpace(GetInstalledVersion(softwareBundle));
		}
	}
}