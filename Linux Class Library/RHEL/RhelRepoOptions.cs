// Ignore Spelling: Gpg

namespace Skyline.DataMiner.Utils.Linux.RHEL
{
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Represents repository options for RHEL based systems as by <see href="https://access.redhat.com/documentation/en-us/red_hat_enterprise_linux/6/html/deployment_guide/sec-setting_repository_options">Setting [repository] Options</see>.
	/// </summary>
	public class RhelRepoOptions
	{
		/// <summary>
		/// Create a new instance of <see cref="RhelRepoOptions"/>.
		/// </summary>
		/// <param name="repositoryId">The ID of the repository.</param>
		/// <param name="repositoryName">The name of the repository.</param>
		/// <param name="repositoryUrl">The URL of the repository.</param>
		public RhelRepoOptions(string repositoryId, string repositoryName, string[] repositoryUrl)
		{
			RepositoryId = repositoryId;
			RepositoryName = repositoryName;
			RepositoryUrl = repositoryUrl;
		}

		/// <summary>
		/// This tells yum whether or not use this repository.
		/// </summary>
		public bool? Enabled { get; set; }

		/// <summary>
		/// This tells yum whether or not it should perform a GPG signature check on the packages gotten from this repository.
		/// </summary>
		public bool? GpgCheck { get; set; }

		/// <summary>
		/// A URL pointing to the ASCII-armed GPG key file for the repository.
		/// Multiple URLs may be specified here in the same manner as the baseurl option.
		/// </summary>
		public string[] GpgKey { get; set; }

		/// <summary>
		/// This tells yum whether or not it should perform a GPG signature check on the repodata from this repository.
		/// </summary>
		public bool? RepoGpgCheck { get; set; }

		/// <summary>
		/// Must be a unique name for each repository, one word.
		/// </summary>
		public string RepositoryId { get; private set; }

		/// <summary>
		/// A human readable string describing the repository.
		/// </summary>
		public string RepositoryName { get; private set; }

		/// <summary>
		/// Must be a URL to the directory where the yum repository's 'repodata' directory lives.
		/// Can be an http://, ftp:// or file:// URL. You can specify multiple URLs in one baseurl
		/// </summary>
		public string[] RepositoryUrl { get; private set; }

		/// <summary>
		/// Get the file path of the repository options (/etc/yum.repos.d/{RepositoryId}.repo).
		/// </summary>
		public string RepoFilePath
		{
			get
			{
				return $"/etc/yum.repos.d/{RepositoryId}.repo";
			}
		}

		/// <summary>
		/// The string format of repository options for RHEL based systems
		/// </summary>
		/// <returns>The text value.</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"[{RepositoryId}]\n");
			sb.Append($"name={RepositoryName}\n");
			sb.Append($"baseurl={RepositoryUrl[0]}\n");
			for (int i = 1; i < RepositoryUrl.Length; i++)
			{
				sb.Append($"{RepositoryUrl[i]}\n");
			}

			if (Enabled.HasValue)
			{
				if (Enabled.Value)
				{
					sb.Append("enabled=1\n");
				}
				else
				{
					sb.Append("enabled=0\n");
				}
			}

			if (GpgCheck.HasValue)
			{
				if (GpgCheck.Value)
				{
					sb.Append("gpgcheck=1\n");
				}
				else
				{
					sb.Append("gpgcheck=0\n");
				}
			}

			if (RepoGpgCheck.HasValue)
			{
				if (RepoGpgCheck.Value)
				{
					sb.Append("repo_gpgcheck=1\n");
				}
				else
				{
					sb.Append("repo_gpgcheck=0\n");
				}
			}

			if (GpgKey != null && GpgKey.Any())
			{
				sb.Append($"gpgkey={GpgKey[0]}\n");
				for (int i = 1; i < GpgKey.Length; i++)
				{
					sb.Append($"{GpgKey[i]}\n");
				}
			}

			return sb.ToString();
		}
	}
}