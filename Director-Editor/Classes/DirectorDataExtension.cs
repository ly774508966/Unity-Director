using System;
using Tangzx.Director;
using UnityEditor;
using Object = UnityEngine.Object;

namespace TangzxInternal
{
    static class DirectorDataExtension
    {
        public static DirectorObject CreateSubAsset(this DirectorObject obj, Type subAssetType)
        {
            DirectorObject so = (DirectorObject)obj.gameObject.AddComponent(subAssetType);
            so.displayName = subAssetType.Name;
            return so;
        }

        public static T CreateSubAsset<T>(this DirectorObject obj) where T : DirectorObject
        {
            return (T)obj.CreateSubAsset(typeof(T));
        }

        public static void RemoveSubAsset(this DirectorObject obj, DirectorObject subAsset)
        {
            Object.DestroyImmediate(subAsset, true);
            EditorUtility.SetDirty(obj);
        }

        public static void AddContainer(this SequencerData dd, SequencerEventContainer ec)
        {
            dd.containers.Add(ec);
            EditorUtility.SetDirty(dd);
        }

        public static void RemoveContainer(this SequencerData dd, SequencerEventContainer ec)
        {
            dd.containers.Remove(ec);

            //remove events
            var e = ec.GetEnumerator();
            while (e.MoveNext())
                ec.RemoveSubAsset(e.Current);

            dd.RemoveSubAsset(ec);
        }

        public static void AddEvent(this SequencerEventContainer ec, TDEvent evt)
        {
            ec.evtList.Add(evt);
            EditorUtility.SetDirty(ec);
        }

        public static void RemoveEvent(this SequencerEventContainer ec, TDEvent evt)
        {
            ec.evtList.Remove(evt);
            ec.RemoveSubAsset(evt);
        }
    }
}
