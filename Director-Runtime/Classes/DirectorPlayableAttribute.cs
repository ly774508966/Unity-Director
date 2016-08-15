using System;

namespace Tangzx.Director
{
    public class DirectorPlayableAttribute : Attribute
    {
        public string category;

        public DirectorPlayableAttribute(string category)
        {
            this.category = category;
        }
    }
}