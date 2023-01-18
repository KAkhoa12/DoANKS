using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn_QLKS_dotnet.Event
{
    class XULYMA
    {
        XULYDULIEU xldl = new XULYDULIEU();

        public string getOneRow(string sql)
        {
            DataTable tb = new DataTable();
            tb = xldl.data(sql);
            DataRow ma = tb.Rows[0];
            return ma.ItemArray[0].ToString();
        }
        public string Get_Gender_Address_KH(string sql)
        {
            string  gender_address="";
            DataTable tb = new DataTable();
            tb = xldl.data(sql);
            DataRow gender = tb.Rows[0];
            DataRow Address = tb.Rows[0];
            return gender_address += gender.ItemArray[0].ToString() + "," + Address.ItemArray[1].ToString();
        }
        public DataTable GeDataTable(string sql)
        {
            DataTable tb = new DataTable();
            tb = xldl.data(sql);
            return tb;
        }
    }
}
