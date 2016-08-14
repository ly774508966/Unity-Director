using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerEventContainer : DirectorObject
    {
        public Transform attach;

        public List<TDEvent> evtList = new List<TDEvent>();

        public SequencerEventContainer()
        {
        }
    }
}

