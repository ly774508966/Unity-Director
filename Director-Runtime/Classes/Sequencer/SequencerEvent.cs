using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEvent : TDEvent
    {
        public SequencerEvent()
        {
        }

        Transform attach { set; get; }
    }
}

