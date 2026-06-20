using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using Reign.Generic;
using System;
using System.Threading.Tasks;

namespace Reign.Systems
{
    [Serializable]
    public struct DiscordSystemData
    {
        public long appID;

        public string name;
        public string details;
        public string state;
        
        public string largeImage;
        public string largeImageText;
        public string smallImage;
        public string smallImageText;
        
        public long startUnixTimestamp;
        public long endUnixTimestamp;

        public string partyID;
        public PartySize partySize;
        public ActivityPartyPrivacy activityPartyPrivacy;

        public ActivityType activityType;

        public ActivitySecrets activitySecrets;
    }

    public sealed class DiscordSystem : System<DiscordSystem>
    {
        private DiscordSystemData DefaultDiscordSystemData => Reign.CurrentGameCertificates.DEFAULT_DISCORD_RPC_DATA;

        public bool CanConnect { get; private set; } = false;
        public bool IsConnected { get; private set; } = false;
        private DiscordSystemData currentDiscordSystemSettings;
        public Discord.Discord Discord { get; private set; } = null;
        private ActivityManager manager;

        /// <summary>
        /// Set current Discord RPC data
        /// </summary>
        public void SetData(DiscordSystemData data)
        {
            if (!data.Equals(currentDiscordSystemSettings))
            {
                currentDiscordSystemSettings = data;
                UpdateStatus();
            }
        }

        /// <summary>
        /// Get current Discord RPC data
        /// </summary>
        public DiscordSystemData GetCurrent()
        {
            return currentDiscordSystemSettings;
        }

        /// <summary>
        /// Set current data's start timestamp to the current unix time 
        /// </summary>
        public void ResetTimestamp()
        {
            currentDiscordSystemSettings.startUnixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private async void Start()
        {
            while (Reign.CurrentGameCertificates == null)
            {
                await Task.Yield();
            }

            CanConnect = Reign.CurrentGameCertificates.DISCORD_ENABLED;

            if (DefaultDiscordSystemData.startUnixTimestamp == 0)
            {
                ResetTimestamp();
            }

            SetData(DefaultDiscordSystemData);

            AttemptConnection();
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void Update()
        {
            CanConnect = !IsConnected;
            AttemptCallbacks();
        }

        private void Disconnect()
        {
            Discord?.Dispose();
            Discord = null;
        }

        private void AttemptConnection()
        {
            if (!CanConnect)
            {
                Disconnect();
                return;
            }

            if (Discord == null)
            {
                try
                {
                    Discord = new Discord.Discord(currentDiscordSystemSettings.appID, (ulong)CreateFlags.NoRequireDiscord);
                    UpdateStatus();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Discord connection failed: {e.Message}");
                    Disconnect();
                }
            }
        }

        private void AttemptCallbacks()
        {
            if (Discord == null) return;

            try
            {
                Discord.RunCallbacks();
            }
            catch
            {
                Disconnect();
            }
        }

        private void UpdateStatus()
        {
            if (Discord == null) return;

            try
            {
                manager ??= Discord.GetActivityManager();

                Activity activity = new()
                {
                    ApplicationId = currentDiscordSystemSettings.appID,

                    Type = currentDiscordSystemSettings.activityType,

                    Details = currentDiscordSystemSettings.details,

                    State = currentDiscordSystemSettings.state,
                    
                    Assets =
                    {
                        LargeImage = currentDiscordSystemSettings.largeImage,
                        LargeText = currentDiscordSystemSettings.largeImageText,
                        SmallImage = currentDiscordSystemSettings.smallImage,
                        SmallText = currentDiscordSystemSettings.smallImageText,
                    },

                    Timestamps =
                    {
                        Start = currentDiscordSystemSettings.startUnixTimestamp,
                        End = currentDiscordSystemSettings.endUnixTimestamp
                    },

                    Party =
                    {
                        Id = currentDiscordSystemSettings.partyID,
                        Size = currentDiscordSystemSettings.partySize,
                        Privacy = currentDiscordSystemSettings.activityPartyPrivacy,
                    },

                    Secrets = currentDiscordSystemSettings.activitySecrets
                };

                manager.UpdateActivity(activity, result =>
                {
                    IsConnected = result == Result.Ok;
                });
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Discord RPC failed to update: {exception.Message}");
            }
        }
    }
}
