namespace Skyline.DataMiner.Utils.Linux.SoftwareBundleManager
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Utils.Linux.FileSystem;
	using Skyline.DataMiner.Utils.SoftwareBundle;

	internal class SoftwareBundlesFunctions
	{
		internal static bool PackageSupported(ILinux linux, SoftwareBundleInfo softwareBundleInfo)
		{
			bool osMatch = linux.OsInfo.Id.Equals(softwareBundleInfo.OS, StringComparison.InvariantCultureIgnoreCase) || linux.OsInfo.Id_Like.Any(l => l.Equals(softwareBundleInfo.OS, StringComparison.InvariantCultureIgnoreCase));
			bool archMatch = linux.Arch.Equals(softwareBundleInfo.Arch, StringComparison.InvariantCultureIgnoreCase);

			return osMatch && archMatch;
		}

		internal static IDirectory UploadFilesFromSoftwareBundle(ILinux linux, IUnZippedSoftwareBundle unZippedSoftwareBundle, string destination)
		{
			var directory = linux.CreateDirectory(destination);
			IEnumerable<string> files = unZippedSoftwareBundle.GetFiles();
			foreach (var file in files)
			{
				var fileName = System.IO.Path.GetFileName(file);
				if (linux.FileExists(string.Join("/", destination, fileName)))
				{
					continue;
				}

				linux.UploadFile(file, string.Join("/", destination, fileName));
			}

			return directory;
		}
	}
}