using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class Director : MonoBehaviour
    {
        public DirectorData data;

        private bool _isPlaying;
        private float _playTime;
        /// <summary>
        /// 当前播放头时间
        /// </summary>
        private float _currentTime;
        /// <summary>
        /// 正在播放的
        /// </summary>
        private List<TDEvent> _playingList = new List<TDEvent>();

        private List<TDEvent> _eventList;

        public void Play()
        {
            if (_isPlaying == false)
            {
                data.Sort();
                _eventList = data.eventList;
                _isPlaying = true;
            }
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
                Process(_playTime);
            }
        }

        public void Process(float time)
        {
            Tick(time - _currentTime);
        }

        public void Tick(float dt)
        {
            float newTime = _currentTime + dt;
            float oldTime = _currentTime;
            //正播
            if (dt > 0)
            {
                //进入 playing list
                for (int i = 0; i < _eventList.Count; i++)
                {
                    TDEvent p = _eventList[i];
                    if (p.time >= oldTime && p.time <= newTime)
                    {
                        p.Fire();
                        _playingList.Add(p);
                    }
                }

                //处理 playing list
                for (int i = 0; i < _playingList.Count; i++)
                {
                    TDEvent p = _playingList[i];
                    if (p.isRangeEvent)
                    {
                        float endTime = p.time + p.duration;
                        endTime = endTime < newTime ? endTime : newTime;
                        p.Process(endTime - p.time);
                    }
                    // exit
                    if (p.time + p.duration <= newTime)
                    {
                        p.End();
                        _playingList.RemoveAt(i);
                        i--;
                    }
                }
            }
            else //反播
            {

            }
            _currentTime = newTime;
        }
    }
}