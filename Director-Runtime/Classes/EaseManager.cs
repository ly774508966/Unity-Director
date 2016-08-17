using System;

namespace Tangzx.Director
{
    public static class EaseManager
    {
        private const float _PiOver2 = 1.570796f;
        private const float _TwoPi = 6.283185f;

        public delegate float EaseFunction(float time, float duration, float overshootOrAmplitude, float period);
        
        public static float Evaluate(Ease easeType, float time, float duration, float overshootOrAmplitude, float period)
        {
            switch (easeType)
            {
                case Ease.Linear:
                    return (time / duration);

                case Ease.InSine:
                    return (-((float)Math.Cos((double)((time / duration) * 1.570796f))) + 1f);

                case Ease.OutSine:
                    return (float)Math.Sin((double)((time / duration) * 1.570796f));

                case Ease.InOutSine:
                    return (-0.5f * (((float)Math.Cos((double)((3.141593f * time) / duration))) - 1f));

                case Ease.InQuad:
                    return ((time /= duration) * time);

                case Ease.OutQuad:
                    return (-(time /= duration) * (time - 2f));

                case Ease.InOutQuad:
                    if ((time /= (duration * 0.5f)) >= 1f)
                    {
                        return (-0.5f * ((--time * (time - 2f)) - 1f));
                    }
                    return ((0.5f * time) * time);

                case Ease.InCubic:
                    return (((time /= duration) * time) * time);

                case Ease.OutCubic:
                    return ((((time = (time / duration) - 1f) * time) * time) + 1f);

                case Ease.InOutCubic:
                    if ((time /= (duration * 0.5f)) >= 1f)
                    {
                        return (0.5f * ((((time -= 2f) * time) * time) + 2f));
                    }
                    return (((0.5f * time) * time) * time);

                case Ease.InQuart:
                    return ((((time /= duration) * time) * time) * time);

                case Ease.OutQuart:
                    return -(((((time = (time / duration) - 1f) * time) * time) * time) - 1f);

                case Ease.InOutQuart:
                    if ((time /= (duration * 0.5f)) >= 1f)
                    {
                        return (-0.5f * (((((time -= 2f) * time) * time) * time) - 2f));
                    }
                    return ((((0.5f * time) * time) * time) * time);

                case Ease.InQuint:
                    return (((((time /= duration) * time) * time) * time) * time);

                case Ease.OutQuint:
                    return ((((((time = (time / duration) - 1f) * time) * time) * time) * time) + 1f);

                case Ease.InOutQuint:
                    if ((time /= (duration * 0.5f)) >= 1f)
                    {
                        return (0.5f * ((((((time -= 2f) * time) * time) * time) * time) + 2f));
                    }
                    return (((((0.5f * time) * time) * time) * time) * time);

                case Ease.InExpo:
                    if (time == 0f)
                    {
                        return 0f;
                    }
                    return (float)Math.Pow(2.0, (double)(10f * ((time / duration) - 1f)));

                case Ease.OutExpo:
                    if (time != duration)
                    {
                        return (-((float)Math.Pow(2.0, (double)((-10f * time) / duration))) + 1f);
                    }
                    return 1f;

                case Ease.InOutExpo:
                    if (time != 0f)
                    {
                        if (time == duration)
                        {
                            return 1f;
                        }
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return (0.5f * ((float)Math.Pow(2.0, (double)(10f * (time - 1f)))));
                        }
                        return (0.5f * (-((float)Math.Pow(2.0, (double)(-10f * --time))) + 2f));
                    }
                    return 0f;

                case Ease.InCirc:
                    return -(((float)Math.Sqrt((double)(1f - ((time /= duration) * time)))) - 1f);

                case Ease.OutCirc:
                    return (float)Math.Sqrt((double)(1f - ((time = (time / duration) - 1f) * time)));

                case Ease.InOutCirc:
                    if ((time /= (duration * 0.5f)) >= 1f)
                    {
                        return (0.5f * (((float)Math.Sqrt((double)(1f - ((time -= 2f) * time)))) + 1f));
                    }
                    return (-0.5f * (((float)Math.Sqrt((double)(1f - (time * time)))) - 1f));

                case Ease.InElastic:
                    if (time != 0f)
                    {
                        float num;
                        if ((time /= duration) == 1f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                        {
                            num = (period / 6.283185f) * ((float)Math.Asin((double)(1f / overshootOrAmplitude)));
                        }
                        return -((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(10f * --time)))) * ((float)Math.Sin((double)((((time * duration) - num) * 6.283185f) / period))));
                    }
                    return 0f;

