using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Thesis
{
    public partial class OrdersForm : Form
    {
        public OrdersForm()
        {
            InitializeComponent();

            // add a back button on the top of the form in the bar
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);

            //create a button to add new orders
            Button addOrder = new Button();
            addOrder.Text = "Add Order";
            addOrder.Location = new Point(100, 0);
            addOrder.Click += new EventHandler(addOrder_Click);
            this.Controls.Add(addOrder);

            // create a button to delete selected orders
            Button deleteOrder = new Button();
            deleteOrder.Text = "Delete Order";
            deleteOrder.Location = new Point(200, 0);
            deleteOrder.Click += new EventHandler(deleteOrder_Click);
            this.Controls.Add(deleteOrder);

            //create a button to open order details
            Button orderDetails = new Button();
            orderDetails.Text = "Open order details";
            orderDetails.Location = new Point(300, 0);
            orderDetails.Click += new EventHandler(orderDetails_Click);
            this.Controls.Add(orderDetails);

            // show controls to add new rows or delete existing ones from datagridview
            dataGridView1.RowHeadersVisible = true;


            //bind data source to the orders table
            dataGridView1.DataSource = new Model1().orders.ToList();


            //hide the CustomerID column
            dataGridView1.Columns["CustomerID"].Visible = false;
            dataGridView1.Columns["customer"].Visible = false;

            //make OrderID column read only
            dataGridView1.Columns["OrderID"].ReadOnly = true;

            DataGridViewComboBoxColumn buyerColumn = new DataGridViewComboBoxColumn();
            buyerColumn.HeaderText = "Buyer";
            buyerColumn.Name = "Buyer";
            buyerColumn.DataPropertyName = "CustomerID";
            buyerColumn.DataSource = new Model1().customers.ToList();
            buyerColumn.ValueMember = "CustomerID";
            buyerColumn.DisplayMember = "companyName";
            dataGridView1.Columns.Add(buyerColumn);
            dataGridView1.Columns["Buyer"].DisplayIndex = 0;

            //make edited data autosaving
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
        }

        private void orderDetails_Click(object sender, EventArgs e)
        {
            //open order details form and pass the selected order id
            try {  
                var test = dataGridView1.SelectedRows[0];
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a row to open");
                return;
            }

            var row = dataGridView1.SelectedRows[0];

            Order order = row.DataBoundItem as Order;


            OrderDetailsForm orderDetailsForm = new OrderDetailsForm(order.OrderID);
            orderDetailsForm.Show();
            this.Hide();

        }

        // make edited data autosaving
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // get the edited row from eventargs
            var row = dataGridView1.Rows[e.RowIndex];

            // get the edited customer
            Order order = row.DataBoundItem as Order;
            // update the database
            using (var db = new Model1())
            {
                //find order with same order id in database
                Order dbOrder = db.orders.Find(order.OrderID);
                dbOrder.CustomerID = order.CustomerID;
                dbOrder.payment_method = order.payment_method;
                dbOrder.orderDate = order.orderDate;
                dbOrder.mentions = order.mentions;

                db.Entry(dbOrder).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        // add new order button click event
        private void addOrder_Click(object sender, EventArgs e)
        {
            // create a new order
            Order order = new Order();
            order.orderDate = DateTime.Now;
            try
            {
                //make the order's customer the first customer in the database
                order.CustomerID = new Model1().customers.ToList()[0].CustomerID;
            }
            catch (Exception)
            {
                MessageBox.Show("Please add a customer first");
                return;
            }

            // add it to the database
            using (var db = new Model1())
            {
                db.orders.Add(order);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().orders.ToList();

        }

        // delete selected orders button click event
        private void deleteOrder_Click(object sender, EventArgs e)
        {

            //try index
            try
            {
                var test = dataGridView1.SelectedRows[0];
            }
            catch (Exception)
            {

                MessageBox.Show("Please select a row to delete");
                return;
            }

            var row = dataGridView1.SelectedRows[0];

            // get the selected customer
            Order order = row.DataBoundItem as Order;
            // delete the selected customer from the database
            using (var db = new Model1())
            {
                //get order with same order id from database
                order = db.orders.Find(order.OrderID);

                //loop through all cores associated with this order and delete their veneers then the cores themselves
                foreach (Core core in db.cores.Where(x => x.OrderID == order.OrderID).ToList())
                {
                    //delete associated veneers

                    Veneer faceVeneer = db.veneers.Find(core.FaceVeneerID);
                    if (faceVeneer != null)
                        db.veneers.Remove(faceVeneer);

                    Veneer backVeneer = db.veneers.Find(core.BackVeneerID);
                    if (backVeneer != null)
                        db.veneers.Remove(backVeneer);

                    //remove order from database
                    db.cores.Remove(core);
                }

                //remove order from database
                db.orders.Remove(order);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().orders.ToList();
        }

        // back button click event
        private void back_Click(object sender, EventArgs e)
        {
            // hide this form
            this.Hide();
            // show the main form
            MainForm main = new MainForm();
            main.Show();
        }
    }
}
