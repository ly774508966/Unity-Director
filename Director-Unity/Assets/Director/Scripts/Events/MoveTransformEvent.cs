using Tangzx.Director;
using UnityEngine;

[DirectorPlayable("Transform/MoveTransformEvent")]
class MoveTransformEvent : TDEvent, IRangeEvent, ISequencerEvent
{
    private Vector3 from;

    public Vector3 to;

    public SequencerEventContainer container { get; set; }

    public override void Fire()
    {
        base.Fire();
        from = container.attach.position;
    }

    public override void Process(float time)
    {
        base.Process(time);
        Vector3 p = Vector3.Lerp(from, to, time / duration);
        container.attach.position = p;
    }

}