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
    public partial class FrmQuanTri : Form
    {

        public FrmQuanTri()
        {
            InitializeComponent();
        }

        private void đăngXuâtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            // Nếu người dùng chọn OK
            if (result == DialogResult.OK)
            {
                // Đóng form hiện tại
                this.Close();

                Application.OpenForms["FrmMain"].Show();
            }
        }

        private void quanLyTaiKhoanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQuanLyTaiKhoan frmQuanLyTaiKhoan = new FrmQuanLyTaiKhoan();
            frmQuanLyTaiKhoan.Show();
        }

        private void quanLySanPhâmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQuanLySanPham frmQuanLySanPham = new FrmQuanLySanPham();
            frmQuanLySanPham.Show();
        }

        private void quanLyĐơnHangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongTinDonHang frmThongTinDonHang = new FrmThongTinDonHang();
            frmThongTinDonHang.Show();
        }
    }
}
