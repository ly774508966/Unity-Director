using System.Collections.Generic;

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
            if (isAutoPlay) Play();
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
            _playingCategory = sc;
            sc.ReadyToPlay();

            List<IEventContainer> list = new List<IEventContainer>();
            var e = sc.GetEnumerator();
            while (e.MoveNext()) list.Add(e.Current);

            timeScale = isForward ? 1 : -1;
            Play(list.ToArray(), sc.totalDuration);
            if (isForward == false)
            {
                Process(sc.totalDuration);
            }
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

        protected override void OnPlayFinish()
        {
            base.OnPlayFinish();
            if (onFinish != null && _playingCategory)
                onFinish(_playingCategory);
        }
    }
}