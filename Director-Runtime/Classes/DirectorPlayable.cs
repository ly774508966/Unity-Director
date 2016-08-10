using System;

namespace Tangzx.Director
{
    public class DirectorPlayable : Attribute
    {
        public string category;

        public DirectorPlayable(string category)
        {
            this.category = category;
        }
    }
}