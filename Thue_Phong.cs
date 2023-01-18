using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoAn_QLKS_dotnet.InformationProcessing;
using DoAn_QLKS_dotnet.LocalStore;
using DoAn_QLKS_dotnet.Validate;
namespace DoAn_QLKS_dotnet
{
    public partial class Thue_Phong : Form
    {
        public Thue_Phong()
        {
            InitializeComponent();
        }
        XULYDULIEU xldl = new XULYDULIEU();
        DataTable tb = new DataTable();
        GetInfo info = new GetInfo();
        GetID id = new GetID();
        Contrains contrain = new Contrains();
        bool tinhngayo = false;
        void load_loai_phong(string sql)
        {
            tb = xldl.data(sql);
            cb_loaiphong.Items.Clear();
            foreach (DataRow item in tb.Rows)
            {
                cb_loaiphong.Items.Add(item.ItemArray[0].ToString());
            }
        }
        void load_phong(string sql)
        {
            tb = xldl.data(sql);
            cb_tenphong.Items.Clear();
            foreach (DataRow item in tb.Rows)
            {
                cb_tenphong.Items.Add(item.ItemArray[0].ToString());
            }
        }
        private void Thue_Phong_Load(object sender, EventArgs e)
        {
            cb_congviec.Items.Add("Thuê phòng");
            cb_congviec.Items.Add("Đặt trước");
            string sql_loai_phong = string.Format("select TenLoai from loaiphong");
            string sql_phong = string.Format("select TenPhong from phong where TinhTrang = N'Trống'");

            load_loai_phong(sql_loai_phong);
            load_phong(sql_phong);
        }

        private void cb_loaiphong_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql_loai_phong = string.Format("select tenphong from loaiphong,phong where phong.MaLoai = loaiphong.MaLoai and TenLoai = N'{0}' and TinhTrang = N'Trống' ",cb_loaiphong.SelectedItem.ToString());
            load_phong(sql_loai_phong);
        }

