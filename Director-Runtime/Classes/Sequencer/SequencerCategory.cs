using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerCategory : DirectorObject
    {
        public string categoryName;

        [SerializeField, HideInInspector]
        internal List<SequencerEventContainer> containers = new List<SequencerEventContainer>();

        [SerializeField, HideInInspector]
        private float _totalDuration = 5;
        
        public List<SequencerEventContainer>.Enumerator GetEnumerator()
        {
            return containers.GetEnumerator();
        }

        public void GetReady()
        {
            for (int i = 0; i < containers.Count; i++)
            {
                containers[i].GetReady();
            }
        }

        public float totalDuration
        {
            get
            {
                return _totalDuration;
            }
            set
            {
                _totalDuration = value;
            }
        }

        public void ReplaceActor(Transform trans, string name)
        {
            for (int i = 0; i < containers.Count; i++)
            {
                SequencerEventContainer sec = containers[i] as SequencerEventContainer;
                if (sec.attachName == name)
                {
                    sec.attach = trans;
                }
            }
        }
    }
}
