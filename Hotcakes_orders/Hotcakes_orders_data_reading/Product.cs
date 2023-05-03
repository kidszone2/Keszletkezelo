using Hotcakes.CommerceDTO.v1.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotcakes_orders_data_reading
{
    public class Product
    {
        public string SKU { get; set; } 
        public string Bvin { get; set; } 
        public string Product_name { get; set; }
        public int Quantity { get; set; }  
        public string Category { get; set; }  
        
    }
}
