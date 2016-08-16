using UnityEngine;

namespace Tangzx.Director
{
    [DirectorPlayable("UI/UIMoveEvent")]
    class UIMoveEvent : UISequencerBaseEvent
    {
        public Vector2 position;

        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public bool isRelative;

        public bool from;

        protected Vector2 originAnchored;

        public override void Fire(bool isFristTime)
        {
            base.Fire(isFristTime);
            if (isFristTime)
                originAnchored = rect.anchoredPosition;
        }

        public override void StopAndRecover()
        {
            base.StopAndRecover();
            rect.anchoredPosition = originAnchored;
        }

        public override void Process(float time, bool isReverse = false)
        {
            base.Process(time, isReverse);

            float p = curve.Evaluate(time / duration);

            if (from)
            {
                Vector2 fromVec = position;
                if (isRelative) fromVec += originAnchored;
                rect.anchoredPosition = Vector2.Lerp(fromVec, originAnchored, p);
            }
            else
            {
                Vector2 toVec = position;
                if (isRelative) toVec += originAnchored;
                rect.anchoredPosition = Vector2.Lerp(originAnchored, toVec, p);
            }
        }
    }
}
