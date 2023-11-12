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
    public partial class CustomersForm : Form
    {
        public CustomersForm()
        {
            InitializeComponent();

            // add a back button on the top of the form in the bar
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);

            //create a button to add new customers
            Button addCustomer = new Button();
            addCustomer.Text = "Add Customer";
            addCustomer.Location = new Point(100, 0);
            addCustomer.Click += new EventHandler(addCustomer_Click);
            this.Controls.Add(addCustomer);

            // create a button to delete selected customers
            Button deleteCustomer = new Button();
            deleteCustomer.Text = "Delete Customer";
            deleteCustomer.Location = new Point(200, 0);
            deleteCustomer.Click += new EventHandler(deleteCustomer_Click);
            this.Controls.Add(deleteCustomer);


            // show controls to add new rows or delete existing ones from datagridview
            dataGridView1.RowHeadersVisible = true;

            //bind data source to the customers table
            dataGridView1.DataSource = new Model1().customers.ToList();

            //hide the CustomerID column
            dataGridView1.Columns["CustomerID"].Visible = false;

            //make edited data autosaving
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

        }

        // delete selected customers button click event
        private void deleteCustomer_Click(object sender, EventArgs e)
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
            Customer customer = row.DataBoundItem as Customer;
            // delete the selected customer from the database
            using (var db = new Model1())
            {
                db.customers.Attach(customer);
                db.customers.Remove(customer);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().customers.ToList();
        }

        // add new customer button click event
        private void addCustomer_Click(object sender, EventArgs e)
        {
            // create a new customer
            Customer customer = new Customer();
            // add the new customer to the database
            using (var db = new Model1())
            {
                db.customers.Add(customer);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().customers.ToList();
        }

        // save edited data
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // get the edited row from eventargs
            var row = dataGridView1.Rows[e.RowIndex];
            // get the edited customer
            Customer customer = row.DataBoundItem as Customer;
            // update the database
            using (var db = new Model1())
            {
                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        // back button click event
        private void back_Click(object sender, EventArgs e)
        {
            // hide this form
            this.Close();
            // show the main form
            MainForm main = new MainForm();
            main.Show();
        }
    }
}
