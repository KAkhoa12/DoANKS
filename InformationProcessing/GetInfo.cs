using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace DoAn_QLKS_dotnet.InformationProcessing
{
    public class GetInfo
    {
        public string Gender_Address_KH(string MaKH)
        {
            string sql = string.Format("select phai,diachi from khachhang where MaKH={0}", MaKH);
            return sql;
        }

        public string DayUse(string DayCome, string DayGo)
        {
            string sql = string.Format("SELECT DATEDIFF(day, '{0}', '{1}')", DayCome, DayGo);
            return sql;
        }

        public string TimeUseZeroDay(string TimeCome, string TimeGo)
        {
            string sql = string.Format("SELECT DATEDIFF(HOUR, '{0}', '{1}')", TimeCome, TimeGo);
            return sql;
        }
        public string TimeUseManyDays(string TimeCome, string TimeGo)
        {
            string sql = string.Format("SELECT DATEDIFF(HOUR, '{0} {1}', '{2} {3}')", DateTime.Now.ToString("MM/dd/yyy"), TimeCome, DateTime.Now.AddDays(1).ToString("MM/dd/yyy"), TimeGo);
            return sql;
        }
    }
}
