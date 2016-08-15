using Tangzx.Director;
using UnityEngine;


[DirectorPlayable("Util/LogEvent")]
public class LogEvent : TDEvent
{
    public string content;

    public override void Fire()
    {
        base.Fire();
        Debug.Log(content);
    }
}