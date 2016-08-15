using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEventContainer : DirectorObject, IEventContainer
    {
        public Transform attach;

        [SerializeField]
        internal List<TDEvent> events = new List<TDEvent>();

        public SequencerEventContainer()
        {
        }

        public List<TDEvent>.Enumerator GetEnumerator()
        {
            return events.GetEnumerator();
        }

        public void Sort()
        {
            events.Sort();
        }
    }
}