using System.Collections.Generic;
using UnityEngine;

namespace Tangzx.Director
{
    public class SequencerPlayer : DirectorPlayer
    {
        public delegate void OnPlayHandler(SequencerCategory sc);

        public OnPlayHandler onBegin;
        public OnPlayHandler onFinish;

        public SequencerData data;

        public bool isAutoPlay;

        private SequencerCategory _playingCategory;

        void Awake()
        {
            ReadyToPlay();
            if (isAutoPlay)
            {
                BeginPlay(data.defaultCategory);
                PlayForward();
            }
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
            sc.ReadyToPlay();

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

        public override void ReadyToPlay()
        {
            base.ReadyToPlay();
            if (data == null)
                data = GetComponent<SequencerData>();
        }

        protected override void OnPlayBegin()
        {
            base.OnPlayBegin();
            if (onBegin != null && _playingCategory)
                onBegin(_playingCategory);
        }

        protected override void OnForwardFinish()
        {
            base.OnForwardFinish();
            if (onFinish != null && _playingCategory)
                onFinish(_playingCategory);
        }
    }
}