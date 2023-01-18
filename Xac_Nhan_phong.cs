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
    public partial class Xac_Nhan_phong : Form
    {
        public Xac_Nhan_phong()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        DialogResult dr;
        private void btn_xn_thue_Click(object sender, EventArgs e)
        {
            string sql = string.Format("update phong set TinhTrang = N'Đang sử dụng' where TenPhong =N'{0}'",Local_Info_room.TenPhong);
            dr = MessageBox.Show("Bạn có chắc xác nhận?", "Thông báo", MessageBoxButtons.OKCancel);
            if(dr == DialogResult.OK)
            {
                if (xldl.Them_Xoa_Sua(sql) > 0)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xác nhận thất bại!");
                }
            }
        }

        private void btn_xn_huy_Click(object sender, EventArgs e)
        {
            string sql_delete = string.Format("delete phieu_thue_phong where MaPT = {0} ", Local_Info_room.MaPT);
            string sql_update = string.Format("update phong set TinhTrang =N'Trống' where MaPhong = {0}",Local_Info_room.MaPhong) ;
            dr = MessageBox.Show("Bạn có chắc xác nhận?", "Thông báo", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                if (xldl.Them_Xoa_Sua(sql_delete) > 0)
                {
                    string sql_delete_KH = string.Format("delete khachhang where MaKH = {0} ", Local_Info_room.MaKH);
                    if (xldl.Them_Xoa_Sua(sql_delete_KH) > 0)
                    {
                        if (xldl.Them_Xoa_Sua(sql_update) > 0)
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Xác nhận thất bại!");

                }
            }
        }

        private void Xac_Nhan_phong_Load(object sender, EventArgs e)
        {
            txt_tenKH.Text = Local_Info_room.TenKH;
            txt_sdt.Text = Local_Info_room.SDT;
            txt_cccd.Text = Local_Info_room.CMND;
            txt_loaiphong.Text = Local_Info_room.LoaiPhong;
            txt_phong.Text = Local_Info_room.TenPhong;
            txt_gioden.Text = Local_Info_room.TGDen;
            txt_ngayden.Text = Local_Info_room.NgayDen;
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
