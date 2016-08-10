using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal
{
    public class EventDrawer
    {
        public Playable playable;

        public virtual void OnGUI(Rect rect)
        {
            GUI.Button(rect, "test");
        }

        public virtual void Reset()
        {

        }
    }
}
