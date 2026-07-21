using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Reign.Essentials
{
    [RequireComponent(typeof(VideoPlayer))]
    public class VideoListener : MonoBehaviour
    {
        VideoPlayer player;

        private void Awake()
        {
            player = GetComponent<VideoPlayer>();
        }

        public async Task AwaitStartPlaying()
        {
            while (!player.isPlaying)
            {
                await Task.Yield();
            }
        }

        public Task AwaitStopPlaying()
        {
            if (player.isLooping) throw new TaskCanceledException("Cannot await end of playing if the video player is looping.");
            
            var tcs = new TaskCompletionSource<bool>();

            player.loopPointReached += _ => tcs.SetResult(true);
            return tcs.Task;
        }
    }
}
