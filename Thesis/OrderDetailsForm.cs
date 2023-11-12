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
    public partial class OrderDetailsForm : Form
    {
        int OpenOrderID=0;

        public OrderDetailsForm(int orderID)
        {   
            InitializeComponent();

            OpenOrderID = orderID;
            label1.Text = " Currently viewing order with ID:" + OpenOrderID.ToString();

            // add a back button on the top of the form in the bar
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);

            //create a button to add new cores
            Button addCore = new Button();
            addCore.Text = "Add Core";
            addCore.Location = new Point(100, 0);
            addCore.Click += new EventHandler(addCore_Click);
            this.Controls.Add(addCore);

            //create a button to delete selected cores
            Button deleteCore = new Button();
            deleteCore.Text = "Delete Core";
            deleteCore.Location = new Point(200, 0);
            deleteCore.Click += new EventHandler(deleteCore_Click);
            this.Controls.Add(deleteCore);

            //create a button to open veneer info
            Button veneerInfo = new Button();
            veneerInfo.Text = "Open Veneer Info";
            veneerInfo.Location = new Point(300, 0);
            veneerInfo.Click += new EventHandler(veneerInfo_Click);
            this.Controls.Add(veneerInfo);

            // show controls to add new rows or delete existing ones from datagridview
            dataGridView1.RowHeadersVisible = true;

            //bind data source to the cores table where OrderId matches
            dataGridView1.DataSource = new Model1().cores.Where(x => x.OrderID == OpenOrderID).ToList();

            //hide the CoreTypeID column
            dataGridView1.Columns["CoreTypeID"].Visible = false;
            dataGridView1.Columns["CoreType"].Visible = false;
            dataGridView1.Columns["Order"].Visible = false;
            dataGridView1.Columns["OrderID"].Visible = false;
            dataGridView1.Columns["FaceVeneerID"].Visible = false;
            dataGridView1.Columns["BackVeneerID"].Visible = false;
            dataGridView1.Columns["FaceVeneer"].Visible = false;
            dataGridView1.Columns["BackVeneer"].Visible = false;

            //make OrderID, CoreID column read only
            dataGridView1.Columns["CoreID"].ReadOnly = true;
            dataGridView1.Columns["OrderID"].ReadOnly = true;

            //create a column to show core type
            DataGridViewComboBoxColumn coreTypeColumn = new DataGridViewComboBoxColumn();
            coreTypeColumn.HeaderText = "Core Type";
            coreTypeColumn.Name = "Core Type";
            coreTypeColumn.DataPropertyName = "CoreTypeID";
            coreTypeColumn.DataSource = new Model1().coreTypes.ToList();
            coreTypeColumn.ValueMember = "CoreTypeID";
            coreTypeColumn.DisplayMember = "coreTypeName";
            dataGridView1.Columns.Add(coreTypeColumn);
            dataGridView1.Columns["Core Type"].DisplayIndex = 2;


            //make edited data autosaving
            dataGridView1.CellValueChanged += new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // get the edited row from eventargs
            var row = dataGridView1.Rows[e.RowIndex];

            // get the edited core
            Core core = row.DataBoundItem as Core;
            // update the database
            using (var db = new Model1())
            {
                //find order with same core id in the database
                Core dbCore = db.cores.Find(core.CoreID);
                dbCore.CoreTypeID = core.CoreTypeID;
                dbCore.quantity = core.quantity;
                dbCore.width = core.width;
                dbCore.length = core.length;
                dbCore.thickness = core.thickness;

                db.Entry(dbCore).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void veneerInfo_Click(object sender, EventArgs e)
        {
            // get core id of selected row
            try
            {
                var test = dataGridView1.SelectedRows[0];
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a row to open");
                return;
            }
            var row = dataGridView1.SelectedRows[0];
            Core core = row.DataBoundItem as Core;

            this.Hide();
            VeneersForm veneerInfoForm = new VeneersForm(core.CoreID, OpenOrderID);
            veneerInfoForm.Show();
        }

        private void deleteCore_Click(object sender, EventArgs e)
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
            Core core = row.DataBoundItem as Core;
            // delete the selected customer from the database
            using (var db = new Model1())
            {
                //get order with same order id from database
                core = db.cores.Find(core.CoreID);

                //delete associated veneers
                
                Veneer faceVeneer = db.veneers.Find(core.FaceVeneerID);
                if(faceVeneer != null)
                    db.veneers.Remove(faceVeneer);

                Veneer backVeneer = db.veneers.Find(core.BackVeneerID);
                if (backVeneer != null)
                    db.veneers.Remove(backVeneer);

                //remove order from database
                db.cores.Remove(core);
                db.SaveChanges();

                // refresh the datagridview with cores where orderID matches
                dataGridView1.DataSource = db.cores.Where(x => x.OrderID == OpenOrderID).ToList();
            }
            
        }

        private void addCore_Click(object sender, EventArgs e)
        {
            // create a new order
            Core core = new Core();
            try
            {
                //make the order's core type the first core type in the database
                core.CoreTypeID = new Model1().coreTypes.ToList()[0].CoreTypeID;
                core.OrderID = OpenOrderID;
            }
            catch (Exception)
            {
                MessageBox.Show("Please create a core type first");
                return;
            }

            // add it to the database
            using (var db = new Model1())
            {
                db.cores.Add(core);
                db.SaveChanges();
            
                // refresh the datagridview
                dataGridView1.DataSource = db.cores.ToList();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            // hide this form
            this.Hide();
            // show the main form
            OrdersForm ordersForm = new OrdersForm();
            ordersForm.Show();

        }
    }
}
