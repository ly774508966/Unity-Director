using Tangzx.Director;

namespace TangzxInternal.Data
{
    class VOTreeData
    {
        public VORowItem root;
    }

    class SimpleTreeData : VOTreeData
    {
        private DirectorData _data;

        public SimpleTreeData(DirectorData data)
        {
            _data = data;

            root = new VORowItem();

            for (int i = 0; i < _data.eventList.Count; i++)
            {
                root.children.Add(new VOEventRowItem(_data.eventList[i]));
            }
        }
    }
}
