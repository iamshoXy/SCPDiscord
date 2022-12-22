﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCPDiscord.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace SCPDiscord
{
	internal static class Language
	{
		private static SCPDiscord plugin;
		public static bool ready;

		private static JObject primary;
		private static JObject backup;

		private static string languagesPath = Config.GetLanguageDir();

		// All default languages included in the .dll
		private static readonly Dictionary<string, string> defaultLanguages = new Dictionary<string, string>
		{
			{ "english",            Encoding.UTF8.GetString(Resources.english)          },
			{ "russian",            Encoding.UTF8.GetString(Resources.russian)          },
			{ "french",             Encoding.UTF8.GetString(Resources.french)           },
			{ "polish",             Encoding.UTF8.GetString(Resources.polish)           },
			{ "italian",            Encoding.UTF8.GetString(Resources.italian)          },
			{ "finnish",            Encoding.UTF8.GetString(Resources.finnish)          },
			{ "english-emote",      Encoding.UTF8.GetString(Resources.english_emote)    },
			{ "russian-emote",      Encoding.UTF8.GetString(Resources.russian_emote)    },
			{ "french-emote",       Encoding.UTF8.GetString(Resources.french_emote)     },
			{ "polish-emote",       Encoding.UTF8.GetString(Resources.polish_emote)     },
			{ "italian-emote",      Encoding.UTF8.GetString(Resources.italian_emote)    },
			{ "finnish-emote",      Encoding.UTF8.GetString(Resources.finnish_emote)    }
		};

		private static readonly List<string> messageNodes = new List<string>
		{
			"round.onconnect",
			"round.onplayerleave",
			"round.onroundend",
			"round.onroundrestart",
			"round.onroundstart",
			"round.onsetservername",
			"round.onwaitingforplayers",

			"environment.ondecontaminate",
			"environment.ondetonate",
			"environment.ongeneratorfinish",
			"environment.onscp914activate",
			"environment.onstartcountdown.initiated",
			"environment.onstartcountdown.noplayer",
			"environment.onstartcountdown.resumed",
			"environment.onstopcountdown.default",
			"environment.onstopcountdown.noplayer",
			"environment.onsummonvehicle.chaos",
			"environment.onsummonvehicle.mtf",

			"player.on079addexp",
			"player.on079camerateleport",
			"player.on079door.closed",
			"player.on079door.opened",
			"player.on079elevator.down",
			"player.on079elevator.up",
			"player.on079elevatorteleport",
			"player.on079levelup",
			"player.on079lock.locked",
			"player.on079lock.unlocked",
			"player.on079lockdown",
			"player.on079startspeaker",
			"player.on079stopspeaker",
			"player.on079teslagate",
			"player.on079unlockdoors",
			"player.on106createportal",
			"player.on106teleport",
			"player.onassignteam",
			"player.oncallcommand.console.player",
			"player.oncallcommand.console.server",
			"player.oncallcommand.game.player",
			"player.oncallcommand.game.server",
			"player.oncallcommand.remoteadmin.player",
			"player.oncallcommand.remoteadmin.server",
			"player.oncontain106",
			"player.ondooraccess.allowed",
			"player.ondooraccess.denied",
			"player.onelevatoruse",
			"player.onexecutedcommand.console.player",
			"player.onexecutedcommand.console.server",
			"player.onexecutedcommand.game.player",
			"player.onexecutedcommand.game.server",
			"player.onexecutedcommand.remoteadmin.player",
			"player.onexecutedcommand.remoteadmin.server",
			"player.ongeneratoractivated",
			"player.ongeneratorclose",
			"player.ongeneratordeactivated",
			"player.ongeneratoropen",
			"player.ongeneratorunlock",
			"player.ongrenadeexplosion",
			"player.ongrenadehitplayer",
			"player.onhandcuff.default",
			"player.onhandcuff.nootherplayer",
			"player.onintercom",
			"player.onlure",
			"player.onmedicaluse",
			"player.onnicknameset",
			"player.onplayerdie.default",
			"player.onplayerdie.friendlyfire",
			"player.onplayerdie.nokiller",
			"player.onplayerdropammo",
			"player.onplayerdropitem",
			"player.onplayerhurt.default",
			"player.onplayerhurt.friendlyfire",
			"player.onplayerhurt.noattacker",
			"player.onplayerinfected",
			"player.onplayerjoin",
			"player.onplayerpickupammo",
			"player.onplayerpickuparmor",
			"player.onplayerpickupitem",
			"player.onplayerpickupscp330",
			"player.onplayerradioswitch",
			"player.onplayertriggertesla.default",
			"player.onplayertriggertesla.ignored",
			"player.onpocketdimensiondie",
			"player.onpocketdimensionenter",
			"player.onpocketdimensionexit",
			"player.onrecallzombie",
			"player.onreload",
			"player.onscp914changeknob",
			"player.onsetrole",
			"player.onshoot.default",
			"player.onshoot.friendlyfire",
			"player.onshoot.notarget",
			"player.onspawn",
			"player.onspawnragdoll",
			"player.onthrowgrenade",

			"admin.onban.server",
			"admin.onkick.server",
			"admin.onban.player",
			"admin.onkick.player",

			"team.onteamrespawn.mtf",
			"team.onteamrespawn.ci",
			"team.onsetntfunitname",

			"botmessages.connectedtobot",
			"botmessages.reconnectedtobot",

			"botresponses.missingarguments",
			"botresponses.invalidsteamid",
			"botresponses.invalidduration",
			"botresponses.playerbanned",
			"botresponses.consolecommandfeedback",
			"botresponses.invalidsteamidorip",
			"botresponses.playerunbanned",
			"botresponses.playerkicked",
			"botresponses.playernotfound",
			"botresponses.exit",
			"botresponses.help",
			"botresponses.kickall",
			"botresponses.toggletag.notinstalled",
			"botresponses.vpnshield.notinstalled",
			"botresponses.scpermissions.notinstalled",
		};

		public static void Reload()
		{
			ready = false;
			plugin = SCPDiscord.plugin;
			languagesPath = Config.GetLanguageDir();

			if (!Directory.Exists(languagesPath))
			{
				Directory.CreateDirectory(languagesPath);
			}

			// Save default language files
			SaveDefaultLanguages();

			// Read primary language file
			plugin.Info("Loading primary language file...");
			try
			{
				LoadLanguageFile(Config.GetString("settings.language"), false);
			}
			catch (Exception e)
			{
				switch (e)
				{
					case DirectoryNotFoundException _:
						plugin.Error("Language directory not found.");
						break;
					case UnauthorizedAccessException _:
						plugin.Error("Primary language file access denied.");
						break;
					case FileNotFoundException _:
						plugin.Error("'" + languagesPath + Config.GetString("settings.language") + ".yml' was not found.");
						break;
					case JsonReaderException _:
					case YamlException _:
						plugin.Error("'" + languagesPath + Config.GetString("settings.language") + ".yml' formatting error.");
						break;
				}
				plugin.Error("Error reading primary language file '" + languagesPath + Config.GetString("settings.language") + ".yml'. Attempting to initialize backup system...");
				plugin.Debug(e.ToString());
			}

			// Read backup language file if not the same as the primary
			if (Config.GetString("settings.language") != "english")
			{
				plugin.Info("Loading backup language file...");
				try
				{
					LoadLanguageFile("english", true);
				}
				catch (Exception e)
				{
					switch (e)
					{
						case DirectoryNotFoundException _:
							plugin.Error("Language directory not found.");
							break;
						case UnauthorizedAccessException _:
							plugin.Error("Backup language file access denied.");
							break;
						case FileNotFoundException _:
							plugin.Error("'" + languagesPath + Config.GetString("settings.language") + ".yml' was not found.");
							break;
						case JsonReaderException _:
						case YamlException _:
							plugin.Error("'" + languagesPath + Config.GetString("settings.language") + ".yml' formatting error.");
							break;
					}
					plugin.Error("Error reading backup language file '" + languagesPath + "english.yml'.");
					plugin.Debug(e.ToString());
				}
			}
			if (primary == null && backup == null)
			{
				plugin.Error("NO LANGUAGE FILE LOADED! DEACTIVATING SCPDISCORD.");
				throw new Exception();
			}

			if (Config.GetBool("settings.configvalidation"))
			{
				ValidateLanguageStrings();
			}

			ready = true;
		}

		/// <summary>
		/// Saves all default language files included in the .dll
		/// </summary>
		private static void SaveDefaultLanguages()
		{
			foreach (KeyValuePair<string, string> language in defaultLanguages)
			{
				if (!File.Exists(languagesPath + language.Key + ".yml"))
				{
					plugin.Info("Creating language file " + languagesPath + language.Key + ".yml...");
					try
					{
						File.WriteAllText((languagesPath + language.Key + ".yml"), language.Value);
					}
					catch (DirectoryNotFoundException)
					{
						plugin.Warn("Could not create language file: Language directory does not exist, attempting to create it... ");
						Directory.CreateDirectory(languagesPath);
						plugin.Info("Creating language file " + languagesPath + language.Key + ".yml...");
						File.WriteAllText((languagesPath + language.Key + ".yml"), language.Value);
					}
				}
			}
		}

		/// <summary>
		/// This function makes me want to die too, don't worry.
		/// Parses a yaml file into a yaml object, parses the yaml object into a json string, parses the json string into a json object
		/// </summary>
		private static void LoadLanguageFile(string language, bool isBackup)
		{
			// Reads file contents into FileStream
			FileStream stream = File.OpenRead(languagesPath + language + ".yml");

			// Converts the FileStream into a YAML Dictionary object
			IDeserializer deserializer = new DeserializerBuilder().Build();
			object yamlObject = deserializer.Deserialize(new StreamReader(stream));

			// Converts the YAML Dictionary into JSON String
			ISerializer serializer = new SerializerBuilder()
				.JsonCompatible()
				.Build();
			string jsonString = serializer.Serialize(yamlObject);

			string identifier;
			if (isBackup)
			{
				identifier = "backup";
				backup = JObject.Parse(jsonString);
			}
			else
			{
				identifier = "primary";
				primary = JObject.Parse(jsonString);
			}
			plugin.Info("Successfully loaded " + identifier + " language file '" + language + ".yml'.");
		}

		public static void ValidateLanguageStrings()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\n||||||||||||| SCPDiscord language validator ||||||||||||||\n");
			bool valid = true;
			foreach (string node in messageNodes)
			{
				try
				{
					primary.SelectToken(node + ".message").Value<string>();
				}
				catch (Exception)
				{
					sb.Append("Your SCPDiscord language file \"" + Config.GetString("settings.language") + ".yml\" does not contain the node \"" + node + ".message\".\nEither add it to your language file or delete the file to generate a new one.\n");
					valid = false;
				}
			}

			if (valid)
			{
				sb.Append("No language errors.\n");
			}

			sb.Append("||||||||||||| End of language validation ||||||||||||||");
			plugin.Info(sb.ToString());
		}

		/// <summary>
		/// Gets a string from the primary or backup language file
		/// </summary>
		/// <param name="path">The path to the node</param>
		/// <returns></returns>
		public static string GetString(string path)
		{
			if (primary == null && backup == null)
			{
				plugin.Verbose("Tried to send Discord message before loading languages.");
				return null;
			}
			try
			{
				return primary?.SelectToken(path).Value<string>();
			}
			catch (Exception primaryException)
			{
				// This exception means the node does not exist in the language file, the plugin attempts to find it in the backup file
				if (primaryException is NullReferenceException || primaryException is ArgumentNullException || primaryException is InvalidCastException || primaryException is JsonException)
				{
					plugin.Warn("Error reading string '" + path + "' from primary language file, switching to backup...");
					try
					{
						return backup.SelectToken(path).Value<string>();
					}
					// The node also does not exist in the backup file
					catch (NullReferenceException e)
					{
						plugin.Error("Error: Language language string '" + path + "' does not exist. Message can not be sent." + e);
						return null;
					}
					catch (ArgumentNullException e)
					{
						plugin.Error("Error: Language language string '" + path + "' does not exist. Message can not be sent." + e);
						return null;
					}
					catch (InvalidCastException e)
					{
						plugin.Error(e.ToString());
						throw;
					}
					catch (JsonException e)
					{
						plugin.Error(e.ToString());
						throw;
					}
				}
				else
				{
					plugin.Error(primaryException.ToString());
					throw;
				}
			}
		}

		/// <summary>
		/// Gets a regex dictionary from the primary or backup language file.
		/// </summary>
		/// <param name="path">The path to the node.</param>
		/// <returns></returns>
		public static Dictionary<string, string> GetRegexDictionary(string path)
		{
			if (primary == null && backup == null)
			{
				plugin.Verbose("Tried to read regex dictionary before loading languages.");
				return new Dictionary<string, string>();
			}
			try
			{
				JArray jsonArray = primary?.SelectToken(path).Value<JArray>();
				return jsonArray?.ToDictionary(k => ((JObject)k).Properties().First().Name, v => v.Values().First().Value<string>());
			}
			catch (NullReferenceException e)
			{
				plugin.Verbose("Error: Language regex dictionary '" + path + "' does not exist." + e);
				return new Dictionary<string, string>();
			}
			catch (ArgumentNullException)
			{
				// Regex array is empty
				return new Dictionary<string, string>();
			}
			catch (InvalidCastException e)
			{
				plugin.Error(e.ToString());
				throw;
			}
			catch (JsonException e)
			{
				plugin.Error(e.ToString());
				throw;
			}
		}
	}
}