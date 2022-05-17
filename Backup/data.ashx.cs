using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Bddd.Common;
using System.Data;
using Bddd.Common.Entites;

namespace Bddd
{
    /// <summary>
    /// data1 的摘要说明
    /// </summary>
    public class data : IHttpHandler
    {
        long timestart = 1477929600; //2016/11/1 00:00:00
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            #region //需要参数传入
            var xcout = 10; //x轴均分数量
            var ycout = 10; //y轴均分数量
            var startx = 116.20;
            var endx = 116.54;
            var starty = 39.83;
            var endy = 40.03;

            string t = context.Request.QueryString["t"]; //data 返回原始数据 allzone rs 返回分析结果 byhour 按时间 shour 开始小时 ehour 结束小时 byzone zoneid  onezone 单个区间分析
            if (string.IsNullOrEmpty(t))
            {
                t = "data";
            }

            ////不再读取csv直接读取数据库
            //string csvFile = context.Server.MapPath("order_20161101");
            //DataTable dtorders = CSVHelper.OpenCSV(csvFile);

            DataTable dtorders = OrderDal.Query();
            if ("data".Equals(t.ToLower()))
            {
                //添加新航
                dtorders.Columns.Add(new DataColumn() { ColumnName = "stime", DataType = typeof(System.String) });
                dtorders.Columns.Add(new DataColumn() { ColumnName = "etime", DataType = typeof(System.String) });

                foreach (DataRow row in dtorders.Rows)
                {
                    row["stime"] = Long2Datetime(Convert.ToInt64(row["starttime"])).ToString("yyyy-MM-dd HH:mm:ss");
                    row["etime"] = Long2Datetime(Convert.ToInt64(row["endtime"])).ToString("yyyy-MM-dd HH:mm:ss");
                }
                dtorders.Columns.Remove("starttime");
                dtorders.Columns.Remove("endtime");
                context.Response.Write(JsonConvert.SerializeObject(dtorders));
                return;
            }

            try
            {
                xcout = Convert.ToInt32(context.Request.QueryString["xcout"]);
            }
            catch
            { }
            try
            {
                ycout = Convert.ToInt32(context.Request.QueryString["ycout"]);
            }
            catch
            { }
            try
            {
                startx = Convert.ToDouble(context.Request.QueryString["startx"]);
            }
            catch
            { }
            try
            {
                endx = Convert.ToDouble(context.Request.QueryString["endx"]);
            }
            catch
            { }
            try
            {
                starty = Convert.ToDouble(context.Request.QueryString["starty"]);
            }
            catch
            { }
            try
            {
                endy = Convert.ToDouble(context.Request.QueryString["endy"]);
            }
            catch
            { }
            #endregion

            Random rd = new Random();

            //分割正方向
            var xdistance = endx - startx;
            var ydistance = endy - starty;

            List<Rect> lisrCells = new List<Rect>();
            //构造小多边形
            for (var i = 0; i < xcout; i++)
            {
                for (var j = 0; j < ycout; j++)
                {

                    var startx_cell = startx + i * xdistance / xcout;
                    var endx_cell = startx + (i + 1) * xdistance / xcout;
                    var starty_cell = starty + j * ydistance / ycout; ;
                    var endy_cell = starty + (j + 1) * ydistance / ycout;

                    Rect r = new Rect();
                    r.rectid = i + "-" + j;
                    r.x = startx_cell;
                    r.y = starty_cell;
                    r.height = ydistance / ycout;
                    r.width = xdistance / xcout;
                    lisrCells.Add(r);
                }
            }

            //开始计算权值
            Dictionary<string, ZonePower> dicpower = new Dictionary<string, ZonePower>();


            //此处需要按日期筛选

