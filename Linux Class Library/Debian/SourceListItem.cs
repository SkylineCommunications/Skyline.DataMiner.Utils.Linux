namespace Skyline.DataMiner.Utils.Linux.Debian
{
	using System;

	/// <summary>
	/// Represents a source list item as defined in <see href="https://wiki.debian.org/SourcesList"/>.
	/// </summary>
	public struct SourceListItem
	{
		/// <summary>
		/// Create a new instance of <see cref="SourceListItem"/>.
		/// </summary>
		/// <param name="archiveType">The type of archive.</param>
		/// <param name="repositoryUrl">The URL to download the repository.</param>
		/// <param name="distribution">The distribution of package to use.</param>
		/// <param name="components">The component of package to use.</param>
		public SourceListItem(ArchiveTypes archiveType, string repositoryUrl, string distribution, string[] components)
		{
			ArchiveType = archiveType;
			RepositoryUrl = repositoryUrl;
			Distribution = distribution;
			Components = components;
			PgpFilePath = null;
		}

		/// <summary>
		/// The Archive types.
		/// </summary>
		public enum ArchiveTypes
		{
			/// <summary>
			/// The archive contains binary packages.
			/// </summary>
			Deb,

			/// <summary>
			/// The archive source packages.
			/// </summary>
			Deb_Src,
		}

		/// <summary>
		/// The type of the archive.
		/// </summary>
		public ArchiveTypes ArchiveType { get; set; }

		/// <summary>
		/// The component of the package to use.
		/// </summary>
		public string[] Components { get; set; }

		/// <summary>
		/// The distribution of the package to use.
		/// </summary>
		public string Distribution { get; set; }

		/// <summary>
		/// The URL to download the repository.
		/// </summary>
		public string RepositoryUrl { get; set; }

		/// <summary>
		/// The PGP file path (e.g. /usr/share/keyrings/deriv-archive-keyring.pgp)
		/// </summary>
		public string PgpFilePath { get; set; }

		public override string ToString()
		{
			string archive = ArchiveType == ArchiveTypes.Deb ? "deb" : "deb-src";
			string pgp = string.IsNullOrWhiteSpace(PgpFilePath) ? string.Empty : $"[signed-by={PgpFilePath}] ";
			return $"{archive} {pgp}{RepositoryUrl} {Distribution} {String.Join(" ", Components)}";
		}
	}
}