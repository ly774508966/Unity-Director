using System.Collections.Generic;
using Tangzx.Director;
using TangzxInternal.RowDrawers;
using UnityEditor;

namespace TangzxInternal.Data
{
    /// <summary>
    /// 每一行的数据基类
    /// </summary>
    public class VOTreeItem
    {
        public List<VOTreeItem> children = new List<VOTreeItem>();

        public virtual IRowDrawer GetDrawer()
        {
            return null;
        }
    }

    class VOEventRowItem : VOTreeItem
    {
        public TDEvent targetEvent;

        private EventDrawer drawer;

        public VOEventRowItem(TDEvent evt)
        {
            targetEvent = evt;
        }

        public override IRowDrawer GetDrawer()
        {
            if (drawer == null)
                drawer = AttributeTool.GetEventDrawer(targetEvent);
            drawer.target = targetEvent;
            return drawer;
        }
    }

    class VOSeqAffectedTransformItem : VOTreeItem
    {

    }
}
