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
        List<string> bvin = new List<string>();
        List<int> quantity = new List<int>();
        List<string> SKU = new List<string>();
        List<string> product_name = new List<string>();


        public Form1()
        {
            InitializeComponent();
            GetProducts();
            GetCategories();

        }

        private void GetCategories()
        {
            List<string> categories = new List<string>();
            string url = "http://20.234.113.211:8090/";
            string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

            Api proxy = new Api(url, key);

            //az össze kategória lekérése
            ApiResponse<List<CategorySnapshotDTO>> response = proxy.CategoriesFindAll();
            string json = JsonConvert.SerializeObject(response);

            ApiResponse<List<CategorySnapshotDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<CategorySnapshotDTO>>>(json);

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
                button.Left = i * 100;
                button.Height = 50;
                button.Click += Button_Click;
                Controls.Add(button);
            } 
        }

        private void Button_Click(object sender, EventArgs e)
        {

            DataTable tblFiltered = dt.AsEnumerable()
                             .Where(r => r.Field<string>("Kategória") == ((Button)sender).Text)
                             .CopyToDataTable();
            ordersDataGridView.DataSource = tblFiltered;
        }

        public void GetProducts()
        {
            string url = "http://20.234.113.211:8090/";
            string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

            Api proxy = new Api(url, key);

            //az összes termék lekérése
            ApiResponse<List<ProductDTO>> response = proxy.ProductsFindAll();
            string json = JsonConvert.SerializeObject(response);

            ApiResponse<List<ProductDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<ProductDTO>>>(json);

            foreach (ProductDTO item in deserializedResponse.Content)
            {
                bvin.Add(item.Bvin);
                SKU.Add(item.Sku);
                product_name.Add(item.ProductName);
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

            for (int i = 0; i < bvin.Count -3 ; i++)
            {
                dt.Rows.Add(SKU[i], product_name[i], quantity[i], category[i]);
            }

            ordersDataGridView.DataSource = dt;
        }

        private void buttonPlus_Click(object sender, EventArgs e)
        {
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out int result))
            {
                errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres és számot kell tartalmaznia)");
            }
            else
            {
                dt.Rows[rowIndex].SetField("Mennyiség", dt.Rows[rowIndex].Field<int>("Mennyiség") + int.Parse(textBox1.Text));
            }

            Saving();
        }

        private void buttonMinus_Click(object sender, EventArgs e)
        {
            int rowIndex = ordersDataGridView.CurrentCell.RowIndex;
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text, out int result))
            {
                errorProvider1.SetError(textBox1, "NEM MEGFELELŐ ÉRTÉK! (A mező nem lehet üres és számot kell tartalmaznia)");
            }
            else
            {
                dt.Rows[rowIndex].SetField("Mennyiség", dt.Rows[rowIndex].Field<int>("Mennyiség") - int.Parse(textBox1.Text));
            }
        }
        private void Saving()
        {
            //List<ProductInventoryDTO> inventory = new List<ProductInventoryDTO>();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{ 
            //    Product product = new Product();
            //    product.SKU = dt.Rows[i]["SKU"].ToString();
            //    product.Product_name = dt.Rows[i]["Terméknév"].ToString();
            //    product.Quantity = int.Parse(dt.Rows[i]["Mennyiség"].ToString());
            //    inventory.Add(product);
            //}

            //string url = "http://20.234.113.211:8090/";
            //string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

            //Api proxy = new Api(url, key);
            //// call the API to create the new product inventory record
            //ApiResponse<ProductInventoryDTO> response = proxy.ProductInventoryUpdate(inventory);

        }

        
    }
}


//foreach (var item in dt.AsEnumerable())
//{

//    if (item.Field<string>("SKU") == SKU[rowIndex])
//    {
//        MessageBox.Show(item.Field<string>("Terméknév").ToString());
//        item.Field<int>("Mennyiség") = 5 ;
//    }

//}

//namespace Hotcakes_orders_data_reading
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//            GetOrders();           
//        }

//        public void ReadData()
//        {
//            string url = "http://20.234.113.211:8090/";
//            string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

//            Api proxy = new Api(url, key);

//            Random rnd = new Random();

//            using (StreamReader sr = new StreamReader("orders/orders.csv", Encoding.UTF8))
//            {
//                var firstLine = sr.ReadLine();

//                while (!sr.EndOfStream) {

//                    var line = sr.ReadLine();
//                    var item = line.Split(';');

//                    // create a new order object
//                    var order = new OrderDTO();

//                    // add billing information
//                    order.BillingAddress = new AddressDTO
//                    {
//                        AddressType = AddressTypesDTO.Billing,
//                        City = item[0],
//                        CountryBvin = item[1],
//                        FirstName = item[2],
//                        LastName = item[3],
//                        Line1 = "",
//                        Line2 = "",
//                        Phone = item[4],
//                        PostalCode = item[5],
//                        RegionBvin = item[6]
//                    };

//                    // add at least one line item
//                    order.Items = new List<LineItemDTO>();
//                    order.Items.Add(new LineItemDTO
//                    {
//                        ProductId = item[7],
//                        Quantity = int.Parse(item[8])
//                    });

//                    // add the shipping address
//                    order.ShippingAddress = new AddressDTO();
//                    order.ShippingAddress = order.BillingAddress;
//                    order.ShippingAddress.AddressType = AddressTypesDTO.Shipping;

//                    // specify who is creating the order
//                    order.UserEmail = "info@hotcakescommerce.com";
//                    order.UserID = "1";

//                    // call the API to create the order
//                    ApiResponse<OrderDTO> response = proxy.OrdersCreate(order);

//                    GetOrders();
//                    Thread.Sleep(rnd.Next(3000, 7000));
//                }

//                sr.Close();
//            }       
//        }

//        public void GetOrders()
//        {
//            string url = "http://20.234.113.211:8090/";
//            string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

//            Api proxy = new Api(url, key);

//            // call the API to find all orders in the store
//            ApiResponse<List<OrderSnapshotDTO>> response = proxy.OrdersFindAll();
//            string json = JsonConvert.SerializeObject(response);

//            ApiResponse<List<OrderSnapshotDTO>> deserializedResponse = JsonConvert.DeserializeObject<ApiResponse<List<OrderSnapshotDTO>>>(json);

//            DataTable dt = new DataTable();
//            dt.Columns.Add("Id", typeof(int));
//            dt.Columns.Add("bvin", typeof(string));
//            dt.Columns.Add("FirstName", typeof(string));
//            dt.Columns.Add("LastName", typeof(string));
//            dt.Columns.Add("StoreId", typeof(long));

//            foreach (OrderSnapshotDTO item in deserializedResponse.Content)
//            {
//                dt.Rows.Add(item.Id, item.bvin, item.BillingAddress.FirstName, item.BillingAddress.LastName, item.StoreId);
//            }

//            ordersDataGridView.DataSource = dt;
//        }

//        private void startButton_Click(object sender, EventArgs e)
//        {
//            ReadData();
//        }
//    }
//}
