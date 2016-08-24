using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class DirectorPlayer
    {
        public enum LoopType
        {
            Restart = 0,
            Yoyo = 1
        }

        private bool _isPlaying;
        private bool _isPause;
        private bool _isPlayForward;

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

        private float _speed = 1;

        private int _loops;

        private LoopType _loopType;

        private int _curLoop;

        protected void BeginPlay(IEventContainer[] containers, float totalTime)
        {
            if (_isPlaying)
                Stop();
            
            _totalTime = totalTime;
            _eventContainers = containers;
            _loops = 1;
            _loopType = LoopType.Yoyo;
            _curLoop = 0;
        }

        public void PlayForward()
        {
            playTime = 0;
            _isPlayForward = true;
            _isPlaying = true;
            _timeScale = 1;
        }

        public void PlayReverse()
        {
            playTime = _totalTime;
            _isPlayForward = false;
            _isPlaying = true;
            _timeScale = -1;
        }

        public bool isPlaying { get { return _isPlaying; } }

        public bool isPause { get { return _isPause; } }

        public float totalTime { get { return _totalTime; } }

        public float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public float playTime
        {
            get { return _playTime; }
            set
            {
                bool isReverseFinish = false;
                bool isForwardFinish = false;
                if (value < 0)
                {
                    value = 0;
                    isReverseFinish = _playTime > 0;
                }
                else if (value > _totalTime)
                {
                    value = _totalTime;
                    isForwardFinish = _playTime < _totalTime;
                }
                 
                if (_playTime != value)
                {
                    _playTime = value;
                    Sample();
                }

                if (isReverseFinish)
                    OnReverseFinish();
                else if (isForwardFinish)
                    OnForwardFinish();
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

        public void Update(float dt)
        {
            if (_isPlaying && !_isPause)
            {
                playTime += dt * _timeScale * _speed;
            }
        }

        public void SetLoops(int loop, LoopType type)
        {
            _loops = loop;
            _loopType = type;
        }

        public void Sample()
        {
            Tick(_playTime - _sampleTime);
        }

        protected void Tick(float dt)
        {
            if (_eventContainers == null || _eventContainers.Length == 0)
                return;

            float newTime = _sampleTime + dt;
            float oldTime = _sampleTime;

            _sampleTime = newTime;

            //正播
            if (dt >= 0)
            {
                Forward(newTime, oldTime);
            }
            else //反播
            {
                Reverse(newTime, oldTime);
            }
        }

        private void Forward(float newTime, float oldTime)
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
        }
        
        private void Reverse(float newTime, float oldTime)
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
        }

        void OnForwardFinish()
        {
            _curLoop++;
            if (_curLoop < _loops)
            {
                if (_loopType == LoopType.Restart)
                {
                    _playTime = 0;
                }
                else
                {
                    _timeScale *= -1;
                }
            }
            else
            {
                OnFinish(_isPlayForward);
            }
        }

        void OnReverseFinish()
        {
            _curLoop++;
            if (_curLoop < _loops)
            {
                if (_loopType == LoopType.Restart)
                {
                    _playTime = _totalTime;
                }
                else
                {
                    _timeScale *= -1;
                }
            }
            else
            {
                OnFinish(_isPlayForward);
            }
        }

        protected virtual void OnFinish(bool isForward)
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