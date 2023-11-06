namespace Skyline.DataMiner.Utils.Linux.Services
{
	using Skyline.DataMiner.Utils.Linux.Exceptions;

	internal class Service : IService
	{
		private readonly ILinux linux;
		private readonly string serviceName;

		internal Service(ILinux linux, string serviceName)
		{
			if (serviceName.EndsWith(".service"))
			{
				this.serviceName = serviceName.Remove(serviceName.LastIndexOf(".service"));
			}
			else
			{
				this.serviceName = serviceName;
			}

			this.linux = linux;
		}

		public bool IsEnabled()
		{
			var enabled = linux.Connection.RunCommand($"sudo systemctl is-enabled {this.serviceName}.service");
			return enabled == "enabled";
		}

		public bool IsInstalled()
		{
			try
			{
				var serviceListFiltered = linux.Connection.RunCommand($@"systemctl list-units | grep {this.serviceName}");
				if (serviceListFiltered.Contains($"{this.serviceName}.service"))
				{
					return true;
				}
			}
			catch (EmptyResultException)
			{
				return false;
			}

			return false;
		}

		public bool IsRunning()
		{
			var status = linux.Connection.RunCommand($"sudo systemctl is-active {this.serviceName}.service");
			return status == "active";
		}

		public void Refresh()
		{
			linux.Connection.RunCommand($"sudo systemctl daemon-reload");
		}

		public void Restart()
		{
			linux.Connection.RunCommand($"sudo systemctl restart {this.serviceName}.service");
		}

		public void Start()
		{
			linux.Connection.RunCommand($"sudo systemctl start {this.serviceName}.service");
		}

		public void Stop()
		{
			linux.Connection.RunCommand($"sudo systemctl stop {this.serviceName}.service");
		}
	}
}