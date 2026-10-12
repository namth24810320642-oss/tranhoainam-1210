using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using hoccsharp.Models;

namespace hoccsharp.Forms
{
    public class FormBai48 : Form
    {
        private Button btnLoadData = null!;
        private ProgressBar progressBar = null!;
        private Label lblPercent = null!;
        private DataGridView dgvResult = null!;
        private readonly string _dataFilePath = Path.Combine(Application.StartupPath, "data_students.csv");

        public FormBai48()
        {
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Bài 4.8: Lập trình Bất đồng bộ (Async/Await) & ProgressBar";
            this.Size = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            btnLoadData = new Button { Text = "Nạp dữ liệu (Async)", Location = new Point(20, 20), Size = new Size(160, 35) };
            progressBar = new ProgressBar { Location = new Point(200, 20), Size = new Size(380, 35), Minimum = 0, Maximum = 100 };
            lblPercent = new Label { Text = "0%", Location = new Point(590, 28), AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            dgvResult = new DataGridView
            {
                Location = new Point(20, 75),
                Size = new Size(645, 310),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.AddRange(new Control[] { btnLoadData, progressBar, lblPercent, dgvResult });

            btnLoadData.Click += BtnLoadData_Click;
        }

        private async void BtnLoadData_Click(object sender, EventArgs e)
        {
            btnLoadData.Enabled = false;
            progressBar.Value = 0;
            lblPercent.Text = "0%";

            var progress = new Progress<int>(percent =>
            {
                progressBar.Value = percent;
                lblPercent.Text = $"{percent}%";

                if (this.MdiParent is FormBai47Main parentMain)
                {
                    parentMain.UpdateStatus($"Đang nạp dữ liệu: {percent}%");
                }
            });

            try
            {
                List<Student> result = await LoadStudentsAsync(progress);
                dgvResult.DataSource = result;

                if (this.MdiParent is FormBai47Main parentMain)
                {
                    parentMain.UpdateStatus("Nạp dữ liệu hoàn tất (100%)");
                }

                MessageBox.Show("Nạp dữ liệu từ Bài 4.6 hoàn tất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xảy ra lỗi trong quá trình nạp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoadData.Enabled = true;
            }
        }

        private async Task<List<Student>> LoadStudentsAsync(IProgress<int> progress)
        {
            return await Task.Run(() =>
            {
                List<Student> list = new List<Student>();

                if (File.Exists(_dataFilePath))
                {
                    var lines = File.ReadAllLines(_dataFilePath, Encoding.UTF8);
                    int totalRows = lines.Length - 1;

                    for (int i = 1; i < lines.Length; i++)
                    {
                        Task.Delay(200).Wait();

                        var parts = lines[i].Split(',');
                        if (parts.Length >= 5)
                        {
                            string id = parts[0].Trim('"');
                            string name = parts[1].Trim('"');
                            int.TryParse(parts[2], out int age);
                            string gender = parts[3].Trim('"');
                            string avatar = parts[4].Trim('"');

                            list.Add(new Student(id, name, age, gender, avatar));
                        }

                        int percentComplete = totalRows > 0 ? (i * 100) / totalRows : 100;
                        progress?.Report(percentComplete);
                    }
                }
                else
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        Task.Delay(150).Wait();
                        list.Add(new Student($"SV{i:D2}", $"Sinh viên mặc định {i}", 20, "Nam", ""));
                        progress?.Report(i * 20);
                    }
                }

                return list;
            });
        }
    }
}