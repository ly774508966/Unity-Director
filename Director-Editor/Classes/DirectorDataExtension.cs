using System;
using Tangzx.Director;
using UnityEngine;

namespace TangzxInternal
{
     static class DirectorDataExtension
    {
        public static Playable Add(this DirectorData data, Type type)
        {
            Playable p = (Playable) ScriptableObject.CreateInstance(type);
            data.Add(p);
            return p;
        }
    }
}
