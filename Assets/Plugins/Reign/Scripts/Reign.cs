using Reign.Systems;
using Reign.Generic;
using Reign.Generic.Shared;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Reign
{
    [DefaultExecutionOrder(-100)]
    public sealed class Reign : Singleton<Reign>
    {
        [SerializeField] private GameCertificates gameCertifciates;
        public static GameCertificates CurrentGameCertificates { get; private set; }

        private void Awake()
        {
            CurrentGameCertificates = gameCertifciates;
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
    }
}
