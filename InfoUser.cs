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
    public partial class InfoUser : Form
    {
        public InfoUser()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        DataTable AccountUser = new DataTable();
        PersonalInfo validate = new PersonalInfo();


        string[] a = File.ReadAllLines("DS_tinhthanhVN.txt");
        private void InfoUser_Load(object sender, EventArgs e)
        {
            string sql = string.Format("select * from nhanvien where MaNV="+Local_info_employee.Id+" ");
            AccountUser = xldl.data(sql);
            foreach(DataRow item in AccountUser.Rows)
            {
                txt_MaNV.Text = item[0].ToString();
                txt_nameuser.Text = item[1].ToString();
                dateTPick_dayofbrith.Text = item[2].ToString();
                cb_gender.Text = item[3].ToString();
                cb_address.Text = item[4].ToString();
                txt_sdt.Text = item[5].ToString();
                txt_userAccountname.Text = item[6].ToString();
                txt_userAccountpassword.Text = item[7].ToString();
                txt_roleNV.Text =Local_info_employee.Role;
                cb_gender.Items.Add("Nam");
                cb_gender.Items.Add("Nữ");
            }
            for (int i =0; i < a.Length; i++)
            {
                cb_address.Items.Add(a[i]);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            txt_nameuser.Enabled = txt_sdt.Enabled = txt_userAccountname.Enabled = txt_userAccountpassword.Enabled = cb_address.Enabled = cb_gender.Enabled =dateTPick_dayofbrith.Enabled= true;
            btn_edit.Enabled = false;
        }

        bool ValidateInfoUser()
        {
            string sqlUser = string.Format("select * from nhanvien where MaNV!=" + Local_info_employee.Id + " ");
            AccountUser = xldl.data(sqlUser);
            foreach (DataRow item in AccountUser.Rows)
            {
                if (txt_sdt.Text.Trim() == item[5].ToString().Trim() || txt_userAccountname.Text.Trim() == item[8].ToString().Trim())
                {
                    return false;
                }
            }
            return true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            txt_nameuser.Enabled = txt_sdt.Enabled = txt_userAccountname.Enabled = txt_userAccountpassword.Enabled = cb_address.Enabled
                = cb_gender.Enabled =dateTPick_dayofbrith.Enabled=  false;
            btn_edit.Enabled = true;
            DialogResult dr;
            if (!PersonalInfo.CheckUserName(txt_userAccountname.Text))
            {
                MessageBox.Show("Tên username không hợp lệ!!!", "Thông báo");
            }
            else
            {
                dr = MessageBox.Show("Bạn có muốn cập nhập thông tin hay không ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    string sql = string.Format("update nhanvien set TenNV=N'{0}', ngaysinh = '{1}',phai=N'{2}',diachi=N'{3}',SDT='{4}',TenDN='{5}',MatKhau='{6}' where MaNv={7}",txt_nameuser.Text,dateTPick_dayofbrith.Value,cb_gender.Text,cb_address.Text,txt_sdt.Text,txt_userAccountname.Text,txt_userAccountpassword.Text,txt_MaNV.Text);
                    if (ValidateInfoUser())
                    {
                        if (xldl.Them_Xoa_Sua(sql) > 0)
                        {
                            MessageBox.Show("Cập nhập thành công!!!", "Thông báo");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cập nhập thất bại, số điện thoại hoặc username account bị trùng!!!", "Thông báo");
                    }
                }
            }
        }
    }
}
