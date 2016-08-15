using System.Collections.Generic;
using Tangzx.Director;

public class SequencerPlayer : DirectorPlayer
{
    public SequencerData data;

    void Awake()
    {
        ReadyToPlay();
        Play();
    }

    public void Play()
    {
        Play(data.defaultCategory);
    }

    public void Play(SequencerCategory sc)
    {
        sc.ReadyToPlay();

        List<IEventContainer> list = new List<IEventContainer>();
        var e = sc.GetEnumerator();
        while (e.MoveNext()) list.Add(e.Current);
        Play(list.ToArray(), data.totalDuration);
    }

    public override void ReadyToPlay()
    {
        base.ReadyToPlay();
        if (data == null)
            data = GetComponent<SequencerData>();
    }
}
