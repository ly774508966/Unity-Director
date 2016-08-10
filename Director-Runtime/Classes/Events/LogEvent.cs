using Tangzx.Director;
using UnityEngine;

[DirectorPlayable("Util/LogEvent")]
public class LogEvent : Playable
{
    public override void Fire()
    {
        Debug.Log("test");
    }
}