using System.Collections.Generic;
using Tangzx.Director;

public class SequencerPlayer : DirectorPlayer
{
    public SequencerData data;


    void Awake()
    {
        if (data == null)
            data = GetComponent<SequencerData>();
        Play();
    }

    public void Play()
    {
        Play(data.defaultCategory);
    }

    public void Play(SequencerCategory sc)
    {
        List<IEventContainer> list = new List<IEventContainer>();
        var e = sc.GetEnumerator();
        while (e.MoveNext()) list.Add(e.Current);
        Play(list.ToArray(), data.totalDuration);
    }
}
