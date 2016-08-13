using System;

namespace TangzxInternal
{
    public class CustomEventDrawer : Attribute
    {
        private Type _targetType;

        public CustomEventDrawer(Type targetType)
        {
            _targetType = targetType;
        }

        public Type targetType
        {
            get { return _targetType; }
        }
    }
}
