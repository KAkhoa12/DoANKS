using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DoAn_QLKS_dotnet.Event
{

    class GetMa
    {

        public string GetMaKH(string cccd)
        {
            string sql =string.Format("select MaKH from khachhang where cmnd ='{0}'",cccd);
            return sql;
        }

        public string GetMaPhong(string Tenphong)
        {
            string sql =string.Format("select MaPhong from phong where TenPhong =N'{0}'", Tenphong);
            return sql;
        }

        public string GetMaPT(string MaPhong,string MaKH)
        {
            string sql = string.Format("select MaPT from phieu_thue_phong where MaKH = {0} and MaPhong = {1}", MaKH, MaPhong);
            return sql;
        }

        public string GetGender_Address_KH(string MaKH)
        {
            string sql = string.Format("select phai,diachi from khachhang where MaKH={0}", MaKH);
            return sql;
        }

        public string GetDayUse(string DayCome, string DayGo)
        {
            string sql = string.Format("SELECT DATEDIFF(day, '{0}', '{1}')", DayCome, DayGo);
            return sql;
        }

        public string GetTimeUseZeroDay(string TimeCome, string TimeGo)
        {
            string sql = string.Format("SELECT DATEDIFF(HOUR, '{0}', '{1}')", TimeCome, TimeGo);
            return sql;
        }
        public string GetTimeUseManyDays(string TimeCome, string TimeGo)
        {
            string sql = string.Format("SELECT DATEDIFF(HOUR, '{0} {1}', '{2} {3}')", DateTime.Now.ToString("MM/dd/yyy"), TimeCome, DateTime.Now.AddDays(1).ToString("MM/dd/yyy"), TimeGo);
            return sql;
        }
    }
}
