using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(new string[] { "QTKD", "CNTT", "NNA" });
            comboBox1.SelectedIndex = 0;
            if(dataGridView1.Columns.Count == 0)
            {
                dataGridView1.Columns.Add("MaSV", "Mã SV");
                dataGridView1.Columns.Add("HoTen", "Họ Tên");
                dataGridView1.Columns.Add("GioiTinh", "Giới Tính");
                dataGridView1.Columns.Add("DiemTB", "Điểm TB");
                dataGridView1.Columns.Add("Chuyennganh", "Khoa");
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.AllowUserToAddRows = false;
            }
            radioButton2.Checked = true;
            UpdateGioiTinhCount();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text) 
                || string.IsNullOrWhiteSpace(textBox2.Text) 
                || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!float.TryParse(textBox3.Text, out float diem) || diem < 0 || diem > 10)
            {
                MessageBox.Show("Điểm trung bình phải là số thực từ 0 đến 10!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string gioitinh = radioButton1.Checked ? "Nam" : "Nữ";
            string MaSV = textBox1.Text.Trim();
            string HoTen = textBox2.Text.Trim();
            string Chuyennganh = comboBox1.SelectedItem.ToString();
            DataGridViewRow existingRow = null;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["MaSV"].Value != null &&
                    row.Cells["MaSV"].Value.ToString().Equals(MaSV, StringComparison.OrdinalIgnoreCase))
                {
                    existingRow = row;
                    break;
                }
            }
            if (existingRow == null)
            {
                dataGridView1.Rows.Add(MaSV, HoTen, gioitinh, diem.ToString("0.00"), Chuyennganh);
            }
            else
            {
                existingRow.Cells["HoTen"].Value = HoTen;
                existingRow.Cells["GioiTinh"].Value = gioitinh;
                existingRow.Cells["DiemTB"].Value = diem.ToString("0.00");
                existingRow.Cells["Chuyennganh"].Value = Chuyennganh;
                MessageBox.Show("Cập nhật thông tin sinh viên thành công!", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            UpdateGioiTinhCount();
        }
        private void UpdateGioiTinhCount()
        {
            int nam = 0, nu = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                if (row.Cells["GioiTinh"].Value?.ToString()=="Nam") nam++;
                else if (row.Cells["GioiTinh"].Value?.ToString() == "Nữ") nu++;
            }
            demnam.Text = $"Nam: {nam}";
            demnu.Text = $"Nữ: {nu}";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView1.SelectedRows[0];
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?",
                                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dataGridView1.Rows.Remove(selectedRow);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateGioiTinhCount();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
