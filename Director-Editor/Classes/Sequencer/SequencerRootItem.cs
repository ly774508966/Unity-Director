using System;
using TangzxInternal.Data;
using Tangzx.Director;
using UnityEngine;
using UnityEditor;

namespace TangzxInternal
{
    class SequencerRootItem : TreeRootItem
    {
        SequencerData _data;

        public SequencerRootItem(SequencerData data)
        {
            _data = data;
        }

        public override void FetchData()
        {
            children = null;
            list.Clear();

            for (int i = 0; i < _data.containers.Count; i++)
            {
                SequencerEventContainer ec = _data.containers[i];
                SequencerEventContainerTreeItem ecTreeItem = new SequencerEventContainerTreeItem(ec);
                Add(ecTreeItem, ec.GetInstanceID(), ec.attach.name);

                for (int j = 0; j < ec.evtList.Count; j++)
                {
                    TDEvent evt = ec.evtList[j];
                    ecTreeItem.Add(new EventTreeItem(evt), evt.GetInstanceID(), evt.name);
                }
            }
        }
    }

    class SequencerEventContainerTreeItem : TreeItem, IRowDrawer
    {
        public SequencerEventContainer target;

        public SequencerEventContainerTreeItem(SequencerEventContainer ec)
        {
            target = ec;
        }

        public void OnSheetRowGUI(ISheetEditor sheetEditor, Rect rect)
        {
            
        }

        public override IRowDrawer GetDrawer()
        {
            return this;
        }

        protected override void OnContextMenu()
        {
            state.ShowCreateEventMenu(HandleCreate);
        }

        void HandleCreate(object data)
        {
            AttributeTool.EventInfo evtInfo = (AttributeTool.EventInfo)data;
            TDEvent evt = (TDEvent)ScriptableObject.CreateInstance(evtInfo.eventType);
            evt.name = evtInfo.eventType.Name;
            target.evtList.Add(evt);
            AssetDatabase.AddObjectToAsset(evt, target);

            state.refreshType = DirectorWindowState.RefreshType.All;
        }
    }
}