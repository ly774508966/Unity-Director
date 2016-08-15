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

        #region Sequencer Data
        public static void AddCategory(this SequencerData data, SequencerCategory sc)
        {
            data.categories.Add(sc);
        }

        public static void RemoveCategory(this SequencerData data, SequencerCategory sc)
        {
            sc.RemoveAllContainers();
            data.categories.Remove(sc);
            data.RemoveSubAsset(sc);
        }
        #endregion Sequencer Data


        #region Category
        public static void AddContainer(this SequencerCategory dd, SequencerEventContainer ec)
        {
            dd.containers.Add(ec);
            EditorUtility.SetDirty(dd);
        }

        public static void RemoveContainer(this SequencerCategory dd, SequencerEventContainer ec)
        {
            ec.RemoveAllEvents();
            dd.containers.Remove(ec);
            dd.RemoveSubAsset(ec);
        }

        public static void RemoveAllContainers(this SequencerCategory dd)
        {
            SequencerEventContainer[] list = dd.containers.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                dd.RemoveContainer(list[i]);
            }
            dd.containers.Clear();
        }
        #endregion Category

        #region Event Container
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

        public static void RemoveAllEvents(this SequencerEventContainer ec)
        {
            TDEvent[] events = ec.evtList.ToArray();
            for (int i = 0; i < events.Length; i++)
            {
                ec.RemoveEvent(events[i]);
            }
            ec.evtList.Clear();
        }
        #endregion Event Container
    }
}
