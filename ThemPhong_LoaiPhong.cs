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
using DoAn_QLKS_dotnet.InformationProcessing;
using System.Globalization;

namespace DoAn_QLKS_dotnet
{
    public partial class ThemPhong_LoaiPhong : Form
    {
        public ThemPhong_LoaiPhong()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        GetID id = new GetID();
        bool them = false, sua = false;
        void loaiLoaiPhongCB()
        {
            foreach (DataRow row in xldl.data("select TenLoai from loaiphong").Rows)
            {
                cb_loaiphong.Items.Add(row.ItemArray[0].ToString());
            }
        }
        void loadPhong()
        {
            string sql = "select MaPhong,TenPhong,TenLoai,Lau from phong,loaiphong where phong.MaLoai = loaiphong.MaLoai";
            foreach (DataRow row in xldl.data(sql).Rows)
            {
                ListViewItem liv = liv_tenphong.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(row[2].ToString());
                liv.SubItems.Add(row[3].ToString());
            }
        }
        void loadLoaiPhong()
        {
            string sql = "select MaLoai,TenLoai,Gia from loaiphong";
            foreach (DataRow row in xldl.data(sql).Rows)
            {
                ListViewItem liv = liv_loaiphong.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(row[2].ToString());
            }
        }
        private void ThemPhong_LoaiPhong_Load(object sender, EventArgs e)
        {
            loadLoaiPhong();
            loadPhong();
            loaiLoaiPhongCB();
        }

        private void liv_tenphong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (liv_tenphong.SelectedIndices.Count > 0)
            {
                txt_tenphong.Text = liv_tenphong.SelectedItems[0].SubItems[1].Text;
                cb_loaiphong.Text = liv_tenphong.SelectedItems[0].SubItems[2].Text;
                string lau =liv_tenphong.SelectedItems[0].SubItems[3].Text;
                numRick_lau.Value = Convert.ToDecimal(lau);
                Local_Info_room.MaPhong = liv_tenphong.SelectedItems[0].SubItems[0].ToString();
            }
        }

