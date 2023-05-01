using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotcakes_orders_data_reading
{
    class button : Button
    {
        public button()
        {
            

            Click += Button_Click;
        }

        public void Button_Click(object sender, EventArgs e)
        {
            string cat = this.Text;
        }
    }
}
