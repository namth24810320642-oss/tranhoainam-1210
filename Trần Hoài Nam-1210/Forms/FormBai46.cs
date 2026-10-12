using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using hoccsharp.Models;

namespace hoccsharp.Forms
{
    public class FormBai46 : Form
    {
        private BindingList<Student> _studentList = new BindingList<Student>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly string _dataFilePath = Path.Combine(Application.StartupPath, "data_students.csv");

        private DataGridView dgvStudents = null!;
        private TextBox txtStudentId = null!, txtFullName = null!, txtAge = null!, txtAvatarPath = null!;
        private ComboBox cbGender = null!;
        private PictureBox picAvatar = null!;
        private Button btnBrowse = null!, btnAdd = null!, btnUpdate = null!, btnDelete = null!, btnExportCsv = null!;

        public FormBai46()
        {
            InitializeComponentCustom();
            LoadDataFromFile();
            SetupBinding();
            this.FormClosing += (s, e) => SaveDataToFile();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Bài 4.6: Quản lý Sinh viên";
            this.Size = new Size(850, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            GroupBox gbInput = new GroupBox { Text = "Thông tin sinh viên", Location = new Point(12, 12), Size = new Size(520, 200) };
            
            gbInput.Controls.Add(new Label { Text = "Mã SV:", Location = new Point(15, 30), AutoSize = true });
            txtStudentId = new TextBox { Location = new Point(90, 27), Size = new Size(150, 23) };

            gbInput.Controls.Add(new Label { Text = "Họ tên:", Location = new Point(15, 65), AutoSize = true });
            txtFullName = new TextBox { Location = new Point(90, 62), Size = new Size(150, 23) };

            gbInput.Controls.Add(new Label { Text = "Tuổi:", Location = new Point(15, 100), AutoSize = true });
            txtAge = new TextBox { Location = new Point(90, 97), Size = new Size(150, 23) };

            gbInput.Controls.Add(new Label { Text = "Giới tính:", Location = new Point(15, 135), AutoSize = true });
            cbGender = new ComboBox { Location = new Point(90, 132), Size = new Size(150, 23), DropDownStyle = ComboBoxStyle.DropDownList };
            cbGender.Items.AddRange(new object[] { "Nam", "Nữ" });

            gbInput.Controls.Add(new Label { Text = "Ảnh SV:", Location = new Point(15, 168), AutoSize = true });
            txtAvatarPath = new TextBox { Location = new Point(90, 165), Size = new Size(310, 23), ReadOnly = true };
            btnBrowse = new Button { Text = "Chọn ảnh", Location = new Point(410, 164), Size = new Size(95, 25) };

            gbInput.Controls.AddRange(new Control[] { txtStudentId, txtFullName, txtAge, cbGender, txtAvatarPath, btnBrowse });

            GroupBox gbAvatar = new GroupBox { Text = "Avatar", Location = new Point(545, 12), Size = new Size(275, 200) };
            picAvatar = new PictureBox { Location = new Point(15, 25), Size = new Size(245, 160), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            gbAvatar.Controls.Add(picAvatar);

            btnAdd = new Button { Text = "Thêm", Location = new Point(12, 220), Size = new Size(100, 32) };
            btnUpdate = new Button { Text = "Cập nhật", Location = new Point(125, 220), Size = new Size(100, 32) };
            btnDelete = new Button { Text = "Xóa", Location = new Point(238, 220), Size = new Size(100, 32) };
            btnExportCsv = new Button { Text = "Xuất File Khác", Location = new Point(351, 220), Size = new Size(120, 32) };

            dgvStudents = new DataGridView { Location = new Point(12, 265), Size = new Size(808, 200), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };

            this.Controls.AddRange(new Control[] { gbInput, gbAvatar, btnAdd, btnUpdate, btnDelete, btnExportCsv, dgvStudents });

            btnBrowse.Click += BtnBrowse_Click;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            btnExportCsv.Click += BtnExportCsv_Click;
        }

        private void SetupBinding()
        {
            _bindingSource.DataSource = _studentList;
            dgvStudents.DataSource = _bindingSource;

            txtStudentId.DataBindings.Clear();
            txtFullName.DataBindings.Clear();
            txtAge.DataBindings.Clear();
            cbGender.DataBindings.Clear();
            txtAvatarPath.DataBindings.Clear();

            txtStudentId.DataBindings.Add("Text", _bindingSource, nameof(Student.StudentId), true, DataSourceUpdateMode.OnPropertyChanged);
            txtFullName.DataBindings.Add("Text", _bindingSource, nameof(Student.FullName), true, DataSourceUpdateMode.OnPropertyChanged);
            txtAge.DataBindings.Add("Text", _bindingSource, nameof(Student.Age), true, DataSourceUpdateMode.OnPropertyChanged);
            cbGender.DataBindings.Add("Text", _bindingSource, nameof(Student.Gender), true, DataSourceUpdateMode.OnPropertyChanged);
            txtAvatarPath.DataBindings.Add("Text", _bindingSource, nameof(Student.AvatarPath), true, DataSourceUpdateMode.OnPropertyChanged);

            _bindingSource.CurrentChanged += (s, e) => LoadAvatarImage();
            LoadAvatarImage();
        }

        private void LoadDataFromFile()
        {
            _studentList.Clear();
            if (File.Exists(_dataFilePath))
            {
                var lines = File.ReadAllLines(_dataFilePath, Encoding.UTF8);
                for (int i = 1; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length >= 5)
                    {
                        string id = parts[0].Trim('"');
                        string name = parts[1].Trim('"');
                        int.TryParse(parts[2], out int age);
                        string gender = parts[3].Trim('"');
                        string avatar = parts[4].Trim('"');

                        _studentList.Add(new Student(id, name, age, gender, avatar));
                    }
                }
            }
            else
            {
                _studentList.Add(new Student("SV01", "Nguyễn Văn A", 20, "Nam", ""));
                _studentList.Add(new Student("SV02", "Trần Thị B", 21, "Nữ", ""));
            }
        }

        private void SaveDataToFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Mã SV,Họ tên,Tuổi,Giới tính,Đường dẫn Avatar");

            foreach (Student std in _studentList)
            {
                sb.AppendLine($"\"{std.StudentId}\",\"{std.FullName}\",{std.Age},\"{std.Gender}\",\"{std.AvatarPath}\"");
            }

            File.WriteAllText(_dataFilePath, sb.ToString(), Encoding.UTF8);
        }

