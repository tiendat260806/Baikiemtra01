using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WINFORMS_GUI
{
    public partial class Form1 : Form
    {
        public class Product
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string Category { get; set; }
            public decimal UnitPrice { get; set; }
            public int Quantity { get; set; }
            public string ImagePath { get; set; }
        }
        public class CategoryItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        private BindingList<Product> _allProducts = new BindingList<Product>();
        private BindingSource _bindingSource = new BindingSource();
        private string _selectedImagePath = "";

        public Form1()
        {
            InitializeComponent();
            InitCustomComponents();
        }

        private void InitCustomComponents()
        {
            var categories = new[]
            {
                new CategoryItem { Id = "DT", Name = "Điện thoại" },
                new CategoryItem { Id = "LT", Name = "Laptop" },
                new CategoryItem { Id = "PK", Name = "Phụ kiện" }
            };
            cboCategory.DataSource = categories;
            cboCategory.DisplayMember = "Name";
            cboCategory.ValueMember = "Name";
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Mã SP",
                DataPropertyName = "ProductId",
                Width = 90
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tên SP",
                DataPropertyName = "ProductName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Danh Mục",
                DataPropertyName = "Category",
                Width = 110
            });
            var colPrice = new DataGridViewTextBoxColumn
            {
                HeaderText = "Đơn Giá",
                DataPropertyName = "UnitPrice",
                Width = 120
            };
            colPrice.DefaultCellStyle.Format = "N0";
            dgvProducts.Columns.Add(colPrice);

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Số Lượng",
                DataPropertyName = "Quantity",
                Width = 90
            });

            _bindingSource.DataSource = _allProducts;
            dgvProducts.DataSource = _bindingSource;
            this.Load += Form1_Load;
            btnChooseImage.Click += BtnChooseImage_Click;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
            exportCSVToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            exportCSVToolStripMenuItem.Click += ExportCSVToolStripMenuItem_Click;
            exitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            exitToolStripMenuItem.Click += (s, e) => Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _allProducts.Add(new Product { ProductId = "SP01", ProductName = "iPhone 15 Pro Max", Category = "Điện thoại", UnitPrice = 30000000m, Quantity = 10, ImagePath = "" });
            _allProducts.Add(new Product { ProductId = "SP02", ProductName = "Chuột Logitech MX Master 3S", Category = "Phụ kiện", UnitPrice = 2500000m, Quantity = 15, ImagePath = "" });

            UpdateStatusCount();
        }

        private void UpdateStatusCount()
        {
            lblStatus.Text = $"Tổng số sản phẩm: {_allProducts.Count}";
        }
        private void BtnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh sản phẩm";
                ofd.Filter = "Ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _selectedImagePath = ofd.FileName;
                    picAvatar.Image = Image.FromFile(_selectedImagePath);
                }
            }
        }
        private bool ValidateInput()
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên sản phẩm không được để trống!");
                isValid = false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải là số và lớn hơn 0!");
                isValid = false;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên không âm (≥ 0)!");
                isValid = false;
            }

            return isValid;
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string id = string.IsNullOrWhiteSpace(txtProductId.Text)
                ? "SP" + (_allProducts.Count + 1).ToString("D2")
                : txtProductId.Text.Trim();

            Product newP = new Product
            {
                ProductId = id,
                ProductName = txtProductName.Text.Trim(),
                Category = cboCategory.SelectedValue?.ToString(),
                UnitPrice = decimal.Parse(txtUnitPrice.Text),
                Quantity = int.Parse(txtQuantity.Text),
                ImagePath = _selectedImagePath
            };

            _allProducts.Add(newP);
            _bindingSource.ResetBindings(false);
            UpdateStatusCount();
            ClearForm();
            MessageBox.Show("Thêm mới sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;
            if (!ValidateInput()) return;

            var current = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (current != null)
            {
                current.ProductName = txtProductName.Text.Trim();
                current.Category = cboCategory.SelectedValue?.ToString();
                current.UnitPrice = decimal.Parse(txtUnitPrice.Text);
                current.Quantity = int.Parse(txtQuantity.Text);
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    current.ImagePath = _selectedImagePath;
                }

                _bindingSource.ResetBindings(false);
                MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            var current = dgvProducts.CurrentRow.DataBoundItem as Product;
            if (current != null)
            {
                DialogResult dialog = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa sản phẩm '{current.ProductName}' không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    _allProducts.Remove(current);
                    _bindingSource.ResetBindings(false);
                    UpdateStatusCount();
                    ClearForm();
                }
            }
        }
        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null)
            {
                var p = dgvProducts.CurrentRow.DataBoundItem as Product;
                if (p != null)
                {
                    txtProductId.Text = p.ProductId;
                    txtProductName.Text = p.ProductName;
                    cboCategory.SelectedValue = p.Category;
                    txtUnitPrice.Text = p.UnitPrice.ToString("0.##");
                    txtQuantity.Text = p.Quantity.ToString();
                    _selectedImagePath = p.ImagePath;

                    if (!string.IsNullOrEmpty(p.ImagePath) && File.Exists(p.ImagePath))
                    {
                        picAvatar.Image = Image.FromFile(p.ImagePath);
                    }
                    else
                    {
                        picAvatar.Image = null;
                    }
                }
            }
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                _bindingSource.DataSource = _allProducts;
            }
            else
            {
                var filtered = _allProducts
                    .Where(p => p.ProductName.ToLower().Contains(keyword))
                    .ToList();
                _bindingSource.DataSource = new BindingList<Product>(filtered);
            }
        }
        private void ExportCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuất danh mục sản phẩm ra CSV";
                sfd.Filter = "CSV File (*.csv)|*.csv";
                sfd.FileName = "DanhSachSanPham.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã SP,Tên SP,Danh Mục,Đơn Giá,Số Lượng");

                    foreach (var p in _allProducts)
                    {
                        sb.AppendLine($"\"{p.ProductId}\",\"{p.ProductName}\",\"{p.Category}\",{p.UnitPrice},{p.Quantity}");
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất danh sách ra file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ClearForm()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            picAvatar.Image = null;
            _selectedImagePath = "";
            errorProvider1.Clear();
        }
    private void exportToolStripMenuItem_Click(object sender, EventArgs e) { }
    private void toolStripStatusLabel1_Click(object sender, EventArgs e) { }
    private void groupBox1_Enter(object sender, EventArgs e) { }
    private void button1_Click(object sender, EventArgs e) { }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
