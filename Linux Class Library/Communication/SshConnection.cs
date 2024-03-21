namespace Skyline.DataMiner.Utils.Linux
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Threading;

	using Renci.SshNet;
	using Renci.SshNet.Common;

	using Skyline.DataMiner.Utils.Linux.Communication;
	using Skyline.DataMiner.Utils.Linux.Exceptions;

	internal sealed class SshConnection : ISshConnection
	{
		private const string UpdatedPs1 = "$|*_*|$";
		private const string UpdatedPs1Regex = "\\$\\|\\*_\\*\\|\\$";
		private const string LockRegex = "(lock: Could not get lock )(?<lockObject>.+)It is held by process (?<pcocessInfo>.+)\\.\\.\\. 3\\ds";

		private static TimeSpan commandTimeout = new TimeSpan(0, 15, 0);
		private readonly ConnectionSettings _connectionSettings;
		private readonly FixedBuffer<string> buffer = new FixedBuffer<string>();
		private SshClient sshClient;
		private bool reachedEnd = false;
		private string lastResp;

		internal SshConnection(ConnectionSettings connectionInfo, int retries = 0)
		{
			this.Retries = retries;
			this._connectionSettings = connectionInfo;
			sshClient = new SshClient(connectionInfo.ConnectionInfo);
		}

		internal SshConnection(int retries = 0)
		{
			this.Retries = retries;
		}

		public bool Connected { get; private set; }

		public ConnectionSettings ConnectionSettings
		{
			get
			{
				return _connectionSettings;
			}
		}

		public int Retries { get; set; }

		internal int Offset { get; private set; } = -1;

		public IEnumerable<string> OutputResults
		{
			get
			{
				return buffer.Values;
			}
		}

		public void Connect()
		{
			try
			{
				sshClient.Connect();
			}
			catch (SshAuthenticationException ex)
			{
				if (ex.ToString().Contains("No suitable authentication method found"))
				{
					// Try again with keyboard Interactive
					KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(ConnectionSettings.UserName);
					keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
					ConnectionSettings._connectionInfo = new ConnectionInfo(ConnectionSettings.Host, ConnectionSettings.UserName, keybAuth);
					if (sshClient != null)
					{
						sshClient.Dispose();
					}

					sshClient = new SshClient(ConnectionSettings.ConnectionInfo);
				}
			}

			Connected = true;
			RunCommand("hostname");
		}

		private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
		{
			foreach (AuthenticationPrompt prompt in e.Prompts)
			{
				if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
				{
					prompt.Response = ConnectionSettings.Password;
				}
			}
		}

		public void Disconnect()
		{
			sshClient.Disconnect();
		}

		public void Dispose()
		{
			if (Connected)
			{
				sshClient.Disconnect();
				sshClient.Dispose();
			}

			Connected = false;
		}

		public string RunCommand(string command, Dictionary<string, string> responsesForPatterns = null)
		{
			if (!Connected)
			{
				throw new NotConnectedException();
			}

			for (int i = 0; i < Retries + 1; i++)
			{
				try
				{
					var result = RunShellCommand(command, responsesForPatterns);
					return result;
				}
				catch (SshConnectionException)
				{
					if (i == Retries)
					{
						throw;
					}

					Connect();
					Connected = true;
				}
				catch (Exception)
				{
					if (i == Retries)
					{
						throw;
					}
				}
			}

			throw new NotSupportedException("The code should never reach this.");
		}

		public string RunShellCommand(string command, Dictionary<string, string> responseForRegexPattern)
		{
			IDictionary<TerminalModes, uint> termkvp = new Dictionary<TerminalModes, uint>()
			{
				{ TerminalModes.ECHO, 53 },
			};
			using (ShellStream shellStream = sshClient.CreateShellStream("dumb", 80, 24, 800, 600, 1024, termkvp))
			{
				//Get logged in
				string rep = shellStream.Expect(new Regex($@"{_connectionSettings.UserName}@(.*)[$#>]"), commandTimeout); //expect user prompt
				if (rep == null)
				{
					throw new System.TimeoutException($"Unable to setup the shellstream for '{command}'.");
				}

				// Change the PS1 variable for easier pattern recognition
				shellStream.WriteLine($"PS1='{UpdatedPs1}'");
				var first = shellStream.Expect(new Regex($"[^']{UpdatedPs1Regex}"), commandTimeout); // read output to clear

				// Run command
				reachedEnd = false;
				shellStream.WriteLine(command);

				// Wait for expected patterns or timeout
				var actions = GetActions(shellStream, responseForRegexPattern);
				SpinWait.SpinUntil(() =>
				{
					shellStream.Expect(commandTimeout, actions);
					return reachedEnd;
				},
				commandTimeout);

				if (lastResp == null)
				{
					throw new System.TimeoutException($"Unable to get the result for '{command}'.");
				}

				if (lastResp.StartsWith(command))
				{
					lastResp = lastResp.Substring(command.Length);
				}

				lastResp = lastResp.Replace(UpdatedPs1, string.Empty);
				lastResp = lastResp.Replace("\r\n", "\n");
				lastResp = lastResp.Trim();
				return lastResp;
			}
		}

		private ExpectAction[] GetActions(ShellStream shellStream, Dictionary<string, string> responseForRegexPattern)
		{
			List<ExpectAction> actions = new List<ExpectAction>();
			var passwordAction = new ExpectAction(
				new Regex($"\\[sudo\\] password for {_connectionSettings.UserName}:"),
				(string resp) =>
				{
					buffer.Add(resp);
					shellStream.WriteLine(_connectionSettings.Password);
				});
			actions.Add(passwordAction);
			var lockUnavailable = new ExpectAction(
				new Regex(LockRegex),
				(string resp) =>
				{
					buffer.Add(resp);
					lastResp = resp;
					shellStream.Close();
					throw new Exceptions.TimeoutException("The command failed as it could not acquire a lock.");
				});
			actions.Add(lockUnavailable);
			var endExpect = new ExpectAction(
				new Regex(UpdatedPs1Regex),
				(string resp) =>
				{
					buffer.Add(resp);
					lastResp = resp;
					reachedEnd = true;
				});
			actions.Add(endExpect);
			if (responseForRegexPattern == null)
			{
				return actions.ToArray();
			}

			foreach (var item in responseForRegexPattern)
			{
				var itemAction = new ExpectAction(
				new Regex(item.Key),
				(string resp) =>
				{
					buffer.Add(resp);
					shellStream.WriteLine(item.Value);
				});
				actions.Add(itemAction);
			}

			return actions.ToArray();
		}
	}
}