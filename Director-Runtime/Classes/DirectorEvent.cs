using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tangzx.Director
{
    /// <summary>
    /// 可播放的基类
    /// </summary>
    public abstract class DirectorEvent : DirectorObject, IComparable<DirectorEvent>
    {
        /// <summary>
        /// 起始时间
        /// </summary>
        public float time;

        [FormerlySerializedAs("duration")]
        [SerializeField]
        private float _duration = 1;

        public float duration
        {
            set { _duration = value; }
            get
            {
                if (this is IRangeEvent)
                    return _duration;
                else
                    return 0;
            }
        }

        public bool isRangeEvent { get { return this is IRangeEvent; } }

        public virtual void Fire()
        {

        }

        public virtual void Process(float time)
        {
            
        }

        public virtual void End()
        {

        }

        public int CompareTo(DirectorEvent other)
        {
            return time.CompareTo(other.time);
        }
    }
}
