using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_QLKS_dotnet.Validate
{
    class Contrains
    {
        XULYDULIEU xldl = new XULYDULIEU();
        public bool CheckContaninsCMND(string cmnd)
        {
            string sql = "select cmnd from khachhang";
            foreach (DataRow row in xldl.data(sql).Rows)
            {
                if (row[0].ToString().Trim() == cmnd.Trim())
                {
                    return false;
                }
            }
            return true;
        }
        public bool CheckConstainsUserName(string username)
        {
            string sql = "select TenDN from nhanvien";
            foreach (DataRow row in xldl.data(sql).Rows)
            {
                if (row[0].ToString().Trim() == username.Trim())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
