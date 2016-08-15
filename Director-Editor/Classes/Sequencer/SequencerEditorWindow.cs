using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    /// <summary>
    /// 过场编辑器
    /// </summary>
    class SequencerEditorWindow : DirectorWindow
    {
        [MenuItem("Tools/Director/SequencerEditor")]
        static void Open()
        {
            GetWindow<SequencerEditorWindow>("Sequencer");
        }

        private GameObject _dataGO;
        private SequencerData _data;

        protected override void InitHierarchy()
        {
            base.InitHierarchy();
            eventHierarchy.treeView.dragging = new EventTreeViewDragging(eventHierarchy.treeView, this);
        }

        protected override void OnCheckDataGUI()
        {
            GameObject selectGO = Selection.activeGameObject;

            if (_dataGO != selectGO)
            {
                SequencerData data = null;
                if (selectGO)
                {
                    data = selectGO.GetComponent<SequencerData>();
                    if (data == null && GUILayout.Button("Create"))
                    {
                        data = selectGO.AddComponent<SequencerData>();
                    }
                }

                if (data)
                {
                    _dataGO = selectGO;
                    SetData(data);
                }
                else
                {
                    _dataGO = null;
                    SetData(null);
                }
            }

            if (treeData == null)
            {
                ShowNotification(new GUIContent("Select GameObject"));
            }
        }

        void SetData(SequencerData sd)
        {
            _data = sd;
            if (sd)
                treeData = new SequencerRootItem(sd);
            else
                treeData = null;
        }

        protected override void OnToolbarGUI()
        {
            GUILayout.BeginHorizontal(Styles.toolbar);
            {
                if (GUILayout.Button("Add", Styles.toolbarButton))
                {
                    CreateCategoryWindow w = GetWindow<CreateCategoryWindow>();
                    w.mainWindow = this;
                }
                if (GUILayout.Button("Remove", Styles.toolbarButton))
                {

                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public DragAndDropVisualMode DoDrag(GameObject[] objs, SequencerCategory sc)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                GameObject go = objs[i];
                SequencerEventContainer ec = null;
                var e = sc.GetEnumerator();
                while (e.MoveNext())
                {
                    if (e.Current.attach == go.transform)
                    {
                        ec = e.Current;
                        break;
                    }
                }

                if (ec == null)
                {
                    ec = _data.CreateSubAsset<SequencerEventContainer>();
                    ec.attach = go.transform;
                    sc.AddContainer(ec);
                }
            }

            state.ReloadData();

            return DragAndDropVisualMode.Move;
        }

        public void CreateCategory(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var e = _data.GetEnumerator();
                while (e.MoveNext())
                {
                    if (e.Current.categoryName == name)
                        return;
                }

                SequencerCategory sc = _data.CreateSubAsset<SequencerCategory>();
                sc.categoryName = name;
                _data.AddCategory(sc);
                state.ReloadData();
            }
        }
    }
    
    class CreateCategoryWindow : EditorWindow
    {
        string input = "Main";

        public SequencerEditorWindow mainWindow;

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Name:");
                input = GUILayout.TextField(input);
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("OK"))
                {
                    mainWindow.CreateCategory(input);
                    Close();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
