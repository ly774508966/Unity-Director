using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerData : DirectorObject
    {
        [SerializeField, HideInInspector]
        internal List<SequencerCategory> categories = new List<SequencerCategory>();

        [SerializeField]
        private SequencerCategory _defaultCategory;

        [SerializeField]
        private float _totalDuration = 5;

        public SequencerData()
        {
            hideFlags = HideFlags.None;
        }

        public SequencerCategory defaultCategory
        {
            set { _defaultCategory = value; }
            get
            {
                if (_defaultCategory == null && categories.Count > 0)
                    _defaultCategory = categories[0];

                return _defaultCategory;
            }
        }

        public List<SequencerCategory>.Enumerator GetEnumerator()
        {
            return categories.GetEnumerator();
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
    }
}