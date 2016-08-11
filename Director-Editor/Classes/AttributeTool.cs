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

        [InitializeOnLoadMethod]
        static void ResetCache()
        {
            drawerTypeMap = null;
        }

        /// <summary>
        /// 获取自定义Drawer
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static EventDrawer GetDrawer(TDEvent p)
        {
            if (drawerTypeMap == null)
            {
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

            //循环查找
            Type drawerType = null;
            Type evtType = p.GetType();
            while ((evtType == typeof(TDEvent) || evtType.IsSubclassOf(typeof(TDEvent))) && drawerType == null)
            {
                drawerTypeMap.TryGetValue(evtType, out drawerType);
                evtType = evtType.BaseType;
            }

            if (drawerType == null)
                drawerType = typeof(EventDrawer);
            EventDrawer drawer = (EventDrawer)Activator.CreateInstance(drawerType);

            return drawer;
        }
    }
}
