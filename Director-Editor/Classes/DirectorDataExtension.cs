using System;
using Tangzx.Director;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TangzxInternal
{
    static class DirectorDataExtension
    {
        public static TDEvent Add(this DirectorData data, Type type)
        {
            TDEvent p = (TDEvent) ScriptableObject.CreateInstance(type);
            data.Add(p);
            p.name = type.Name;
            p.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(p, data);
            return p;
        }

        public static void Remove(this DirectorData data, TDEvent evt)
        {
            data.eventList.Remove(evt);
            Object.DestroyImmediate(evt, true);
        }
    }
}
