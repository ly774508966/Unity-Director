using UnityEngine;

namespace Tangzx.Director
{
    [DirectorPlayable("GameObject/SetActiveEvent")]
    class SetActiveEvent : DirectorEvent, ISequencerEvent
    {
        public GameObject target;
        public bool enable;

        bool originEnable;

        public SequencerEventContainer container { get; set; }

        public override void Fire(bool isFristTime)
        {
            base.Fire(isFristTime);
            if (isFristTime)
            {
                if (target == null)
                    target = container.attach.gameObject;
                originEnable = target.activeInHierarchy;
            }
            target.SetActive(enable);
        }

        public override void EndReverse()
        {
            base.EndReverse();
            target.SetActive(originEnable);
        }

        public override void StopAndRecover()
        {
            base.StopAndRecover();
            target.SetActive(originEnable);
        }
    }
}
