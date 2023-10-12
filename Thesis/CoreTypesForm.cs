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
    public partial class CoreTypesForm : Form
    {
        public CoreTypesForm()
        {
            InitializeComponent();

            // add a back button on the top of the form in the bar
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);

            //bind data source to the coreTypes table
            dataGridView1.DataSource = new Model1().coreTypes.ToList();

            //save edited data
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

            //hide the CoreTypeID column
            dataGridView1.Columns["CoreTypeID"].Visible = false;

            //create a button to add new CoreType row to datagridview
            Button addCoreType = new Button();
            addCoreType.Text = "Add CoreType";
            addCoreType.Location = new Point(100, 0);
            addCoreType.Click += new EventHandler(addCoreType_Click);
            this.Controls.Add(addCoreType);

            //create a button to remove selected CoreType row from datagridview
            Button deleteCoreType = new Button();
            deleteCoreType.Text = "Delete CoreType";
            deleteCoreType.Location = new Point(200, 0);
            deleteCoreType.Click += new EventHandler(deleteCoreType_Click);
            this.Controls.Add(deleteCoreType);

        }

        // save edited data
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // get the selected row from eventargs
            var selectedRow = dataGridView1.Rows[e.RowIndex];
            // get the selected CoreType
            CoreType coreType = (CoreType)selectedRow.DataBoundItem;
            // update the selected CoreType in the database
            using (var db = new Model1())
            {
                db.coreTypes.Attach(coreType);
                db.Entry(coreType).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().coreTypes.ToList();
        }

        // add new CoreType button click event
        private void addCoreType_Click(object sender, EventArgs e)
        {
            // create a new customer
            CoreType coreType = new CoreType();
            // add the new customer to the database
            using (var db = new Model1())
            {
                db.coreTypes.Add(coreType);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().coreTypes.ToList();
        }

        // delete selected CoreType button click event
        private void deleteCoreType_Click(object sender, EventArgs e)
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
            // get the selected CoreType
            CoreType coreType = (CoreType)selectedRow.DataBoundItem;
            // remove the selected CoreType from the database
            using (var db = new Model1())
            {
                db.coreTypes.Attach(coreType);
                db.coreTypes.Remove(coreType);
                db.SaveChanges();
            }
            // refresh the datagridview
            dataGridView1.DataSource = new Model1().coreTypes.ToList();
        }

        // back button click event
        private void back_Click(object sender, EventArgs e)
        {
            //create a new form and open it while hiding the current one
            Form1 main = new Form1();
            main.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //open corePricesForm
            CorePricesForm corePricesForm = new CorePricesForm();
            corePricesForm.Show();
            this.Hide();
        }
    }
}
