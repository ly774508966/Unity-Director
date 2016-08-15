using Tangzx.Director;
using TangzxInternal.Data;
using UnityEditor;
using UnityEngine;

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

            var e = _data.GetEnumerator();
            while (e.MoveNext())
            {
                SequencerEventContainer ec = e.Current;
                SequencerEventContainerTreeItem ecTreeItem = new SequencerEventContainerTreeItem(ec);
                Add(ecTreeItem, ec.GetInstanceID(), ec.attach.name);
                ecTreeItem.BuildTree(windowState);
                //默认展开
                windowState.dataSource.SetExpanded(ecTreeItem, true);
            }
        }

        public override void RemoveChild(TreeItem child)
        {
            if (child is SequencerEventContainerTreeItem)
            {
                SequencerEventContainerTreeItem item = child as SequencerEventContainerTreeItem;
                _data.RemoveContainer(item.target);

                state.ReloadData();
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
            base.BuildTree(windowState);

            var e = target.GetEnumerator();
            while (e.MoveNext())
            {
                TDEvent evt = e.Current;
                EventTreeItem item = new EventTreeItem(evt);
                Add(item, evt.GetInstanceID(), evt.displayName);
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

        protected override GenericMenu OnContextMenu()
        {
            GenericMenu menu = state.ShowCreateEventMenu(HandleCreate);
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                TreeItem p = parent as TreeItem;
                p.RemoveChild(this);
            });
            return menu;
        }

        void HandleCreate(object data)
        {
            AttributeTool.EventInfo evtInfo = (AttributeTool.EventInfo)data;
            TDEvent evt = (TDEvent)target.CreateSubAsset(evtInfo.eventType);
            target.AddEvent(evt);

            state.dataSource.SetExpanded(this, true);
            state.ReloadData();
        }

        public override void RemoveChild(TreeItem child)
        {
            if (child is EventTreeItem)
            {
                EventTreeItem eti = child as EventTreeItem;
                target.RemoveEvent(eti.target);

                state.ReloadData();
            }
        }
    }
}