using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn_QLKS_dotnet.Validate;
using DoAn_QLKS_dotnet.LocalStore;
using DoAn_QLKS_dotnet.InformationProcessing;

namespace DoAn_QLKS_dotnet
{
    public partial class User_manager : Form
    {
        public User_manager()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        DataTable tb = new DataTable();
        GetID id = new GetID();
        GetInfo info = new GetInfo();
        Contrains contain = new Contrains();
        bool logout = false, ThemNV =false, SuaNV = false;
        string path_default_img_nv = "D:\\Work_space\\3_C#\\DoAn_QLKS_dotnet\\Img\\Nhan_Vien\\default_Img.png";
        string[] a = File.ReadAllLines("DS_tinhthanhVN.txt");



        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDOWN = 161;
            const int WM_SYSCOMMAND = 274;
            const int HTCAPTION = 2;
            const int SC_MOVE = 61456;
            if ((m.Msg == WM_SYSCOMMAND) && (m.WParam.ToInt32() == SC_MOVE))
            {
                return;
            }
            if ((m.Msg == WM_NCLBUTTONDOWN) && (m.WParam.ToInt32() == HTCAPTION))
            {
                return;
            }
            base.WndProc(ref m);
        }
        void load_List_Room_Use_DV()
        {
            lib_Phong_DV.Items.Clear();
            tb = xldl.data("select TenPhong from phong,chitietsudungdichvu where chitietsudungdichvu.MaPhong = phong.MaPhong");
            foreach(DataRow row in tb.Rows)
            {
                if (!lib_Phong_DV.Items.Contains(row[0].ToString()))
                {
                    lib_Phong_DV.Items.Add(row[0].ToString());
                }
            }
        }
        void loadListRoom()
        {
            string sql = string.Format("select phong.MaPhong,phong.TenPhong,loaiphong.TenLoai,phong.Lau,khachhang.TenKH,khachhang.phone,cmnd,phieu_thue_phong.NgayDen,phieu_thue_phong.time_start,phong.TinhTrang,phieu_thue_phong.SoNguoiLon,phieu_thue_phong.SoTreEm from phong full join phieu_thue_phong on phieu_thue_phong.MaPhong = phong.MaPhong full join loaiphong on phong.MaLoai = loaiphong.MaLoai full join khachhang on khachhang.MaKH = phieu_thue_phong.MaKH");
            tb = xldl.data(sql);
            listView1.Items.Clear();
            foreach (DataRow row in tb.Rows)
            {
                ListViewItem liv = listView1.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(row[2].ToString());
                liv.SubItems.Add(row[3].ToString());
                liv.SubItems.Add(row[4].ToString());
                liv.SubItems.Add(row[5].ToString());
                liv.SubItems.Add(row[6].ToString());
                liv.SubItems.Add(string.Format("{0:MM/dd/yyyy}",row[7]));
                liv.SubItems.Add(row[8].ToString());
                liv.SubItems.Add(row[9].ToString());
                liv.SubItems.Add(row[10].ToString());
                liv.SubItems.Add(row[11].ToString());
            }
            for(int i =0; i<listView1.Items.Count; i++)
            {
                if(listView1.Items[i].SubItems[9].Text =="Đặt trước")
                {
                    listView1.Items[i].BackColor = Color.FromArgb(246, 229, 141);
                }
                else if(listView1.Items[i].SubItems[9].Text == "Đang sử dụng")
                {
                    listView1.Items[i].BackColor = Color.FromArgb(235, 77, 75);
                }
                else
                {
                    listView1.Items[i].BackColor = Color.FromArgb(189, 195, 199);
                }
            }

        }
        void loadDV()
        {
            liv_dichvu.Items.Clear();
            cb_dvphong.Items.Clear();
            string sql = string.Format("select * from dichvu");
            string sql_dv_phong = string.Format("select TenPhong from phong where TinhTrang=N'Đang sử dụng'");
            DataTable dvphong = xldl.data(sql_dv_phong);
            tb = xldl.data(sql);
            foreach(DataRow row in dvphong.Rows)
            {
                cb_dvphong.Items.Add(row.ItemArray[0].ToString());
            }
            foreach (DataRow row in tb.Rows)
            {
                ListViewItem liv = liv_dichvu.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(string.Format("{0:0}", row[2]));
            }

        }
        void load_CT_SD_dich_vu()
        {
            liv_ctdv.Items.Clear();
            string sql = string.Format("select dichvu.MaDV,tendv,dichvu.Gia,soluong,ThanhTien from chitietsudungdichvu,dichvu where MaPhong = {0} and chitietsudungdichvu.MaDV = dichvu.MaDV",Local_info_service.MaPhong);
            tb = xldl.data(sql);
            foreach (DataRow row in tb.Rows)
            {
                ListViewItem liv = liv_ctdv.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(string.Format("{0:0}", row[2]));
                liv.SubItems.Add(row[3].ToString());
                liv.SubItems.Add(string.Format("{0:0}", row[4]));
            }
        }
        void load_nv()
        {
            liv_nv.Items.Clear();
            string sql = string.Format("select MaNV,TenNV from nhanvien");
            tb = xldl.data(sql);
            foreach (DataRow row in tb.Rows)
            {
                ListViewItem liv = liv_nv.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
            }
        }
        
