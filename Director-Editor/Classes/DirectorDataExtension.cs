using System;
using Tangzx.Director;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TangzxInternal
{
    static class DirectorDataExtension
    {
        public static ScriptableObject CreateSubAsset(this DirectorObject obj, Type subAssetType)
        {
            ScriptableObject so = ScriptableObject.CreateInstance(subAssetType);
            so.name = subAssetType.Name;
            so.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(so, obj);
            return so;
        }

        public static T CreateSubAsset<T>(this DirectorObject obj) where T : DirectorObject
        {
            return (T)obj.CreateSubAsset(typeof(T));
        }

        public static void RemoveSubAsset(this DirectorObject obj, ScriptableObject subAsset)
        {
            Object.DestroyImmediate(subAsset, true);
        }

        public static void AddContainer(this SequencerData dd, SequencerEventContainer ec)
        {
            dd.containers.Add(ec);
        }

        public static void Remove(this SequencerData dd, SequencerEventContainer ec)
        {
            dd.containers.Remove(ec);
            dd.RemoveSubAsset(ec);
        }

        public static void AddEvent(this SequencerEventContainer ec, TDEvent evt)
        {
            ec.evtList.Add(evt);
        }

        public static void Remove(this SequencerEventContainer ec, TDEvent evt)
        {
            ec.evtList.Remove(evt);
            ec.RemoveSubAsset(evt);
        }
    }
}
