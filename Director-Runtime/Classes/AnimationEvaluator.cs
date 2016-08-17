using System;
using UnityEngine;

namespace Tangzx.Director
{
    [Serializable]
    public sealed class AnimationEvaluator
    {
        public bool useCurve;

        public AnimationCurve curve;
        public Ease easeType;

        public float overshootOrAmplitude = 0;
        public float period = 0;

        public float Evaluate(float time, float duration)
        {
            if (useCurve)
                return curve.Evaluate(time / duration);
            else
                return EaseManager.Evaluate(easeType, time, duration, overshootOrAmplitude, period);
        }
    }
}
