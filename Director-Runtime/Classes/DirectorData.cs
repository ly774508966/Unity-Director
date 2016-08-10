using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{

    /// <summary>
    /// 
    /// </summary>
    public class DirectorData : ScriptableObject
    {
        public List<Playable> playableList = new List<Playable>();
        /// <summary>
        /// 正在播放的
        /// </summary>
        private List<Playable> _playingList = new List<Playable>();

        /// <summary>
        /// 当前播放头时间
        /// </summary>
        private float _currentTime;

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
                for (int i = 0; i < playableList.Count; i++)
                {
                    Playable p = playableList[i];
                    if (p.time > oldTime && p.time <= newTime)
                    {
                        p.Fire();
                        _playingList.Add(p);
                    }
                }

                //处理 playing list
                for (int i = 0; i < _playingList.Count; i++)
                {
                    Playable p = _playingList[i];
                    if (p is RangePlayable)
                    {
                        RangePlayable rp = (RangePlayable)p;
                        float endTime = p.time + p.duration;
                        endTime = endTime < newTime ? endTime : newTime;

                        rp.Process(endTime);
                    }
                    // exit
                    if (p.time + p.duration > 0)
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

        public void Add(Playable playable)
        {
            playableList.Add(playable);
            playableList.Sort();
        }
    }
}