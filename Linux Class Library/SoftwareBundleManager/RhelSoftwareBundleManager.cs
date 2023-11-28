namespace Skyline.DataMiner.Utils.Linux.SoftwareBundleManager
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	using Skyline.DataMiner.Utils.Linux.Exceptions;
	using Skyline.DataMiner.Utils.SoftwareBundle;

	internal class RhelSoftwareBundleManager : ISoftwareBundleManager
	{
		private readonly ILinux linux;

		public RhelSoftwareBundleManager(ILinux linux)
		{
			this.linux = linux;
		}

		public void AutoRemove()
		{
			linux.Connection.RunCommand("sudo yum autoremove -y");
		}

		public void Download(string softwareBundle, string path)
		{
			linux.Connection.RunCommand($"sudo yum install --downloadonly --downloaddir={path} {softwareBundle}");
		}

		public IEnumerable<string> GetInstalledSoftwareBundles()
		{
			var response = linux.Connection.RunCommand($"sudo yum list installed");
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
			var response = linux.Connection.RunCommand($"sudo yum check-update");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			List<string> softwareBundles = new List<string>();
			foreach (var line in lines)
			{
				softwareBundles.Add(line.Split('/')[0]);
			}

			return softwareBundles;
		}

		public void Install(string softwareBundle)
		{
			linux.Connection.RunCommand($"sudo yum install -y {softwareBundle}");
		}

		public void Remove(string softwareBundle)
		{
			linux.Connection.RunCommand($"sudo yum remove -y {softwareBundle}");
		}

		public void Update()
		{
			linux.Connection.RunCommand("sudo yum -y update");
		}

		public void Update(string softwareBundle)
		{
			linux.Connection.RunCommand($"sudo yum -y update {softwareBundle}");
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
			if (!directory.Files.Any(f => f.Name.EndsWith(".rpm")))
			{
				throw new IncompatibleSoftwareBundleException("The package does not include any *.rpm files");
			}

			if (isUpgrade)
			{
				linux.Connection.RunCommand($@"sudo rpm -U --replacepkgs --oldpackage {folderLocation}/*.rpm", responsesForPatterns);
			}
			else
			{
				linux.Connection.RunCommand($@"sudo rpm -i --replacepkgs --oldpackage {folderLocation}/*.rpm", responsesForPatterns);
			}

			linux.Connection.RunCommand($"sudo rm -rf {folderLocation}; echo 'done'");
			return true;
		}

		public string GetInstalledVersion(string softwareBundle)
		{
			string version = string.Empty;
			var response = linux.Connection.RunCommand($"sudo yum list installed {softwareBundle}");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Regex regex = new Regex("(?<name>\\S+)\\.(\\S+)(\\s+)(?<version>\\S+)(\\s+)");
			foreach (var line in lines)
			{
				if (!line.Contains(softwareBundle))
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
			var response = linux.Connection.RunCommand($"sudo yum check-update {softwareBundle}");
			var lines = response.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Regex regex = new Regex("(?<name>\\S+)\\.(\\S+)(\\s+)(?<version>\\S+)(\\s+)");
			foreach (var line in lines)
			{
				if (!line.Contains(softwareBundle))
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