using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerData : DirectorObject
    {
        [SerializeField, HideInInspector]
        internal List<SequencerCategory> categories = new List<SequencerCategory>();

        [SerializeField, HideInInspector]
        private SequencerCategory _defaultCategory;

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
    }
}