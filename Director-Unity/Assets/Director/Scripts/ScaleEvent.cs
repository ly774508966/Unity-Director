using UnityEngine;

namespace Tangzx.Director
{
    [DirectorPlayable("GameObject/Scale")]
    class ScaleEvent : UISequencerBaseEvent
    {
        public Vector3 scale;

        public bool from;

        public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        Vector3 originScale;

        public override void Fire(bool isFristTime)
        {
            base.Fire(isFristTime);
            if (isFristTime)
            {
                originScale = rect.localScale;
            }
        }

        public override void Process(float time, bool isReverse = false)
        {
            float p = curve.Evaluate(time / duration);

            if (from)
            {
                rect.localScale = Vector2.Lerp(scale, originScale, p);
            }
            else //to
            {
                rect.localScale = Vector2.Lerp(originScale, scale, p);
            }
        }

        public override void StopAndRecover()
        {
            base.StopAndRecover();
            rect.localScale = originScale;
        }
    }
}
