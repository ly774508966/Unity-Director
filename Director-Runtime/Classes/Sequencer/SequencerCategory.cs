using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerCategory : DirectorObject, IEventContainer
    {
        public string categoryName;

        [SerializeField, HideInInspector]
        internal List<SequencerEventContainer> containers = new List<SequencerEventContainer>();
        
        public List<SequencerEventContainer>.Enumerator GetEnumerator()
        {
            return containers.GetEnumerator();
        }
    }
}
