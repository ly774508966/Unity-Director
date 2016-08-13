using Tangzx.Director;

namespace TangzxInternal.Data
{
    class VOTree : VOTreeItem
    {
        
    }

    class SimpleTree : VOTree
    {
        private DirectorData _data;

        public SimpleTree(DirectorData data)
        {
            _data = data;

            for (int i = 0; i < _data.eventList.Count; i++)
            {
                children.Add(new VOEventRowItem(_data.eventList[i]));
            }
        }
    }
}
