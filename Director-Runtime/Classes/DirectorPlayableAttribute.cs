using System;

namespace Tangzx.Director
{
    public class DirectorPlayableAttribute : Attribute
    {
        public string category;

        public bool hideDuration;

        public DirectorPlayableAttribute(string category, bool hideDuration = false)
        {
            this.category = category;
            this.hideDuration = hideDuration;
        }
    }
}