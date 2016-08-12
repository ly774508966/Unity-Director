using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{

    /// <summary>
    /// 
    /// </summary>
    public class DirectorData : ScriptableObject
    {
        public List<TDEvent> eventList = new List<TDEvent>();
        
        public void Add(TDEvent playable)
        {
            eventList.Add(playable);
        }

        /// <summary>
        /// 按时间把这些事件排序一下
        /// </summary>
        public void Sort()
        {
            eventList.Sort();
        }
    }
}