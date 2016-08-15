using Tangzx.Director;
using UnityEditor;
using UnityEngine;

namespace TangzxInternal
{
    abstract class DirectorWindow : EditorWindow
    {
        protected class Styles
        {
            public static GUIStyle box;
            public static GUIStyle toolbar;
            public static GUIStyle toolbarButton;
            public static GUIStyle tooltip;
            public static GUIStyle timeRulerBackground;
        }

        private TreeRootItem _treeData;

        //相当于上下文数据
        private DirectorWindowState _state;

        protected EventHierarchy eventHierarchy;
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

        public TreeRootItem treeData
        {
            set
            {
                _treeData = value;
                _state.refreshType = DirectorWindowState.RefreshType.All;
            }
            get { return _treeData; }
        }

        public DirectorWindowState state { get { return _state; } }

        public float playHeadTime
        {
            get { return _playHeadTime; }
            set
            {
                if (value < 0) value = 0;
                _playHeadTime = value;
            }
        }

        protected virtual void InitState()
        {
            _state = new DirectorWindowState(this);
        }

        protected virtual void InitHierarchy()
        {
            eventHierarchy = new EventHierarchy(_state);
        }

        protected virtual void InitSheetEditor()
        {
            eventSheetEditor = new EventSheetEditor(_state);
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
            OnCheckDataGUI();

            if (treeData != null)
            {
                RemoveNotification();
                UpdateStyles();

                _state.OnGUI();

                OnToolbarGUI();
                GUILayout.BeginHorizontal();
                {
                    SplitterGUILayout.BeginHorizontalSplit(_splitterState);
                    //左
                    Rect lRect;
                    GUILayout.BeginVertical();
                    {
                        lRect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
                    OnHierarchyGUI(lRect);
                    
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
                Styles.timeRulerBackground = "AnimationEventBackground";
            }
        }

        protected abstract void OnCheckDataGUI();

        protected virtual void OnToolbarGUI()
        {
            GUILayout.BeginHorizontal(Styles.toolbar);
            {
                //TODO
            }
            GUILayout.EndHorizontal();
        }
        
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
                    float scrollY = state.treeViewState.scrollPos.y;
                    scrollY = GUI.VerticalScrollbar(scrollbarRect, scrollY, scrollbarRect.height, 0, bottomValue);
                    state.treeViewState.scrollPos.y = scrollY;
                }

                areaRect.yMin += _state.timeRulerHeight;
                //画主体
                eventSheetEditor.OnGUI(areaRect, -state.treeViewState.scrollPos.y);
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
            timeRulerRect.height = _state.timeRulerHeight;

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

        void OnHierarchyGUI(Rect rect)
        {
            eventHierarchy.OnGUI(rect);
        }

        void OnInspectorGUI()
        {
            int lastClicked = state.treeViewState.lastClickedID;
            TreeViewItem item = state.dataSource.FindItem(lastClicked);
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
    }

    class PlayHeadDrawer : Draggable
    {
        private DirectorWindow directorWindow;
        private EventSheetEditor sheetEditor;
        private GUIStyle stylePlayhead;

        private float timeWhenDragStart;

        public PlayHeadDrawer(DirectorWindow directorWindow, EventSheetEditor sheetEditor)
        {
            this.directorWindow = directorWindow;
            this.sheetEditor = sheetEditor;
        }

        public void OnGUI(Rect rect, float time)
        {
            if (stylePlayhead == null)
            {
                stylePlayhead = "MeTransPlayhead";
            }

            Rect drawArea = rect;
            drawArea.xMin = sheetEditor.TimeToPixel2(time) - stylePlayhead.fixedWidth * 0.5f - 0.5f;
            drawArea.width = stylePlayhead.fixedWidth;
            drawArea.height = stylePlayhead.fixedHeight;

            //画竖线
            float lineYMin = isDragging ? drawArea.yMax : rect.yMin;
            TimeArea.DrawVerticalLine(sheetEditor.TimeToPixel2(time), lineYMin, rect.yMax, Color.red);
            
            //播放头标记
            if (Event.current.type == EventType.Repaint && isDragging)
                stylePlayhead.Draw(drawArea, GUIContent.none, 0);

            //拖动处理
            Rect dragArea = rect;
            dragArea.height = 15;
            HandleDrag(dragArea, 0,
                () =>
                {
                    timeWhenDragStart = directorWindow.playHeadTime;
                    directorWindow.OnDragPlayHeadStart();
                },
                () =>
                {
                    directorWindow.OnDragPlayHeadEnd();
                },
                (float offset) =>
                {
                    directorWindow.SetPlayHead(timeWhenDragStart + sheetEditor.PixelToTime2(offset));
                },
                null);
        }
    }
}
