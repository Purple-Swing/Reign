using System.Collections.Generic;
using UnityEngine;

namespace Reign.Configuration
{
    public static partial class Config
    {
        public static class Reign
        {
            // Reign uses a predefined version variable in the Config.Reign class so that distinguishing 
            // different versions is much easier.

            public const string VERSION = "2.0.0";

            public static readonly Dictionary<string, string> CONTRIBUTORS = new()
            {
                {"Tureen64", "https://github.com/Tureen64"}
            };
        }
    }
}
