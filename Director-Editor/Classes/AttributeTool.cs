using System;
using System.Collections;
using System.Collections.Generic;
using Tangzx.Director;
using UnityEditor;

namespace TangzxInternal
{
    class AttributeTool
    {
        static Dictionary<Type, Type> drawerTypeMap;
        static EventInfo[] allEventTypes;

        [InitializeOnLoadMethod]
        static void ResetCache()
        {
            drawerTypeMap = null;
            allEventTypes = null;
        }

        public struct EventInfo
        {
            public DirectorPlayableAttribute eventAttri;
            public Type eventType;
        }

        /// <summary>
        /// 获取自定义Drawer
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static EventDrawer GetEventDrawer(DirectorEvent p)
        {
            if (drawerTypeMap == null)
            {
                drawerTypeMap = new Dictionary<Type, Type>();

                Type attrType = typeof(CustomEventDrawer);
                ArrayList al = AttributeHelper.FindEditorClassesWithAttribute(attrType);
                for (int i = 0; i < al.Count; i++)
                {
                    Type t = al[i] as Type;
                    object[] arr = t.GetCustomAttributes(attrType, false);
                    CustomEventDrawer customDrawer = (CustomEventDrawer)arr[0];
                    drawerTypeMap[customDrawer.targetType] = t;
                }
            }

            //循环查找
            Type drawerType = null;
            Type evtType = p.GetType();
            while ((evtType == typeof(DirectorEvent) || evtType.IsSubclassOf(typeof(DirectorEvent))) && drawerType == null)
            {
                drawerTypeMap.TryGetValue(evtType, out drawerType);
                if (drawerType == null)
                {
                    Type[] interfaces = evtType.GetInterfaces();
                    for (int i = 0; i < interfaces.Length && drawerType == null; i++)
                    {
                        drawerTypeMap.TryGetValue(interfaces[i], out drawerType);
                    }
                }
                evtType = evtType.BaseType;
            }

            if (drawerType == null)
                drawerType = typeof(EventDrawer);
            EventDrawer drawer = (EventDrawer)Activator.CreateInstance(drawerType);

            return drawer;
        }

        public static EventInfo[] GetAllEventTypes()
        {
            if (allEventTypes == null)
            {
                Type attrType = typeof(DirectorPlayableAttribute);
                ArrayList al = AttributeHelper.FindEditorClassesWithAttribute(attrType);
                allEventTypes = new EventInfo[al.Count];
                for (int i = 0; i < al.Count; i++)
                {
                    Type t = al[i] as Type;
                    object[] arr = t.GetCustomAttributes(attrType, false);

                    EventInfo ei = new EventInfo();
                    ei.eventAttri = arr[0] as DirectorPlayableAttribute;
                    ei.eventType = t;
                    allEventTypes[i] = ei;
                }
            }
            return allEventTypes;
        }
    }
}
