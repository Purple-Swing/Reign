using Reign.Configuration;
using Reign.Core.Discord;
using UnityEngine;

namespace Reign.Core.InstanceRequired
{
    public class DiscordUpdater : MonoBehaviour
    {
        private void Awake()
        {
            if (!Config.Project.DISCORD_ENABLED)
            {
                Destroy(this);
                return;
            }
        }

        private void Update()
        {
            DiscordManager.AttemptCallbacks();
        }

        private void OnDestroy()
        {
            DiscordManager.Disconnect();
        }
    }
}
