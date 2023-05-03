using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotcakes_orders_data_reading
{
    public static class Helpers
    {
        public static Button AddCategoryButton(int i, string category, EventHandler Button_Click, bool lastButton)
        {
            Button button = new Button();
            if (lastButton)
            {
                button.Text = "Minden kategória";
            }
            else
            {
                button.Text = category;
            }
            button.Width = 105;
            button.Left = i * 105;
            button.Height = 80;
            button.BackColor = Color.DarkCyan;
            button.Font = new Font("Times New Roman", 11, FontStyle.Bold);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.White;
            button.FlatAppearance.BorderSize = 2;
            button.Padding = new Padding(0, 0, 0, 0);
            button.Click += Button_Click;
            return button;
        }

        public static List<Product> OrderListByQuantity(List<Product> unorderedList)
        {
            return unorderedList.OrderBy(x => x.Quantity).ToList();
        }

        public static List<Product> Filter(List<Product> unorderedList, string filterCategory)
        {
            return unorderedList.Where(x => x.Category == filterCategory).ToList();
        }
    }
}
