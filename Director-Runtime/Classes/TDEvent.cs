using System;
using UnityEngine;

namespace Tangzx.Director
{
    /// <summary>
    /// 可播放的基类
    /// </summary>
    public abstract class TDEvent : ScriptableObject, IComparable<TDEvent>
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        public float time;

        /// <summary>
        /// 持续时长
        /// </summary>
        public virtual float duration { get { return 0; } set { } }

        public virtual void Fire()
        {

        }

        public virtual void End()
        {

        }

        public int CompareTo(TDEvent other)
        {
            return time.CompareTo(other.time);
        }
    }
}
