using UnityEngine;

namespace Tangzx.Director
{
    public abstract class DirectorObject : MonoBehaviour
    {
        [HideInInspector]
        public string displayName;

        public DirectorObject()
        {
            //hideFlags = HideFlags.HideInInspector;
        }
    }
}

