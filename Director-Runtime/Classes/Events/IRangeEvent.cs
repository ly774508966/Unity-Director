using UnityEngine;
using UnityEngine.Serialization;

namespace Tangzx.Director
{
    /// <summary>
    /// 有长度的
    /// </summary>
    public interface IRangeEvent
    {
        void Process(float time);
    }
}
