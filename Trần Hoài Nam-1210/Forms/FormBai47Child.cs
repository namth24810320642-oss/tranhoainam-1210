using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using hoccsharp.Models;

namespace hoccsharp.Forms
{
    public class FormBai47Child : Form
    {
        private DataGridView dgvData = null!;
        private ContextMenuStrip contextMenu = null!;

        public FormBai47Child()
        {
            InitializeComponentCustom();
            SetupContextMenu();
        }

        private void InitializeComponentCustom()
        {
            this.Size = new Size(600, 380);
            dgvData = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvData);

            this.Load += FormBai47Child_Load;
        }

        private void FormBai47Child_Load(object sender, EventArgs e)
        {
            if (this.Text.Contains("Sinh viên"))
            {
                dgvData.DataSource = new BindingList<Student>
                {
                    new Student("SV01", "Nguyễn Văn A", 20, "Nam"),
                    new Student("SV02", "Lê Thị B", 22, "Nữ")
                };
            }
            else
            {
                dgvData.DataSource = new BindingList<Product>
                {
                    new Product("SP01", "Laptop Dell", 15000000),
                    new Product("SP02", "Chuột Logitech", 350000)
                };
            }
        }

        private void SetupContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            ToolStripMenuItem itemDelete = new ToolStripMenuItem("Xóa dòng chọn", null, (s, e) => DeleteRow());
            ToolStripMenuItem itemRefresh = new ToolStripMenuItem("Làm mới dữ liệu", null, (s, e) => RefreshData());
            ToolStripMenuItem itemDetail = new ToolStripMenuItem("Xem chi tiết", null, (s, e) => ShowDetails());

            contextMenu.Items.AddRange(new ToolStripItem[] { itemDelete, itemRefresh, itemDetail });
            dgvData.ContextMenuStrip = contextMenu;
        }

        private void DeleteRow()
        {
            if (dgvData.CurrentRow != null && !dgvData.CurrentRow.IsNewRow)
            {
                dgvData.Rows.Remove(dgvData.CurrentRow);
                NotifyParentStatus("Đã xóa 1 dòng dữ liệu.");
            }
        }

        private void RefreshData()
        {
            dgvData.Refresh();
            NotifyParentStatus("Đã làm mới danh sách dữ liệu.");
        }

        private void ShowDetails()
        {
            if (dgvData.CurrentRow != null)
            {
                MessageBox.Show($"Chi tiết bản ghi đang chọn ở dòng thứ: {dgvData.CurrentRow.Index + 1}", "Chi tiết");
            }
        }

        private void NotifyParentStatus(string message)
        {
            if (this.MdiParent is FormBai47Main parentMain)
            {
                parentMain.UpdateStatus(message);
            }
        }
    }
}