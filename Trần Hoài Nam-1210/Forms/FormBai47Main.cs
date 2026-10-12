using System;
using System.Drawing;
using System.Windows.Forms;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace hoccsharp.Forms
{
    public class FormBai47Main : Form
    {
        private MenuStrip menuStrip = null!;
        private ToolStrip toolStrip = null!;
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel lblUserStatus = null!;
        private ToolStripStatusLabel lblOperationStatus = null!;
        private ToolStripStatusLabel lblTimeStatus = null!;
        private WinFormsTimer timerClock = null!;

        public FormBai47Main()
        {
            InitializeComponentCustom();
        }

        private void InitializeComponentCustom()
        {
            this.Text = "BÀI 4.7: KHUNG ỨNG DỤNG MDI & QUẢN LÝ ĐA CỬA SỔ";
            this.IsMdiContainer = true;
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            menuStrip = new MenuStrip();
            
            ToolStripMenuItem menuSystem = new ToolStripMenuItem("Hệ thống");
            ToolStripMenuItem menuLogout = new ToolStripMenuItem("Đăng xuất", null, (s, e) => UpdateStatus("Đã đăng xuất"));
            ToolStripMenuItem menuExit = new ToolStripMenuItem("Thoát", null, (s, e) => this.Close());
            menuSystem.DropDownItems.AddRange(new ToolStripItem[] { menuLogout, menuExit });

            ToolStripMenuItem menuCategory = new ToolStripMenuItem("Danh mục");
            ToolStripMenuItem menuStudents = new ToolStripMenuItem("Quản lý Sinh viên", null, (s, e) => OpenMdiChild<FormBai47Child>("Quản lý Sinh viên"));
            ToolStripMenuItem menuProducts = new ToolStripMenuItem("Quản lý Sản phẩm", null, (s, e) => OpenMdiChild<FormBai47Child>("Quản lý Sản phẩm"));
            menuCategory.DropDownItems.AddRange(new ToolStripItem[] { menuStudents, menuProducts });

            ToolStripMenuItem menuWindow = new ToolStripMenuItem("Cửa sổ");
            ToolStripMenuItem menuCascade = new ToolStripMenuItem("Cascade", null, (s, e) => this.LayoutMdi(MdiLayout.Cascade));
            ToolStripMenuItem menuTileH = new ToolStripMenuItem("Tile Horizontal", null, (s, e) => this.LayoutMdi(MdiLayout.TileHorizontal));
            ToolStripMenuItem menuTileV = new ToolStripMenuItem("Tile Vertical", null, (s, e) => this.LayoutMdi(MdiLayout.TileVertical));
            menuWindow.DropDownItems.AddRange(new ToolStripItem[] { menuCascade, menuTileH, menuTileV });

            menuStrip.Items.AddRange(new ToolStripItem[] { menuSystem, menuCategory, menuWindow });

            toolStrip = new ToolStrip();
            ToolStripButton btnShortcutStudent = new ToolStripButton("Sinh Viên", null, (s, e) => OpenMdiChild<FormBai47Child>("Quản lý Sinh viên"));
            ToolStripButton btnShortcutProduct = new ToolStripButton("Sản Phẩm", null, (s, e) => OpenMdiChild<FormBai47Child>("Quản lý Sản phẩm"));
            toolStrip.Items.AddRange(new ToolStripItem[] { btnShortcutStudent, btnShortcutProduct });

            statusStrip = new StatusStrip();
            lblUserStatus = new ToolStripStatusLabel("Người dùng: Admin | ");
            lblOperationStatus = new ToolStripStatusLabel("Sẵn sàng | ");
            lblTimeStatus = new ToolStripStatusLabel(DateTime.Now.ToString("HH:mm:ss"));
            statusStrip.Items.AddRange(new ToolStripItem[] { lblUserStatus, lblOperationStatus, lblTimeStatus });

            timerClock = new WinFormsTimer { Interval = 1000 };
            timerClock.Tick += (s, e) => lblTimeStatus.Text = DateTime.Now.ToString("HH:mm:ss");
            timerClock.Start();

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(toolStrip);
            this.Controls.Add(menuStrip);
            this.Controls.Add(statusStrip);
        }

        public void OpenMdiChild<T>(string title) where T : Form, new()
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T && child.Text == title)
                {
                    child.Activate();
                    UpdateStatus($"Đã chuyển đến cửa sổ: {title}");
                    return;
                }
            }

            T newChild = new T();
            newChild.Text = title;
            newChild.MdiParent = this;
            newChild.Show();
            UpdateStatus($"Đã mở cửa sổ: {title}");
        }

        public void UpdateStatus(string message)
        {
            lblOperationStatus.Text = $"{message} | ";
        }
    }
}