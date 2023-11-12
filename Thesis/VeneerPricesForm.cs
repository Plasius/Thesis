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
    public partial class VeneerPricesForm : Form
    {
        public VeneerPricesForm()
        {
            InitializeComponent();

            //populate datagridview with data from VeneerPrices table
            dataGridView1.DataSource = new Model1().veneerPrices.ToList();

            //hide the CorePriceID column and VeneerTypeID column and CoreType column
            dataGridView1.Columns["VeneerPriceID"].Visible = false;
            dataGridView1.Columns["VeneerTypeID"].Visible = false;
            dataGridView1.Columns["VeneerType"].Visible = false;

            //create column for CoreType and make it first column
            DataGridViewComboBoxColumn veneerTypeColumns = new DataGridViewComboBoxColumn();
            veneerTypeColumns.HeaderText = "VeneerType";
            veneerTypeColumns.Name = "VeneerType";
            veneerTypeColumns.DataPropertyName = "VeneerTypeID";
            veneerTypeColumns.DataSource = new Model1().veneerTypes.ToList();
            veneerTypeColumns.ValueMember = "VeneerTypeID";
            veneerTypeColumns.DisplayMember = "veneerTypeName";
            dataGridView1.Columns.Add(veneerTypeColumns);
            dataGridView1.Columns["VeneerType"].DisplayIndex = 0;


            //ensure saving edited data
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            //create back button to CoreTypesForm
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);


            //create add and delete buttons
            Button addVeneerPrice = new Button();
            addVeneerPrice.Text = "Add VeneerPrice";
            addVeneerPrice.Location = new Point(100, 0);
            addVeneerPrice.Click += new EventHandler(addVeneerPrice_Click);
            this.Controls.Add(addVeneerPrice);

            Button deleteVeneerPrice = new Button();
            deleteVeneerPrice.Text = "Delete CorePrice";
            deleteVeneerPrice.Location = new Point(200, 0);
            deleteVeneerPrice.Click += new EventHandler(deleteVeneerPrice_Click);
            this.Controls.Add(deleteVeneerPrice);
        }

        void back_Click(object sender, EventArgs e)
        {
            VeneerTypesForm veneerTypesForm = new VeneerTypesForm();
            veneerTypesForm.Show();
            this.Close();
        }

        void addVeneerPrice_Click(object sender, EventArgs e)
        {
            //create new CorePrice
            VeneerPrice veneerPrice = new VeneerPrice();
            veneerPrice.price = 0;
            veneerPrice.date = DateTime.Now;
            //set CoreTypeID to the first CoreType in the database
            using (var db = new Model1())
            {
                veneerPrice.VeneerTypeID = db.veneerTypes.First().VeneerTypeID;
            }


            //add new CorePrice to database
            using (var db = new Model1())
            {
                db.veneerPrices.Add(veneerPrice);
                db.SaveChanges();
            }
            //refresh datagridview
            dataGridView1.DataSource = new Model1().veneerPrices.ToList();
        }

        void deleteVeneerPrice_Click(object sender, EventArgs e)
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
            VeneerPrice veneerPrice = (VeneerPrice)selectedRow.DataBoundItem;
            //delete the selected CorePrice from the database by finding its ID
            using (var db = new Model1())
            {
                VeneerPrice existingVeneerPrice = (from c in db.veneerPrices
                                                 where c.VeneerPriceID == veneerPrice.VeneerPriceID
                                               select c).Single();
                db.veneerPrices.Remove(existingVeneerPrice);
                db.SaveChanges();
            }
            //refresh datagridview
            dataGridView1.DataSource = new Model1().veneerPrices.ToList();
        }

        //save edited data with help of eventargs row location
        void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //get the changed row with DataGridViewCellEventArgs
            var selectedRow = dataGridView1.Rows[e.RowIndex];

            //get the selected CorePrice
            VeneerPrice veneerPrice = (VeneerPrice)selectedRow.DataBoundItem;
            //update the selected CorePrice by finding the existing one in the database and replacing it
            //with the edited one
            using (var db = new Model1())
            {
                VeneerPrice existingVeneerPrice = (from c in db.veneerPrices
                                               where c.VeneerPriceID == veneerPrice.VeneerPriceID
                                               select c).Single();
                existingVeneerPrice.VeneerTypeID = veneerPrice.VeneerTypeID;
                existingVeneerPrice.price = veneerPrice.price;
                db.SaveChanges();
            }

            //refresh datagridview
            dataGridView1.DataSource = new Model1().veneerPrices.ToList();
        }

    }
}
