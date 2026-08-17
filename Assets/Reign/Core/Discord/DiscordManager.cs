#pragma warning disable 0162

using HouraiTeahouse.Discord;
using Reign.Configuration;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Reign.Core.Discord
{
    public static class DiscordManager
    {
        public static bool IsConnected { get; private set; } = false;

        private static HouraiTeahouse.Discord.Discord currentDiscord = null;

        private static DiscordStatusContainer currentDiscordStatus;

        private static ActivityManager currentActivityManager;

        private static Activity currentActivity;

        // ----- 

        private static long GetCurrentUnixTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public static async void SetupAsync(DiscordStatusContainer container)
        {
            if (!Config.Project.DISCORD_ENABLED)
            {
                return;
            }

            if (container.unixTimestampStart == 0)
            {
                container.unixTimestampStart = GetCurrentUnixTimestamp();
            }

            await SetStatus(container);
        }

        public static async Task SetStatus(DiscordStatusContainer container)
        {
            if (container == null)
            {
                return;
            }

            if (!container.Equals(currentDiscordStatus))
            {
                currentDiscordStatus = container;
            }

            if (currentDiscord == null)
            {
                AttemptCreation();
            }

            RefreshStatus();

            await Task.CompletedTask;
        }

        public static void Disconnect()
        {
            currentActivity = default;
            currentActivityManager = null;
            currentDiscordStatus = null;

            currentDiscord?.Dispose();
            currentDiscord = null;

            IsConnected = false;
        }

        private static void AttemptCreation()
        {
            if (currentDiscord != null)
            {
                return;
            }

            try
            {
                currentDiscord = new(currentDiscordStatus.appID, (ulong)CreateFlags.NoRequireDiscord);
                RefreshStatus();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create and connect to Discord: {e.Message}");
                Disconnect();
            }
        }

        public static bool AttemptCallbacks()
        {
            if (currentDiscord == null)
            {
                return false;
            }

            try
            {
                currentDiscord.RunCallbacks();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Discord callbacks failed: {e.Message}. Disconnecting.");
                Disconnect();
                return false;
            }
        }

        private static void RefreshStatus()
        {
            if (currentDiscord == null)
            {
                return;
            }

            try
            {
                currentActivityManager ??= currentDiscord.GetActivityManager();

                Activity activity = new()
                {
                    ApplicationId = currentDiscordStatus.appID,

                    Assets =
                    {
                        LargeImage = currentDiscordStatus.largeImageID,
                        LargeText = currentDiscordStatus.largeImageText,

                        SmallImage = currentDiscordStatus.smallImageID,
                        SmallText = currentDiscordStatus.smallImageText
                    },

                    Name = currentDiscordStatus.name,
                    State = currentDiscordStatus.state,
                    Details = currentDiscordStatus.details,
                    Type = currentDiscordStatus.activityType,

                    Timestamps =
                    {
                        Start = currentDiscordStatus.unixTimestampStart,
                        End = currentDiscordStatus.unixTimestampEnd
                    },

                    Party =
                    {
                        Id = currentDiscordStatus.partyID,
                        Size = currentDiscordStatus.partySize,
                        Privacy = currentDiscordStatus.partyPrivacy
                    },

                    Secrets = currentDiscordStatus.activitySecrets
                };

                if (!currentActivity.Equals(activity))
                {
                    currentActivity = activity;

                    currentActivityManager.UpdateActivity(activity, result =>
                    {
                        IsConnected = result == Result.Ok;
                    });
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Discord status could not update: {e.Message}");
            }
        }
    }
}
