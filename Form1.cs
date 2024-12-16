using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUOI6.Model;

namespace BUOI6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                List<Student> students = context.Students.ToList(); //lấy các sinh viên
                List<Faculty> faculties = context.Faculties.ToList(); //lấy các khoa
                FillFalcultyCombobox(faculties);
                BindGrid(students);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void FillFalcultyCombobox(List<Faculty> faculties)
        {
            this.cmbFact.DataSource = faculties;
            this.cmbFact.DisplayMember = "FacultyName";
            this.cmbFact.ValueMember = "FacultyID";
        }


        private void BindGrid(List<Student> students)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in students)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                dataGridView1.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells[3].Value = item.AverageScore;
                
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có ô nào được chọn không
            if (dataGridView1.CurrentCell != null)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex; // Lấy chỉ số hàng của ô đang chọn

                // Kiểm tra chỉ số hợp lệ và không phải hàng mới
                if (rowIndex >= 0 && !dataGridView1.Rows[rowIndex].IsNewRow)
                {
                    // Xác nhận trước khi xóa
                    var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này không?",
                                                 "Xác nhận",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        dataGridView1.Rows.RemoveAt(rowIndex); // Xóa hàng tại chỉ số rowIndex
                        MessageBox.Show("Đã xóa sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Không thể xóa hàng trống hoặc không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một ô để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtScore.Text) || string.IsNullOrEmpty(cmbFact.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float averageScore;
            if (!float.TryParse(txtScore.Text, out averageScore))
            {
                MessageBox.Show("Điểm trung bình không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (IsStudentIDExist(txtID.Text))
            {
                MessageBox.Show("Mã sinh viên đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Thêm sinh viên
            AddStudent(txtID.Text, txtName.Text, cmbFact.Text, averageScore);
        }

        private void AddStudent(string studentID, string fullName, string fact, float score)
        {
            dataGridView1.Rows.Add(studentID, fullName, fact, score.ToString());
            MessageBox.Show("Thông tin sinh viên đã được thêm mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtName.Text) ||
        string.IsNullOrEmpty(txtScore.Text) || string.IsNullOrEmpty(cmbFact.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            float averageScore;
            if (!float.TryParse(txtScore.Text, out averageScore))
            {
                MessageBox.Show("Điểm trung bình không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedRow = GetSelectedRow(txtID.Text);
            if (selectedRow >= 0)
            {
                // Nếu ID mới trùng với một ID khác ngoài dòng hiện tại
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (i != selectedRow && dataGridView1.Rows[i].Cells[0].Value?.ToString() == txtID.Text)
                    {
                        MessageBox.Show("Mã sinh viên đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                UpdateStudent(selectedRow, txtID.Text, txtName.Text, cmbFact.Text, averageScore);
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateStudent(int rowIndex, string studentID, string fullName, string fact, float averageScore)
        {
            dataGridView1.Rows[rowIndex].Cells[0].Value = studentID;
            dataGridView1.Rows[rowIndex].Cells[1].Value = fullName;
            dataGridView1.Rows[rowIndex].Cells[2].Value = fact;
            dataGridView1.Rows[rowIndex].Cells[3].Value = averageScore.ToString();

            MessageBox.Show("Thông tin sinh viên đã được cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int GetSelectedRow(string studentID)
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null && !string.IsNullOrEmpty(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == studentID)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private bool IsStudentIDExist(string studentID)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == studentID)
                {
                    return true;
                }
            }
            return false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Lấy dữ liệu từ các ô trong hàng được chọn
                txtID.Text = selectedRow.Cells[0].Value?.ToString();
                txtName.Text = selectedRow.Cells[1].Value?.ToString();
                cmbFact.SelectedItem = cmbFact.Items.Cast<Faculty>()
                                      .FirstOrDefault(f => f.FacultyName == selectedRow.Cells[2].Value?.ToString());
                txtScore.Text = selectedRow.Cells[3].Value?.ToString();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn thoát chương trình hay không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
