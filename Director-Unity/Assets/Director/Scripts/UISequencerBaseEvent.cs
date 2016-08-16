using UnityEngine;
using UnityEngine.UI;

namespace Tangzx.Director
{
    abstract class UISequencerBaseEvent : DirectorEvent, IRangeEvent, ISequencerEvent
    {
        public SequencerEventContainer container { get; set; }
        
        protected RectTransform rect;

        protected Graphic graphic;

        public override void Fire(bool isFristTime)
        {
            base.Fire(isFristTime);
            if (isFristTime)
            {
                rect = (RectTransform)container.attach.transform;
                graphic = rect.GetComponent<Graphic>();
            }
        }
    }
}