            string currentcarid = string.Empty;
            string currentzoneid = string.Empty;
            if (dtorders != null && dtorders.Rows.Count > 0)
            {
                if ("allzone".Equals(t.ToLower()))
                {
                    List<OrderZoneInfo> listorder = new List<OrderZoneInfo>();
                    for (int i = 0; i < dtorders.Rows.Count; i++)
                    {
                        long starttime = timestart;
                        try
                        {
                            starttime = Convert.ToInt64(dtorders.Rows[i]["starttime"]);
                        }
                        catch
                        { }

                        long endtime = timestart;
                        try
                        {
                            endtime = Convert.ToInt64(dtorders.Rows[i]["endtime"]);
                        }
                        catch
                        { }
                        int hour = GetHour(starttime);

                        //起点
                        double x1 = 0;
                        double y1 = 0;
                        try
                        {
                            x1 = Convert.ToDouble(dtorders.Rows[i]["x1"]);
                        }
                        catch
                        { }
                        try
                        {
                            y1 = Convert.ToDouble(dtorders.Rows[i]["y1"]);
                        }
                        catch
                        { }

                        string zoneidstart = getZoneid(lisrCells, x1, y1);

                        //终点
                        double x2 = 0;
                        double y2 = 0;
                        try
                        {
                            x2 = Convert.ToDouble(dtorders.Rows[i]["x2"]);
                        }
                        catch
                        { }
                        try
                        {
                            y2 = Convert.ToDouble(dtorders.Rows[i]["y2"]);
                        }
                        catch
                        { }

                        string zoneidend = getZoneid(lisrCells, x2, y2);

                        OrderZoneInfo orderZoneInfo = new OrderZoneInfo();
                        orderZoneInfo.startrectid = zoneidstart;
                        orderZoneInfo.endrectid = zoneidend;
                        orderZoneInfo.StartTime = Long2Datetime(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                        orderZoneInfo.EndTime = Long2Datetime(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                        orderZoneInfo.orderid = dtorders.Rows[i]["orderid"].ToString();
                        listorder.Add(orderZoneInfo);
                    }

                    try
                    {
                        //OrderDal.UpdateZoneData(listorder); //更新数据库 如果不注释会更新数据库 速度超慢
                    }
                    catch
                    { }
                    context.Response.Write(JsonConvert.SerializeObject(listorder));
                    return;
                }
                else if ("onezone".Equals(t.ToLower()))
                {
                    //byzone zoneid
                    string zoneid = context.Request.QueryString["zoneid"];
                    Dictionary<int, ZoneTime> diczoneinfo = new Dictionary<int, ZoneTime>();
                    //添加默认值好排序
                    for (int i = 0; i < 24; i++)
                    {
                        ZoneTime zonetime = new ZoneTime();
                        zonetime.timestring = string.Format("{0}:00~{1}:00", i, i + 1);
                        zonetime.hour = i;
                        zonetime.zoneid = zoneid;
                        zonetime.startcount = 0;
                        zonetime.endcount = 0;
                        diczoneinfo.Add(i, zonetime);
                    }
                    for (int i = 0; i < dtorders.Rows.Count; i++)
                    {

                        long starttime = timestart;
                        try
                        {
                            starttime = Convert.ToInt64(dtorders.Rows[i]["starttime"]);
                        }
                        catch
                        { }

                        long endtime = timestart;
                        try
                        {
                            endtime = Convert.ToInt64(dtorders.Rows[i]["endtime"]);
                        }
                        catch
                        { }
                        int hour = 0;

                        //起点
                        double x1 = 0;
                        double y1 = 0;
                        try
                        {
                            x1 = Convert.ToDouble(dtorders.Rows[i]["x1"]);
                        }
                        catch
                        { }
                        try
                        {
                            y1 = Convert.ToDouble(dtorders.Rows[i]["y1"]);
                        }
                        catch
                        { }

                        string zoneidstart = getZoneid(lisrCells, x1, y1);

                        //终点
                        double x2 = 0;
                        double y2 = 0;
                        try
                        {
                            x2 = Convert.ToDouble(dtorders.Rows[i]["x2"]);
                        }
                        catch
                        { }
                        try
                        {
                            y2 = Convert.ToDouble(dtorders.Rows[i]["y2"]);
                        }
                        catch
                        { }

                        string zoneidend = getZoneid(lisrCells, x2, y2);

                        if (zoneid.Equals(zoneidstart))
                        {
                            hour = GetHour(starttime);

                        }
                        else if (zoneid.Equals(zoneidend))
                        {
                            hour = GetHour(endtime);
                        }
                        
                        if (diczoneinfo.ContainsKey(hour))
                        {
                            if (zoneid.Equals(zoneidstart))
                            {
                                diczoneinfo[hour].startcount += 1;

                            }
                            else if (zoneid.Equals(zoneidend))
                            {
                                diczoneinfo[hour].endcount += 1;
                            }
                        }
                    }

                    List<ZoneTime> listZoneTime = diczoneinfo.Values.ToList<ZoneTime>();
                    context.Response.Write(JsonConvert.SerializeObject(from e in listZoneTime orderby e.hour select e));
                    return;
                }
                else if ("byhour".Equals(t.ToLower()))
                {
                    //byhour 按时间 shour 开始小时 ehour 结束小时 
                    int starthour = 0;
                    try
                    {
                        starthour = Convert.ToInt32(context.Request.QueryString["shour"]);
                    }
                    catch
                    { }

                    int endhour = 0;
                    try
                    {
                        endhour = Convert.ToInt32(context.Request.QueryString["ehour"]);
                    }
                    catch
                    { }

                    if (starthour > endhour)
                    {
                        int temp = starthour;
                        starthour = endhour;
                        endhour = temp;
                    }

                    for (int i = 0; i < dtorders.Rows.Count; i++)
                    {
                        long starttime = timestart;
                        try
                        {
                            starttime = Convert.ToInt64(dtorders.Rows[i]["starttime"]);
                        }
                        catch
                        { }

                        long endtime = timestart;
                        try
                        {
                            endtime = Convert.ToInt64(dtorders.Rows[i]["endtime"]);
                        }
                        catch
                        { }
                        int shour = GetHour(starttime);
                        int ehour = GetHour(endtime);

                        if ((shour >= starthour && shour <= endhour) || ((ehour >= starthour && ehour <= endhour)))
                        {

                            //起点
                            double x1 = 0;
                            double y1 = 0;
                            try
                            {
                                x1 = Convert.ToDouble(dtorders.Rows[i]["x1"]);
                            }
                            catch
                            { }
                            try
                            {
                                y1 = Convert.ToDouble(dtorders.Rows[i]["y1"]);
                            }
                            catch
                            { }

                            string zoneidstart = getZoneid(lisrCells, x1, y1);

                            //终点
                            double x2 = 0;
                            double y2 = 0;
                            try
                            {
                                x2 = Convert.ToDouble(dtorders.Rows[i]["x2"]);
                            }
                            catch
                            { }
                            try
                            {
                                y2 = Convert.ToDouble(dtorders.Rows[i]["y2"]);
                            }
                            catch
                            { }

                            string zoneidend = getZoneid(lisrCells, x2, y2);

                            if (!string.IsNullOrEmpty(zoneidstart) && !string.IsNullOrEmpty(zoneidend) && zoneidstart != zoneidend)
                            {
                                string key1 = zoneidstart + "|" + zoneidend;
                                if (dicpower.ContainsKey(key1))
                                {
                                    dicpower[key1].power += 1;
                                }
                                else
                                {
                                    ZonePower zonePower = new ZonePower();
                                    zonePower.power = 1;
                                    zonePower.startrectid = zoneidstart;
                                    zonePower.endrectid = zoneidend;
                                    dicpower.Add(key1, zonePower);
                                }
                            }
                        }
                    }
                    List<ZonePower> lsbyhourpower = dicpower.Values.ToList<ZonePower>();
                    context.Response.Write(JsonConvert.SerializeObject(from e in lsbyhourpower orderby e.hour select e));
                    return;
                }
                else if ("byzone".Equals(t.ToLower()))
                {
                    //byzone zoneid
                    string szoneid = context.Request.QueryString["szoneid"];
                    string ezoneid = context.Request.QueryString["ezoneid"];

                    //添加默认值好排序
                    for (int i = 0; i < 24; i++)
                    {
                        string key1 = szoneid + "|" + ezoneid + "|" + i;
                        ZonePower zonePower = new ZonePower();
                        zonePower.power = 0;
                        zonePower.hours = string.Format("{0}:00~{1}:00", i, i + 1);
                        zonePower.hour = i;
                        zonePower.startrectid = szoneid;
                        zonePower.endrectid = ezoneid;
                        dicpower.Add(key1, zonePower);
                    }
                    for (int i = 0; i < dtorders.Rows.Count; i++)
                    {

                        long starttime = timestart;
                        try
                        {
                            starttime = Convert.ToInt64(dtorders.Rows[i]["starttime"]);
                        }
                        catch
                        { }

                        long endtime = timestart;
                        try
                        {
                            endtime = Convert.ToInt64(dtorders.Rows[i]["endtime"]);
                        }
                        catch
                        { }
                        int hour = GetHour(starttime);

                        //起点
                        double x1 = 0;
                        double y1 = 0;
                        try
                        {
                            x1 = Convert.ToDouble(dtorders.Rows[i]["x1"]);
                        }
                        catch
                        { }
                        try
                        {
                            y1 = Convert.ToDouble(dtorders.Rows[i]["y1"]);
                        }
                        catch
                        { }

                        string zoneidstart = getZoneid(lisrCells, x1, y1);

                        //终点
                        double x2 = 0;
                        double y2 = 0;
                        try
                        {
                            x2 = Convert.ToDouble(dtorders.Rows[i]["x2"]);
                        }
                        catch
                        { }
                        try
                        {
                            y2 = Convert.ToDouble(dtorders.Rows[i]["y2"]);
                        }
                        catch
                        { }

                        string zoneidend = getZoneid(lisrCells, x2, y2);

                        if ((szoneid == zoneidstart && ezoneid == zoneidend) || (ezoneid == zoneidstart && szoneid == zoneidend))
                        {
                            string key1 = zoneidstart + "|" + zoneidend + "|" + hour;
                            string key2 = zoneidend + "|" + zoneidstart + "|" + hour;
                            if (dicpower.ContainsKey(key1) || dicpower.ContainsKey(key2))
                            {
                                if (dicpower.ContainsKey(key1))
                                {
                                    dicpower[key1].power += 1;
                                }
                                else
                                {
                                    dicpower[key2].power += 1;
                                }
                            }
                            else
                            {
                                ZonePower zonePower = new ZonePower();
                                zonePower.power = 1;
                                zonePower.hours = string.Format("{0}:00~{1}:00", hour, hour + 1);
                                zonePower.hour = hour;
                                zonePower.startrectid = zoneidstart;
                                zonePower.endrectid = zoneidend;
                                dicpower.Add(key1, zonePower);
                            }
                        }
                    }

                    List<ZonePower> lsbyhourpower = dicpower.Values.ToList<ZonePower>();
                    context.Response.Write(JsonConvert.SerializeObject(from e in lsbyhourpower orderby e.hour select e));
                    return;
                }

                for (int i = 0; i < dtorders.Rows.Count; i++)
                {
                    long starttime = timestart;
                    try
                    {
                        starttime = Convert.ToInt64(dtorders.Rows[i]["starttime"]);
                    }
                    catch
                    { }

                    long endtime = timestart;
                    try
                    {
                        endtime = Convert.ToInt64(dtorders.Rows[i]["endtime"]);
                    }
                    catch
                    { }
                    int hour = GetHour(starttime);

                    //起点
                    double x1 = 0;
                    double y1 = 0;
                    try
                    {
                        x1 = Convert.ToDouble(dtorders.Rows[i]["x1"]);
                    }
                    catch
                    { }
                    try
                    {
                        y1 = Convert.ToDouble(dtorders.Rows[i]["y1"]);
                    }
                    catch
                    { }

                    string zoneidstart = getZoneid(lisrCells, x1, y1);

                    //终点
                    double x2 = 0;
                    double y2 = 0;
                    try
                    {
                        x2 = Convert.ToDouble(dtorders.Rows[i]["x2"]);
                    }
                    catch
                    { }
                    try
                    {
                        y2 = Convert.ToDouble(dtorders.Rows[i]["y2"]);
                    }
                    catch
                    { }

                    string zoneidend = getZoneid(lisrCells, x2, y2);

                    if (!string.IsNullOrEmpty(zoneidstart) && !string.IsNullOrEmpty(zoneidend) && zoneidstart != zoneidend)
                    {
                        string key1 = zoneidstart + "|" + zoneidend + "|" + hour;
                        if (dicpower.ContainsKey(key1))
                        {
                            dicpower[key1].power += 1;
                        }
                        else
                        {
                            ZonePower zonePower = new ZonePower();
                            zonePower.orderid = dtorders.Rows[i]["orderid"].ToString();
                            zonePower.power = 1;
                            zonePower.hour = hour;
                            zonePower.startrectid = zoneidstart;
                            zonePower.endrectid = zoneidend;
                            zonePower.StartTime = Long2Datetime(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                            zonePower.EndTime = Long2Datetime(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                            dicpower.Add(key1, zonePower);
                        }
                    }

                    //List<ZonePower> lsbyhourpower = dicpower.Values.ToList<ZonePower>();
                    //context.Response.Write(JsonConvert.SerializeObject(lsbyhourpower));
                    //return;
                }
            }

            //ZonePower 转zoneinfo
            List<ZonePower> listzonepower = dicpower.Values.ToList<ZonePower>();
            if ("rs".Equals(t.ToLower()))
            {
                context.Response.Write(JsonConvert.SerializeObject(listzonepower));
                return;
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(""));
                return;
            }

            //后边无用
            List<ZoneInfo> listzoneinfo = new List<ZoneInfo>();

            foreach (ZonePower power in listzonepower)
            {
                double rdl = (rd.NextDouble() - 0.5) / 3000;
                Rect start = getZone(lisrCells, power.startrectid);
                Rect end = getZone(lisrCells, power.endrectid);
                ZoneInfo info = new ZoneInfo();
                info.startx = start.x + start.width / 2 + rdl;
                info.starty = start.y + start.height / 2 + rdl;

                info.endx = end.x + end.width / 2 + rdl;
                info.endy = end.y + end.height / 2 + rdl;
                info.power = power.power;
                listzoneinfo.Add(info);
            }
            //在此写入您的处理程序实现。
            //string typestring = context.Request.QueryString["t"];
            string json = string.Empty;
            json = JsonConvert.SerializeObject(listzoneinfo);
            context.Response.Write(json);
        }

        public DateTime Long2Datetime(long l)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dt = startTime.AddSeconds(l);
            return dt;
        }

        public Rect getZone(List<Rect> lisrCells, string zoneid)
        {
            if (lisrCells != null & lisrCells.Count >= 0)
            {
                foreach (var cell in lisrCells)
                {
                    if (zoneid.Equals(cell.rectid))
                    {
                        return cell;
                    }
                }
            }
            return null;
        }
        public string getZoneid(List<Rect> lisrCells, double x, double y)
        {
            if (lisrCells != null & lisrCells.Count >= 0)
            {
                foreach (var cell in lisrCells)
                {
                    if (cell.ContainsPoint(x, y))
                    {
                        return cell.rectid;
                    }
                }
            }
            return string.Empty;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //获取戳的时间段
        public int GetHour(long time)
        {
            long h = 0;
            long ts = time - timestart;
            h = ts / 3600;
            //if(ts%3600>0)
            //{
            //    h=ts/3600+1;
            //}
            //else
            //{
            //    h=ts/3600;
            //}
            if (h > 23 || h < 0)
            {
                h = 0;
            }

            return (int)h;
        }
    }
}