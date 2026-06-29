using System.Threading.Tasks;
using NaughtyAttributes;
using Reign.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reign.Essentials
{
    [RequireComponent(typeof(VideoListener))]
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField, Scene] private string sceneDirectory;
        private VideoListener listener;

        private void Awake()
        {
            listener = GetComponent<VideoListener>();
        }

        private async void Start()
        {
            await Splash();
        }

        private async Task Splash()
        {
            await listener.AwaitStopPlaying();
            await SceneLoadSystem.Instance.LoadSceneAsync(sceneDirectory);
        }
    }
}