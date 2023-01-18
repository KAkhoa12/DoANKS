using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_QLKS_dotnet.InformationProcessing
{

    public class GetID
    {

        public string MaKH(string cccd)
        {
            string sql =string.Format("select MaKH from khachhang where cmnd ='{0}'",cccd);
            return sql;
        }

        public string MaPhong(string Tenphong)
        {
            string sql =string.Format("select MaPhong from phong where TenPhong =N'{0}'", Tenphong);
            return sql;
        }
        public string MaLoaiPhong(string TenLoai)
        {
            string sql = string.Format("select MaLoai from loaiphong where TenLoai =N'{0}'", TenLoai);
            return sql;
        }
        public string MaPT(string MaPhong,string MaKH)
        {
            string sql = string.Format("select MaPT from phieu_thue_phong where MaKH = {0} and MaPhong = {1}", MaKH, MaPhong);
            return sql;
        }

        
        public string MaQuyen(string quyen)
        {
            string sql = string.Format("select MaQuyen from phanquyen where TenQuyen = N'{0}'", quyen);
            return sql;
        }


    }
}
