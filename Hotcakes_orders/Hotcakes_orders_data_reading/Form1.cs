using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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


namespace Hotcakes_orders_data_reading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetProducts();
            GetCategories();
        }

        private void GetCategories()
        {
            var categories = new List<string>();

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
                button.Left = i * 100;
                button.Width = 100;
                Controls.Add(button);
            }
        }

        public void GetProducts()
        {
            var bvin = new List<string>();
            var SKU = new List<string>();
            var product_name = new List<string>();
            var quantity = new List<int>();

            string url = "http://20.234.113.211:8090/";
            string key = "1-903011f5-696d-4ed0-9cf8-3a6fa51607f2";

            Api proxy = new Api(url, key);

            //az össze termék lekérése
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


            DataTable dt = new DataTable();
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("Terméknév", typeof(string));
            dt.Columns.Add("Mennyiség", typeof(string));

            for (int i = 0; i < bvin.Count; i++)
            {
                dt.Rows.Add(SKU[i], product_name[i], quantity[i]);
            }
            

            ordersDataGridView.DataSource = dt;
       
        }

        private void startButton_Click(object sender, EventArgs e)
        {
           
        }
    }
}


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
