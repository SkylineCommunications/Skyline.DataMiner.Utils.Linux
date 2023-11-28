# Skyline.DataMiner.Utils.Linux

## About

### About Skyline.DataMiner.Utils.Linux

Skyline.DataMiner.Utils.Linux is a package available in the public [nuget store](https://www.nuget.org/) that contain assemblies to communicate with a Linux server in DataMiner.

This library will retrieve information from the DataMiner system to give information on the Linux server and provide functions to perform actions on the server.

### About DataMiner

DataMiner is a transformational platform that provides vendor-independent control and monitoring of devices and services. Out of the box and by design, it addresses key challenges such as security, complexity, multi-cloud, and much more. It has a pronounced open architecture and powerful capabilities enabling users to evolve easily and continuously.

The foundation of DataMiner is its powerful and versatile data acquisition and control layer. With DataMiner, there are no restrictions to what data users can access. Data sources may reside on premises, in the cloud, or in a hybrid setup.

A unique catalog of 7000+ connectors already exist. In addition, you can leverage DataMiner Development Packages to build you own connectors (also known as "protocols" or "drivers").

> [!TIP]
> See also: [About DataMiner](https://aka.dataminer.services/about-dataminer)

### About Skyline Communications

At Skyline Communications, we deal in world-class solutions that are deployed by leading companies around the globe. Check out [our proven track record](https://aka.dataminer.services/about-skyline) and see how we make our customers' lives easier by empowering them to take their operations to the next level.

## Requirements

The "DataMiner Integration Studio" Visual Studio extension is required for development of connectors and Automation scripts using NuGets.

See [Installing DataMiner Integration Studio](https://aka.dataminer.services/DisInstallation)

> [!IMPORTANT]
> NuGets are mandatory to be installed with PackageReferences. DIS was redesigned to work with PackageReferences and be future-proof. 
>
> For more information on how to migrate from packages.config to PackageReferences, see [docs.microsoft.com](https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference).

## Getting started

You will need to add the following NuGet packages to your automation project from the public [nuget store](https://www.nuget.org/):
- Skyline.DataMiner.Utils.Linux

Example code:
 ```cs
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
				//Get the linux IP address, username and password from the DM system.
				string ip = "<YOUR_IP_HERE>";
				string username = "<YOUR_USERNAME_HERE>";
				string password = "<YOUR_PASSWORD_HERE>";
				ConnectionSettings settings = new ConnectionSettings(ip, username, password);
				using (var linux = LinuxFactory.GetLinux(settings))
				{
					linux.Connection.Connect();

					//Sends a text command "whoami" into the linux server
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
```