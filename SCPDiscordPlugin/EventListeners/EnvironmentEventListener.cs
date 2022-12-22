﻿using System.Collections.Generic;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace SCPDiscord.EventListeners
{
	internal class EnvironmentEventListener
	{
		private readonly SCPDiscord plugin;

		public EnvironmentEventListener(SCPDiscord plugin)
		{
			this.plugin = plugin;
		}

		/* TODO [PluginEvent(ServerEventType)]
		public void OnSCP914Activate(SCP914ActivateEvent ev)
		{

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "knobsetting",    ev.KnobSetting.ToString()   }
			};
			this.plugin.SendMessage(Config.GetArray("channels.onscp914activate"), "environment.onscp914activate", variables);
		}
		*/

		[PluginEvent(ServerEventType.WarheadStart)]
		public void OnStartCountdown(bool isAutomatic, Player player)
		{
			if (player == null || player.PlayerId == Server.Instance.PlayerId)
			{
				Dictionary<string, string> vars = new Dictionary<string, string>
				{
					{ "isAutomatic",    isAutomatic.ToString()                 },
					//{ "isresumed",      ev.IsResumed.ToString()                 },
					//{ "timeleft",       ev.TimeLeft.ToString()                  }
				};
				plugin.SendMessage(Config.GetArray("channels.onstartcountdown.noplayer"), "environment.onstartcountdown.noplayer", vars);
				return;
			}

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "isAutomatic", isAutomatic.ToString()                   },
				//{ "isresumed",   ev.IsResumed.ToString()                 },
				//{ "timeleft",    ev.TimeLeft.ToString()                  },
				{ "ipaddress",   player.IpAddress                         },
				{ "name",        player.Nickname                          },
				{ "playerid",    player.PlayerId.ToString()               },
				{ "steamid",     player.GetParsedUserID()                 },
				{ "class",       player.Role.ToString()                   },
				{ "team",        player.ReferenceHub.GetTeam().ToString() }
			};
			/*
			if (ev.IsResumed)
			{
				plugin.SendMessage(Config.GetArray("channels.onstartcountdown.resumed"), "environment.onstartcountdown.resumed", variables);
			}
			else
			{
			*/
				plugin.SendMessage(Config.GetArray("channels.onstartcountdown.initiated"), "environment.onstartcountdown.initiated", variables);
			//}
		}

		[PluginEvent(ServerEventType.WarheadStop)]
		public void OnStopCountdown(Player player)
		{
			if (player == null || player.PlayerId == Server.Instance.PlayerId)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					//{ "timeleft",       ev.TimeLeft.ToString()                  }
				};
				plugin.SendMessage(Config.GetArray("channels.onstopcountdown.noplayer"), "environment.onstopcountdown.noplayer", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					//{ "timeleft",       ev.TimeLeft.ToString()                  },
					{ "ipaddress",   player.IpAddress                         },
					{ "name",        player.Nickname                          },
					{ "playerid",    player.PlayerId.ToString()               },
					{ "steamid",     player.GetParsedUserID()                 },
					{ "class",       player.Role.ToString()                   },
					{ "team",        player.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage(Config.GetArray("channels.onstopcountdown.default"), "environment.onstopcountdown.default", variables);
			}
		}

		[PluginEvent(ServerEventType.WarheadDetonation)]
		public void OnDetonate()
		{
			plugin.SendMessage(Config.GetArray("channels.ondetonate"), "environment.ondetonate");
		}

		[PluginEvent(ServerEventType.LczDecontaminationStart)]
		public void OnDecontaminate()
		{
			plugin.SendMessage(Config.GetArray("channels.ondecontaminate"), "environment.ondecontaminate");
		}

		/* TODO: [PluginEvent(ServerEventType)]
		public void OnSummonVehicle(SummonVehicleEvent ev)
		{
			if (ev.IsCI)
			{
				this.plugin.SendMessage(Config.GetArray("channels.onsummonvehicle.chaos"), "environment.onsummonvehicle.chaos");
			}
			else
			{
				this.plugin.SendMessage(Config.GetArray("channels.onsummonvehicle.mtf"), "environment.onsummonvehicle.mtf");
			}
		}
		*/

		/* TODO: [PluginEvent(ServerEventType)]
		public void OnGeneratorFinish(GeneratorFinishEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "room",           ev.Generator.Room.RoomType.ToString()            },
				{ "ipaddress",      ev.ActivatingPlayer.IPAddress                    },
				{ "name",           ev.ActivatingPlayer.Name                         },
				{ "playerid",       ev.ActivatingPlayer.PlayerID.ToString()          },
				{ "steamid",        ev.ActivatingPlayer.GetParsedUserID()            },
				{ "class",          ev.ActivatingPlayer.PlayerRole.RoleID.ToString() },
				{ "team",           ev.ActivatingPlayer.PlayerRole.Team.ToString()   }
			};
			this.plugin.SendMessage(Config.GetArray("channels.ongeneratorfinish"), "environment.ongeneratorfinish", variables);
		}
		*/
	}
}