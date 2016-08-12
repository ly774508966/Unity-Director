using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventHierarchy
    {
        TreeView treeView;
        TreeViewState treeViewState;

        public EventHierarchy(DirectorWindowState state)
        {
            treeViewState = new TreeViewState();
            treeView = new TreeView(state.window, treeViewState);

            state.treeViewState = treeViewState;
            state.dataSource = new EventTreeViewDataSource(treeView, state);
            EventTreeViewGUI gui = new EventTreeViewGUI(treeView, state);

            treeView.Init(new Rect(0, 0, 400, 500), state.dataSource, gui, null);
            treeView.ReloadData();
        }

        public void OnGUI(Rect rect)
        {
            treeView.OnEvent();
            int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
            treeView.OnGUI(rect, controlID);
        }

        public float contentHeight
        {
            get
            {
                return treeView.gui.GetTotalSize().y;
            }
        }
    }
}
