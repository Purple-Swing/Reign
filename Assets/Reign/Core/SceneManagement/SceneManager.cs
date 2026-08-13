using Reign.API.Saving;
using Reign.Core.InstanceRequired;
using Reign.Core.Saving;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Reign.Core.SceneManagement
{
    public static class SceneManager
    {
        public static Scene ActiveScene
        {
            get
            {
                return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            }
        }

        public static async Task LoadSceneAsync(string sceneName, float overlayFadeDuration = 1.0f, Action<float> progress = null)
        {
            await SceneTransitioner.Current.Fade(SceneTransitioner.TransitionFadeDirection.In, overlayFadeDuration);

            try
            {
                AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            
                while (!operation.isDone)
                {
                    progress?.Invoke(operation.progress);
                    await Task.Yield();
                }
            }
            finally
            {
                await SceneTransitioner.Current.Fade(SceneTransitioner.TransitionFadeDirection.Out, overlayFadeDuration);
            }
        }
    }
}
