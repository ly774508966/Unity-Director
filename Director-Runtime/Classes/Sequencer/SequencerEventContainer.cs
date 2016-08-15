using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEventContainer : DirectorObject, IEventContainer
    {
        public Transform attach;

        [SerializeField, HideInInspector]
        internal List<DirectorEvent> events = new List<DirectorEvent>();
        
        void Awake()
        {
            ReadyToPlay();
        }

        public List<DirectorEvent>.Enumerator GetEnumerator()
        {
            return events.GetEnumerator();
        }

        public void Sort()
        {
            events.Sort();
        }

        public void ReadyToPlay()
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
            }
        }
    }
}