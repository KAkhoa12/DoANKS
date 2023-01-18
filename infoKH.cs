using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn_QLKS_dotnet.LocalStore;
using DoAn_QLKS_dotnet.Validate;

namespace DoAn_QLKS_dotnet
{
    public partial class infoKH : Form
    {
        public infoKH()
        {
            InitializeComponent();
        }
        string[] a = File.ReadAllLines("DS_tinhthanhVN.txt");
        DialogResult dr;
        XULYDULIEU xldl = new XULYDULIEU();
        Contrains constrain = new Contrains();
        private void infoKH_Load(object sender, EventArgs e)
        {
            string adult= xldl.getOneRow(string.Format("select SoNguoiLon from LoaiPhong where TenLoai = N'{0}'", Local_Info_room.LoaiPhong));
            string child =xldl.getOneRow(string.Format("select SoTreEm from LoaiPhong where TenLoai = N'{0}'", Local_Info_room.LoaiPhong));
            txt_nameuser.Text =Local_Info_room.TenKH ;
            txt_sdt.Text = Local_Info_room.SDT;
            txt_cccd.Text = Local_Info_room.CMND;
            txt_loaiphong.Text = Local_Info_room.LoaiPhong;
            txt_tenphong.Text = Local_Info_room.TenPhong;
            numRick_adult.Text =adult;
            numRick_child.Text =child;
            cb_gender.Text = Local_Info_room.Goitinh;
            cb_address.Text = Local_Info_room.Address;
            cb_gender.Items.Add("Nam");
            cb_gender.Items.Add("Nữ");
            for (int i = 0; i < a.Length; i++)
            {
                cb_address.Items.Add(a[i]);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            txt_nameuser.Enabled = txt_sdt.Enabled = cb_gender.Enabled = numRick_adult.Enabled = numRick_child.Enabled = txt_cccd.Enabled = cb_address.Enabled = btn_save.Enabled= true;
            btn_edit.Enabled = false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
                if(!PersonalInfo.CheckSDT(txt_sdt.Text) )
                {
                    MessageBox.Show("Số điện thoại không hợp lệ, vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }else if (!PersonalInfo.CheckCMND(txt_cccd.Text) || !constrain.CheckContaninsCMND(txt_cccd.Text))
                {
                    MessageBox.Show("Số chứng minh thư không hợp lệ hoặc bị trùng, vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                }
                else
                {
                    txt_nameuser.Enabled = txt_sdt.Enabled = cb_gender.Enabled = numRick_child.Enabled = numRick_adult.Enabled = txt_cccd.Enabled = cb_address.Enabled = false;
                    btn_edit.Enabled = true;
                    dr = MessageBox.Show("Bạn có muốn cập nhập thông tin khách hàng hay không ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        string sql = string.Format("update phieu_thue_phong set SoNguoiLon = {0},SoTreEm={1} where MaPT={2}", numRick_adult.Text, numRick_child.Text, Local_Info_room.MaPT);

                        if (xldl.Them_Xoa_Sua(sql) > 0)
                        {
                            string info_KH = string.Format("update khachhang set phai =N'{0}',diachi =N'{1}',cmnd='{2}',phone='{3}',TenKH=N'{4}' where MaKH ={5}", cb_gender.Text, cb_address.Text, txt_cccd.Text, txt_sdt.Text, txt_nameuser.Text, Local_Info_room.MaKH);
                            if (xldl.Them_Xoa_Sua(info_KH) > 0)
                            {
                                MessageBox.Show("Cập nhập thành công!!!", "Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                
            
            
        }

        private void btn_esc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
