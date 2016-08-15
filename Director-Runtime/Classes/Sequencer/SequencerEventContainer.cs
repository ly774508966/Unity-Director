using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEventContainer : DirectorObject
    {
        public Transform attach;

        [SerializeField]
        internal List<TDEvent> evtList = new List<TDEvent>();

        public SequencerEventContainer()
        {
        }

        public List<TDEvent>.Enumerator GetEnumerator()
        {
            return evtList.GetEnumerator();
        }
    }
}

