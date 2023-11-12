using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace Thesis
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }

        private void customersButton_Click(object sender, EventArgs e)
        {
            //create a new form and open it while hiding the current one
            CustomersForm form2 = new CustomersForm();
            form2.Show();
            this.Hide();
        }

        private void coresButton_Click(object sender, EventArgs e)
        {
            //create a new CoreTypesForm and open it while hiding the current one
            CoreTypesForm coreTypesForm = new CoreTypesForm();
            coreTypesForm.Show();
            this.Hide();
        }

        private void veneersButton_Click(object sender, EventArgs e)
        {
            //create a new VeneerTypesForm and open it while hiding the current one
            VeneerTypesForm veneerTypesForm = new VeneerTypesForm();
            veneerTypesForm.Show();
            this.Hide();
        }

        private void ordersButton_Click(object sender, EventArgs e)
        {
            OrdersForm ordersForm = new OrdersForm();
            ordersForm.Show();
            this.Hide();
        }
    }
}
