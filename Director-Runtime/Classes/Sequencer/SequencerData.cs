using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerData : DirectorObject
    {
        [SerializeField, HideInInspector]
        internal List<SequencerEventContainer> containers = new List<SequencerEventContainer>();

        public SequencerData()
        {
            hideFlags = HideFlags.None;
        }

        public List<SequencerEventContainer>.Enumerator GetEnumerator()
        {
            return containers.GetEnumerator();
        }
    }
}