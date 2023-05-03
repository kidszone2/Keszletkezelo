namespace Hotcakes_orders_data_reading
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Hotcakes.CommerceDTO.v1;
    using Hotcakes.CommerceDTO.v1.Catalog;
    using Hotcakes.CommerceDTO.v1.Client;
    using Newtonsoft.Json;
    public partial class Form1 : Form
    {
        #region Variables

        List<string> category = new List<string>();
        List<string> bvin = new List<string>();
        List<int> quantity = new List<int>();
        List<string> SKU = new List<string>();
        List<string> product_name = new List<string>();
        List<string> categories = new List<string>();
        List<Product> products = new List<Product>();
        static readonly string url = "http://20.234.113.211:8090/";
        static readonly string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

        static Api proxy = new Api(url, key);

        //az összes kategória lekérése
        static ApiResponse<List<CategorySnapshotDTO>> response = proxy.CategoriesFindAll();
        static string json = JsonConvert.SerializeObject(response);

        ApiResponse<List<CategorySnapshotDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<CategorySnapshotDTO>>>(json);

        //az összes termék lekérése
        static ApiResponse<List<ProductDTO>> productResponse = proxy.ProductsFindAll();
        static string productJson = JsonConvert.SerializeObject(productResponse);

        ApiResponse<List<ProductDTO>> productDeserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<ProductDTO>>>(productJson);

        #endregion

        #region Constructor and destructor

        public Form1()
        {
            InitializeComponent();
            GetProducts();
            GetCategoryButtons();
            InitializeWindowAndDataGrid();
        }
        #endregion

        #region Methods and events

        private void InitializeWindowAndDataGrid()
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            ordersDataGridView.Columns[0].HeaderText = "SKU Azonosító";
            ordersDataGridView.Columns[2].HeaderText = "Termék neve";
            ordersDataGridView.Columns[3].HeaderText = "Mennyiség raktáron";
            ordersDataGridView.Columns[4].HeaderText = "Kategória";
            ordersDataGridView.Columns[1].Visible = false;
            foreach (DataGridViewColumn c in ordersDataGridView.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Times New Roman", 24F, GraphicsUnit.Pixel);
            }
        }

        private void UpdateDatagrid(List<Product> lista)
        {
            var result = Helpers.OrderListByQuantity(lista);
            ordersDataGridView.DataSource = result;
            ChangeStyleOfLackingProducts();
        }

        private void GetCategoryButtons()
        {
            foreach (CategorySnapshotDTO item in deserializedResponse.Content)
            {
                if (item.ParentId == "aa7af6a8-917e-4e69-8471-33205ded3897")
                {
                    categories.Add(item.Name);
                }
            }
            for (int i = 0; i <= categories.Count; i++)
            {
                string category = "";
                Button button = new Button();
                if (i != categories.Count)
                { 
                    category = categories[i];
                }
                button = Helpers.AddCategoryButton(i, category, Button_Click, i == categories.Count);
                panel1.Controls.Add(button);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Minden kategória")
            {
                UpdateDatagrid(products);
            }
            else
            {
                var result = Helpers.Filter(products, ((Button)sender).Text);
                UpdateDatagrid(result);
            }
        }

        public void GetProducts()
        {
            foreach (ProductDTO item in productDeserializedResponse.Content)
            {
                Product product = new Product() { SKU = item.Sku, Bvin = item.Bvin, Product_name = item.ProductName };
                bvin.Add(item.Bvin);
                SKU.Add(item.Sku);
                product_name.Add(item.ProductName);
                //prices.Add(item.SitePrice);
                products.Add(product);
            }

            //termékek darabszámának lekérése
            for (int i = 0; i < bvin.Count; i++)
            {
                string aktBvin = bvin[i];
                ApiResponse<List<ProductInventoryDTO>> response_v2 = proxy.ProductInventoryFindForProduct(aktBvin);
                string json_v2 = JsonConvert.SerializeObject(response_v2);

                ApiResponse<List<ProductInventoryDTO>> deserializedResponse_v2 = JsonConvert.DeserializeObject<ApiResponse<List<ProductInventoryDTO>>>(json_v2);

                foreach (ProductInventoryDTO item in deserializedResponse_v2.Content)
                {
                    foreach (var product in products)
                    {
                        if (product.Bvin == item.ProductBvin)
                        {
                            product.Quantity = item.QuantityOnHand;
                        }
                    }
                    quantity.Add(item.QuantityOnHand);
                }
            }

            //termékek kategóriájának lekérése
            GetCategories();

            for (int i = 0; i < category.Count; i++)
            {
                products[i].Category = category[i];
            }

            products.RemoveAt(products.Count-1);
            products.RemoveAt(products.Count-1);

            UpdateDatagrid(products);
        }


        public void GetCategories()
        {
            for (int i = 0; i < bvin.Count; i++)
            {
                string aktBvin = bvin[i];
                ApiResponse<List<CategorySnapshotDTO>> response_v3 = proxy.CategoriesFindForProduct(aktBvin);
                string json_v3 = JsonConvert.SerializeObject(response_v3);

                ApiResponse<List<CategorySnapshotDTO>> deserializedResponse_v3 = JsonConvert.DeserializeObject<ApiResponse<List<CategorySnapshotDTO>>>(json_v3);

                foreach (CategorySnapshotDTO item in deserializedResponse_v3.Content)
                {

                    if (!item.Name.Any(char.IsDigit) && !item.Name.Contains("Akciós termékek") && !item.Name.Contains("Újdonságok"))
                    {
                        category.Add(item.Name);
                    }
                }
            }
        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            int count = (int)ordersDataGridView.Rows[rowIndex].Cells[3].Value;
            string sku = (string)ordersDataGridView.Rows[rowIndex].Cells[0].Value;
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out int result))
            {
                errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres és számot kell tartalmaznia!)");
            }
            else
            {
                foreach (var item in products)
                {
                    if (item.SKU == sku)
                    {
                        item.Quantity = count + int.Parse(textBox1.Text);
                        if (Saving_Quantity(sku, count + int.Parse(textBox1.Text)))
                        {
                            MessageBox.Show("SIKERES MENTÉS!", "Rendszerüzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("SIKERTELEN MENTÉS!", "Rendszerüzenet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                textBox1.Text = null;
            }
        }

        private void ChangeStyleOfLackingProducts()
        {
            foreach (DataGridViewRow row in ordersDataGridView.Rows)
                if (Convert.ToInt32(row.Cells[3].Value) == 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            ordersDataGridView.Update();
        }

       private void buttonMinus_Click(object sender, EventArgs e)
        {
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            int count = (int)ordersDataGridView.Rows[rowIndex].Cells[3].Value;
            string sku = (string)ordersDataGridView.Rows[rowIndex].Cells[0].Value;
            Product selected = new Product();
            foreach (var item in products)
            {
                if (item.SKU == sku)
                {
                    selected = item;
                }
            }
            errorProvider1.Clear();
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out int result))
            {
                errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres,számot kell tartalmaznia és a végeredménye a terméknek nem lehet negatív!)");
            }
            else if ((selected.Quantity - int.Parse(textBox1.Text)) < 0)
            {
                errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres,számot kell tartalmaznia és a végeredménye a terméknek nem lehet negatív!)");
            }
            else
            {
                foreach (var item in products)
                {
                    if (item.SKU == sku)
                    {
                        item.Quantity = count - int.Parse(textBox1.Text);
                        if (Saving_Quantity(sku, count - int.Parse(textBox1.Text)))
                        {
                            MessageBox.Show("SIKERES MENTÉS!", "Rendszerüzenet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("SIKERTELEN MENTÉS!", "Rendszerüzenet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                textBox1.Text = null;
            }
        }
       
        private bool Saving_Quantity(string Id, int quantity)
        {
            var result = productDeserializedResponse.Content.Find(x => x.Sku == Id);
            ApiResponse<List<ProductInventoryDTO>> response_v2 = proxy.ProductInventoryFindForProduct(result.Bvin);
            string json_v2 = JsonConvert.SerializeObject(response_v2);

            ApiResponse<List<ProductInventoryDTO>> deserializedResponse_v2 = JsonConvert.DeserializeObject<ApiResponse<List<ProductInventoryDTO>>>(json_v2);
            deserializedResponse_v2.Content.FirstOrDefault().QuantityOnHand = quantity;

            proxy.ProductInventoryUpdate(deserializedResponse_v2.Content.FirstOrDefault());
            try
            {
                UpdateDatagrid(products);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        
        private void ordersDataGridView_BindingContextChanged(object sender, EventArgs e)
        {
            ChangeStyleOfLackingProducts();
        }

        #endregion

    }
}
