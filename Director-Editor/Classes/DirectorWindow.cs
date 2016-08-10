
using System;
using System.Reflection;
using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    public class DirectorWindow : EditorWindow
    {
        [MenuItem("Tools/Director/Open")]
        static void Open()
        {
            GetWindow<DirectorWindow>("Director");
        }

        class Styles
        {
            public static GUIStyle box;
            public static GUIStyle toolbar;
            public static GUIStyle toolbarButton;
            public static GUIStyle tooltip;
        }

        private GameObject _selectionGameObject;
        private DirectorData _data;
        
        private AreaEventEditor _eventSheetEditor;
        private SplitterState _splitterState;

        public DirectorWindow()
        {
            _eventSheetEditor = new AreaEventEditor(this);
            _eventSheetEditor.hRangeMin = 0;
            _splitterState = new SplitterState(new float[] { 300, 900, 300 }, new int[] { 300, 300, 300 }, null);

            InitTree();
        }

        public DirectorData data { get { return _data; } }

        void InitTree()
        {
            treeViewState = new TreeViewState();
            treeView = new TreeView(this, treeViewState);

            EventTreeViewDataSource dataSource = new EventTreeViewDataSource(treeView);
            EventTreeViewGUI gui = new EventTreeViewGUI(treeView);

            treeView.Init(new Rect(0, 0, 400, 500), dataSource, gui, null);
            treeView.ReloadData();
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        Rect dopeSheetRect;
        TreeView treeView;
        TreeViewState treeViewState;

        void OnGUI()
        {
            if (Selection.activeGameObject != _selectionGameObject)
            {
                _selectionGameObject = null;
                _data = null;
            }

            if (_data == null)
            {
                OnCreatorGUI();
            }

            if (_data)
            {
                RemoveNotification();
                UpdateStyles();

                OnToolBarGUI();
                GUILayout.BeginHorizontal();
                {
                    SplitterGUILayout.BeginHorizontalSplit(_splitterState);
                    //左
                    GUILayout.BeginVertical();
                    {
                        OnTreeGUI();
                    }
                    GUILayout.EndVertical();
                    //中
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        OnMainContentGUI();
                    }
                    GUILayout.EndVertical();
                    //右
                    GUILayout.Button("R");
                    SplitterGUILayout.EndHorizontalSplit();
                }
                GUILayout.EndHorizontal();
            }
        }

        void UpdateStyles()
        {
            Styles.box = new GUIStyle(GUI.skin.box);
            Styles.box.margin = new RectOffset();
            Styles.box.padding = new RectOffset();
            Styles.toolbar = new GUIStyle(EditorStyles.toolbar);
            Styles.toolbar.margin = new RectOffset();
            Styles.toolbar.padding = new RectOffset();
            Styles.toolbarButton = EditorStyles.toolbarButton;
            Styles.tooltip = GUI.skin.GetStyle("AssetLabel");
        }

        void OnCreatorGUI()
        {
            GameObject go = Selection.activeGameObject;
            Director director = null;
            if (go)
            {
                director = go.GetComponent<Director>();
            }

            if (go)
            {
                if (director && director.data)
                {
                    _selectionGameObject = go;
                    _data = director.data;
                }
                else if (GUILayout.Button("Create"))
                {
                    if (!director)
                        director = go.AddComponent<Director>();
                    _data = CreateInstance<DirectorData>();
                    _selectionGameObject = go;
                    director.data = _data;
                    AssetDatabase.CreateAsset(_data, "Assets/Director.asset");
                }
            }
            else
            {
                ShowNotification(new GUIContent("选择一个GameObject"));
            }
        }

        void OnToolBarGUI()
        {
            GUILayout.BeginHorizontal(Styles.toolbar);
            {
                //添加新项
                if (GUILayout.Button("Add New Event", Styles.toolbarButton))
                {
                    ShowCreateEventMenu();
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 显示创建事件菜单
        /// </summary>
        void ShowCreateEventMenu()
        {
            GenericMenu menu = new GenericMenu();

            Type attType = typeof(DirectorPlayable);
            Assembly assembly = attType.Assembly;
            Type[] types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type t = types[i];
                object[] arr = t.GetCustomAttributes(attType, false);
                if (arr.Length > 0)
                {
                    DirectorPlayable att = (DirectorPlayable)arr[0];
                    menu.AddItem(new GUIContent(att.category), false, HandlerCreate, t);
                }
            }

            menu.ShowAsContext();
        }

        /// <summary>
        /// 处理点击创建事件菜单项
        /// </summary>
        /// <param name="typeData"></param>
        void HandlerCreate(object typeData)
        {
            Type eventType = (Type)typeData;
            Debug.Log(eventType);
        }

        void OnMainContentGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(""), GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (rect.width > 1)
                dopeSheetRect = rect;

            OnTimeRulerGUI(rect);

            _eventSheetEditor.BeginViewGUI();
            _eventSheetEditor.OnGUI(dopeSheetRect, Vector2.zero);
            _eventSheetEditor.EndViewGUI();
        }

        /// <summary>
        /// 画时间标尺
        /// </summary>
        /// <param name="rect"></param>
        void OnTimeRulerGUI(Rect rect)
        {
            Rect timeRulerRect = rect;
            timeRulerRect.y -= 20;
            timeRulerRect.width -= 15;
            timeRulerRect.height = 20;
            _eventSheetEditor.TimeRuler(timeRulerRect, 60);
        }

        Rect treeRect;
        void OnTreeGUI()
        {
            int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(""), GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (rect.width > 1)
                treeRect = rect;

            treeView.OnGUI(treeRect, controlID);
        }
    }
}
