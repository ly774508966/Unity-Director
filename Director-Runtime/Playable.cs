using System;
using UnityEngine;

namespace Tangzx.Director
{
    /// <summary>
    /// 可播放的基类
    /// </summary>
    public abstract class Playable : ScriptableObject, IComparable<Playable>
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        public float time;

        /// <summary>
        /// 持续时长
        /// </summary>
        public float duration { get { return 0; } }

        public virtual void Fire()
        {

        }

        public virtual void End()
        {

        }

        public int CompareTo(Playable other)
        {
            return time.CompareTo(other.time);
        }
    }
}
