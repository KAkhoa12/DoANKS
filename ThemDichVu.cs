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
    public partial class ThemDichVu : Form
    {
        XULYDULIEU xldl = new XULYDULIEU();
        bool sua = false, them = false;
        public ThemDichVu()
        {
            InitializeComponent();
        }

        void load_dv()
        {
            string sql = "select * from dichvu";
            foreach(DataRow row in xldl.data(sql).Rows)
            {
                ListViewItem liv = liv_dv.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(row[2].ToString());
            }
        }
        private void ThemDichVu_Load(object sender, EventArgs e)
        {
            txt_tendv.Enabled = txt_giadv.Enabled =btn_luu.Enabled= false;
            load_dv();
        }

        private void liv_dv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(liv_dv.SelectedIndices.Count > 0)
            {
                txt_tendv.Text = liv_dv.SelectedItems[0].SubItems[1].Text;
                txt_giadv.Text  = liv_dv.SelectedItems[0].SubItems[2].Text;
                Local_info_service.TenDV = txt_tendv.Text;
                Local_info_service.MaDV = liv_dv.SelectedItems[0].SubItems[0].Text;
            }
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            them = true;
            txt_tendv.Text = string.Empty;
            txt_giadv.Text = "0";
            txt_tendv.Enabled = txt_giadv.Enabled = btn_luu.Enabled= true;
            btn_them.Enabled = btn_xoa.Enabled = btn_sua.Enabled = liv_dv.Enabled = false;
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            sua = true;
            txt_tendv.Enabled = txt_giadv.Enabled = btn_luu.Enabled = true;
            btn_them.Enabled = btn_xoa.Enabled = btn_sua.Enabled = liv_dv.Enabled = false;
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql = string.Format("delete dichvu where MaDV = {0}", Local_info_service.MaDV);
            DialogResult dr;
            dr = MessageBox.Show(string.Format("Bạn muốn xóa dịch vụ {0} ??", txt_tendv.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (liv_dv.SelectedIndices.Count > 0)
            {
                if((dr == DialogResult.OK))
                {
                    xldl.Them_Xoa_Sua(sql);
                    txt_tendv.Enabled = txt_giadv.Enabled = btn_luu.Enabled = false;
                    btn_them.Enabled = btn_xoa.Enabled = btn_sua.Enabled = liv_dv.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn dịch vụ nào để xóa??", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            load_dv();
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            if (them)
            {
                string sql = string.Format("insert into dichvu (TenDV, Gia) values(N'{0}', {1})", txt_tendv.Text, txt_giadv.Text);
                dr = MessageBox.Show(string.Format("Bạn muốn thêm dịch vụ {0} vào danh sách", txt_tendv.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    txt_tendv.Enabled = txt_giadv.Enabled = btn_luu.Enabled =them=sua= false;
                    btn_them.Enabled = btn_xoa.Enabled = btn_sua.Enabled = liv_dv.Enabled = true;
                }
            }
            else if (sua)
            {
                string sql = string.Format("update dichvu set TenDV = N'{0}', Gia = {1} where MaDV = {2}", txt_tendv.Text, txt_giadv.Text, Local_info_service.MaDV);
                dr = MessageBox.Show(string.Format("Bạn muốn sửa dịch vụ {0} trong danh sách", txt_tendv.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    xldl.Them_Xoa_Sua(sql);
                    txt_tendv.Enabled = txt_giadv.Enabled = btn_luu.Enabled = them = sua = false;
                    btn_them.Enabled = btn_xoa.Enabled = btn_sua.Enabled = liv_dv.Enabled = true;
                }
            }
            else MessageBox.Show("Thao tác thất bại", "Lỗi",MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            load_dv();
        }
    }
}
