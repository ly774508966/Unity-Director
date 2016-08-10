using System;

namespace TangzxInternal
{
    public class CustomPlayableDrawer : Attribute
    {
        private Type _targetType;

        public CustomPlayableDrawer(Type targetType)
        {
            _targetType = targetType;
        }

        public Type targetType
        {
            get { return _targetType; }
        }
    }
}
