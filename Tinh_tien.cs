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

namespace DoAn_QLKS_dotnet
{
    public partial class Tinh_tien : Form
    {
        public Tinh_tien()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkDayDatTrc(float.Parse(txt_tongtien.Text));
            this.Close();
        }

        DialogResult dr;
        XULYDULIEU xldl = new XULYDULIEU();
        DataTable tb = new DataTable();
        GetInfo info = new GetInfo();
        double sumAll;
        void load_list_dv()
        {
            string sql = string.Format("select dichvu.MaDV, TenDV, SoLuong, chitietsudungdichvu.Gia from dichvu, chitietsudungdichvu where MaPhong = {0} and dichvu.MaDV = chitietsudungdichvu.MaDV",Local_Info_room.MaPhong);
            tb = xldl.data(sql);
            foreach (DataRow row in tb.Rows)
            {
                ListViewItem liv = liv_tt_dv.Items.Add(row[0].ToString());
                liv.SubItems.Add(row[1].ToString());
                liv.SubItems.Add(row[2].ToString());
                liv.SubItems.Add(row[3].ToString());
            }
        }
        void checkDayDatTrc(float num)
        {
            if (num < 0) {
                DialogResult dr;
                dr= MessageBox.Show("Ngày dự kiến đi của khách vẫn chưa tới, bạn có chắc muốn thanh toán!!","Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    string sql = string.Format("insert into hoadon (MaPhong,TenKH,MaNV,NgayLapHoaDon,NgayDen,NgayDi,Time_come,Time_end,TongTien) values ({0},N'{1}',{2},'{4}','{5}','{6}','{7}','{8}',{9})", Local_Info_room.MaPhong,Local_Info_room.TenKH,Local_info_employee.Id, DateTime.Now.ToString("MM/dd/yyyy"),txt_ngayden.Text,txt_ngaydi.Text,txt_gioden.Text,txt_giodi.Text,txt_tongtien.Text);
                    if (xldl.Them_Xoa_Sua(sql) > 0)
                    {
                        string sql_del = string.Format("delete phieu_thue_phong where MaPT ={1} delete khachhang where MaKH = {0}  update phong set TinhTrang= N'Trống' where MaPhong = {2} delete chitietsudungdichvu where MaPhong = {3}", Local_Info_room.MaKH,Local_Info_room.MaPT,Local_Info_room.MaPhong,Local_Info_room.MaPhong);
                        if (xldl.Them_Xoa_Sua(sql_del) > 0)
                        {
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                DialogResult dr;
                dr = MessageBox.Show("Bạn có muốn thanh toán hay không!!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.OK)
                {
                    string sql = string.Format("insert into hoadon (MaPhong,TenKH,MaNV,NgayLapHoaDon,NgayDen,NgayDi,Time_come,Time_end,TongTien) values ({0},N'{1}',{2},'{3}','{4}','{5}','{6}','{7}',{8})", Local_Info_room.MaPhong, Local_Info_room.TenKH, Local_info_employee.Id, DateTime.Now.ToString("MM/dd/yyyy"), txt_ngayden.Text, txt_ngaydi.Text, txt_gioden.Text, txt_giodi.Text, txt_tongtien.Text);
                    if (xldl.Them_Xoa_Sua(sql) > 0)
                    {
                        string sql_del = string.Format("delete phieu_thue_phong where MaPT ={1} delete khachhang where MaKH = {0}  update phong set TinhTrang= N'Trống' where MaPhong = {2}  delete chitietsudungdichvu where MaPhong = {3}", Local_Info_room.MaKH, Local_Info_room.MaPT, Local_Info_room.MaPhong,Local_Info_room.MaPhong);
                        if (xldl.Them_Xoa_Sua(sql_del) > 0)
                        {
                            this.Close(); 
                        }
                    }
                }
            }
            

        }
        private void Tinh_tien_Load(object sender, EventArgs e)
        {
            load_list_dv(); 
            string ttdv = string.Format("select sum(chitietsudungdichvu.ThanhTien)as tong from dichvu,chitietsudungdichvu where MaPhong ={0} and dichvu.MaDV = chitietsudungdichvu.MaDV ", Local_Info_room.MaPhong);
            string sql_price = string.Format("select Gia from loaiphong where TenLoai=N'{0}'", Local_Info_room.LoaiPhong);
            string Price = xldl.getOneRow(sql_price);
            string price_DV = string.Format("{0:0}", xldl.getOneRow(ttdv));
            if (price_DV == string.Empty)
            {
                txt_tiendichvu.Text = "0";
            }
            else txt_tiendichvu.Text =string.Format("{0:0}", xldl.getOneRow(ttdv));
            txt_tenKH.Text = Local_Info_room.TenKH;
            txt_cccd.Text = Local_Info_room.CMND;
            txt_sdt.Text = Local_Info_room.SDT;
            txt_loaiphong.Text = Local_Info_room.LoaiPhong;
            txt_tenphong.Text = Local_Info_room.TenPhong;
            txt_ngayden.Text = Local_Info_room.NgayDen;
            txt_gioden.Text = Local_Info_room.TGDen;
            txt_ngaydi.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txt_giodi.Text = DateTime.Now.ToString("HH:mm:00");
            txt_congviec.Text = xldl.getOneRow(string.Format("select CongViec from phieu_thue_phong where MaPT ={0} ",Local_Info_room.MaPT));
            txt_tiencoc.Text =string.Format("{0:0}", xldl.getOneRow(string.Format("select DatCoc from phieu_thue_phong where MaPT ={0} ", Local_Info_room.MaPT))); 
            string daycome = string.Format("{0:MM/dd/yyyy}", Local_Info_room.NgayDen);
            string daygo = DateTime.Now.ToString("MM-dd-yyyy");
            string timego = DateTime.Now.ToString("HH:mm:ss");
            txt_ngayo.Text = xldl.getOneRow(info.DayUse(daycome,daygo));
            if(txt_ngayo.Text =="0") txt_gioo.Text = xldl.getOneRow(info.TimeUseZeroDay(string.Format("{0:HH:mm:ss}", Local_Info_room.TGDen),timego ));
            else txt_gioo.Text = "0";
            double sum;
            if (txt_ngayo.Text == "0")
            {
                sum = double.Parse(Price.ToString()) + int.Parse(txt_gioo.Text) * 20000 - double.Parse(txt_tiencoc.Text);
                if (sum < 0) sum = double.Parse(Price.ToString()) + int.Parse(txt_gioo.Text) * 20000;
                else sum = double.Parse(Price.ToString()) + int.Parse(txt_gioo.Text) * 20000 - double.Parse(txt_tiencoc.Text);
            }
            else {
                sum = int.Parse(txt_ngayo.Text) * double.Parse(Price.ToString()) - double.Parse(txt_tiencoc.Text);
                if (sum < 0) sum = int.Parse(txt_ngayo.Text) * double.Parse(Price.ToString()) ;
                else sum = int.Parse(txt_ngayo.Text) * double.Parse(Price.ToString()) - double.Parse(txt_tiencoc.Text);
            } 
            txt_tien_phong.Text = sum.ToString();

            sumAll = sum + double.Parse(txt_tiendichvu.Text)  ;
            txt_tongtien.Text =string.Format("{0:0}",sumAll.ToString());
        }

        private void numRickPercent_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void txt_percent_TextChanged(object sender, EventArgs e)
        {

        }

        private void numRick_percent_ValueChanged(object sender, EventArgs e)
        {
            if (numRick_percent.Value < 0)
            {
                numRick_percent.Value = 0;
            }
            double sum = sumAll - (sumAll * double.Parse(numRick_percent.Value.ToString()) / 100);
            txt_tongtien.Text = string.Format("{0:0}", sum);
        }

        private void txt_money_minus_TextChanged(object sender, EventArgs e)
        {
            double sum = sumAll - (sumAll * double.Parse(numRick_percent.Value.ToString()) / 100) - double.Parse(txt_money_minus.Text);
            txt_tongtien.Text = string.Format("{0:0}", sum);
        }
    } 
}
