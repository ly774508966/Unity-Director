using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class DirectorPlayer : MonoBehaviour
    {
        public delegate void OnPlayCompleteHandler();

        public OnPlayCompleteHandler onPlayComplete;

        private bool _isPlaying;

        /// <summary>
        /// 当前播放头时间
        /// </summary>
        private float _playTime;
        /// <summary>
        /// 正在播放的
        /// </summary>
        private List<DirectorEvent> _playingList = new List<DirectorEvent>();

        private IEventContainer[] _eventContainers;
        private float _totalTime;

        public virtual void ReadyToPlay()
        {
            
        }

        public void Play(IEventContainer[] containers, float totalTime)
        {
            if (_isPlaying == false)
            {
                _totalTime = totalTime;
                _playingList.Clear();
                _eventContainers = containers;
                _isPlaying = true;

                for (int c = 0; c < _eventContainers.Length; c++)
                {
                    IEventContainer ec = _eventContainers[c];
                    var e = ec.GetEnumerator();
                    while (e.MoveNext())
                        e.Current.isFried = false;
                }
            }
        }

        public bool isPlaying { get { return _isPlaying; } }

        public void Pause()
        {
            _isPlaying = false;
        }

        public void Stop()
        {
            _playTime = 0;
            _isPlaying = false;
            _playingList.Clear();
        }

        void Update()
        {
            if (_isPlaying)
            {
                Tick(Time.deltaTime);
            }
        }

        public void Process(float time)
        {
            Tick(time - _playTime);
        }

        public float playTime
        {
            get { return _playTime; }
        }

        public void Tick(float dt)
        {
            if (_eventContainers == null || _eventContainers.Length == 0 || dt == 0)
                return;

            float newTime = _playTime + dt;
            float oldTime = _playTime;

            _playTime = newTime;

            //正播
            if (dt > 0)
            {
                PlayForward(dt, newTime, oldTime);
            }
            else //反播
            {
                PlayBack(dt, newTime, oldTime);
            }
        }

        private void PlayForward(float dt, float newTime, float oldTime)
        {
            if (newTime > _totalTime)
                newTime = _totalTime;

            for (int c = 0; c < _eventContainers.Length; c++)
            {
                IEventContainer ec = _eventContainers[c];
                //进入 playing list
                var e = ec.GetEnumerator();
                while (e.MoveNext())
                {
                    DirectorEvent p = e.Current;
                    if (p.time >= oldTime && p.time <= newTime)
                    {
                        p.Fire(!p.isFried);
                        p.isFried = true;
                        _playingList.Add(p);
                    }
                }
            }

            //处理 playing list
            for (int i = 0; i < _playingList.Count; i++)
            {
                DirectorEvent p = _playingList[i];
                if (p.isRangeEvent)
                {
                    float endTime = p.time + p.duration;
                    endTime = endTime < newTime ? endTime : newTime;
                    p.Process(endTime - p.time, false);
                }
                // exit
                if (p.time + p.duration <= newTime)
                {
                    p.End();
                    _playingList.RemoveAt(i);
                    i--;
                }
            }

            //结束
            if (newTime >= _totalTime)
            {
                OnPlayForwardComplete();
            }
        }
        
        private void PlayBack(float dt, float newTime, float oldTime)
        {
            if (newTime < 0)
                newTime = 0;

            for (int c = 0; c < _eventContainers.Length; c++)
            {
                IEventContainer ec = _eventContainers[c];
                //进入 playing list
                var e = ec.GetEnumerator();
                while (e.MoveNext())
                {
                    DirectorEvent p = e.Current;
                    float endTime = p.time + p.duration;
                    if (endTime <= oldTime && endTime >= newTime)
                    {
                        p.FireReverse();
                        _playingList.Add(p);
                    }
                }
            }

            //处理 playing list
            for (int i = 0; i < _playingList.Count; i++)
            {
                DirectorEvent p = _playingList[i];
                if (p.isRangeEvent)
                {
                    float endTime = p.time + p.duration;
                    endTime = endTime < newTime ? endTime : newTime;
                    p.Process(endTime - p.time, true);
                }
                // exit
                if (p.time >= newTime)
                {
                    p.EndReverse();
                    _playingList.RemoveAt(i);
                    i--;
                }
            }

            //结束
            if (newTime >= _totalTime)
            {
                OnPlayForwardComplete();
            }

            if (newTime <= 0)
                OnPlayForwardComplete();
        }

        protected virtual void OnPlayForwardComplete()
        {
            _isPlaying = false;
            _playingList.Clear();

            if (onPlayComplete != null)
                onPlayComplete();
        }

        public void StopAndRecover()
        {
            if (_eventContainers != null)
            {
                for (int c = 0; c < _eventContainers.Length; c++)
                {
                    IEventContainer ec = _eventContainers[c];
                    for (int i = ec.list.Count - 1; i >= 0; i--)
                    {
                        DirectorEvent p = ec.list[i];
                        if (p.isFried)
                        {
                            p.isFried = false;
                            p.StopAndRecover();
                        }
                    }
                }
            }
            Stop();
        }
    }
}