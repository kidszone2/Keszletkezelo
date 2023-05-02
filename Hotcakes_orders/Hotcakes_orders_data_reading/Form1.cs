using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Proxies;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Catalog;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Contacts;
using Hotcakes.CommerceDTO.v1.Orders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Hotcakes_orders_data_reading
{
    public partial class Form1 : Form
    {
       
        DataTable dt = new DataTable();
        List<string> category = new List<string>();
        List<decimal> prices = new List<decimal>();
        List<string> bvin = new List<string>();
        List<int> quantity = new List<int>();
        List<string> SKU = new List<string>();
        List<string> product_name = new List<string>();
        List<string> categories = new List<string>();
        static string url = "http://20.234.113.211:8090/";
        static string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

        static Api proxy = new Api(url, key);

        //az összes kategória lekérése
        static ApiResponse<List<CategorySnapshotDTO>> response = proxy.CategoriesFindAll();
        static string json = JsonConvert.SerializeObject(response);

        ApiResponse<List<CategorySnapshotDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<CategorySnapshotDTO>>>(json);

        //az összes termék lekérése
        static ApiResponse<List<ProductDTO>> productResponse = proxy.ProductsFindAll();
        static string productJson = JsonConvert.SerializeObject(productResponse);

        ApiResponse<List<ProductDTO>> productDeserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<ProductDTO>>>(productJson);

        public Form1()
        {
            InitializeComponent();
            GetProducts();
            GetCategories();

            this.WindowState = FormWindowState.Maximized;
            panel1.Width = ordersDataGridView.Width;
            
        }

        private void GetCategories()
        {
            foreach (CategorySnapshotDTO item in deserializedResponse.Content)
            {
                if (item.ParentId == "aa7af6a8-917e-4e69-8471-33205ded3897")
                {
                    categories.Add(item.Name);
                }
            }

            for (int i = 0; i < categories.Count; i++)
            {
                Button button = new Button();
                button.Text = categories[i];
                button.Width = 100;
                button.Left =  i* 120;
                button.Height = 50;
                button.BackColor = Color.LimeGreen;
                button.Click += Button_Click;
                panel1.Controls.Add(button);
                
            } 
        }

        private void Button_Click(object sender, EventArgs e)
        {

            DataTable tblFiltered = dt.AsEnumerable()
                             .Where(r => r.Field<string>("Kategória") == ((Button)sender).Text)
                             .CopyToDataTable();
            ordersDataGridView.DataSource = tblFiltered;
            ChangeStyleOfLackingProducts();
        }

        public void GetProducts()
        {
            foreach (ProductDTO item in productDeserializedResponse.Content)
            {
                bvin.Add(item.Bvin);
                SKU.Add(item.Sku);
                product_name.Add(item.ProductName);
                //prices.Add(item.SitePrice);
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
                    quantity.Add(item.QuantityOnHand);
                }
            }

            //termékek kategóriájának lekérése
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

            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("Terméknév", typeof(string));
            dt.Columns.Add("Mennyiség", typeof(int));
            dt.Columns.Add("Kategória", typeof(string));
            //dt.Columns.Add("Ár", typeof(int));

            for (int i = 0; i < bvin.Count -3 ; i++)
            {
               dt.Rows.Add(SKU[i], product_name[i], quantity[i], category[i]);
            }

            ordersDataGridView.DataSource = dt;
        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out int result))
            {
                errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres és számot kell tartalmaznia)");
            }
            else
            {
                dt.Rows[rowIndex].SetField("Mennyiség", dt.Rows[rowIndex].Field<int>("Mennyiség") + int.Parse(textBox1.Text));
                Saving_Quantity(dt.Rows[rowIndex].Field<string>("SKU"), dt.Rows[rowIndex].Field<int>("Mennyiség"));
                textBox1.Text = null;
            }
        }

        private void ChangeStyleOfLackingProducts()
        {
            ordersDataGridView.Update();
            foreach (DataGridViewRow row in ordersDataGridView.Rows)
                if (Convert.ToInt32(row.Cells[2].Value) == 0)
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
            errorProvider1.Clear();
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            if (dt.Rows[rowIndex].Field<int>("Mennyiség") > 0)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out int result))
                {
                    errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres és számot kell tartalmaznia)");
                }
                else
                {
                    dt.Rows[rowIndex].SetField("Mennyiség", dt.Rows[rowIndex].Field<int>("Mennyiség") - int.Parse(textBox1.Text));
                    Saving_Quantity(dt.Rows[rowIndex].Field<string>("SKU"), dt.Rows[rowIndex].Field<int>("Mennyiség"));
                }
                textBox1.Text = null;
            }

            
        }
        private void Saving_Quantity(string Id, int quantity)
        {
            var result = productDeserializedResponse.Content.Find(x => x.Sku == Id);
            ApiResponse<List<ProductInventoryDTO>> response_v2 = proxy.ProductInventoryFindForProduct(result.Bvin);
            string json_v2 = JsonConvert.SerializeObject(response_v2);

            ApiResponse<List<ProductInventoryDTO>> deserializedResponse_v2 = JsonConvert.DeserializeObject<ApiResponse<List<ProductInventoryDTO>>>(json_v2);
            deserializedResponse_v2.Content.FirstOrDefault().QuantityOnHand = quantity;

            proxy.ProductInventoryUpdate(deserializedResponse_v2.Content.FirstOrDefault());
            ChangeStyleOfLackingProducts();
        }

        

        private void Saving_Prices(string Id, int new_price)
        {
            var result = productDeserializedResponse.Content.Find(x => x.Sku == Id);
            result.SitePrice = new_price;
            proxy.ProductsUpdate(result);
        }
        /*
        private void armodositasButton_Click(object sender, EventArgs e)
        {
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            Saving_Prices(dt.Rows[rowIndex].Field<string>("SKU"), int.Parse(artextbox.Text));
            dt.Rows[rowIndex].SetField("Ár", int.Parse(artextbox.Text));
            
        }
        */
        private void ordersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
        }
        
        private void ordersDataGridView_BindingContextChanged(object sender, EventArgs e)
        {
            ChangeStyleOfLackingProducts();
        }
    }
}
