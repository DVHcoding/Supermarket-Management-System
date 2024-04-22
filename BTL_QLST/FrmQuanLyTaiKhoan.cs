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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BTL_QLST
{
    public partial class FrmQuanLyTaiKhoan : Form
    {
        string constr = "server=localhost;uid=root;password=;database=db_qlst";
        MySqlConnection con;
        public FrmQuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM user"; 

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
                    dataAccount.DataSource = dataTable;
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
        private void FrmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }
        
        private void dataAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có phải là hàng dữ liệu hay không

            if (e.RowIndex >= 0)
            {
                
                // Lấy dữ liệu từ hàng được chọn
                DataGridViewRow row = dataAccount.Rows[e.RowIndex];

                // Điền dữ liệu vào các ô input tương ứng
                txtTaiKhoan.Text = row.Cells["username"].Value.ToString();
                txtPassword.Text = row.Cells["password"].Value.ToString();
                txtFullName.Text = row.Cells["fullname"].Value.ToString();

                int roleValue = Convert.ToInt32(row.Cells["role"].Value);

                // Kiểm tra giá trị và chọn giá trị tương ứng trong ComboBox
                if (roleValue == 0)
                {
                    selectRole.SelectedIndex = selectRole.FindStringExact("Nhân Viên");
                }
                else if (roleValue == 1)
                {
                    selectRole.SelectedIndex = selectRole.FindStringExact("Quản Trị");
                }
               
            }
        }

        // #############################
        // #     THÊM MỘT TÀI KHOẢN    #
        // #############################
        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtTaiKhoan.Text;
            string password = txtPassword.Text;
            string fullname = txtFullName.Text;
            string selectedRoleText = selectRole.SelectedItem?.ToString();

            

            string query = "INSERT INTO `user` (username, `password`, fullname, role) VALUES (@username, @password, @fullname, @role)";
            using(con = new MySqlConnection(constr))
            {
                try
                {
                    // Kiểm tra xem các trường dữ liệu có bị bỏ trống không
                    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(selectedRoleText))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                        return; // Dừng thực hiện phương thức nếu có trường dữ liệu bị bỏ trống
                    }


                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@fullname", fullname);  
                    if (selectedRoleText == "Quản Trị")
                    {
                        cmd.Parameters.AddWithValue("@role", 1);
                    } else if(selectedRoleText == "Nhân Viên")
                    {
                        cmd.Parameters.AddWithValue("@role", 0);
                    }
                


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu đã được chèn thành công!");
                        LoadDataIntoDataGridView();
                    }
                    else
                    {
                        MessageBox.Show("Không thể chèn dữ liệu.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chèn dữ liệu: " + ex.Message);
                }
            }
        }

        // #############################
        // #     XÓA MỘT TÀI KHOẢN     #
        // #############################
        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Xác nhận xóa dữ liệu
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes
            if (result == DialogResult.Yes)
            {
                // Lấy username của dòng được chọn trong DataGridView
                string usernameToDelete = txtTaiKhoan.Text;

                // Tạo câu lệnh SQL để xóa dữ liệu dựa trên username
                string query = "DELETE FROM `user` WHERE username = @username";

                // Kết nối đến cơ sở dữ liệu và thực hiện xóa
                using (con = new MySqlConnection(constr))
                {
                    try
                    {
                        // Kiểm tra xem các trường dữ liệu có bị bỏ trống không
                        if (string.IsNullOrWhiteSpace(usernameToDelete) )
                        {
                            MessageBox.Show("Vui lòng nhập đúng tên tài khoản");
                            return; // Dừng thực hiện phương thức nếu có trường dữ liệu bị bỏ trống
                        }
                        con.Open();

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@username", usernameToDelete);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Dữ liệu đã được xóa thành công!");
                            LoadDataIntoDataGridView(); // Tải lại dữ liệu sau khi xóa
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
        // #     SỬA MỘT TÀI KHOẢN    #
        // #############################
        private void btnSua_Click(object sender, EventArgs e)
        {
            // Lấy thông tin mới từ các ô input
            string newUsername = txtTaiKhoan.Text;
            string newPassword = txtPassword.Text;
            string newFullname = txtFullName.Text;
            string newSelectedRoleText = selectRole.SelectedItem?.ToString();

            // Xác định username của dòng được chọn trong DataGridView
            string currentUsername = txtTaiKhoan.Text;

            // Tạo câu lệnh SQL để cập nhật dữ liệu dựa trên username
            string query = "UPDATE `user` SET username = @newUsername, `password` = @newPassword, fullname = @newFullname, role = @newRole WHERE username = @currentUsername";

            // Kết nối đến cơ sở dữ liệu và thực hiện cập nhật
            using (con = new MySqlConnection(constr))
            {
                try
                {
                    // Kiểm tra xem các trường dữ liệu có bị bỏ trống không
                    if (string.IsNullOrWhiteSpace(currentUsername))
                    {
                        MessageBox.Show("Vui lòng nhập đúng tên tài khoản");
                        return; // Dừng thực hiện phương thức nếu có trường dữ liệu bị bỏ trống
                    }
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@newUsername", newUsername);
                    cmd.Parameters.AddWithValue("@newPassword", newPassword);
                    cmd.Parameters.AddWithValue("@newFullname", newFullname);

                    // Đặt giá trị mới của role dựa trên newSelectedRoleText
                    if (newSelectedRoleText == "Quản Trị")
                    {
                        cmd.Parameters.AddWithValue("@newRole", 1);
                    }
                    else if (newSelectedRoleText == "Nhân Viên")
                    {
                        cmd.Parameters.AddWithValue("@newRole", 0);
                    }

             
                    cmd.Parameters.AddWithValue("@currentUsername", currentUsername);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Dữ liệu đã được cập nhật thành công!");
                        LoadDataIntoDataGridView(); // Tải lại dữ liệu sau khi cập nhật
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
        // #            THOÁT          #
        // #############################
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        // #############################
        // #     XUẤT FILE EXCEL    #
        // #############################
        private void btnExcel_Click(object sender, EventArgs e)
        {
            // Sao chép dữ liệu từ DataGridView vào Clipboard
            dataAccount.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataAccount.SelectAll();
            DataObject copyData = dataAccount.GetClipboardContent();
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
    }
}
