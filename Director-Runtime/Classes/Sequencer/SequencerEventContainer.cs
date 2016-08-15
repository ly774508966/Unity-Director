using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEventContainer : DirectorObject, IEventContainer
    {
        public Transform attach;

        [SerializeField]
        internal List<TDEvent> events = new List<TDEvent>();


        void Awake()
        {
            for (int i = 0; i < events.Count; i++)
            {
                TDEvent e = events[i];
                if (e is ISequencerEvent)
                {
                    ISequencerEvent se = e as ISequencerEvent;
                    se.container = this;
                }
            }
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