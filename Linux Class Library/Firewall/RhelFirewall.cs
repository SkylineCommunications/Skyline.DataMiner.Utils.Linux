namespace Skyline.DataMiner.Utils.Linux.Firewall
{
	internal class RhelFirewall : IFirewall
	{
		private readonly ILinux linux;

		public RhelFirewall(ILinux linux)
		{
			this.linux = linux;
		}

		public void Close(int portNumber, FirewallNetworkProtocols protocol)
		{
			linux.Connection.RunCommand($"sudo firewall-cmd --permanent --add-port={portNumber}/{protocol.ToString().ToLower()}");
		}

		public bool IsOpen(int portNumber)
		{
			if (!IsRunning())
			{
				return false;
			}

			var statusList = linux.Connection.RunCommand($"sudo firewall-cmd --permanent --list-ports");

			return statusList.Contains(portNumber.ToString());
		}

		public bool IsRunning()
		{
			var status = linux.Connection.RunCommand("sudo firewall-cmd --state");
			return status.Contains("running");
		}

		public void Open(int portNumber, FirewallNetworkProtocols protocol)
		{
			// Run once without permanent
			linux.Connection.RunCommand($"sudo firewall-cmd --add-port={portNumber}/{protocol.ToString().ToLower()}");

			// Run with permanent
			linux.Connection.RunCommand($"sudo firewall-cmd --permanent --add-port={portNumber}/{protocol.ToString().ToLower()}");
		}

		public void Start()
		{
			linux.Connection.RunCommand("sudo systemctl start firewalld");
		}

		public void Stop()
		{
			linux.Connection.RunCommand("sudo systemctl stop firewalld");
		}
	}
}