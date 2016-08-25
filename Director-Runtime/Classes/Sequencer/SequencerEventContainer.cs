using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEventContainer : DirectorObject, IEventContainer
    {
        public Transform attach;

        public string attachName;

        [SerializeField, HideInInspector]
        internal List<DirectorEvent> events = new List<DirectorEvent>();

        public List<DirectorEvent> list
        {
            get
            {
                return events;
            }
        }

        public List<DirectorEvent>.Enumerator GetEnumerator()
        {
            return events.GetEnumerator();
        }

        public void Sort()
        {
            events.Sort();
        }

        public void GetReady()
        {
            Sort();
            for (int i = 0; i < events.Count; i++)
            {
                DirectorEvent e = events[i];
                if (e is ISequencerEvent)
                {
                    ISequencerEvent se = e as ISequencerEvent;
                    se.container = this;
                }
                e.GetReady();
            }
        }
    }
}