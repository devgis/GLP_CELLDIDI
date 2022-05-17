using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bddd.Common.Entites
{
    public class ZonePower
    {
        public string orderid
        {
            get;
            set;
        }
        public string hours
        {
            get;
            set;
        }
        public string StartTime
        {
            get;
            set;
        }

        public string EndTime
        {
            get;
            set;
        }

        //时间段 0~23 
        public int hour
        {
            get;
            set;
        }

        //开始区域
        public string startrectid
        {
            get;
            set;
        }
        //结束区域
        public string endrectid
        {
            get;
            set;
        }
        //权值
        public int power
        {
            get;
            set;
        }
    }
}
