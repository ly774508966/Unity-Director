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
        private SequencerCategory _beginCategory;

        void Awake()
        {
            ReadyToPlay();
            if (isAutoPlay) Play();
        }

        public void BeginPlay(SequencerCategory sc)
        {
            sc.ReadyToPlay();
            _beginCategory = sc;
            
            BeginPlay(GetVaildConatainers(sc), sc.totalDuration);
        }

        public void Play()
        {
            Play(data.defaultCategory);
        }

        public void Play(string name)
        {
            SequencerCategory sc = data.GetCategoryByName(name);
            Play(sc);
        }

        public void Play(SequencerCategory sc)
        {
            PlayCategory(sc, true);
        }

        public void PlayReverse()
        {
            PlayReverse(data.defaultCategory);
        }

        public void PlayReverse(SequencerCategory sc)
        {
            PlayCategory(sc, false);
        }

        void PlayCategory(SequencerCategory sc, bool isForward)
        {
            if (_beginCategory != sc && isForward)
                BeginPlay(sc);
            _beginCategory = sc;
            _playingCategory = sc;
            
            timeScale = 1;
            Play(GetVaildConatainers(sc), sc.totalDuration);
            if (isForward == false)
            {
                timeScale = -1;
                Tick(sc.totalDuration, false);
            }
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
            _beginCategory = null;
            if (onBegin != null && _playingCategory)
                onBegin(_playingCategory);
        }

        protected override void OnPlayFinish()
        {
            base.OnPlayFinish();
            _beginCategory = null;
            if (onFinish != null && _playingCategory)
                onFinish(_playingCategory);
        }

        public override void Stop()
        {
            _beginCategory = null;
            base.Stop();
        }
    }
}