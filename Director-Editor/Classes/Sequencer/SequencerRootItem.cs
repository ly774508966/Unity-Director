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

        public override void BuildTree(DirectorWindowState windowState)
        {
            children = null;
            list.Clear();

            for (int i = 0; i < _data.containers.Count; i++)
            {
                SequencerEventContainer ec = _data.containers[i];
                SequencerEventContainerTreeItem ecTreeItem = new SequencerEventContainerTreeItem(ec);
                Add(ecTreeItem, ec.GetInstanceID(), ec.attach.name);
                ecTreeItem.BuildTree(windowState);
                //默认展开
                windowState.dataSource.SetExpanded(ecTreeItem, true);
            }
        }
    }

    class SequencerEventContainerTreeItem : TreeItem, ISheetRowDrawer
    {
        public SequencerEventContainer target;

        public SequencerEventContainerTreeItem(SequencerEventContainer ec)
        {
            target = ec;
            icon = AssetPreview.GetAssetPreview(ec.attach);
        }

        public override void BuildTree(DirectorWindowState windowState)
        {
            for (int j = 0; j < target.evtList.Count; j++)
            {
                TDEvent evt = target.evtList[j];
                EventTreeItem item = new EventTreeItem(evt);
                Add(item, evt.GetInstanceID(), evt.name);
                item.BuildTree(windowState);
            }
        }

        public void OnSheetRowGUI(ISheetEditor sheetEditor, Rect rect)
        {
            
        }

        public override ISheetRowDrawer GetDrawer()
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

            state.dataSource.SetExpanded(this, true);

            state.refreshType = DirectorWindowState.RefreshType.All;
        }
    }
}