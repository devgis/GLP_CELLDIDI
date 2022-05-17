using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bddd.Common.Entites
{
    //长方形区域
    public class Rect
    {
        public string rectid
        {
            get;
            set;
        }

        public double x
        {
            get;
            set;
        }

        public double y
        {
            get;
            set;
        }

        public double width
        {
            get;
            set;
        }

        public double height
        {
            get;
            set;
        }

        public bool ContainsPoint(double px, double py)
        {
            if (px >= x && px <= x + width && py >= y && py <= y + height)
            {
                return true;
            }

            if (px <= x && px >= x + width && py <= y && py >= y + height)
            {
                return true;
            }
            return false;
        }


    }
}