        private void User_manager_Load(object sender, EventArgs e)
        {
            lbl_MaNV.Text += Local_info_employee.Id;
            lbl_chaomung_nv.Text += Local_info_employee.Name + " (" + Local_info_employee.Role+")";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            WindowState = FormWindowState.Maximized;
            MaximizeBox = false;
            ShowInTaskbar = true;
            TopMost = false;
            logout = false;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            timer1.Start();
            loadDV();
            load_nv();
            load_List_Room_Use_DV();
            for (int i = 0; i < a.Length; i++)
            {
                cb_diachi_nv.Items.Add(a[i]);
            }
            cb_gender_nv.Items.Add("Nam");
            cb_gender_nv.Items.Add("Nữ");
            if (Local_info_employee.Role == "Nhân viên")
            {
                editToolStripMenuItem.Enabled = ((Control)this.tabPage3).Enabled = btn_doiphong.Enabled = btn_taophong.Enabled = btn_tinhtien.Enabled = btn_dv_xacnhan.Enabled =btn_dv_xacnhan.Enabled=btn_dv_xoa.Enabled= false;
            }else if(Local_info_employee.Role == "Lễ tân")
            {
                editToolStripMenuItem.Enabled= false;
            }else
            {
                panel4.Hide();
            }
        }
        private void btn_tinhtien_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Tinh_tien tt = new Tinh_tien();
            tt.FormClosed += Tt_FormClosed;
            tt.Show();
        }

        private void Tt_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            loadListRoom();
        }

        private void btn_taophong_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Thue_Phong tp = new Thue_Phong();
            tp.FormClosed += Tp_FormClosed;
            tp.Show();
        }

        private void Tp_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            loadListRoom();
        }

        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Xac_Nhan_phong xn = new Xac_Nhan_phong();
            xn.FormClosed += Xn_FormClosed;xn.Show();
        }

        private void Xn_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            loadListRoom();


        }

        private void btn_doiphong_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Doi_phong dp = new Doi_phong();
            dp.FormClosed += Dp_FormClosed;
            dp.Show();

        }

        private void Dp_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            loadListRoom();
        }

        private void TolStrip_Logout_Click(object sender, EventArgs e)
        {
            logout = true;
            DialogResult dr;
            if (logout)
            {
                dr = MessageBox.Show("Bạn có muốn đăng xuất hay không ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    Dang_Nhap DN = new Dang_Nhap();
                    DN.Show();
                    this.Hide();
                }
                else
                {
                    this.Show();
                }
            }
        }

        private void infoUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            InfoUser info = new InfoUser();
            info.FormClosed += Info_FormClosed;
            info.Show();
        }

        private void Info_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        private void btn_loablist_Click(object sender, EventArgs e)
        {
            loadListRoom();
        }
        
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                txt_TenKH.Text = listView1.SelectedItems[0].SubItems[4].Text;
                txt_sdt.Text = listView1.SelectedItems[0].SubItems[5].Text;
                txt_tenphong.Text = listView1.SelectedItems[0].SubItems[1].Text;
                txt_loaiphong.Text = listView1.SelectedItems[0].SubItems[2].Text;
                txt_child.Text = listView1.SelectedItems[0].SubItems[10].Text;
                txt_adult.Text = listView1.SelectedItems[0].SubItems[11].Text;
                txt_timecome.Text = listView1.SelectedItems[0].SubItems[8].Text;
                txt_daycome.Text = listView1.SelectedItems[0].SubItems[7].Text;
                txt_tinhtrang.Text = listView1.SelectedItems[0].SubItems[9].Text;
                txt_lau.Text = listView1.SelectedItems[0].SubItems[3].Text;
                txt_cccd.Text = listView1.SelectedItems[0].SubItems[6].Text;
                btn_taophong.Enabled = true;
                if (listView1.SelectedItems[0].SubItems[9].Text == "Trống")
                {
                    btn_xacnhan.Enabled = btn_tinhtien.Enabled = btn_doiphong.Enabled =btn_info_kh.Enabled= false;
                }
                else if(listView1.SelectedItems[0].SubItems[9].Text == "Đặt trước")
                {
                    btn_tinhtien.Enabled = btn_doiphong.Enabled= false;
                    btn_xacnhan.Enabled = btn_info_kh.Enabled = true;
                }else if (listView1.SelectedItems[0].SubItems[9].Text == "Đang sử dụng")
                {
                    btn_xacnhan.Enabled = false;
                    btn_doiphong.Enabled = btn_tinhtien.Enabled = btn_info_kh.Enabled= true;
                }
                if(listView1.SelectedItems[0].SubItems[9].Text == "Đặt trước" || listView1.SelectedItems[0].SubItems[9].Text == "Đang sử dụng")
                {
                    string sql_id_kh =id.MaKH(txt_cccd.Text);
                    string sql_id_phong = id.MaPhong(txt_tenphong.Text);
                    string maPT = id.MaPT(xldl.getOneRow(sql_id_phong), xldl.getOneRow(sql_id_kh));
                    string[] gender_address = xldl.Get_Gender_Address_KH(info.Gender_Address_KH(xldl.getOneRow(sql_id_kh))).Split(',');
                    Local_Info_room.MaKH = xldl.getOneRow(sql_id_kh);
                    Local_Info_room.MaPT = xldl.getOneRow(maPT);
                    Local_Info_room.MaPhong = xldl.getOneRow(sql_id_phong);
                    Local_Info_room.TenKH = txt_TenKH.Text;
                    Local_Info_room.LoaiPhong = txt_loaiphong.Text;
                    Local_Info_room.TenPhong = txt_tenphong.Text;
                    Local_Info_room.CMND = txt_cccd.Text;
                    Local_Info_room.SDT = txt_sdt.Text;
                    Local_Info_room.SoNgLon = txt_adult.Text;
                    Local_Info_room.SoTreEm = txt_child.Text;
                    Local_Info_room.Goitinh = gender_address[0];
                    Local_Info_room.Address = gender_address[1];
                    Local_Info_room.NgayDen = txt_daycome.Text;
                    Local_Info_room.TGDen = txt_timecome.Text;
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_thoigian.Text = " Thời gian: " + DateTime.Now.Month + " / " + DateTime.Now.Day + " / " + DateTime.Now.Year + " - " + DateTime.Now.Hour + " giờ " + DateTime.Now.Minute + " phút " + DateTime.Now.Second + " giây";
        }
        private void btn_reset_Click(object sender, EventArgs e)
        {
            load_CT_SD_dich_vu();
        }
        private void liv_nv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (liv_nv.SelectedItems.Count > 0)
            {
                string manv = liv_nv.SelectedItems[0].SubItems[0].Text;
                DataTable nhanvien = xldl.data(string.Format("select * from nhanvien where MaNV = {0}", manv));
                foreach (DataRow row in nhanvien.Rows)
                {
                    string[] day = string.Format("{0:MM/dd/yyyy}",row[2]).Split('/');
                    DateTime daybirth = new DateTime(int.Parse(day[2].ToString()), int.Parse(day[0].ToString()), int.Parse(day[1].ToString()));
                    txt_ma_nv.Text = row[0].ToString();
                    txt_name_nv.Text = row[1].ToString();
                    datePick_nv.Value = daybirth;
                    cb_gender_nv.Text = row[3].ToString();
                    cb_diachi_nv.Text = row[4].ToString();
                    txt_sdt_nv.Text = row[5].ToString();
                    txt_username_nv.Text = row[6].ToString();
                    txt_pass_nv.Text = row[7].ToString();
                    cb_quyen_nv.Text = xldl.getOneRow(string.Format("select TenQuyen from phanquyen where MaQuyen = {0}", row[8].ToString()));
                    if (row[9].ToString() == string.Empty) { pic_nv.Image = Image.FromFile(path_default_img_nv); txt_img_link_path.Text = path_default_img_nv; }
                    else { pic_nv.Image = Image.FromFile(row[9].ToString()); txt_img_link_path.Text = path_default_img_nv; }
                }
            }
        }


        private void btn_info_kh_Click(object sender, EventArgs e)
        {
            
            this.Enabled = false;
            infoKH info = new infoKH();
            info.FormClosed += Info_KH_FormClosed;
            info.Show();
        }
        private void Info_KH_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }
        private void btn_dv_xoa_Click(object sender, EventArgs e)
        {
            if (liv_ctdv.SelectedItems.Count > 0)
            {
                btn_dv_xoa.Enabled = true;
                DialogResult dr;
                dr = MessageBox.Show("Bạn muốn xóa dịch vụ " + Local_info_service.TenDV + " tại " + Local_info_service.TenPhong + " ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    string sql = string.Format("delete chitietsudungdichvu  where MaDV='{0}'and MaPhong={1}", Local_info_service.MaDV, Local_info_service.MaPhong);
                    if (xldl.Them_Xoa_Sua(sql) > 0)
                    {
                        //MessageBox.Show("Xóa thành công!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load_List_Room_Use_DV();
                        liv_ctdv.Items.Clear();
                    }
                }
            }
            else
            {
                btn_dv_xoa.Enabled = false;
            }
        }

        private void liv_dichvu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(liv_dichvu.SelectedIndices.Count > 0)
            {
                Local_info_service.MaDV = liv_dichvu.SelectedItems[0].SubItems[0].Text;
                Local_info_service.TenDV = liv_dichvu.SelectedItems[0].SubItems[1].Text;
                Local_info_service.Gia = liv_dichvu.SelectedItems[0].SubItems[2].Text;
            }
        }

        private void cb_dvphong_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Maphong = xldl.getOneRow(id.MaPhong(cb_dvphong.Text));
            Local_info_service.MaPhong = Maphong;
            Local_info_service.TenPhong = cb_dvphong.Text;
        }

        private void btn_dv_xacnhan_Click(object sender, EventArgs e)
        {
            if(cb_dvphong.Text != string.Empty)
            {
                DialogResult dr;
                Local_info_service.SoLuong = Number_count_amount.Value.ToString();
                dr = MessageBox.Show("Bạn muốn thêm dịch vụ " + Local_info_service.TenDV + " tại " + Local_info_service.TenPhong + " ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    string sql = string.Format("insert into chitietsudungdichvu(MaDV,MaPhong,Gia,SoLuong) values ({0},{1},{2},{3})", Local_info_service.MaDV, Local_info_service.MaPhong, Local_info_service.Gia, Local_info_service.SoLuong);
                    if (xldl.Them_Xoa_Sua(sql) > 0)
                    {
                        string sql_update = string.Format("update chitietsudungdichvu set ThanhTien = Gia * SoLuong where MaDV = {0} and MaPhong={1}", Local_info_service.MaDV, Local_info_service.MaPhong);
                        if (xldl.Them_Xoa_Sua(sql_update) > 0)
                        {
                            load_List_Room_Use_DV();
                        }
                    }
                }        
            }
            else MessageBox.Show("Chọn phòng cần thêm dịch vụ!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void lib_Phong_DV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lib_Phong_DV.SelectedIndices.Count > 0)
            {
                liv_ctdv.Enabled = btn_reset.Enabled= true;
                string Maphong = xldl.getOneRow(id.MaPhong(lib_Phong_DV.SelectedItems[0].ToString()));
                Local_info_service.MaPhong = Maphong;
                Local_info_service.TenPhong = lib_Phong_DV.SelectedItem.ToString();
                lbl_dv_ten_phong.Text = Local_info_service.TenPhong;
                load_CT_SD_dich_vu();
            }
            else
            {
                liv_ctdv.Enabled = btn_reset.Enabled= false;
            }

        }

        private void liv_ctdv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(liv_ctdv.SelectedIndices.Count > 0)
            {
                Local_info_service.MaDV = liv_ctdv.SelectedItems[0].SubItems[0].Text;
                Local_info_service.TenDV = xldl.getOneRow(string.Format("select TenDV from dichvu where MaDV ={0}", Local_info_service.MaDV));
            }
        }

        private void lbl_thoigian_Click(object sender, EventArgs e)
        {

        }

        private void User_manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;

            dr = MessageBox.Show("Bạn có muốn thoát khỏi chương trình quản lý hay không ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btn_link_img_nv_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog() { Filter = "*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" })
            {
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pic_nv.Image = Image.FromFile(open.FileName);
                    txt_img_link_path.Text = open.FileName;
                }
            }
        }

        private void btn_them_nv_Click(object sender, EventArgs e)
        {
            txt_name_nv.Enabled = txt_sdt_nv.Enabled = cb_gender_nv.Enabled = cb_diachi_nv.Enabled = txt_username_nv.Enabled = txt_pass_nv.Enabled = datePick_nv.Enabled = cb_quyen_nv.Enabled = ThemNV=btn_link_img_nv.Enabled=btn_luu_nv.Enabled=btn_huy_thao_tac.Enabled= true;
            btn_sua_nv.Enabled = btn_them_nv.Enabled = btn_xoa_nv.Enabled =liv_nv.Enabled= false;
            txt_ma_nv.Text = txt_name_nv.Text = cb_gender_nv.Text = cb_diachi_nv.Text = txt_sdt_nv.Text = txt_username_nv.Text = txt_pass_nv.Text = cb_quyen_nv.Text = string.Empty;
            txt_img_link_path.Text = path_default_img_nv;

        }

        private void btn_sua_nv_Click(object sender, EventArgs e)
        {
            txt_name_nv.Enabled = txt_sdt_nv.Enabled = cb_gender_nv.Enabled = cb_diachi_nv.Enabled = txt_username_nv.Enabled = txt_pass_nv.Enabled = datePick_nv.Enabled = cb_quyen_nv.Enabled = SuaNV= btn_link_img_nv.Enabled = btn_luu_nv.Enabled=btn_huy_thao_tac.Enabled= true;
            btn_sua_nv.Enabled = btn_them_nv.Enabled = btn_xoa_nv.Enabled =liv_nv.Enabled= false;
        }

        private void btn_xoa_nv_Click(object sender, EventArgs e)
        {
            string sql = string.Format("delete nhanvien where MaNV = {0}", txt_ma_nv.Text);
            DialogResult dr;
            dr = MessageBox.Show("Bạn muốn xóa nhân viên " + liv_nv.SelectedItems[0].SubItems[1].ToString() + " ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                if (xldl.Them_Xoa_Sua(sql) > 0)
                {
                    MessageBox.Show("Thành công");
                    load_nv();
                    txt_name_nv.Enabled = txt_sdt_nv.Enabled = cb_gender_nv.Enabled = cb_diachi_nv.Enabled = txt_username_nv.Enabled = txt_pass_nv.Enabled = datePick_nv.Enabled = cb_quyen_nv.Enabled = ThemNV = false;
                    btn_them_nv.Enabled = btn_sua_nv.Enabled = btn_xoa_nv.Enabled = true;
                }

            }
        }

        private void btn_huy_thao_tac_Click(object sender, EventArgs e)
        {
            txt_name_nv.Enabled = txt_sdt_nv.Enabled = cb_gender_nv.Enabled = cb_diachi_nv.Enabled = txt_username_nv.Enabled = txt_pass_nv.Enabled = datePick_nv.Enabled = cb_quyen_nv.Enabled = SuaNV = btn_link_img_nv.Enabled = btn_luu_nv.Enabled = btn_huy_thao_tac.Enabled = false;
            liv_nv.Enabled = btn_sua_nv.Enabled = btn_them_nv.Enabled = btn_xoa_nv.Enabled = true;

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void thêmLoạiPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            ThemPhong_LoaiPhong phong = new ThemPhong_LoaiPhong();
            phong.FormClosed += Phong_FormClosed;
            phong.Show();
        }

        private void Phong_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        private void thêmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            ThemDichVu dichvu = new ThemDichVu();
            dichvu.FormClosed += Dichvu_FormClosed;
            dichvu.Show();
        }

        private void Dichvu_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
        }

        
        private void btn_luu_nv_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn muốn lưu thông tin nhân viên ?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (!PersonalInfo.CheckSDT(txt_sdt_nv.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ, vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }else if (!PersonalInfo.CheckUserName(txt_username_nv.Text))
            {
                MessageBox.Show("Tên đăng nhập không được chứa ký tự đặc biệt!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else if (!contain.CheckConstainsUserName(txt_username_nv.Text))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else if (dr == DialogResult.OK)
            {
                if (ThemNV)
                {
                    string sql = string.Format("insert into nhanvien(TenNV,ngaysinh,phai,diachi,SDT,TenDN,MatKhau,MaQuyen,Img) values(N'{0}','{1}',N'{2}',N'{3}','{4}','{5}','{6}',{7},N'{8}')", txt_name_nv.Text, datePick_nv.Value.ToString(), cb_gender_nv.Text, cb_diachi_nv.Text, txt_sdt_nv.Text, txt_username_nv.Text, txt_pass_nv.Text, xldl.getOneRow(string.Format("select MaQuyen from phanquyen where TenQuyen =N'{0}'", cb_quyen_nv.Text)), txt_img_link_path.Text);
                    if (xldl.Them_Xoa_Sua(sql) > 0)
                    {
                        MessageBox.Show("Thành công");
                        load_nv();
                        txt_name_nv.Enabled = txt_sdt_nv.Enabled = cb_gender_nv.Enabled = cb_diachi_nv.Enabled = txt_username_nv.Enabled = txt_pass_nv.Enabled = datePick_nv.Enabled = cb_quyen_nv.Enabled = ThemNV = btn_link_img_nv.Enabled = btn_luu_nv.Enabled = false;
                        btn_them_nv.Enabled = btn_sua_nv.Enabled = btn_xoa_nv.Enabled = liv_nv.Enabled = true;
                    }
                }
                else
                {
                    string sql = string.Format("update nhanvien set TenNV =N'{0}', ngaysinh = '{1}',phai =N'{2}', diachi = N'{3}', SDT= '{4}', TenDN ='{5}', MatKhau ='{6}', MaQuyen = {7}, Img = N'{8}' where MaNV ={9}", txt_name_nv.Text, datePick_nv.Value.ToString(), cb_gender_nv.Text, cb_diachi_nv.Text, txt_sdt_nv.Text, txt_username_nv.Text, txt_pass_nv.Text, xldl.getOneRow(id.MaQuyen(cb_quyen_nv.Text)), txt_img_link_path.Text, txt_ma_nv.Text);
                    if (xldl.Them_Xoa_Sua(sql) > 0)
                    {
                        MessageBox.Show("Thành công");
                        load_nv();
                        txt_name_nv.Enabled = txt_sdt_nv.Enabled = cb_gender_nv.Enabled = cb_diachi_nv.Enabled = txt_username_nv.Enabled = txt_pass_nv.Enabled = datePick_nv.Enabled = cb_quyen_nv.Enabled = SuaNV = btn_link_img_nv.Enabled = btn_luu_nv.Enabled = false;
                        btn_them_nv.Enabled = btn_sua_nv.Enabled = btn_xoa_nv.Enabled = liv_nv.Enabled = true;
                    }
                }
            }
        }
    }
}
