using System.Collections.Generic;

namespace Tangzx.Director
{
    public interface IEventContainer
    {
        List<SequencerEventContainer>.Enumerator GetEnumerator();
    }
}
