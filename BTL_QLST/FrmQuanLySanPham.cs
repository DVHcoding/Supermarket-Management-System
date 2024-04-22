using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BTL_QLST
{
    public partial class FrmQuanLySanPham : Form
    {
        string constr = "server=localhost;uid=root;password=;database=db_qlst";
        private int selectedProductId = -1;
        MySqlConnection con;
        public FrmQuanLySanPham()
        {
            InitializeComponent();
        }


        private void UpdateProductCountLabel()
        {
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                try
                {
                    con.Open();

                    string query = "SELECT COUNT(*) FROM product";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    int productCount = Convert.ToInt32(cmd.ExecuteScalar());

                    labelSoLuongSanPham.Text = $"{productCount}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đếm số lượng sản phẩm: " + ex.Message);
                }
            }
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

        // #############################
        // #     LOAD DATA LÊN BẢNG    #
        // #############################
        private void FrmQuanLySanPham_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            UpdateProductCountLabel();
        }


        // #############################
        // #     THÊM MỘT SẢN PHẨM     #
        // #############################

        private void btnThem_Click(object sender, EventArgs e)
        {
            string ma_san_pham = txtMaSanPham.Text;
            string ten_san_pham = txtTenSanPham.Text;
            float gia_nhap, gia_ban;
            int so_luong;
            string noi_san_xuat = txtNoiSanXuat.Text;
            string don_vi_tinh = txtDonViTinh.Text;
            string nguoi_nhap = txtNguoiNhap.Text;


            string query = "INSERT INTO product (ma_san_pham,ten_sp, gia_nhap, gia_ban, so_luong, noi_sx, don_vi_tinh, nguoi_nhap) VALUES (@ma_san_pham,@ten_san_pham, @gia_nhap, @gia_ban, @so_luong, @noi_san_xuat, @don_vi_tinh, @nguoi_nhap)";
            using (con = new MySqlConnection(constr))
            {
                try 
                {
                    // Kiểm tra xem các trường dữ liệu có bị bỏ trống không và có đúng định dạng không
                    if (string.IsNullOrWhiteSpace(ma_san_pham) || string.IsNullOrWhiteSpace(ten_san_pham)
                        || !float.TryParse(txtGiaNhap.Text, out gia_nhap) 
                        || !float.TryParse(txtGiaBan.Text, out gia_ban)
                        || !int.TryParse(txtSoLuong.Text, out so_luong)
                        || so_luong <= 0 // Kiểm tra số lượng phải lớn hơn 0
                        || string.IsNullOrWhiteSpace(noi_san_xuat)
                        || string.IsNullOrWhiteSpace(don_vi_tinh)
                        || string.IsNullOrWhiteSpace(nguoi_nhap))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin và đúng định dạng!");
                        return; // Dừng thực hiện phương thức nếu có trường dữ liệu bị bỏ trống hoặc không đúng định dạng
                    }

                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ma_san_pham", ma_san_pham);
                    cmd.Parameters.AddWithValue("@ten_san_pham", ten_san_pham);
                    cmd.Parameters.AddWithValue("@gia_nhap", gia_nhap);
                    cmd.Parameters.AddWithValue("@gia_ban", gia_ban);
                    cmd.Parameters.AddWithValue("@so_luong", so_luong);
                    cmd.Parameters.AddWithValue("@noi_san_xuat", noi_san_xuat);
                    cmd.Parameters.AddWithValue("@don_vi_tinh", don_vi_tinh);
                    cmd.Parameters.AddWithValue("@nguoi_nhap", nguoi_nhap);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu đã được thêm thành công!");
                        LoadDataIntoDataGridView();
                        // Clear dữ liệu trong các ô TextBox
                        txtMaSanPham.Clear();
                        txtTenSanPham.Clear();
                        txtGiaNhap.Clear();
                        txtGiaBan.Clear();
                        txtSoLuong.Clear();
                        txtNoiSanXuat.Clear();
                        txtDonViTinh.Clear();
                        txtNguoiNhap.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm dữ liệu.");
                    }
                } catch(Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message);
                }
            }
        }


        // #############################
        // #     SỬA MỘT SẢN PHẨM      #
        // #############################
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn một sản phẩm để cập nhật chưa
            if (selectedProductId == -1)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để cập nhật.");
                return;
            }

            // Tiến hành cập nhật thông tin sản phẩm có product_id là selectedProductId 
            string ma_san_pham = txtMaSanPham.Text;
            string ten_san_pham = txtTenSanPham.Text;
            float gia_nhap, gia_ban;
            int so_luong;
            string noi_san_xuat = txtNoiSanXuat.Text;
            string don_vi_tinh = txtDonViTinh.Text;
            string nguoi_nhap = txtNguoiNhap.Text;

            string query = "UPDATE product SET ma_san_pham = @ma_san_pham, ten_sp = @ten_san_pham, gia_nhap = @gia_nhap, gia_ban = @gia_ban, so_luong = @so_luong, noi_sx = @noi_san_xuat, don_vi_tinh = @don_vi_tinh, nguoi_nhap = @nguoi_nhap WHERE product_id = @product_id";

            using (con = new MySqlConnection(constr))
            {
                try
                {
                    // Kiểm tra xem các trường dữ liệu có bị bỏ trống không và có đúng định dạng không
                    if (string.IsNullOrWhiteSpace(ma_san_pham) || string.IsNullOrWhiteSpace(ten_san_pham)
                        || !float.TryParse(txtGiaNhap.Text, out gia_nhap)
                        || !float.TryParse(txtGiaBan.Text, out gia_ban)
                        || !int.TryParse(txtSoLuong.Text, out so_luong)
                        || so_luong <= 0 // Kiểm tra số lượng phải lớn hơn 0
                        || string.IsNullOrWhiteSpace(noi_san_xuat)
                        || string.IsNullOrWhiteSpace(don_vi_tinh)
                        || string.IsNullOrWhiteSpace(nguoi_nhap))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin và đúng định dạng!");
                        return; // Dừng thực hiện phương thức nếu có trường dữ liệu bị bỏ trống hoặc không đúng định dạng
                    }

                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ma_san_pham", ma_san_pham);
                    cmd.Parameters.AddWithValue("@ten_san_pham", ten_san_pham);
                    cmd.Parameters.AddWithValue("@gia_nhap", gia_nhap);
                    cmd.Parameters.AddWithValue("@gia_ban", gia_ban);
                    cmd.Parameters.AddWithValue("@so_luong", so_luong);
                    cmd.Parameters.AddWithValue("@noi_san_xuat", noi_san_xuat);
                    cmd.Parameters.AddWithValue("@don_vi_tinh", don_vi_tinh);
                    cmd.Parameters.AddWithValue("@nguoi_nhap", nguoi_nhap);
                    cmd.Parameters.AddWithValue("@product_id", selectedProductId); // Sử dụng selectedProductId

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
                        LoadDataIntoDataGridView();
                        // Clear dữ liệu trong các ô TextBox
                        txtMaSanPham.Clear();
                        txtTenSanPham.Clear();
                        txtGiaNhap.Clear();
                        txtGiaBan.Clear();
                        txtSoLuong.Clear();
                        txtNoiSanXuat.Clear();
                        txtDonViTinh.Clear();
                        txtNguoiNhap.Clear();

                        // Reset selectedProductId về -1
                        selectedProductId = -1;
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật dữ liệu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
            }
        }

        // #############################
        // #     XÓA MỘT SẢN PHẨM      #
        // #############################
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Xác nhận xóa dữ liệu
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes
            if (result == DialogResult.Yes)
            {
                // Kiểm tra xem đã chọn một sản phẩm để cập nhật chưa
                if (selectedProductId == -1)
                {
                    MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.");
                    return;
                }
                

                // Tạo câu lệnh SQL để xóa dữ liệu dựa trên username
                string query = "DELETE FROM `product` WHERE product_id = @product_id";

                // Kết nối đến cơ sở dữ liệu và thực hiện xóa
                using (con = new MySqlConnection(constr))
                {
                    try
                    {
                        con.Open();

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@product_id", selectedProductId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Dữ liệu đã được xóa thành công!");
                            LoadDataIntoDataGridView(); // Tải lại dữ liệu sau khi xóa

                            // Clear dữ liệu trong các ô TextBox
                            txtMaSanPham.Clear();
                            txtTenSanPham.Clear();
                            txtGiaNhap.Clear();
                            txtGiaBan.Clear();
                            txtSoLuong.Clear();
                            txtNoiSanXuat.Clear();
                            txtDonViTinh.Clear();
                            txtNguoiNhap.Clear();

                            // Reset selectedProductId về -1
                            selectedProductId = -1;
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


        private void txtGiaNhap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép nhập số, dấu chấm thập phân và phím control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Ngăn không cho nhập ký tự không phải số hoặc dấu chấm thập phân
            }

            // Không cho phép nhập nhiều hơn một dấu chấm thập phân
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void txtGiaBan_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép nhập số, dấu chấm thập phân và phím control
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Ngăn không cho nhập ký tự không phải số hoặc dấu chấm thập phân
            }

            // Không cho phép nhập nhiều hơn một dấu chấm thập phân
            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Không cho phép nhập ký tự không phải số
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


        // ###############################
        // #   HÀM TÌM KIẾM SẢN PHẨM     #
        // ###############################
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
                txtSoLuong.Text = row.Cells["so_luong"].Value.ToString();
                txtNoiSanXuat.Text = row.Cells["noi_sx"].Value.ToString();
                txtDonViTinh.Text = row.Cells["don_vi_tinh"].Value.ToString();
                txtNguoiNhap.Text = row.Cells["nguoi_nhap"].Value.ToString();
            }
        }

        // ###############################
        // #            EXCEL            #
        // ###############################
        private void btnExcel_Click(object sender, EventArgs e)
        {
            // Sao chép dữ liệu từ DataGridView vào Clipboard
            dataProducts.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataProducts.SelectAll();
            DataObject copyData = dataProducts.GetClipboardContent();
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
            this.Hide();
        }
    }
}
