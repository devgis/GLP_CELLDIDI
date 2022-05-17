using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Bddd.Common.Entites;

namespace Bddd.Common
{
    public class OrderDal
    {
        public static DataTable Query()
        {
            string sql = "select * from t_orders";
            return SQLHelper.Instance.GetDataTable(sql);
        }

        public static bool UpdateZoneData(List<OrderZoneInfo> listzone)
        {
            if (listzone == null || listzone.Count < 0)
            {
                return false;
            }
            List<string> listsqls=new List<string>();
            List<SqlParameter[]> listparameters = new List<SqlParameter[]>();

            string sql = "delete from t_orderszone";
            listsqls.Add(sql);
            listparameters.Add(null);

            SQLHelper.Instance.ExecSql(sql);

            foreach (OrderZoneInfo zoneinfo in listzone)
            {
                //OrderZoneInfo orderZoneInfo = new OrderZoneInfo();
                //orderZoneInfo.startrectid = zoneidstart;
                //orderZoneInfo.endrectid = zoneidend;
                //orderZoneInfo.StartTime = Long2Datetime(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                //orderZoneInfo.EndTime = Long2Datetime(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                //orderZoneInfo.orderid = dtorders.Rows[i]["orderid"].ToString();

                sql = "INSERT INTO t_orderszone (orderid,startrectid,endrectid,StartTime,EndTime) VALUES(@orderid,@startrectid,@endrectid,@StartTime,@EndTime)";
                SqlParameter[] par = new SqlParameter[] { 
                    new SqlParameter("@orderid",zoneinfo.orderid),
                    new SqlParameter("@startrectid",zoneinfo.startrectid),
                    new SqlParameter("@endrectid",zoneinfo.endrectid),
                    new SqlParameter("@StartTime",zoneinfo.StartTime),
                    new SqlParameter("@EndTime",zoneinfo.EndTime)
                };
                listsqls.Add(sql);
                listparameters.Add(par);

                SQLHelper.Instance.ExecSql(sql,par);

            }

            return true;
        }

        public static bool UpdateZoneDataTran(List<OrderZoneInfo> listzone)
        {
            if (listzone == null || listzone.Count < 0)
            {
                return false;
            }
            List<string> listsqls=new List<string>();
            List<SqlParameter[]> listparameters = new List<SqlParameter[]>();

            string sql = "delete from t_orderszone";
            listsqls.Add(sql);
            listparameters.Add(null);

            foreach (OrderZoneInfo zoneinfo in listzone)
            {
                //OrderZoneInfo orderZoneInfo = new OrderZoneInfo();
                //orderZoneInfo.startrectid = zoneidstart;
                //orderZoneInfo.endrectid = zoneidend;
                //orderZoneInfo.StartTime = Long2Datetime(starttime).ToString("yyyy-MM-dd HH:mm:ss");
                //orderZoneInfo.EndTime = Long2Datetime(endtime).ToString("yyyy-MM-dd HH:mm:ss");
                //orderZoneInfo.orderid = dtorders.Rows[i]["orderid"].ToString();

                sql = "INSERT INTO t_orderszone (orderid,startrectid,endrectid,StartTime,EndTime) VALUES(@orderid,@startrectid,@endrectid,@StartTime,@EndTime)";
                SqlParameter[] par = new SqlParameter[] { 
                    new SqlParameter("@orderid",zoneinfo.orderid),
                    new SqlParameter("@startrectid",zoneinfo.startrectid),
                    new SqlParameter("@endrectid",zoneinfo.endrectid),
                    new SqlParameter("@StartTime",zoneinfo.StartTime),
                    new SqlParameter("@EndTime",zoneinfo.EndTime)
                };
                listsqls.Add(sql);
                listparameters.Add(par);

            }

            return SQLHelper.Instance.ExecSqlByTran(listsqls,listparameters);
        }
    }
}
