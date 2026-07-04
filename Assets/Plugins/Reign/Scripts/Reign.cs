using Reign.Systems;
using Reign.Generic;
using Reign.Generic.Shared;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using Reign.Events;

namespace Reign
{
    public struct OnReignInitialisedEvent : IEvent 
    {
        public GameCertificates currentGameCertificates;
    }

    [DefaultExecutionOrder(-100)]
    public sealed class Reign : Singleton<Reign>
    {
        [SerializeField, Expandable] private GameCertificates gameCertifciates;
        public static GameCertificates CurrentGameCertificates { get; private set; }

        private void Awake()
        {
            CurrentGameCertificates = gameCertifciates;

            _ = EventBus.Publish(new OnReignInitialisedEvent { currentGameCertificates = CurrentGameCertificates });
        }

        private void Update()
        {
            if (gameCertifciates.ALLOW_HOT_RELOAD && InputSystem.Instance.GetButton("HotReload"))
            {
                ReloadCurrentScene();
            }
        }

        /// <summary>
        /// Reload current active scene from scene manager
        /// </summary>
        public async void ReloadCurrentScene()
        {
            await SceneLoadSystem.Instance.LoadSceneAsync(SceneLoadSystem.Instance.CurrentScene());
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
