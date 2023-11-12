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
    public partial class VeneerTypesForm : Form
    {
        public VeneerTypesForm()
        {
            InitializeComponent();

            // add a back button on the top of the form in the bar
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);

            //bind data source to the veneerTypes table
            dataGridView1.DataSource = new Model1().veneerTypes.ToList();

            //save edited data
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            //hide the VeneerTypeID column
            dataGridView1.Columns["VeneerTypeID"].Visible = false;

            //create a button to add new VeneerType row to datagridview
            Button addVeneerType = new Button();
            addVeneerType.Text = "Add VeneerType";
            addVeneerType.Location = new Point(100, 0);
            addVeneerType.Click += new EventHandler(addVeneerType_Click);
            this.Controls.Add(addVeneerType);

            //create a button to remove selected VeneerType row from datagridview
            Button deleteVeneerType = new Button();
            deleteVeneerType.Text = "Delete VeneerType";
            deleteVeneerType.Location = new Point(200, 0);
            deleteVeneerType.Click += new EventHandler(deleteVeneerType_Click);
            this.Controls.Add(deleteVeneerType);


        }

        // save edited data
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // get the selected row from eventargs
            var selectedRow = dataGridView1.Rows[e.RowIndex];
            // get the selected VeneerType
            VeneerType veneerType = (VeneerType)selectedRow.DataBoundItem;
            // update the selected VeneerType in the database
            using (var db = new Model1())
            {
                db.veneerTypes.Attach(veneerType);
                db.Entry(veneerType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        // add new VeneerType row to datagridview
        private void addVeneerType_Click(object sender, EventArgs e)
        {
            // create a new VeneerType
            VeneerType veneerType = new VeneerType();
            // add the new VeneerType to the database
            using (var db = new Model1())
            {
                db.veneerTypes.Add(veneerType);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().veneerTypes.ToList();
        }

        // delete selected VeneerType row from datagridview
        private void deleteVeneerType_Click(object sender, EventArgs e)
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
            // get the selected VeneerType
            VeneerType veneerType = (VeneerType)selectedRow.DataBoundItem;
            // remove the selected VeneerType from the database
            using (var db = new Model1())
            {
                db.veneerTypes.Attach(veneerType);
                db.veneerTypes.Remove(veneerType);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().veneerTypes.ToList();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //open veneerPricesForm
            VeneerPricesForm veneerPricesForm = new VeneerPricesForm();
            veneerPricesForm.Show();
            this.Hide();
        }

        private void back_Click(object sender, EventArgs e)
        {
            //create a new form and open it while hiding the current one
            MainForm main = new MainForm();
            main.Show();
            this.Close();
        }
    }
}
