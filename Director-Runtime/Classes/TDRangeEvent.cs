using UnityEngine;
using UnityEngine.Serialization;

namespace Tangzx.Director
{
    /// <summary>
    /// 有长度的
    /// </summary>
    public abstract class TDRangeEvent : TDEvent
    {
        [FormerlySerializedAs("duration")]
        [SerializeField]
        private float _duration = 1;

        public override float duration
        {
            set { _duration = value; }
            get { return _duration; }
        }

        public virtual void Process(float runningTime)
        {

        }
    }
}
