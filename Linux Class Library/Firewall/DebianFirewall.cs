// Ignore Spelling: Debian

namespace Skyline.DataMiner.Utils.Linux.Firewall
{
	internal class DebianFirewall : IFirewall
	{
		private readonly ILinux linux;

		public DebianFirewall(ILinux linux)
		{
			this.linux = linux;
		}

		public void Close(int portNumber, FirewallNetworkProtocols protocol)
		{
			this.linux.Connection.RunCommand($"sudo ufw delete allow {portNumber}/{protocol.ToString().ToLower()}");
		}

		public bool IsOpen(int portNumber)
		{
			if (!IsRunning())
			{
				return false;
			}

			var statusList = this.linux.Connection.RunCommand($"sudo ufw status | grep '{portNumber}'");

			foreach (var status in statusList.Split('\n'))
			{
				if ((status.Contains("ALLOW")) && (status.Contains("Anywhere")))
				{
					return true;
				}
			}

			return false;
		}

		public bool IsRunning()
		{
			var status = this.linux.Connection.RunCommand("sudo ufw status | grep 'Status:'");
			return status.Contains("active");
		}

		public void Open(int portNumber, FirewallNetworkProtocols protocol)
		{
			this.linux.Connection.RunCommand($"sudo ufw allow {portNumber}/{protocol.ToString().ToLower()}");
		}

		public void Start()
		{
			this.linux.Connection.RunCommand("sudo ufw enable");
		}

		public void Stop()
		{
			this.linux.Connection.RunCommand("sudo ufw disable");
		}
	}
}