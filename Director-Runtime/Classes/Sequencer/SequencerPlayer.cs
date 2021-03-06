﻿using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerPlayer : DirectorPlayer
    {
        public delegate void OnPlayHandler(SequencerCategory sc);

        public OnPlayHandler onReverseFinish;
        public OnPlayHandler onForwardFinish;

        private SequencerData data;

        private SequencerCategory _playingCategory;

        public SequencerPlayer(SequencerData data)
        {
            this.data = data;
        }

        public void BeginPlay()
        {
            BeginPlay(data.defaultCategory);
        }

        public void BeginPlay(string name)
        {
            SequencerCategory sc = data.GetCategoryByName(name);
            _playingCategory = sc;
            BeginPlay(sc);
        }

        public void BeginPlay(SequencerCategory sc)
        {
            sc.GetReady();

            BeginPlay(GetVaildConatainers(sc), sc.totalDuration);
        }

        IEventContainer[] GetVaildConatainers(SequencerCategory sc)
        {
            List<IEventContainer> list = new List<IEventContainer>();
            var e = sc.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Current.attach)
                {
                    list.Add(e.Current);
                }
                else
                {
                    Debug.LogWarning("No attach object found for EventConatainer : " + e.Current.displayName);
                }
            }

            return list.ToArray();
        }

        protected override void OnFinish(bool isForward)
        {
            base.OnFinish(isForward);
            if (isForward)
            {
                if (onForwardFinish != null)
                    onForwardFinish(_playingCategory);
            }
            else
            {
                if (onReverseFinish != null)
                    onReverseFinish(_playingCategory);
            }
        }
    }
}