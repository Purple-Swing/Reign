using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Reign.Systems
{
    public sealed class SceneLoadSystem : System<SceneLoadSystem>
    {
        [SerializeField] Image loadOverlay;

        private void Start()
        {
            loadOverlay.color = new Color(0, 0, 0, 0);
        }

        /// <summary>
        /// Transition the load overlay image in or out
        /// </summary>
        private async Task TransitionAsync(bool isIn, float speed = 1.0f)
        {
            float target = isIn ? 1f : 0f;

            Color color = loadOverlay.color;

            while (!Mathf.Approximately(color.a, target))
            {
                await Task.Yield();

                color = loadOverlay.color;

                float newAlpha = Mathf.MoveTowards(color.a, target, Time.deltaTime * speed);

                loadOverlay.color = new Color(color.r, color.g, color.b, newAlpha);
            }
        }

        /// <summary>
        /// Load scene by name asynchronously
        /// </summary>
        private Task<bool> LoadSceneAsync(string name, LoadSceneMode mode)
        {
            var completionSource = new TaskCompletionSource<bool>();

            SceneManager.LoadSceneAsync(name, mode).completed += _ =>
            {
                completionSource.SetResult(true);
            };

            return completionSource.Task;
        }

        /// <summary>
        /// Transition and await asynchronous scene load
        /// </summary>
        public async Task LoadSceneAsync(string name, float transitionSpeed = 1.0f, LoadSceneMode mode = LoadSceneMode.Single)
        {
            // Fade in
            await TransitionAsync(true, transitionSpeed);

            // Await load
            await LoadSceneAsync(name, mode);

            // Fade out
            await TransitionAsync(false, transitionSpeed);
        }

        public string CurrentScene()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
