namespace Tangzx.Director
{
    /// <summary>
    /// 有长度的事件
    /// </summary>
    public interface IRangeEvent
    {
        void Process(float time, bool isReverse = false);
    }
}
