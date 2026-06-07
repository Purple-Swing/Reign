using Reign.Systems;
using Reign.Generic;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Reign
{
    [DefaultExecutionOrder(-100)]
    public sealed class Reign : Singleton<Reign>
    {
        [SerializeField] private List<SystemBase> requiredSystems;
        [SerializeField] private bool log;
        [SerializeField] private GameCertificates gameCertifciates;
        public static GameCertificates CurrentGameCertificates { get; private set; }

        private void Awake()
        {
            CurrentGameCertificates = gameCertifciates;

            if (!log) return;

            if (HasRequiredSystems())
            {
                Debug.Log($"<color=#008ec2ff><b>Initialised Reign v{ReignServiceDetails.REIGN_VERSION}</b></color>");
            }
            else
            {
                Debug.LogError($"Reign encountered errors when starting.");
            }
        }

        /// <summary>
        /// Close the game window both in editor and in standalone circumstances
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Does the script have any required systems? If so, are they present?
        /// </summary>
        public bool HasRequiredSystems()
        {
            bool success = !requiredSystems.Any() || requiredSystems.All(system => system != null);

            if (success && log) Debug.Log("<color=#008ec2ff><b>Reign</b></color> found all required systems!");
            return success;
        }
    }
}
