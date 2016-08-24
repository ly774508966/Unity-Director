using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public abstract class DirectorPlayer : MonoBehaviour
    {
        private bool _isPlaying;
        private bool _isPause;

        /// <summary>
        /// 当前播放头时间
        /// </summary>
        private float _playTime;
        private float _sampleTime;
        /// <summary>
        /// 正在播放的
        /// </summary>
        private List<DirectorEvent> _playingList = new List<DirectorEvent>();

        private IEventContainer[] _eventContainers;

        private float _totalTime;

        private float _timeScale = 1;

        public virtual void ReadyToPlay()
        {
            
        }

        protected void BeginPlay(IEventContainer[] containers, float totalTime)
        {
            if (_isPlaying)
                Stop();

            OnPlayBegin();
            
            _totalTime = totalTime;
            _eventContainers = containers;
        }

        public void Play()
        {
            _isPlaying = true;
        }

        public bool isPlaying { get { return _isPlaying; } }

        public bool isPause { get { return _isPause; } }

        public float totalTime { get { return _totalTime; } }

        public float timeScale
        {
            get { return _timeScale; }
            set { _timeScale = value; }
        }

        public float playTime
        {
            get { return _playTime; }
            set
            {
                if (value < 0) value = 0;
                else if (value > _totalTime) value = _totalTime;

                if (_playTime != value)
                {
                    _playTime = value;
                    Sample();
                }
            }
        }

        public float normalizeTime
        {
            get
            {
                if (_totalTime == 0) return 0;

                return _playTime / _totalTime;
            }
            set
            {
                playTime = value * _totalTime;
            }
        }

        public void Pause()
        {
            _isPause = true;
        }

        public virtual void Stop()
        {
            _playTime = 0;
            _sampleTime = 0;
            _isPause = false;
            _isPlaying = false;
            _eventContainers = null;
            _playingList.Clear();
        }

        void Update()
        {
            if (_isPlaying && !_isPause)
            {
                playTime += Time.deltaTime * timeScale;
            }
        }

        public void Sample()
        {
            Tick(_playTime - _sampleTime, true);
        }

        protected void Tick(float dt, bool fireCompleteEvent)
        {
            if (_eventContainers == null || _eventContainers.Length == 0)
                return;

            float newTime = _sampleTime + dt;
            float oldTime = _sampleTime;

            _sampleTime = newTime;

            //正播
            if (dt >= 0)
            {
                PlayForward(newTime, oldTime, fireCompleteEvent);
            }
            else //反播
            {
                PlayBack(newTime, oldTime, fireCompleteEvent);
            }
        }

        private void PlayForward(float newTime, float oldTime, bool fireCompleteEvent)
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
                DirectorEvent evt = _playingList[i];
                if (evt.isRangeEvent)
                {
                    float endTime = evt.time + evt.duration;
                    endTime = endTime < newTime ? endTime : newTime;
                    evt.Process(endTime - evt.time, false);
                }
                // exit
                if (evt.time + evt.duration <= newTime)
                {
                    evt.End();
                    _playingList.RemoveAt(i);
                    i--;
                }
            }

            //结束
            if (newTime >= _totalTime && fireCompleteEvent)
                OnPlayFinish();
        }
        
        private void PlayBack(float newTime, float oldTime, bool fireCompleteEvent)
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
                if (p.time > newTime)
                {
                    p.EndReverse();
                    _playingList.RemoveAt(i);
                    i--;
                }
            }

            //结束
            if (newTime <= 0 && fireCompleteEvent)
                OnPlayFinish();
        }

        protected virtual void OnPlayFinish()
        {
            _isPlaying = false;
            _isPause = false;
            _playingList.Clear();
        }

        protected virtual void OnPlayBegin()
        {

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