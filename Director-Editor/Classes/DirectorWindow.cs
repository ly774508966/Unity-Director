using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    abstract class DirectorWindow : EditorWindow
    {
        protected class Styles
        {
            public static GUIStyle timeRulerBackground;
        }

        private TreeRootItem _treeRootItem;

        //相当于上下文数据
        private DirectorWindowState _windowState;

        protected EventHierarchy eventHierarchy;
        Rect _hierarchyRect;
        protected EventSheetEditor eventSheetEditor;
        Rect _eventSheetRect;

        private EventInspector _eventInspector;
        private SplitterState _splitterState;

        //播放头位置
        private float _playHeadTime;
        //播放头
        private PlayHeadDrawer _playHeadDrawer;

        public DirectorWindow()
        {
            InitState();
            InitHierarchy();
            InitSheetEditor();
            InitEventInspector();

            _splitterState = new SplitterState(new float[] { 200, 900, 300 }, new int[] { 200, 300, 300 }, null);
            _playHeadDrawer = new PlayHeadDrawer(this, eventSheetEditor);
        }

        public TreeRootItem treeRootItem
        {
            set
            {
                _treeRootItem = value;
                _windowState.ReloadData();
            }
            get { return _treeRootItem; }
        }

        public DirectorWindowState windowState { get { return _windowState; } }

        public float playHeadTime
        {
            get { return _playHeadTime; }
            set
            {
                if (value < 0) value = 0;
                _playHeadTime = value;
            }
        }

        protected virtual void OnDisable()
        {

            EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
        }

        protected virtual void OnEnable()
        {

            EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
        }

        protected virtual void OnPlayModeStateChanged()
        {

        }

        protected virtual void InitState()
        {
            _windowState = new DirectorWindowState(this);
        }

        protected virtual void InitHierarchy()
        {
            eventHierarchy = new EventHierarchy(_windowState);
        }

        protected virtual void InitSheetEditor()
        {
            eventSheetEditor = new EventSheetEditor(_windowState);
            eventSheetEditor.hRangeMin = 0;
            eventSheetEditor.vRangeLocked = true;
            eventSheetEditor.vSlider = false;
            eventSheetEditor.margin = 40;
            eventSheetEditor.frameRate = 60;
        }

        protected virtual void InitEventInspector()
        {
            _eventInspector = new EventInspector();
        }
        
        /// <summary>
        /// 设置播放头时间
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetPlayHead(float value)
        {
            playHeadTime = value;
        }

        void OnInspectorUpdate()
        {
            Repaint();
        }

        protected virtual void OnGUI()
        {
            UpdateStyles();
            OnToolbarGUI();
            OnCheckDataGUI();

            if (treeRootItem != null)
            {
                RemoveNotification();

                _windowState.OnGUI();

                GUILayout.BeginHorizontal();
                {
                    SplitterGUILayout.BeginHorizontalSplit(_splitterState);
                    //左
                    Rect lRect;
                    GUILayout.BeginVertical();
                    {
                        lRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                        if (lRect.width > 1)
                            _hierarchyRect = lRect;
                    }
                    GUILayout.EndVertical();

                    //中
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    {
                        OnMainContentGUI();
                    }
                    GUILayout.EndVertical();

                    //右
                    OnInspectorGUI();
                    SplitterGUILayout.EndHorizontalSplit();

                    //重新画左:
                    //这里有一个奇怪的问题：如果先画左再画中间会出现中间的滚动条失灵的BUG
                    OnHierarchyGUI(_hierarchyRect);
                    
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
            if (Styles.timeRulerBackground == null)
            {
                Styles.timeRulerBackground = AnimationWindowStyles.eventBackground;
            }
        }

        protected abstract void OnCheckDataGUI();

        protected abstract void OnToolbarGUI();
        
        void OnMainContentGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(""), GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (rect.width > 1)
                _eventSheetRect = rect;

            //画标尺
            OnTimeRulerGUI(rect);
            //播放头
            OnPlayHeadGUI(rect);

            eventSheetEditor.BeginViewGUI();
            {
                Rect areaRect = _eventSheetRect;
                //排除垂直滚动条的宽
                areaRect.width -= DirectorWindowState.SCROLLBAR_WIDTH;

                //画最右边的垂直滚动条
                {
                    Rect scrollbarRect = areaRect;
                    scrollbarRect.x = areaRect.xMax;
                    scrollbarRect.width = DirectorWindowState.SCROLLBAR_WIDTH;

                    float bottomValue = Mathf.Max(eventHierarchy.contentHeight, scrollbarRect.height);
                    float scrollY = windowState.treeViewState.scrollPos.y;
                    scrollY = GUI.VerticalScrollbar(scrollbarRect, scrollY, scrollbarRect.height, 0, bottomValue);
                    windowState.treeViewState.scrollPos.y = scrollY;
                }

                areaRect.yMin += _windowState.timeRulerHeight;
                //画主体
                eventSheetEditor.OnGUI(areaRect, -windowState.treeViewState.scrollPos.y);
            }
            eventSheetEditor.EndViewGUI();
        }

        /// <summary>
        /// 画时间标尺
        /// </summary>
        /// <param name="rect"></param>
        void OnTimeRulerGUI(Rect rect)
        {
            Rect timeRulerRect = rect;
            timeRulerRect.width -= DirectorWindowState.SCROLLBAR_WIDTH;
            timeRulerRect.height = _windowState.timeRulerHeight;

            if (Event.current.type == EventType.Repaint)
                Styles.timeRulerBackground.Draw(timeRulerRect, GUIContent.none, 0);

            eventSheetEditor.TimeRuler(timeRulerRect, eventSheetEditor.frameRate);
        }

        void OnPlayHeadGUI(Rect rect)
        {
            rect.width -= DirectorWindowState.SCROLLBAR_WIDTH;
            GUI.BeginGroup(rect);
            rect.y = rect.x = 0;
            _playHeadDrawer.OnGUI(rect, playHeadTime);
            GUI.EndGroup();
        }

        protected virtual void OnHierarchyGUI(Rect rect)
        {
            eventHierarchy.OnGUI(rect);
        }

        void OnInspectorGUI()
        {
            int lastClicked = windowState.treeViewState.lastClickedID;
            TreeViewItem item = windowState.dataSource.FindItem(lastClicked);
            if (item != null && item is IInspectorItem)
            {
                IInspectorItem iii = item as IInspectorItem;
                Object obj = iii.GetInspectorObject();
                if (obj)
                    _eventInspector.OnGUI(obj);
            }
        }

        public virtual void OnDragPlayHeadStart()
        {
            
        }

        public virtual void OnDragPlayHeadEnd()
        {
            
        }

        public BottomTreeItem CreateBottomTreeItem(TreeViewItem parent)
        {
            return new BottomTreeItem(parent);
        }
    }
}