        private void LoadAvatarImage()
        {
            if (_bindingSource.Current is Student currentStudent && !string.IsNullOrEmpty(currentStudent.AvatarPath) && File.Exists(currentStudent.AvatarPath))
            {
                using (var stream = new FileStream(currentStudent.AvatarPath, FileMode.Open, FileAccess.Read))
                {
                    picAvatar.Image = Image.FromStream(stream);
                }
            }
            else
            {
                picAvatar.Image = null;
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.png;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK && _bindingSource.Current is Student currentStudent)
                {
                    currentStudent.AvatarPath = ofd.FileName;
                    _bindingSource.ResetCurrentItem();
                    LoadAvatarImage();
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtAge.Text, out int age)) age = 18;
            var newStudent = new Student(txtStudentId.Text, txtFullName.Text, age, cbGender.Text, txtAvatarPath.Text);
            _studentList.Add(newStudent);
            _bindingSource.MoveLast();
            SaveDataToFile();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            _bindingSource.ResetCurrentItem();
            SaveDataToFile();
            MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_bindingSource.Current != null && MessageBox.Show("Bạn có chắc muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _bindingSource.RemoveCurrent();
                SaveDataToFile();
            }
        }

        private void BtnExportCsv_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File (*.csv)|*.csv", FileName = "DanhSachSinhVien.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã SV,Họ tên,Tuổi,Giới tính,Đường dẫn Avatar");

                    foreach (Student std in _studentList)
                    {
                        sb.AppendLine($"\"{std.StudentId}\",\"{std.FullName}\",{std.Age},\"{std.Gender}\",\"{std.AvatarPath}\"");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}