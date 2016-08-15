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
                SequencerDataHolder holder = null;
                if (selectGO)
                {
                    holder = selectGO.GetComponent<SequencerDataHolder>();
                    if (holder == null && GUILayout.Button("Create"))
                    {
                        holder = selectGO.AddComponent<SequencerDataHolder>();
                        SequencerData data = CreateInstance<SequencerData>();
                        AssetDatabase.CreateAsset(data, string.Format("Assets/{0}.asset", holder.name));
                        holder.data = data;
                    }
                }

                if (holder)
                {
                    _dataGO = selectGO;
                    SetData(holder.data);
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
                //TODO
                if (GUILayout.Button("Remove", Styles.toolbarButton))
                {

                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        public DragAndDropVisualMode DoDrag(GameObject[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                GameObject go = objs[i];
                SequencerEventContainer ec = null;
                var e = _data.GetEnumerator();
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
                    _data.AddContainer(ec);
                }
            }

            state.ReloadData();

            return DragAndDropVisualMode.Move;
        }
    }
}
