using UnityEngine;

namespace Tangzx.Director
{
    public class Director : MonoBehaviour
    {
        public DirectorData data;

        private bool _isPlaying;
        private float _playTime;

        public void Play()
        {
            _isPlaying = true;
        }

        public void Pause()
        {
            _isPlaying = false;
        }

        public void Stop()
        {
            _isPlaying = false;
            _playTime = 0;
        }

        void Update()
        {
            if (_isPlaying)
            {
                _playTime += Time.deltaTime;
                data.Process(_playTime);
            }
        }
    }
}