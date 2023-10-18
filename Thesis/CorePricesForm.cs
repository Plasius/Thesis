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
    public partial class CorePricesForm : Form
    {
        public CorePricesForm()
        {
            InitializeComponent();

            //populate datagridview with data from CorePrices table
            dataGridView1.DataSource = new Model1().corePrices.ToList();

            //hide the CorePriceID column and CoreTypeID column and CoreType column
            dataGridView1.Columns["CorePriceID"].Visible = false;
            dataGridView1.Columns["CoreTypeID"].Visible = false;
            dataGridView1.Columns["CoreType"].Visible = false;

            //create column for CoreType and make it first column
            DataGridViewComboBoxColumn coreTypeColumn = new DataGridViewComboBoxColumn();
            coreTypeColumn.HeaderText = "CoreType";
            coreTypeColumn.Name = "CoreType";
            coreTypeColumn.DataPropertyName = "CoreTypeID";
            coreTypeColumn.DataSource = new Model1().coreTypes.ToList();
            coreTypeColumn.ValueMember = "CoreTypeID";
            coreTypeColumn.DisplayMember = "coreTypeName";
            dataGridView1.Columns.Add(coreTypeColumn);
            dataGridView1.Columns["CoreType"].DisplayIndex = 0;




            //ensure saving edited data
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            //create back button to CoreTypesForm
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);


            //create add and delete buttons
            Button addCorePrice = new Button();
            addCorePrice.Text = "Add CorePrice";
            addCorePrice.Location = new Point(100, 0);
            addCorePrice.Click += new EventHandler(addCorePrice_Click);
            this.Controls.Add(addCorePrice);

            Button deleteCorePrice = new Button();
            deleteCorePrice.Text = "Delete CorePrice";
            deleteCorePrice.Location = new Point(200, 0);
            deleteCorePrice.Click += new EventHandler(deleteCorePrice_Click);
            this.Controls.Add(deleteCorePrice);

        }

        //back button to CoreTypesForm
        private void back_Click(object sender, EventArgs e)
        {
            CoreTypesForm coreTypesForm = new CoreTypesForm();
            coreTypesForm.Show();
            this.Hide();
        }

        //save edited data with help of eventargs row location
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //get the changed row with DataGridViewCellEventArgs
            var selectedRow = dataGridView1.Rows[e.RowIndex];

            //get the selected CorePrice
            CorePrice corePrice = (CorePrice)selectedRow.DataBoundItem;
            //update the selected CorePrice by finding the existing one in the database and replacing it
            //with the edited one
            using (var db = new Model1())
            {
                CorePrice existingCorePrice = (from c in db.corePrices
                where c.CorePriceID == corePrice.CorePriceID select c).Single();
                existingCorePrice.CoreTypeID = corePrice.CoreTypeID;
                existingCorePrice.price = corePrice.price;
                db.SaveChanges();
            }

            //refresh datagridview
            dataGridView1.DataSource = new Model1().corePrices.ToList();
        }

        //add new CorePrice row to datagridview
        private void addCorePrice_Click(object sender, EventArgs e)
        {
            //create new CorePrice
            CorePrice corePrice = new CorePrice();
            corePrice.price = 0;
            corePrice.date = DateTime.Now;
            //set CoreTypeID to the first CoreType in the database
            using (var db = new Model1())
            {
                corePrice.CoreTypeID = db.coreTypes.First().CoreTypeID;
            }


            //add new CorePrice to database
            using (var db = new Model1())
            {
                db.corePrices.Add(corePrice);
                db.SaveChanges();
            }
            //refresh datagridview
            dataGridView1.DataSource = new Model1().corePrices.ToList();
        }

        //delete selected CorePrice row from datagridview
        private void deleteCorePrice_Click(object sender, EventArgs e)
        {
            // get the selected row
            try
            {
                var test = dataGridView1.SelectedRows[0];
            }
            catch (Exception)
            {

                MessageBox.Show("Please select a row to delete");
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            //get the selected CorePrice
            CorePrice corePrice = (CorePrice)selectedRow.DataBoundItem;
            //delete the selected CorePrice from the database by finding its ID
            using (var db = new Model1())
            {
                CorePrice existingCorePrice = (from c in db.corePrices
                                                              where c.CorePriceID == corePrice.CorePriceID select c).Single();
                db.corePrices.Remove(existingCorePrice);
                db.SaveChanges();
            }
            //refresh datagridview
            dataGridView1.DataSource = new Model1().corePrices.ToList();
        }
    }
}
