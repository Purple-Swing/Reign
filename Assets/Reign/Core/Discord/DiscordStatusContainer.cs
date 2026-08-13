using HouraiTeahouse.Discord;
using System;
using UnityEngine;

namespace Reign.Core.Discord
{
    [Serializable]
    public class DiscordStatusContainer
    {
        // App
        [Header("App")]
        public long appID;

        // Activity
        [Header("Activity")]
        public ActivityType activityType;
        public ActivitySecrets activitySecrets;

        // About
        [Header("About")]
        public string name;
        public string details;
        public string state;

        // Images
        [Header("Images")]
        public string largeImageID;
        public string largeImageText;
        public string smallImageID;
        public string smallImageText;

        // Party
        [Header("Party")]
        public string partyID;
        public PartySize partySize;
        public ActivityPartyPrivacy partyPrivacy;

        // Timestamps
        [Header("Timestamps")]
        public long unixTimestampStart = 0;
        public long unixTimestampEnd = 0;
    }
}
