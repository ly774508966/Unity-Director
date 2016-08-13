using System;
using System.Collections.Generic;

namespace Tangzx.Director
{
    public class SequencerData : DirectorObject
    {
        public List<SequencerEventContainer> containers = new List<SequencerEventContainer>();

        public SequencerData()
        {
        }
    }
}