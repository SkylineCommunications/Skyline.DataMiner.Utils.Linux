namespace ExampleCode
{
	using System;
	using System.Net.Sockets;
	using Skyline.DataMiner.Utils.Linux;
	using Skyline.DataMiner.Utils.Linux.Communication;

	/// <summary>
	/// DataMiner Script Class.
	/// </summary>
	public class Examples
	{
		/// <summary>
		/// Connect to a Linux system by providing SSH credentials.
		/// </summary>
		public void ConnectToLinuxAndRunCommand()
		{
			try
			{
				//Get the Linux IP address, user name and password from the DM system.
				string ip = "<YOUR_IP_HERE>";
				string username = "<YOUR_USERNAME_HERE>";
				string password = "<YOUR_PASSWORD_HERE>";
				ConnectionSettings settings = new ConnectionSettings(ip, username, password);
				using (var linux = LinuxFactory.GetLinux(settings))
				{
					linux.Connection.Connect();

					//Sends a text command "whoami" into the Linux server
					string whoami = linux.Connection.RunCommand("whoami");
					if (!string.IsNullOrWhiteSpace(whoami))
					{
						// Connection successful
						Console.WriteLine("Connection Successful");
					}
					else
					{
						Console.WriteLine("Something went wrong");
					}
				}
			}
			catch (SocketException)
			{
				Console.WriteLine("Host not reachable");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}