        private void cb_congviec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cb_congviec.SelectedItem.ToString()=="Thuê phòng")
            {
                dateTPick_daygo.Enabled = dateTPick_timego.Enabled = false;
            }
            else
            {
                dateTPick_daygo.Enabled = dateTPick_timego.Enabled = true;
            }
        }

        private void btn_caculated_day_Click(object sender, EventArgs e)
        {
            string days = xldl.getOneRow(info.DayUse(string.Format("{0:MM/dd/yyyy}", dateTPick_daycome.Value), string.Format("{0:MM/dd/yyyy}", dateTPick_daygo.Value)));
            string times;
            if (days == "0") {
                 times = xldl.getOneRow(info.TimeUseZeroDay(string.Format("{0:HH:mm}", dateTPick_timecome.Value), string.Format("{0:HH:mm}", dateTPick_timego.Value)));
            } else
            {
                 times = xldl.getOneRow(info.TimeUseManyDays(string.Format("{0:HH:mm}", dateTPick_timecome.Value), string.Format("{0:HH:mm}", dateTPick_timego.Value)));
            }

            string sql_price = string.Format("select Gia from loaiphong where TenLoai=N'{0}'", cb_loaiphong.Text);
            string Price = xldl.getOneRow(sql_price);
            float sum = ((int.Parse(days.ToString()) * float.Parse(Price.ToString())) + (int.Parse(times) * 20000)) / 2;
            if(int.Parse(days) < 0)
            {
                MessageBox.Show("Ngày dự kiến đi phải lớn hơn ngày tới!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                lbl_numberDayOurs.Text = string.Format("Khách đi {0} ngày {1} tiếng", days, times);
                lbl_price.Text = "Giá tiền cọc phòng: " + sum.ToString();
                Local_Info_room.DatCoc = sum.ToString();
                tinhngayo = true;
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int adult, child;
        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            adult = int.Parse(xldl.getOneRow(string.Format("select SoNguoiLon from LoaiPhong where TenLoai = N'{0}'", cb_loaiphong.Text)));
            child = int.Parse(xldl.getOneRow(string.Format("select SoTreEm from LoaiPhong where TenLoai = N'{0}'", cb_loaiphong.Text)));
            if (!PersonalInfo.CheckCMND(txt_CMND.Text) || !contrain.CheckContaninsCMND(txt_CMND.Text))
            {
                MessageBox.Show("Số chứng minh thư không hợp lệ hoặc bị trùng, vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }else if (!PersonalInfo.CheckSDT(txt_SDT.Text))
            {
                MessageBox.Show("Số điện thoại không hợp lệ, vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }else if (cb_congviec.Text == string.Empty || cb_loaiphong.Text == string.Empty || cb_tenphong.Text == string.Empty)
            {
                MessageBox.Show("Chưa chọn loại phòng hoặc tên phòng hoặc công việc!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }else if(int.Parse(numRick_adult.Value.ToString()) > adult || int.Parse(numRick_child.Value.ToString()) > child)
            {
                MessageBox.Show("Số người lớn hoặc trẻ em quá số lượng, vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }else if (!tinhngayo)
            {
                MessageBox.Show("Chưa tính ngày giờ cho khách!!!", "Lỗi", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }
            else
            {
                string sql = string.Format("insert into khachhang(TenKH, cmnd, phone) values (N'{0}','{1}','{2}')", txt_tenKH.Text, txt_CMND.Text, txt_SDT.Text);
                if (xldl.Them_Xoa_Sua(sql) > 0)
                {
                    string sql_tinh_trang_dat_trc = string.Format("update phong set TinhTrang =N'Đặt trước' where TenPhong =N'{0}'", cb_tenphong.Text);
                    string sql_tinh_trang_thue_phong = string.Format("update phong set TinhTrang =N'Đang sử dụng' where TenPhong =N'{0}'", cb_tenphong.Text);
                    string MaKH = xldl.getOneRow(id.MaKH(txt_CMND.Text));
                    string MaPhong = xldl.getOneRow(id.MaPhong(cb_tenphong.Text));
                    if (cb_congviec.Text == "Đặt trước")
                    {
                        if (xldl.Them_Xoa_Sua(sql_tinh_trang_dat_trc) > 0)
                        {
                            string sql_phieu_thue_phong = string.Format("insert into phieu_thue_phong(MaPhong,MaKH,time_start,NgayDen,CongViec,NgayDi,TimeGo,DatCoc,SoNguoiLon,SoTreEm) values({0},{1},'{2}','{3}',N'{4}','{5}','{6}',{7},{8},{9})", MaPhong, MaKH, string.Format("{0:HH:mm}", dateTPick_timecome.Value), string.Format("{0:MM/dd/yyyy}", dateTPick_daycome.Value), cb_congviec.Text, string.Format("{0:MM/dd/yyyy}", dateTPick_daygo.Value) , string.Format("{0:HH:mm}", dateTPick_timego.Value), Local_Info_room.DatCoc,numRick_adult.Value.ToString(),numRick_child.Value.ToString());
                            if (xldl.Them_Xoa_Sua(sql_phieu_thue_phong) > 0)
                            {
                                Local_Info_room.CongViec = cb_congviec.ToString();
                                Local_Info_room.SoNgLon = numRick_adult.Value.ToString();
                                Local_Info_room.SoTreEm = numRick_child.Value.ToString();
                                MessageBox.Show("Lập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        if (xldl.Them_Xoa_Sua(sql_tinh_trang_thue_phong) > 0)
                        {
                            string sql_phieu_thue_phong = string.Format("insert into phieu_thue_phong(MaPhong,MaKH,time_start,NgayDen,CongViec,DatCoc,SoNguoiLon,SoTreEm) values({0},{1},'{2}','{3}',N'{4}',{5},{6},{7})", MaPhong, MaKH, string.Format("{0:HH:mm}", dateTPick_timecome.Value), string.Format("{0:MM/dd/yyyy}", dateTPick_daycome.Value), cb_congviec.Text, Local_Info_room.DatCoc, numRick_adult.Value.ToString(), numRick_child.Value.ToString());
                            if (xldl.Them_Xoa_Sua(sql_phieu_thue_phong) > 0)
                            {
                                Local_Info_room.CongViec = cb_congviec.ToString();
                                Local_Info_room.SoNgLon = numRick_adult.Value.ToString();
                                Local_Info_room.SoTreEm = numRick_child.Value.ToString();
                                MessageBox.Show("Lập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                    }
                }
            }
        }
    }
}
