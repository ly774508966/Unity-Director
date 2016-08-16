﻿using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class SequencerCategoryTreeItem : TreeRootItem, ISheetRowDrawer, IRenameableTreeItem, IVisibleRootItem
    {
        public SequencerCategory target;

        private SequencerEditorWindow editor;

        public SequencerCategoryTreeItem(SequencerEditorWindow ed, SequencerCategory sc)
        {
            editor = ed;
            target = sc;
            displayName = sc.categoryName;
        }
        
        //static GUIStyle tagStyle;

        public void OnSheetRowGUI(ISheetEditor sheetEditor, Rect rect)
        {
            /*
            if (tagStyle == null)
            {
                tagStyle = "ChannelStripAttenuationMarkerSquare";
            }

            if (Event.current.type == EventType.Repaint)
            {
                GUIContent c = new GUIContent(string.Format("  {0} Container(s)", target.containers.Count));
                Vector2 size = tagStyle.CalcSize(c);
                Rect r = rect;
                r.width = size.x;

                tagStyle.Draw(r, c, 0);
            }*/
        }

        public override ISheetRowDrawer GetDrawer()
        {
            return this;
        }

        public override void BuildTree(DirectorWindowState windowState)
        {
            children = null;
            list.Clear();

            var e = target.GetEnumerator();
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
                target.RemoveContainer(item.target);

                state.ReloadData();
            }
        }

        protected override GenericMenu OnContextMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                if (EditorUtility.DisplayDialog("警告", "确认删除分类：[" + target.categoryName + "] ?", "确定", "取消"))
                {
                    editor.RemoveCategory(target);
                }
            });
            return menu;
        }

        public void RenameEnded(string name)
        {
            displayName = name;
            target.categoryName = name;
        }
    }

    class SequencerEventContainerTreeItem : TreeItem, ISheetRowDrawer, IInspectorItem
    {
        public SequencerEventContainer target;

        private bool _isSelected;

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
                DirectorEvent evt = e.Current;
                EventTreeItem item = new EventTreeItem(evt);
                Add(item, evt.GetInstanceID(), evt.displayName);
                item.BuildTree(windowState);
            }
        }

        static GUIStyle tagStyle;

        public void OnSheetRowGUI(ISheetEditor sheetEditor, Rect rect)
        {
            if (tagStyle == null)
            {
                tagStyle = "ChannelStripAttenuationMarkerSquare";
            }

            if (Event.current.type == EventType.Repaint)
            {
                GUIContent c = new GUIContent(string.Format("  {0} Event(s)", target.events.Count));
                Vector2 size = tagStyle.CalcSize(c);
                Rect r = rect;
                r.width = size.x;

                tagStyle.Draw(r, c, 0);
            }
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
                if (EditorUtility.DisplayDialog("警告", "确认删除分类：[" + target.attach.name + "] ?", "确定", "取消"))
                {
                    TreeItem p = parent as TreeItem;
                    p.RemoveChild(this);
                }
            });
            return menu;
        }

        void HandleCreate(object data)
        {
            AttributeTool.EventInfo evtInfo = (AttributeTool.EventInfo)data;
            DirectorEvent evt = (DirectorEvent)target.CreateSubAsset(evtInfo.eventType, HideFlags.HideInInspector);
            evt.time = state.window.playHeadTime;
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

        static GUIStyle treeRowBG;

        public override void OnTreeRowGUI(EventTreeViewGUI gui, Rect rect, int row, bool selected, bool focused, bool useBoldFont)
        {
            if (treeRowBG == null)
                treeRowBG = "ChannelStripAttenuationBar";

            if (Event.current.type == EventType.Repaint)
            {
                treeRowBG.Draw(rect, GUIContent.none, 0);
            }

            base.OnTreeRowGUI(gui, rect, row, false, false, true);

            if (_isSelected != selected && selected)
                EditorGUIUtility.PingObject(target.attach);
            _isSelected = selected;
        }

        public Object GetInspectorObject()
        {
            return target;
        }
    }
}