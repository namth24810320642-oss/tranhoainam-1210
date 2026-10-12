using System;
using System.Drawing;
using System.Windows.Forms;

namespace hoccsharp.Forms
{
    public class FormMenu : Form
    {
        private Button btnBai46 = null!;
        private Button btnBai47 = null!;
        private Button btnBai48 = null!;

        public FormMenu()
        {
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "HỆ THỐNG BÀI TẬP C#";
            this.Size = new Size(420, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblTitle = new Label
            {
                Text = "CHỌN BÀI TẬP CẦN THỰC THI",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            btnBai46 = new Button { Text = "Bài 4.6: BindingSource & File CSV/Avatar", Location = new Point(50, 60), Size = new Size(300, 45) };
            btnBai47 = new Button { Text = "Bài 4.7: Khung Ứng Dụng MDI & Đa Cửa Sổ", Location = new Point(50, 120), Size = new Size(300, 45) };
            btnBai48 = new Button { Text = "Bài 4.8: Lập Trình Bất Đồng Bộ (Async/Await)", Location = new Point(50, 180), Size = new Size(300, 45) };

            btnBai46.Click += (s, e) => new FormBai46().ShowDialog();
            btnBai47.Click += (s, e) => new FormBai47Main().Show();
            btnBai48.Click += (s, e) => new FormBai48().ShowDialog();

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnBai46);
            this.Controls.Add(btnBai47);
            this.Controls.Add(btnBai48);
        }
    }
}