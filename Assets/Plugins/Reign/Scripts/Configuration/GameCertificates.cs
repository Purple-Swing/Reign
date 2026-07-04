using System.Collections.Generic;
using Discord;
using NaughtyAttributes;
using Reign.Systems;
using Reign.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Generic
{
    [CreateAssetMenu(fileName = "Game Certificates", menuName = "Reign/New Game Certificates")]
    public sealed class GameCertificates : ScriptableObject
    {
        [Foldout("Game Settings")] public string GAME_NAME = "My Game";
        [Foldout("Game Settings")] public string VERSION = "1.0";
        [Foldout("Game Settings")] public List<string> AUTHORS = new();
        [Foldout("Game Settings")] public bool IS_DEBUG = true;
        [Foldout("Game Settings")] public bool ALLOW_HOT_RELOAD = false;

        [Foldout("Save System")] public bool SAVE_SYSTEM_ENABLED = true;
        [Foldout("Save System")] public bool SAVE_ENCRYPT = true;
        [Foldout("Save System")] public bool SAVE_ON_QUIT = true;
        [Foldout("Save System")] public string SAVE_FILE_DIRECTORY = "save.REIGN";
        [Foldout("Save System")] public string SAVE_PASSWORD = "save_password";
        [Foldout("Save System")] public string SAVE_SALT = "save_salt";
        [Foldout("Save System"), Range(16, 4096)] public int SAVE_ITERATIONS = 2048;

        [Foldout("Discord")] public bool DISCORD_ENABLED = true;

        [Foldout("Discord")] public DiscordSystemData DEFAULT_DISCORD_RPC_DATA = new()
        {
            appID = 1453862071543271508,
            details = "",
            state = "",

            largeImage = "",
            largeImageText = "",
            smallImage = "",
            smallImageText = "",
            
            startUnixTimestamp = 0,
            endUnixTimestamp = 0,

            partyID = "",
            partySize = {CurrentSize = 0, MaxSize = 0},
            activityPartyPrivacy = ActivityPartyPrivacy.Private,

            activityType = ActivityType.Playing,
            activitySecrets = { Match = "", Join = "", Spectate = "" }            
        };

        [Button("Set Random Save Password and Salt")]
        public void SetRandomSaveKeys()
        {
            SAVE_PASSWORD = ReignUtility.GenerateKey();
            SAVE_SALT = ReignUtility.GenerateKey();
        }
    }
}
