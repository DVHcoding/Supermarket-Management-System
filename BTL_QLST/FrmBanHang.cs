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
    public partial class FrmBanHang : Form
    {
        string constr = "server=localhost;uid=root;password=;database=db_qlst";
        MySqlConnection con;
        private int selectedProductId = -1;
        private int selectedOrderId = -1;
        public FrmBanHang()
        {
            InitializeComponent();
        }

        // Hiển thị xin chào ...
        public void SetUserName(string userName)
        {
            labelHello.Text = "Xin chào " + userName;
        }

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM product";

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
                    dataProducts.DataSource = dataTable;
                    dataProducts.Columns["product_id"].HeaderText = "Product Id";
                    dataProducts.Columns["ma_san_pham"].HeaderText = "Mã Sản Phẩm";
                    dataProducts.Columns["ten_sp"].HeaderText = "Tên Sản Phẩm";
                    dataProducts.Columns["gia_ban"].HeaderText = "Giá Bán";
                    dataProducts.Columns["so_luong"].HeaderText = "Số Lượng";
                    dataProducts.Columns["noi_sx"].HeaderText = "Nơi sản xuất";
                    dataProducts.Columns["don_vi_tinh"].HeaderText = "Đơn vị tính";
                    dataProducts.Columns["nguoi_nhap"].HeaderText = "Người nhập";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }

        private void LoadDataBanHang()
        {
            string query = "SELECT order_id, ma_don_hang, ten_don_hang, so_luong, don_vi_tinh, gia_ban, tong_gia_tri, doanh_thu FROM `order`";

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
                    dataDaBan.Columns["gia_ban"].HeaderText = "Giá Bán";
                    dataDaBan.Columns["so_luong"].HeaderText = "Số Lượng";
                    dataDaBan.Columns["don_vi_tinh"].HeaderText = "Đơn vị tính";
                    dataDaBan.Columns["tong_gia_tri"].HeaderText = "Thành tiền";
                    dataDaBan.Columns["doanh_thu"].HeaderText = "Doanh Thu";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
                }
            }
        }


        // #############################
        // #     LOAD DATA LÊN BẢNG    #
        // #############################
        private void FrmBanHang_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            LoadDataBanHang();
        }

        private void dataProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải là hàng dữ liệu hay không

            if (e.RowIndex >= 0)
            {

                // Lấy dữ liệu từ hàng được chọn
                DataGridViewRow row = dataProducts.Rows[e.RowIndex];

                // Lấy product_id từ hàng được chọn
                if (int.TryParse(row.Cells["product_id"].Value.ToString(), out int productId))
                {
                    selectedProductId = productId; // Lưu trữ product_id được chọn
                }
                else
                {
                    selectedProductId = -1; // Nếu không thành công, đặt giá trị của selectedProductId là -1
                }



                // Điền dữ liệu vào các ô input tương ứng
                txtMaSanPham.Text = row.Cells["ma_san_pham"].Value.ToString();
                txtTenSanPham.Text = row.Cells["ten_sp"].Value.ToString();
                txtGiaNhap.Text = row.Cells["gia_nhap"].Value.ToString();
                txtGiaBan.Text = row.Cells["gia_ban"].Value.ToString();
                txtSLSP.Text = row.Cells["so_luong"].Value.ToString();
                txtDonViTinh.Text = row.Cells["don_vi_tinh"].Value.ToString();

            }
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

            }
        }

        private void txtSoLuongNhap_TextChanged(object sender, EventArgs e)
        {
            // Loại bỏ chữ nếu nhập. Chỉ cho phép nhập số
            string input = txtSoLuongNhap.Text;
            string result = "";

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    result += c;
                }
            }

            txtSoLuongNhap.Text = result;
            txtSoLuongNhap.SelectionStart = result.Length;


            // Kiểm tra xem đã chọn một sản phẩm để cập nhật chưa
            if (selectedProductId == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để bán.");
                return;
            }

            // Lấy giá trị từ TextBox và chuyển đổi sang kiểu số
            double gia_nhap, gia_ban;
            int so_luong;
            int slsp_hien_co;
            

            if (!double.TryParse(txtGiaNhap.Text, out gia_nhap) || !double.TryParse(txtGiaBan.Text, out gia_ban) || !int.TryParse(txtSoLuongNhap.Text, out so_luong) || !int.TryParse(txtSLSP.Text, out slsp_hien_co))
            {
                // Nếu không thể chuyển đổi thành số, thông báo lỗi
                // Và không tiến hành tính toán
                labelThanhTien.Text = "0"; // Đặt giá trị mặc định cho labelThanhTien
                return;
            }

            

            // Tính toán giá trị của labelThanhTien
            double thanh_tien = gia_ban * so_luong;
            double loi_nhuan = so_luong * (gia_ban - gia_nhap);

            // Hiển thị giá trị của thanh_tien trong labelThanhTien
            labelThanhTien.Text = thanh_tien.ToString();
            labelDoanhThu.Text = loi_nhuan.ToString();

            if(so_luong > slsp_hien_co)
            {
                btnThem.Enabled = false;
                MessageBox.Show("Số lượng nhập không được quá số lượng sản phẩm hiện có");
                txtSoLuongNhap.Clear();
                labelDoanhThu.Text = "......";
                return;
            } else
            {
                btnThem.Enabled = true;
            }
        }

        // #############################
        // #     THÊM MỘT ORDER        #
        // #############################
        private void btnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn một sản phẩm để cập nhật chưa
            if (selectedProductId == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để bán.");
                return;
            }


            string ma_don_hang = txtMaSanPham.Text;
            string ten_don_hang = txtTenSanPham.Text;
            int so_luong;
            string don_vi_tinh = txtDonViTinh.Text;
            string ngay_mua = dateTimePicker.Value.ToString();
            double gia_ban;
            

            string query = "INSERT INTO `order` (ma_don_hang, ten_don_hang, so_luong, don_vi_tinh, ngay_mua, gia_ban, tong_gia_tri, doanh_thu) VALUES (@ma_don_hang, @ten_don_hang, @so_luong, @don_vi_tinh, @ngay_mua, @gia_ban, @tong_gia_tri, @doanh_thu)";
            using (con = new MySqlConnection(constr))
            {
                try
                {
                    // Kiểm tra xem các trường dữ liệu có bị bỏ trống không và có đúng định dạng không
                    if (string.IsNullOrWhiteSpace(ma_don_hang) || string.IsNullOrWhiteSpace(ten_don_hang)
                        || !double.TryParse(txtGiaBan.Text, out gia_ban)
                        || !int.TryParse(txtSoLuongNhap.Text, out so_luong)
                        || so_luong <= 0 // Kiểm tra số lượng phải lớn hơn 0
                        || string.IsNullOrWhiteSpace(don_vi_tinh))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin và đúng định dạng!");
                        return; // Dừng thực hiện phương thức nếu có trường dữ liệu bị bỏ trống hoặc không đúng định dạng
                    }

                    double tong_gia_tri = double.Parse(labelThanhTien.Text);
                    double doanh_thu = double.Parse(labelDoanhThu.Text);
                    

                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ma_don_hang", ma_don_hang);
                    cmd.Parameters.AddWithValue("@ten_don_hang", ten_don_hang);
                    cmd.Parameters.AddWithValue("@so_luong", so_luong);
                    cmd.Parameters.AddWithValue("@don_vi_tinh", don_vi_tinh);
                    cmd.Parameters.AddWithValue("@ngay_mua", ngay_mua);
                    cmd.Parameters.AddWithValue("@gia_ban", gia_ban);
                    cmd.Parameters.AddWithValue("@tong_gia_tri", tong_gia_tri);
                    cmd.Parameters.AddWithValue("@doanh_thu", doanh_thu);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu đã được thêm thành công!");
                        LoadDataBanHang();
                        // Clear dữ liệu trong các ô TextBox
                        txtMaSanPham.Clear();
                        txtTenSanPham.Clear();
                        txtDonViTinh.Clear();
                        txtGiaNhap.Clear();
                        txtGiaBan.Clear();
                        txtSLSP.Clear();
                        txtSoLuongNhap.Clear();
                        labelThanhTien.Text = "..................";
                        labelDoanhThu.Text = "..................";
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm dữ liệu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
                }
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
            // Xác nhận thoát
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                Application.OpenForms["FrmMain"].Show();
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
                dataProducts.DataSource = SearchProducts(keyword);
            }
            else
            {
                LoadDataIntoDataGridView();
            }
        }

        private DataTable SearchProducts(string keyword)
        {
            DataTable dataTable = new DataTable();

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT * FROM product WHERE ma_san_pham LIKE @keyword OR ten_sp LIKE @keyword OR gia_nhap LIKE @keyword OR gia_ban LIKE @keyword OR so_luong LIKE @keyword OR noi_sx LIKE @keyword OR don_vi_tinh LIKE @keyword OR nguoi_nhap LIKE @keyword";

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
