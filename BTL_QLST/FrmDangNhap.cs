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
    public partial class FrmDangNhap : Form
    {

        string constr = "server=localhost;uid=root;password=;database=db_qlst";
        MySqlConnection con;
        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            string query = "SELECT * FROM user WHERE username = @username AND password = @password";

            using (con = new MySqlConnection(constr))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string role = reader["role"].ToString();
                            string name = reader["username"].ToString();

                            MessageBox.Show("Đăng nhập thành công!");

                            if (role == "1")
                            {
                               
                                FrmQuanTri frmQuanTri = new FrmQuanTri();
                                frmQuanTri.Show();
                                this.Close();
                            }
                            if (role == "0")
                            {
                                FrmBanHang frmNhanVien = new FrmBanHang();
                                frmNhanVien.SetUserName(name + " (nhân viên)");
                                frmNhanVien.Show();
                                this.Close();
                            }

                            // Ẩn FrmMain nếu tồn tại
                            if (Application.OpenForms["FrmMain"] != null)
                            {
                                Application.OpenForms["FrmMain"].Hide();
                            }



                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên người dùng hoặc mật khẩu không đúng!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối đến cơ sở dữ liệu: " + ex.Message);
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void checkShowHide_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = checkShowHide.Checked ? '\0' : '*';
        }
    }
}
