using Tangzx.Director;

public class SequencerPlayer : DirectorPlayer
{
    public SequencerData data;


    void Awake()
    {
        if (data == null)
            data = GetComponent<SequencerData>();
    }

    public void Play()
    {
        //Play(data.defaultCategory);
    }
}
