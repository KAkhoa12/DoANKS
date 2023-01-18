using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn_QLKS_dotnet
{
    class XULYDULIEU
    {
        SqlConnection connect; 
        string chuoiConnect = "server=.\\SQLEXPRESS;database=Do_An_QLKS;Integrated Security=true";
        public XULYDULIEU()
        {
            try
            {
                connect = new SqlConnection(chuoiConnect);
                //MessageBox.Show("kết nối thành công", "thông báo");
            }
            catch (SqlException ex)
            {
                 MessageBox.Show("kết nối thất bại", "thông báo");
            }
        }
        public DataTable data(string commandSql)
        {
            SqlDataAdapter dt = new SqlDataAdapter(commandSql, connect);
            DataTable datatb = new DataTable();
            dt.Fill(datatb);
            return datatb;
        } 
        public int Them_Xoa_Sua(string commandSql)
        {
            if (connect.State == ConnectionState.Closed)
                connect.Open();
            SqlCommand command = new SqlCommand(commandSql, connect);
            return command.ExecuteNonQuery();
            connect.Close();

        }
        public int CAPNHAP(string sqlcommand, DataTable tablename)
        {
            SqlDataAdapter da = new SqlDataAdapter(sqlcommand, connect);
            SqlCommandBuilder combuilder = new SqlCommandBuilder(da);
            da = combuilder.DataAdapter;
            return da.Update(tablename);
        }


    }

}
