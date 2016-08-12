using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class EventHierarchy
    {
        TreeView treeView;
        TreeViewState treeViewState;

        public EventHierarchy(DirectorWindow window)
        {
            treeViewState = new TreeViewState();
            treeView = new TreeView(window, treeViewState);

            window.state.treeViewState = treeViewState;
            window.state.dataSource = new EventTreeViewDataSource(treeView, window);
            EventTreeViewGUI gui = new EventTreeViewGUI(treeView, window);

            treeView.Init(new Rect(0, 0, 400, 500), window.state.dataSource, gui, null);
            treeView.ReloadData();
        }

        public void OnGUI(Rect rect)
        {
            treeView.OnEvent();
            int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
            treeView.OnGUI(rect, controlID);
        }
    }
}
