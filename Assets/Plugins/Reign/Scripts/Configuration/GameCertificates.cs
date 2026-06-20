using System.Collections.Generic;
using Discord;
using Reign.Systems;
using UnityEngine;

namespace Reign.Generic
{
    [CreateAssetMenu(fileName = "Game Certificates", menuName = "Reign/New Game Certificates")]
    public sealed class GameCertificates : ScriptableObject
    {
        [Header("Game Settings")]
        public string GAME_NAME = "My Game";
        public string VERSION = "1.0";
        public List<string> AUTHORS = new();
        public bool IS_DEBUG = true;

        [Header("Save System")]
        public bool SAVE_SYSTEM_ENABLED = true;
        public bool SAVE_ENCRYPT = true;
        public bool SAVE_ON_QUIT = true;
        public string SAVE_FILE_DIRECTORY = "save.REIGN";
        public string SAVE_PASSWORD = "save_password";
        public string SAVE_SALT = "save_salt";
        public int SAVE_ITERATIONS = 2048;

        [Header("Discord")]
        public bool DISCORD_ENABLED = true;

        public DiscordSystemData DEFAULT_DISCORD_RPC_DATA = new()
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
    }
}
