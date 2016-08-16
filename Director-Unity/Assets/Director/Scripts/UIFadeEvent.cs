using UnityEngine;

namespace Tangzx.Director
{
    [DirectorPlayable("UI/UIFadeEvent")]
    class UIFadeEvent : UISequencerBaseEvent
    {
        public CanvasGroup group;

        public float alpha;

        public bool from;

        protected float originAlpah = 1;
        private bool isGroupCreateByEvent;

        public override void Fire(bool isFristTime)
        {
            base.Fire(isFristTime);
            if (isFristTime)
            {
                isGroupCreateByEvent = false;
                if (group == null)
                    group = rect.gameObject.GetComponent<CanvasGroup>();
                if (group == null)
                {
                    isGroupCreateByEvent = true;
                    group = rect.gameObject.AddComponent<CanvasGroup>();
                }
                originAlpah = group.alpha;
            }
        }

        public override void StopAndRecover()
        {
            base.StopAndRecover();
            if (group)
            {
                if (isGroupCreateByEvent)
                    DestroyImmediate(group, true);
                else
                    SetAlpha(originAlpah);
            }
        }

        protected void SetAlpha(float alpha)
        {
            group.alpha = alpha;
        }

        public override void Process(float time, bool isReverse = false)
        {
            base.Process(time, isReverse);
            if (from)
            {
                SetAlpha(Mathf.Lerp(alpha, originAlpah, time / duration));
            }
            else
            {
                SetAlpha(Mathf.Lerp(originAlpah, alpha, time / duration));
            }
        }
    }
}
