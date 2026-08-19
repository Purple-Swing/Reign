using HouraiTeahouse.Discord;
using System;
using UnityEngine;

namespace Reign.Core.Discord
{
    [Serializable]
    public class DiscordStatusContainer
    {
        [Header("App")]
        public long appID;

        [Header("Activity")]
        public ActivityType activityType;
        public ActivitySecrets activitySecrets;

        [Header("About")]
        public string name;
        public string details;
        public string state;

        [Header("Images")]
        public string largeImageID;
        public string largeImageText;
        public string smallImageID;
        public string smallImageText;

        [Header("Party")]
        public string partyID;
        public PartySize partySize;
        public ActivityPartyPrivacy partyPrivacy;

        [Header("Timestamps")]
        public long unixTimestampStart = 0;
        public long unixTimestampEnd = 0;
    }
}
