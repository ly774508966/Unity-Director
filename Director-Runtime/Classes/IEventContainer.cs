using System.Collections.Generic;

namespace Tangzx.Director
{
    public interface IEventContainer
    {
        List<TDEvent>.Enumerator GetEnumerator();

        void Sort();
    }
}
