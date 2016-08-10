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
        static Dictionary<Type, EventDrawer> drawerInstMap;

        [InitializeOnLoadMethod]
        static void ResetCache()
        {
            drawerInstMap = null;
            drawerTypeMap = null;
        }

        /// <summary>
        /// 获取自定义Drawer
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static EventDrawer GetDrawer(Playable p)
        {
            if (drawerTypeMap == null)
            {
                drawerInstMap = new Dictionary<Type, EventDrawer>();
                drawerTypeMap = new Dictionary<Type, Type>();

                Type attrType = typeof(CustomPlayableDrawer);
                ArrayList al = AttributeHelper.FindEditorClassesWithAttribute(attrType);
                for (int i = 0; i < al.Count; i++)
                {
                    Type t = al[i] as Type;
                    object[] arr = t.GetCustomAttributes(attrType, false);
                    CustomPlayableDrawer customDrawer = (CustomPlayableDrawer)arr[0];
                    drawerTypeMap[customDrawer.targetType] = t;
                }
            }

            Type drawerType = null;
            drawerTypeMap.TryGetValue(p.GetType(), out drawerType);
            if (drawerType == null)
                drawerType = typeof(EventDrawer);
            EventDrawer drawer = null;
            drawerInstMap.TryGetValue(drawerType, out drawer);
            if (drawer == null)
            {
                drawer = (EventDrawer)Activator.CreateInstance(drawerType);
                drawerInstMap[drawerType] = drawer;
            }

            return drawer;
        }
    }
}
