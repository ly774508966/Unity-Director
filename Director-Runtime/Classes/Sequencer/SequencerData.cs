using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerData : DirectorObject
    {
        [SerializeField]
        internal List<SequencerEventContainer> containers = new List<SequencerEventContainer>();

        public SequencerData()
        {
        }

        public List<SequencerEventContainer>.Enumerator GetEnumerator()
        {
            return containers.GetEnumerator();
        }
    }
}