
using System;
using System.Reflection;
using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    class DirectorWindow : EditorWindow
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

        //相当于上下文数据
        private DirectorWindowState _state;

        private EventHierarchy _eventHierarchy;
        Rect _eventHierarchyRect;
        private EventSheetEditor _eventSheetEditor;
        Rect _eventSheetRect;

        private EventInspector _eventInspector;
        private SplitterState _splitterState;

        public DirectorWindow()
        {
            _state = new DirectorWindowState(this);
            _eventHierarchy = new EventHierarchy(this);

            _eventSheetEditor = new EventSheetEditor(this);
            _eventSheetEditor.hRangeMin = 0;
            _eventSheetEditor.SetShownHRange(0, 10);
            _eventSheetEditor.vRangeLocked = true;
            _eventSheetEditor.vSlider = false;
            _eventSheetEditor.scaleWithWindow = true;
            _eventSheetEditor.margin = 40;
            _eventSheetEditor.frameRate = 30;

            _eventInspector = new EventInspector();
            _splitterState = new SplitterState(new float[] { 200, 900, 300 }, new int[] { 200, 300, 300 }, null);
        }

        public DirectorData data
        {
            set { _data = value; _state.refreshType = DirectorWindowState.RefreshType.All; }
            get { return _data; }
        }

        public DirectorWindowState state { get { return _state; } }
        
        void OnInspectorUpdate()
        {
            Repaint();
        }

        void OnGUI()
        {
            if (Selection.activeGameObject != _selectionGameObject)
            {
                _selectionGameObject = null;
                data = null;
            }

            if (data == null)
            {
                OnCreatorGUI();
            }

            if (data)
            {
                RemoveNotification();
                UpdateStyles();

                _state.OnGUI();

                OnToolBarGUI();
                GUILayout.BeginHorizontal();
                {
                    SplitterGUILayout.BeginHorizontalSplit(_splitterState);
                    //左
                    GUILayout.BeginVertical();
                    {
                        OnHierarchyGUI();
                    }
                    GUILayout.EndVertical();
                    Rect lRect = GUILayoutUtility.GetLastRect();

                    //中
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        OnMainContentGUI();
                    }
                    GUILayout.EndVertical();

                    //右
                    OnRightGUI();
                    SplitterGUILayout.EndHorizontalSplit();

                    //在 [左与中] 中间画条黑线
                    GUI.color = Color.black;
                    lRect.x = lRect.xMax;
                    lRect.width = 1;
                    GUI.DrawTexture(lRect, EditorGUIUtility.whiteTexture);
                    GUI.color = Color.white;
                }
                GUILayout.EndHorizontal();
            }
        }

        void UpdateStyles()
        {
            if (Styles.box == null)
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
                    data = director.data;
                }
                else if (GUILayout.Button("Create"))
                {
                    if (!director)
                        director = go.AddComponent<Director>();
                    data = CreateInstance<DirectorData>();
                    _selectionGameObject = go;
                    director.data = data;
                    AssetDatabase.CreateAsset(data, "Assets/Director.asset");
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
            TDEvent p = data.Add(eventType);
            // Refresh
            _state.refreshType = DirectorWindowState.RefreshType.All;
        }

        void OnMainContentGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(""), GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (rect.width > 1)
                _eventSheetRect = rect;

            OnTimeRulerGUI(rect);

            _eventSheetEditor.BeginViewGUI();
            {
                rect = _eventSheetRect;
                //排除垂直滚动条的宽
                rect.width -= 15;

                //画主体
                _eventSheetEditor.OnGUI(rect, Vector2.zero);

                //画最右边的垂直滚动条
                {
                    Rect scrollbarRect = new Rect(rect.xMax, rect.yMin, 15, rect.height - 15);
                    GUI.VerticalScrollbar(scrollbarRect, 0, 1, 0, 0);
                }
            }
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

            _eventSheetEditor.TimeRuler(timeRulerRect, _eventSheetEditor.frameRate);
        }

        void OnHierarchyGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(""), GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (rect.width > 1)
                _eventHierarchyRect = rect;
            _eventHierarchy.OnGUI(_eventHierarchyRect);
        }

        void OnRightGUI()
        {
            TDEvent evt = _eventSheetEditor.selected;
            if (evt)
            {
                _eventInspector.OnGUI(evt);
            }
        }
    }
}
