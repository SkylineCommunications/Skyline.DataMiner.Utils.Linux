namespace Skyline.DataMiner.Utils.Linux.OperatingSystems
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// The information on the operating system.
	/// </summary>
	public class OsReleaseInfo
	{
		/// <summary>
		/// The URL to the bug reporting page for the operating system.
		/// </summary>
		public string BugReportUrl { get; private set; }

		/// <summary>
		/// A unique identifier for the specific build of the operating system (optional, can be null).
		/// </summary>
		public string BuildId { get; private set; }

		/// <summary>
		/// The URL to the home page of the operating system.
		/// </summary>
		public string HomeUrl { get; private set; }

		/// <summary>
		/// The unique identifier of the operating system.
		/// </summary>
		public string Id { get; private set; }

		/// <summary>
		/// Other identifiers of operating systems to which this operating system is related to (optional, can be null).
		/// </summary>
		public IEnumerable<string> Id_Like
		{
			get
			{
				if (IdLike == null)
				{
					return new string[0];
				}

				return IdLike.Split(' ');
			}
		}

		/// <summary>
		/// Indicates if the operating system is supported by this library.
		/// </summary>
		public bool IsSupported => OsType != OperatingSystemType.Unknown;

		/// <summary>
		/// The human-readable name of the operating system.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Get the type of operating system.
		/// </summary>
		public OperatingSystemType OsType
		{
			get
			{
				if (Id_Like.Any(l => l.Equals("debian", System.StringComparison.InvariantCultureIgnoreCase)) || Id.Equals("debian", System.StringComparison.InvariantCultureIgnoreCase))
				{
					return OperatingSystemType.Debian;
				}

				if (Id_Like.Any(l => l.Equals("rhel", System.StringComparison.InvariantCultureIgnoreCase)) || Id.Equals("rhel", System.StringComparison.InvariantCultureIgnoreCase))
				{
					return OperatingSystemType.RHEL;
				}

				return OperatingSystemType.Unknown;
			}
		}

		/// <summary>
		/// A prettier version of the operating system name.
		/// </summary>
		public string PrettyName { get; private set; }

		/// <summary>
		/// The URL to the support page for the operating system.
		/// </summary>
		public string SupportUrl { get; private set; }

		/// <summary>
		/// The variant of the operating system (optional, can be null).
		/// </summary>
		public string Variant { get; private set; }

		/// <summary>
		/// The unique identifier of the variant (optional, can be null).
		/// </summary>
		public string VariantId { get; private set; }

		/// <summary>
		/// The version of the operating system.
		/// </summary>
		public string Version { get; private set; }

		/// <summary>
		/// A lower-case string with no spaces or other characters outside of 0-9, a-z, ".", "_" and "-".
		/// Identifying the operating system version for the specific build of the operating system (optional, can be null).
		/// </summary>
		public string VersionId { get; private set; }

		/// <summary>
		/// Additional identifiers that the operating system is related to.
		/// </summary>
		private string IdLike { get; set; }

		internal static OsReleaseInfo ParseOsReleaseFile(string fileContent)
		{
			Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
			foreach (string line in fileContent.Split('\n'))
			{
				string trimmedLine = line.Trim();
				if (!string.IsNullOrWhiteSpace(trimmedLine))
				{
					string[] parts = trimmedLine.Split(new[] { '=' }, 2);
					if (parts.Length == 2)
					{
						string key = parts[0].Trim();
						string value = parts[1].Trim('\'', '"');
						keyValuePairs[key] = value;
					}
				}
			}

			return new OsReleaseInfo
			{
				Name = keyValuePairs.GetValueOrDefault("NAME"),
				Version = keyValuePairs.GetValueOrDefault("VERSION"),
				Id = keyValuePairs.GetValueOrDefault("ID"),
				IdLike = keyValuePairs.GetValueOrDefault("ID_LIKE"),
				PrettyName = keyValuePairs.GetValueOrDefault("PRETTY_NAME"),
				HomeUrl = keyValuePairs.GetValueOrDefault("HOME_URL"),
				SupportUrl = keyValuePairs.GetValueOrDefault("SUPPORT_URL"),
				BugReportUrl = keyValuePairs.GetValueOrDefault("BUG_REPORT_URL"),
				BuildId = keyValuePairs.GetValueOrDefault("BUILD_ID"),
				VersionId = keyValuePairs.GetValueOrDefault("VERSION_ID"),
				Variant = keyValuePairs.GetValueOrDefault("VARIANT"),
				VariantId = keyValuePairs.GetValueOrDefault("VARIANT_ID")
			};
		}
	}
}