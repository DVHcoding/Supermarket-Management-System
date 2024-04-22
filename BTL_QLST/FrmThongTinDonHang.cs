using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_QLST
{
    public partial class FrmThongTinDonHang : Form
    {
        string constr = "server=localhost;uid=root;password=;database=db_qlst";
        MySqlConnection con;
        private int selectedOrderId = -1;
        public FrmThongTinDonHang()
        {
            InitializeComponent();
        }

        private void LoadDataBanHang()
        {
            string query = "SELECT order_id, ma_don_hang, ten_don_hang, so_luong, don_vi_tinh, tong_gia_tri, ngay_mua FROM `order`";

            using (con = new MySqlConnection(constr))
            {
                try
                {
                    con.Open();

                    // Tạo SqlDataAdapter và DataTable
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    DataTable dataTable = new DataTable();

                    // Đổ dữ liệu từ cơ sở dữ liệu vào DataTable
                    adapter.Fill(dataTable);

                    // Gán DataTable làm nguồn dữ liệu cho DataGridView
                    dataDaBan.DataSource = dataTable;
                    dataDaBan.Columns["ma_don_hang"].HeaderText = "Mã Sản Phẩm";
                    dataDaBan.Columns["ten_don_hang"].HeaderText = "Tên Sản Phẩm";
                    dataDaBan.Columns["so_luong"].HeaderText = "Số Lượng";
                    dataDaBan.Columns["don_vi_tinh"].HeaderText = "Đơn vị tính";
                    dataDaBan.Columns["ngay_mua"].HeaderText = "Ngày Mua";
                    dataDaBan.Columns["tong_gia_tri"].HeaderText = "Tổng giá trị";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }


        private void FrmThongTinDonHang_Load(object sender, EventArgs e)
        {
            LoadDataBanHang();
        }

        private void dataDaBan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải là hàng dữ liệu hay không

            if (e.RowIndex >= 0)
            {

                // Lấy dữ liệu từ hàng được chọn
                DataGridViewRow row = dataDaBan.Rows[e.RowIndex];

                // Lấy product_id từ hàng được chọn
                if (int.TryParse(row.Cells["order_id"].Value.ToString(), out int orderId))
                {
                    selectedOrderId = orderId; // Lưu trữ order_id được chọn
                }
                else
                {
                    selectedOrderId = -1; // Nếu không thành công, đặt giá trị của selectOrderId là -1
                }

                // Điền dữ liệu vào các ô input tương ứng
                txtMaDonHang.Text = row.Cells["ma_don_hang"].Value.ToString();
                txtTenDonHang.Text = row.Cells["ten_don_hang"].Value.ToString();
                txtSoLuong.Text = row.Cells["so_luong"].Value.ToString();
                txtNgayMua.Text = row.Cells["ngay_mua"].Value.ToString();
                txtDonViTinh.Text = row.Cells["don_vi_tinh"].Value.ToString();
                txtMaDonHang.Text = row.Cells["ma_don_hang"].Value.ToString();
                txtTongGiaTri.Text = row.Cells["tong_gia_tri"].Value.ToString();
            }
        }


        // #############################
        // #     XÓA MỘT ORDER         #
        // #############################
        private void btnXoa_Click(object sender, EventArgs e)
        {

            // Xác nhận xóa dữ liệu
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes
            if (result == DialogResult.Yes)
            {

                // Kiểm tra xem đã chọn một order để xóa hay chưa
                if (selectedOrderId == -1)
                {
                    MessageBox.Show("Vui lòng chọn một hàng trong bản order để xóa.");
                    return;
                }


                // Tạo câu lệnh SQL để xóa dữ liệu dựa trên username
                string query = "DELETE FROM `order` WHERE order_id = @order_id";

                // Kết nối đến cơ sở dữ liệu và thực hiện xóa
                using (con = new MySqlConnection(constr))
                {
                    try
                    {
                        con.Open();

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@order_id", selectedOrderId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Dữ liệu đã được xóa thành công!");
                            LoadDataBanHang(); // Tải lại dữ liệu sau khi xóa

                            // Reset selectedProductId về -1
                            selectedOrderId = -1;
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa dữ liệu.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);
                    }
                }
            }
        }


        // #############################
        // #            EXCEL          #
        // #############################
        private void btnExcel_Click(object sender, EventArgs e)
        {
            // Sao chép dữ liệu từ DataGridView vào Clipboard
            dataDaBan.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataDaBan.SelectAll();
            DataObject copyData = dataDaBan.GetClipboardContent();
            if (copyData != null)
            {
                Clipboard.SetDataObject(copyData);

                // Mở ứng dụng Excel và dán dữ liệu
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = true;
                Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(System.Reflection.Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range xlRange = (Microsoft.Office.Interop.Excel.Range)xlWorksheet.Cells[1, 1];

                xlWorksheet.PasteSpecial(xlRange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            }
        }


        // ###############################
        // #            THOAT            #
        // ###############################
        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Xác nhận xóa dữ liệu
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
            }
        }



        // #############################
        // #     TÌM KIẾM SẢN PHẨM     #
        // #############################
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            if (!string.IsNullOrEmpty(keyword))
            {
                dataDaBan.DataSource = SearchOrder(keyword);
            }
            else
            {
                LoadDataBanHang();
            }
        }

        private DataTable SearchOrder(string keyword)
        {
            DataTable dataTable = new DataTable();

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT * FROM `order` WHERE ma_don_hang LIKE @keyword OR ten_don_hang LIKE @keyword OR so_luong LIKE @keyword OR don_vi_tinh LIKE @keyword OR ngay_mua LIKE @keyword OR tong_gia_tri LIKE @keyword";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm dữ liệu: " + ex.Message);
                }
            }

            return dataTable;
        }
    }
}