                case Ease.OutElastic:
                    if (time != 0f)
                    {
                        float num2;
                        if ((time /= duration) == 1f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num2 = period / 4f;
                        }
                        else
                        {
                            num2 = (period / 6.283185f) * ((float)Math.Asin((double)(1f / overshootOrAmplitude)));
                        }
                        return (((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(-10f * time)))) * ((float)Math.Sin((double)((((time * duration) - num2) * 6.283185f) / period)))) + 1f);
                    }
                    return 0f;

                case Ease.InOutElastic:
                    if (time != 0f)
                    {
                        float num3;
                        if ((time /= (duration * 0.5f)) == 2f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.45f;
                        }
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num3 = period / 4f;
                        }
                        else
                        {
                            num3 = (period / 6.283185f) * ((float)Math.Asin((double)(1f / overshootOrAmplitude)));
                        }
                        if (time < 1f)
                        {
                            return (-0.5f * ((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(10f * --time)))) * ((float)Math.Sin((double)((((time * duration) - num3) * 6.283185f) / period)))));
                        }
                        return ((((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(-10f * --time)))) * ((float)Math.Sin((double)((((time * duration) - num3) * 6.283185f) / period)))) * 0.5f) + 1f);
                    }
                    return 0f;

                case Ease.InBack:
                    return (((time /= duration) * time) * (((overshootOrAmplitude + 1f) * time) - overshootOrAmplitude));

                case Ease.OutBack:
                    return ((((time = (time / duration) - 1f) * time) * (((overshootOrAmplitude + 1f) * time) + overshootOrAmplitude)) + 1f);

                case Ease.InOutBack:
                    if ((time /= (duration * 0.5f)) >= 1f)
                    {
                        return (0.5f * ((((time -= 2f) * time) * ((((overshootOrAmplitude *= 1.525f) + 1f) * time) + overshootOrAmplitude)) + 2f));
                    }
                    return (0.5f * ((time * time) * ((((overshootOrAmplitude *= 1.525f) + 1f) * time) - overshootOrAmplitude)));

                case Ease.InBounce:
                    return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);

                case Ease.OutBounce:
                    return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);

                case Ease.InOutBounce:
                    return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
            }
            return (-(time /= duration) * (time - 2f));
        }

        public static EaseFunction ToEaseFunction(Ease ease)
        {
            switch (ease)
            {
                case Ease.Linear:
                    return (time, duration, overshootOrAmplitude, period) => (time / duration);

                case Ease.InSine:
                    return (time, duration, overshootOrAmplitude, period) => (-((float)Math.Cos((double)((time / duration) * 1.570796f))) + 1f);

                case Ease.OutSine:
                    return (time, duration, overshootOrAmplitude, period) => ((float)Math.Sin((double)((time / duration) * 1.570796f)));

                case Ease.InOutSine:
                    return (time, duration, overshootOrAmplitude, period) => (-0.5f * (((float)Math.Cos((double)((3.141593f * time) / duration))) - 1f));

                case Ease.InQuad:
                    return (time, duration, overshootOrAmplitude, period) => ((time /= duration) * time);

                case Ease.OutQuad:
                    return (time, duration, overshootOrAmplitude, period) => (-(time /= duration) * (time - 2f));

                case Ease.InOutQuad:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return ((0.5f * time) * time);
                        }
                        return (-0.5f * ((--time * (time - 2f)) - 1f));
                    };

                case Ease.InCubic:
                    return (time, duration, overshootOrAmplitude, period) => (((time /= duration) * time) * time);

                case Ease.OutCubic:
                    return (time, duration, overshootOrAmplitude, period) => ((((time = (time / duration) - 1f) * time) * time) + 1f);

                case Ease.InOutCubic:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return (((0.5f * time) * time) * time);
                        }
                        return (0.5f * ((((time -= 2f) * time) * time) + 2f));
                    };

                case Ease.InQuart:
                    return (time, duration, overshootOrAmplitude, period) => ((((time /= duration) * time) * time) * time);

                case Ease.OutQuart:
                    return (time, duration, overshootOrAmplitude, period) => -(((((time = (time / duration) - 1f) * time) * time) * time) - 1f);

                case Ease.InOutQuart:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return ((((0.5f * time) * time) * time) * time);
                        }
                        return (-0.5f * (((((time -= 2f) * time) * time) * time) - 2f));
                    };

                case Ease.InQuint:
                    return (time, duration, overshootOrAmplitude, period) => (((((time /= duration) * time) * time) * time) * time);

                case Ease.OutQuint:
                    return (time, duration, overshootOrAmplitude, period) => ((((((time = (time / duration) - 1f) * time) * time) * time) * time) + 1f);

                case Ease.InOutQuint:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return (((((0.5f * time) * time) * time) * time) * time);
                        }
                        return (0.5f * ((((((time -= 2f) * time) * time) * time) * time) + 2f));
                    };

                case Ease.InExpo:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if (time != 0f)
                        {
                            return (float)Math.Pow(2.0, (double)(10f * ((time / duration) - 1f)));
                        }
                        return 0f;
                    };

                case Ease.OutExpo:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if (time == duration)
                        {
                            return 1f;
                        }
                        return (-((float)Math.Pow(2.0, (double)((-10f * time) / duration))) + 1f);
                    };

                case Ease.InOutExpo:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if (time == duration)
                        {
                            return 1f;
                        }
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return (0.5f * ((float)Math.Pow(2.0, (double)(10f * (time - 1f)))));
                        }
                        return (0.5f * (-((float)Math.Pow(2.0, (double)(-10f * --time))) + 2f));
                    };

                case Ease.InCirc:
                    return (time, duration, overshootOrAmplitude, period) => -(((float)Math.Sqrt((double)(1f - ((time /= duration) * time)))) - 1f);

                case Ease.OutCirc:
                    return (time, duration, overshootOrAmplitude, period) => ((float)Math.Sqrt((double)(1f - ((time = (time / duration) - 1f) * time))));

                case Ease.InOutCirc:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return (-0.5f * (((float)Math.Sqrt((double)(1f - (time * time)))) - 1f));
                        }
                        return (0.5f * (((float)Math.Sqrt((double)(1f - ((time -= 2f) * time)))) + 1f));
                    };

                case Ease.InElastic:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        float num;
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if ((time /= duration) == 1f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                        {
                            num = (period / 6.283185f) * ((float)Math.Asin((double)(1f / overshootOrAmplitude)));
                        }
                        return -((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(10f * --time)))) * ((float)Math.Sin((double)((((time * duration) - num) * 6.283185f) / period))));
                    };

                case Ease.OutElastic:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        float num;
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if ((time /= duration) == 1f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                        {
                            num = (period / 6.283185f) * ((float)Math.Asin((double)(1f / overshootOrAmplitude)));
                        }
                        return (((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(-10f * time)))) * ((float)Math.Sin((double)((((time * duration) - num) * 6.283185f) / period)))) + 1f);
                    };

                case Ease.InOutElastic:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        float num;
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if ((time /= (duration * 0.5f)) == 2f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.45f;
                        }
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                        {
                            num = (period / 6.283185f) * ((float)Math.Asin((double)(1f / overshootOrAmplitude)));
                        }
                        if (time < 1f)
                        {
                            return (-0.5f * ((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(10f * --time)))) * ((float)Math.Sin((double)((((time * duration) - num) * 6.283185f) / period)))));
                        }
                        return ((((overshootOrAmplitude * ((float)Math.Pow(2.0, (double)(-10f * --time)))) * ((float)Math.Sin((double)((((time * duration) - num) * 6.283185f) / period)))) * 0.5f) + 1f);
                    };

                case Ease.InBack:
                    return (time, duration, overshootOrAmplitude, period) => (((time /= duration) * time) * (((overshootOrAmplitude + 1f) * time) - overshootOrAmplitude));

                case Ease.OutBack:
                    return (time, duration, overshootOrAmplitude, period) => ((((time = (time / duration) - 1f) * time) * (((overshootOrAmplitude + 1f) * time) + overshootOrAmplitude)) + 1f);

                case Ease.InOutBack:
                    return delegate (float time, float duration, float overshootOrAmplitude, float period)
                    {
                        if ((time /= (duration * 0.5f)) < 1f)
                        {
                            return (0.5f * ((time * time) * ((((overshootOrAmplitude *= 1.525f) + 1f) * time) - overshootOrAmplitude)));
                        }
                        return (0.5f * ((((time -= 2f) * time) * ((((overshootOrAmplitude *= 1.525f) + 1f) * time) + overshootOrAmplitude)) + 2f));
                    };

                case Ease.InBounce:
                    return (time, duration, overshootOrAmplitude, period) => Bounce.EaseIn(time, duration, overshootOrAmplitude, period);

                case Ease.OutBounce:
                    return (time, duration, overshootOrAmplitude, period) => Bounce.EaseOut(time, duration, overshootOrAmplitude, period);

                case Ease.InOutBounce:
                    return (time, duration, overshootOrAmplitude, period) => Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
            }
            return (time, duration, overshootOrAmplitude, period) => (-(time /= duration) * (time - 2f));
        }
    }

    public static class Bounce
    {
        public static float EaseIn(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
        {
            return (1f - EaseOut(duration - time, duration, -1f, -1f));
        }

        public static float EaseInOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
        {
            if (time < (duration * 0.5f))
            {
                return (EaseIn(time * 2f, duration, -1f, -1f) * 0.5f);
            }
            return ((EaseOut((time * 2f) - duration, duration, -1f, -1f) * 0.5f) + 0.5f);
        }

        public static float EaseOut(float time, float duration, float unusedOvershootOrAmplitude, float unusedPeriod)
        {
            if ((time /= duration) < 0.3636364f)
            {
                return ((7.5625f * time) * time);
            }
            if (time < 0.7272727f)
            {
                return (((7.5625f * (time -= 0.5454546f)) * time) + 0.75f);
            }
            if (time < 0.9090909f)
            {
                return (((7.5625f * (time -= 0.8181818f)) * time) + 0.9375f);
            }
            return (((7.5625f * (time -= 0.9545454f)) * time) + 0.984375f);
        }
    }
}