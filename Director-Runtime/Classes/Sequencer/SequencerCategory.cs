using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerCategory : DirectorObject
    {
        public string categoryName;

        [SerializeField, HideInInspector]
        internal List<SequencerEventContainer> containers = new List<SequencerEventContainer>();
        
        public List<SequencerEventContainer>.Enumerator GetEnumerator()
        {
            return containers.GetEnumerator();
        }

        public void ReadyToPlay()
        {
            for (int i = 0; i < containers.Count; i++)
            {
                containers[i].ReadyToPlay();
            }
        }
    }
}
