using Reign.Core.Discord;
using UnityEngine;

namespace Reign.Configuration
{
    public static partial class Config
    {
        public static class Project
        {
            // [ABOUT]
            public const string VERSION = "1.0.0";
            public const string NAME = "My Project";
            public const string COMPANY = "Default Company";

            // [SAVING]
            public const bool ENCRYPTED_SAVES = true;
            public const string SAVE_FILE_NAME = ".save";
            public const string SAVE_FILE_PASSWORD = "save_pass";
            public const string SAVE_FILE_SALT = "save_salt";
            public const ushort SAVE_ENCRYPTION_ITERATIONS = 2048;

            // [DISCORD]
            public const bool DISCORD_ENABLED = true;
        }
    }
}
