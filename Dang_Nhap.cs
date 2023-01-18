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
using DoAn_QLKS_dotnet.Validate;

namespace DoAn_QLKS_dotnet
{
    public partial class Dang_Nhap : Form
    {
        public Dang_Nhap()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        DataTable tbAccount = new DataTable();
        //PersonalInfo user = new PersonalInfo();
        private void Dang_Nhap_Load(object sender, EventArgs e)
        {
            string sqlRole = "select TenQuyen from phanquyen";
            DataTable tbRole = xldl.data(sqlRole);
            foreach (DataRow account in tbRole.Rows)
            {
                comboBox1.Items.Add(account[0]);
            }
        }
        bool ktaccount(string name,string pass,string role)
        {
            string sql = "select MaNV,TenNV,TenQuyen,TenDn,MatKhau from nhanvien,phanquyen where phanquyen.MaQuyen = nhanvien.MaQuyen";
            tbAccount = xldl.data(sql);
            foreach (DataRow account in tbAccount.Rows)
            {
                if(name.Trim() == account[3].ToString() && pass.Trim() == account[4].ToString()&&role.Trim() == account[2].ToString().Trim())
                {
                    Local_info_employee.Id = account[0].ToString();
                    Local_info_employee.Name = account[1].ToString();
                    Local_info_employee.Role = account[2].ToString();
                    return true;
                }
            }
            return false;
        }
        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            if(txt_dangnhap.Text.Trim() == string.Empty || txt_matkhau.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Bạn phải nhập tên đăng nhập hoặc mật khẩu,quyền...","Thông báo" );
            }
            else
            {
                if(!PersonalInfo.CheckUserName(txt_dangnhap.Text))
                {
                    MessageBox.Show("User name không được có ký tự đặc biệt");
                }
                else
                {
                    if(ktaccount(txt_dangnhap.Text,txt_matkhau.Text,comboBox1.Text))
                    {
                        MessageBox.Show("Đăng nhập thành công !!!!","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        User_manager user = new User_manager();
                        Dang_Nhap DN = new Dang_Nhap();
                        user.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thất bại,sai tên đăng nhập, mật khẩu hoặc quyền","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            DialogResult dr;

            dr = MessageBox.Show("Bạn có muốn thoát khỏi đăng nhập hay không ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void chk_hienthimk_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_hienthimk.Checked == true) txt_matkhau.PasswordChar = '\0';
            else txt_matkhau.PasswordChar = '*';
        }
    }
}
