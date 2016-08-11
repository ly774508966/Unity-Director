using Tangzx.Director;
using UnityEngine;

[DirectorPlayable("Util/LogEvent")]
public class LogEvent : TDEvent
{
    public override void Fire()
    {
        Debug.Log("test");
    }
}