        private void liv_loaiphong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (liv_loaiphong.SelectedIndices.Count > 0)
            {
                txt_loaiphong.Text = liv_loaiphong.SelectedItems[0].SubItems[1].Text;
                txt_gialoai.Text = liv_loaiphong.SelectedItems[0].SubItems[2].Text;
                Local_Info_room.MaLoaiPhong = liv_loaiphong.SelectedItems[0].SubItems[0].Text;
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            tabPage2.Enabled = btn_luu.Enabled = btn_them.Enabled = btn_xoa.Enabled = liv_tenphong.Enabled = false;
            btn_luu.Enabled = txt_tenphong.Enabled = numRick_lau.Enabled = cb_loaiphong.Enabled = true;
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            if (them)
            {
                string sql = string.Format("insert into phong (MaLoai,TenPhong,Lau,TinhTrang) values({0},N'{1}',{2},N'Trống')", xldl.getOneRow(id.MaLoaiPhong(cb_loaiphong.Text.Trim())), txt_tenphong.Text,numRick_lau.Value.ToString());
                dr = MessageBox.Show(string.Format("Bạn muốn thêm {0} vào danh sách", txt_tenphong.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    tabPage2.Enabled = btn_luu.Enabled = btn_them.Enabled = btn_xoa.Enabled = liv_tenphong.Enabled = true;
                    btn_luu.Enabled = txt_tenphong.Enabled = numRick_lau.Enabled = cb_loaiphong.Enabled =them=sua= false;
                }
            }
            else if (sua)
            {
                string sql = string.Format("update phong set MaLoai = {0}, TenPhong = N'{1}',Lau ={2} where MaPhong = {3}", xldl.getOneRow(id.MaLoaiPhong(cb_loaiphong.Text)),txt_tenphong.Text, numRick_lau.Value.ToString(), Local_Info_room.MaPhong);
                dr = MessageBox.Show("Bạn muốn sửa phòng trong danh sách ?? ", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    tabPage2.Enabled = btn_luu.Enabled = btn_them.Enabled = btn_xoa.Enabled = liv_tenphong.Enabled = true;
                    btn_luu.Enabled = txt_tenphong.Enabled = numRick_lau.Enabled = cb_loaiphong.Enabled = them = sua = false;
                }
            }
            else MessageBox.Show("Thao tác thất bại", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            loadLoaiPhong();
            loadPhong();
            loaiLoaiPhongCB();

        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql = string.Format("delete phong where MaPhong = {0}", Local_Info_room.MaPhong);
            DialogResult dr;
            dr = MessageBox.Show(string.Format("Bạn muốn xóa {0} ??", txt_tenphong.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (liv_tenphong.SelectedIndices.Count > 0)
            {
                if ((dr == DialogResult.OK))
                {
                    xldl.Them_Xoa_Sua(sql);
                    tabPage2.Enabled = btn_luu.Enabled = btn_them.Enabled = btn_xoa.Enabled = liv_tenphong.Enabled = true;
                    btn_luu.Enabled = txt_tenphong.Enabled = numRick_lau.Enabled = cb_loaiphong.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dịch vụ nào để xóa", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            loadLoaiPhong();
            loadPhong();
            loaiLoaiPhongCB();
        }

        private void btn_themloai_Click(object sender, EventArgs e)
        {
            txt_loaiphong.Text = string.Empty;
            txt_gialoai.Text = "0";
            tabPage1.Enabled = btn_themloai.Enabled = btn_xoaloai.Enabled = btn_sualoai.Enabled =liv_loaiphong.Enabled= false;
            btn_luuloai.Enabled = txt_loaiphong.Enabled = txt_gialoai.Enabled = true;
        }

        private void btn_sualoai_Click(object sender, EventArgs e)
        {
            tabPage1.Enabled = btn_themloai.Enabled = btn_xoaloai.Enabled = btn_sualoai.Enabled = liv_loaiphong.Enabled = false;
            btn_luuloai.Enabled = txt_loaiphong.Enabled = txt_gialoai.Enabled = true;
        }

        private void btn_luuloai_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            if (them)
            {
                string sql = string.Format("insert into loaiphong (TenLoai,Gia) values(N'{0}',{2})", txt_loaiphong.Text,txt_gialoai.Text);
                dr = MessageBox.Show(string.Format("Bạn muốn thêm {0} vào danh sách", txt_loaiphong.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    tabPage1.Enabled = btn_themloai.Enabled = btn_xoaloai.Enabled = btn_sualoai.Enabled = liv_loaiphong.Enabled = sua =them= false;
                    btn_luuloai.Enabled = txt_loaiphong.Enabled = txt_gialoai.Enabled = true;
                }
            }
            else if (sua)
            {
                string sql = string.Format("update loaiphong set TenLoai =N'{0}', Gia = {2} where MaLoai = {3}", xldl.getOneRow(id.MaLoaiPhong(cb_loaiphong.Text)), txt_tenphong.Text, numRick_lau.Value.ToString(), Local_Info_room.MaLoaiPhong);
                dr = MessageBox.Show("Bạn muốn sửa loại phòng trong danh sách ?? ", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    tabPage1.Enabled = btn_themloai.Enabled = btn_xoaloai.Enabled = btn_sualoai.Enabled = liv_loaiphong.Enabled = sua = them = false;
                    btn_luuloai.Enabled = txt_loaiphong.Enabled = txt_gialoai.Enabled = true;
                }
            }
            else MessageBox.Show("Thao tác thất bại", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            loadLoaiPhong();
            loadPhong();
            loaiLoaiPhongCB();
        }

        private void btn_xoaloai_Click(object sender, EventArgs e)
        {
            string sql = string.Format("delete loaiphong where MaLoai = {0}", Local_Info_room.MaLoaiPhong);
            DialogResult dr;
            dr = MessageBox.Show(string.Format("Bạn muốn xóa loại phòng {0} ??", txt_loaiphong.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (liv_loaiphong.SelectedIndices.Count > 0)
            {
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    tabPage1.Enabled = btn_themloai.Enabled = btn_xoaloai.Enabled = btn_sualoai.Enabled = liv_loaiphong.Enabled = sua = them = false;
                    btn_luuloai.Enabled = txt_loaiphong.Enabled = txt_gialoai.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn loại phòng nào để xóa", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            loadLoaiPhong();
            loadPhong();
            loaiLoaiPhongCB();
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            them = true;
            tabPage2.Enabled = btn_luu.Enabled = btn_them.Enabled = btn_xoa.Enabled = liv_tenphong.Enabled = false;
            btn_luu.Enabled =txt_tenphong.Enabled=numRick_lau.Enabled=cb_loaiphong.Enabled= true;
            txt_tenphong.Text =cb_loaiphong.Text= string.Empty;
            numRick_lau.Value = 1;
            
        }
    }
}
