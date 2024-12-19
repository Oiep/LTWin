using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kiemtra.Models;

namespace kiemtra
{
    public partial class Form1 : Form
    {
        
       ModelDbSinhVien DbSinhVien = new ModelDbSinhVien();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setupDGVColumns();
            fillDGVSinhVien();
            fillCBBLop();
       

        }

        private void setupDGVColumns()
        {
            dgvListView.Columns.Clear(); // Xóa tất cả cột trước (nếu có)
            dgvListView.Columns.Add("MaSV", "Mã SV");
            dgvListView.Columns.Add("HoTenSV", "Họ Tên");
            dgvListView.Columns.Add("NgaySinh", "Ngày Sinh");
            dgvListView.Columns.Add("MaLop", "Mã Lớp");
        }

        private void fillCBBLop()
        {
            List<Lop> ListLop = DbSinhVien.Lops.ToList();

            cbbLop.DataSource = ListLop;
            cbbLop.DisplayMember = "TenLop";
            cbbLop.ValueMember = "MaLop";         


        }

        private void fillDGVSinhVien()
        {
            dgvListView.Rows.Clear();

            List<Sinhvien> ListSinhVien = DbSinhVien.Sinhviens.ToList();

            foreach (Sinhvien sinhvien in ListSinhVien)
            {
                int newRow = dgvListView.Rows.Add();

                dgvListView.Rows[newRow].Cells[0].Value = sinhvien.MaSV;
                dgvListView.Rows[newRow].Cells[1].Value = sinhvien.HoTenSV;
                dgvListView.Rows[newRow].Cells[2].Value = sinhvien.NgaySinh;
                dgvListView.Rows[newRow].Cells[3].Value = sinhvien.MaLop;

            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text;
            string hoTen = txtHoTen.Text;
            DateTime ngaySinh = dtNgaySinh.Value;
            string maLop = cbbLop.SelectedValue.ToString();

            Sinhvien newSinhVien = new Sinhvien
            {
                MaSV = maSV,
                HoTenSV = hoTen,
                NgaySinh = ngaySinh,
                MaLop = maLop
            };

            DbSinhVien.Sinhviens.Add(newSinhVien);
            DbSinhVien.SaveChanges();

            MessageBox.Show("Thêm sinh viên thành công!");
            fillDGVSinhVien();
            ClearInputFields();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvListView.SelectedRows.Count > 0)
            {
                string maSV = dgvListView.SelectedRows[0].Cells[0].Value.ToString();
                Sinhvien existingSinhVien = DbSinhVien.Sinhviens.FirstOrDefault(sv => sv.MaSV == maSV);


                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Xác nhận thoát", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {


                    if (existingSinhVien != null)
                    {
                        DbSinhVien.Sinhviens.Remove(existingSinhVien);
                        DbSinhVien.SaveChanges();

                        MessageBox.Show("Xóa sinh viên thành công!");
                        fillDGVSinhVien();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một sinh viên để xóa!");
                }
            }

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvListView.SelectedRows.Count > 0)
            {
                string maSV = dgvListView.SelectedRows[0].Cells[0].Value.ToString();
                Sinhvien existingSinhVien = DbSinhVien.Sinhviens.FirstOrDefault(sv => sv.MaSV == maSV);

                if (existingSinhVien != null)
                {
                    existingSinhVien.HoTenSV = txtHoTen.Text;
                    existingSinhVien.NgaySinh = dtNgaySinh.Value;
                    existingSinhVien.MaLop = cbbLop.SelectedValue.ToString();

                    DbSinhVien.SaveChanges();

                    MessageBox.Show("Cập nhật sinh viên thành công!");
                    fillDGVSinhVien();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa!");
            }
            ClearInputFields();
        }

        //Hiệu ứng cho nút lưu và không lưu
        private void setButtonState(bool isEditing)
        {
            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing;
            btnXoa.Enabled = !isEditing;

            btnLuu.Enabled = isEditing;
            btnKhong.Enabled = isEditing;

          
            
            //txtMaSV.Enabled = isEditing; 
            //txtHoTen.Enabled = isEditing;
            //dtNgaySinh.Enabled = isEditing;
            //cbbLop.Enabled = isEditing;
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtMaSV.Text = ""; // Xóa các ô nhập liệu
            txtHoTen.Text = "";
            dtNgaySinh.Value = DateTime.Now;
            cbbLop.SelectedIndex = -1;

            setButtonState(true); // Kích hoạt chế độ chỉnh sửa
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvListView.SelectedRows.Count > 0)
            {
                setButtonState(true); // Kích hoạt chế độ chỉnh sửa
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa!");
            }
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maSV = txtMaSV.Text;
            string hoTen = txtHoTen.Text;
            DateTime ngaySinh = dtNgaySinh.Value;
            string maLop = cbbLop.SelectedValue?.ToString();

            if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(maLop))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Kiểm tra xem đây là thêm mới hay chỉnh sửa
            Sinhvien sinhvien = DbSinhVien.Sinhviens.FirstOrDefault(s => s.MaSV == maSV);
            if (sinhvien == null)
            {
                // Thêm mới
                sinhvien = new Sinhvien { MaSV = maSV };
                DbSinhVien.Sinhviens.Add(sinhvien);
            }

            // Cập nhật thông tin
            sinhvien.HoTenSV = hoTen;
            sinhvien.NgaySinh = ngaySinh;
            sinhvien.MaLop = maLop;

            DbSinhVien.SaveChanges(); // Lưu vào cơ sở dữ liệu

            fillDGVSinhVien(); // Làm mới DataGridView
            setButtonState(false); // Tắt chế độ chỉnh sửa
        }

        private void btnKhong_Click(object sender, EventArgs e)
        {
            if (dgvListView.SelectedRows.Count > 0)
            {
                // Khôi phục dữ liệu từ hàng đang chọn
                DataGridViewRow selectedRow = dgvListView.SelectedRows[0];
                txtMaSV.Text = selectedRow.Cells[0].Value?.ToString();
                txtHoTen.Text = selectedRow.Cells[1].Value?.ToString();
                dtNgaySinh.Value = DateTime.Parse(selectedRow.Cells[2].Value?.ToString());
                cbbLop.SelectedValue = selectedRow.Cells[3].Value?.ToString();
            }
            else
            {
                // Nếu không có hàng nào được chọn
                txtMaSV.Text = "";
                txtHoTen.Text = "";
                dtNgaySinh.Value = DateTime.Now;
                cbbLop.SelectedIndex = -1;
            }

            setButtonState(false); // Tắt chế độ chỉnh sửa
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
           // Show a message box to confirm exit
        var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string searchName = txtTimKiem.Text.ToLower().Trim();

            List<Sinhvien> filteredList = DbSinhVien.Sinhviens
                .Where(sv => sv.HoTenSV.ToLower().Contains(searchName))
                .ToList();

            dgvListView.Rows.Clear();

            foreach (Sinhvien sinhvien in filteredList)
            {
                int newRow = dgvListView.Rows.Add();
                dgvListView.Rows[newRow].Cells[0].Value = sinhvien.MaSV;
                dgvListView.Rows[newRow].Cells[1].Value = sinhvien.HoTenSV;
                dgvListView.Rows[newRow].Cells[2].Value = sinhvien.NgaySinh;
                dgvListView.Rows[newRow].Cells[3].Value = sinhvien.MaLop;
            }

            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sinh viên nào với tên '" + searchName + "'.");
            }
        }
        private void ClearInputFields()
        {
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            dtNgaySinh.Value = DateTime.Now;
            cbbLop.SelectedIndex = -1;
        }
    }
}
