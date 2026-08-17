using System.Collections.Generic;
using Reign.Core.Generic;
using UnityEngine;

namespace Reign.Configuration
{
    public static partial class Config
    {
        public static class Reign
        {
            // Reign uses predefined variables in the Config.Reign class so that distinguishing 
            // differences is much easier.

            public const string VERSION = "2.0.0";

            public static readonly Dictionary<string, string> CONTRIBUTORS = new()
            {
                // Contributor | Social link
                {"Tureen", "https://github.com/Tureen64"}
            };
        }
    }
}
