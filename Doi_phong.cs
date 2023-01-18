using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn_QLKS_dotnet.LocalStore;

namespace DoAn_QLKS_dotnet
{
    public partial class Doi_phong : Form
    {
        public Doi_phong()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        DataTable tb = new DataTable();
        DialogResult dr;
        void load_loai_phong(string sql)
        {
            tb = xldl.data(sql);
            cb_loaiphongsang.Items.Clear();
            foreach (DataRow item in tb.Rows)
            {
                cb_loaiphongsang.Items.Add(item.ItemArray[0].ToString());
            }
        }
        void load_phong(string sql)
        {
            tb = xldl.data(sql);
            cb_phongsang.Items.Clear();
            foreach (DataRow item in tb.Rows)
            {
                cb_phongsang.Items.Add(item.ItemArray[0].ToString());
            }
        }
        private void Doi_phong_Load(object sender, EventArgs e)
        {
            txt_tenKH.Text = Local_Info_room.TenKH;
            txt_cccd.Text = Local_Info_room.CMND;
            txt_sdt.Text =Local_Info_room.SDT;
            txt_loaiphong.Text = Local_Info_room.LoaiPhong;
            txt_phong.Text = Local_Info_room.TenPhong;
            string sql_loai_phong = string.Format("select TenLoai from loaiphong");
            string sql_phong = string.Format("select TenPhong from phong where TinhTrang = N'Trống'");
            load_loai_phong(sql_loai_phong);
            load_phong(sql_phong);
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            string sql_id_phong_sang = string.Format("select MaPhong from phong where TenPhong=N'{0}'", cb_phongsang.Text);
            string id_phong_new = xldl.getOneRow(sql_id_phong_sang);
            string update_sql_phong = string.Format("update phieu_thue_phong set MaPhong ={0} where MaPT={1}", id_phong_new,Local_Info_room.MaPT);
            dr = MessageBox.Show("Bạn có chắc cập nhập?", "Thông báo", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                if (xldl.Them_Xoa_Sua(update_sql_phong) > 0)
                {
                    string update_tinhtrang = string.Format("update phong set Tinhtrang = N'Trống' where TenPhong = N'{0}' update phong set Tinhtrang = N'Đang sử dụng' where TenPhong = N'{1}'", txt_phong.Text, cb_phongsang.Text);
                    if (xldl.Them_Xoa_Sua(update_tinhtrang) > 0)
                    {
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Xác nhận thất bại!");
                }
            }
        }

        private void cb_loaiphongsang_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql_loai_phong = string.Format("select tenphong from loaiphong,phong where phong.MaLoai = loaiphong.MaLoai and TenLoai = N'{0}' and TinhTrang = N'Trống' and tenphong != N'{1}'", cb_loaiphongsang.SelectedItem.ToString(),txt_phong.Text);
            load_phong(sql_loai_phong);
        }

    }
}
