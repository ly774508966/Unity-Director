using System.Collections.Generic;

namespace Tangzx.Director
{
    /// <summary>
    /// 事件容器
    /// </summary>
    public interface IEventContainer
    {
        List<DirectorEvent>.Enumerator GetEnumerator();

        List<DirectorEvent> list { get; }

        void GetReady();

        void Sort();
    }
}
