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
    public partial class VeneersForm : Form
    {
        int OpenCoreID;
        int OpenOrderID;

        public VeneersForm(int coreID, int openOrderID)
        {
            InitializeComponent();

            OpenCoreID = coreID;
            OpenOrderID = openOrderID;
            label6.Text = " Currently viewing core no." + OpenCoreID.ToString() + " from order no." + OpenOrderID.ToString();

            // add a back button on the top of the form in the bar
            Button back = new Button();
            back.Text = "Back";
            back.Location = new Point(0, 0);
            back.Click += new EventHandler(back_Click);
            this.Controls.Add(back);

            //make comboBox2 a dropdown of veneerTypes with IDs as values
            comboBox2.DataSource = new Model1().veneerTypes.ToList();
            comboBox2.DisplayMember = "veneerTypeName";
            comboBox2.ValueMember = "VeneerTypeID";
            comboBox2.SelectedIndex = 0;

            //make comboBox1 a dropdown of veneerTypes with IDs as values
            comboBox1.DataSource = new Model1().veneerTypes.ToList();
            comboBox1.DisplayMember = "veneerTypeName";
            comboBox1.ValueMember = "VeneerTypeID";
            comboBox1.SelectedIndex = 0;

            loadVeneerDetails();
        }

        private void loadVeneerDetails() {
            //check if we have a face veneer
            using (Model1 db = new Model1())
            {
                Veneer faceVeneer = db.veneers.Where(x => x.VeneerID == db.cores.Where(y => y.CoreID == OpenCoreID).FirstOrDefault().FaceVeneerID).FirstOrDefault();

                if (faceVeneer != null)
                {
                    comboBox2.SelectedValue = faceVeneer.VeneerTypeID;
                }

                //check if we have a back veneer
                Veneer backVeneer = db.veneers.Where(x => x.VeneerID == db.cores.Where(y => y.CoreID == OpenCoreID).FirstOrDefault().BackVeneerID).FirstOrDefault();

                    if (backVeneer == null)
                    {
                        checkBox1.Checked = false;
                        comboBox1.Enabled = false;
                    }
                    else {
                        checkBox1.Checked = true;
                        comboBox1.Enabled = true;

                        //select thickness and veneer type
                        comboBox1.SelectedValue = backVeneer.VeneerTypeID;

                    }

            }

        }

        private void back_Click(object sender, EventArgs e)
        {
            //save thickness and selected veneerTypeIDs to the database
            using (Model1 db = new Model1())
            {
                Veneer faceVeneer = db.veneers.Where(x => x.VeneerID == db.cores.Where(y => y.CoreID == OpenCoreID).FirstOrDefault().FaceVeneerID).FirstOrDefault();

                if (faceVeneer == null)
                {
                    //create a brand new faceVeneer with 0 as thickness and the first veneertype as veneerTypeID
                    faceVeneer = new Veneer();
                    faceVeneer.VeneerTypeID = Convert.ToInt32(comboBox2.SelectedValue);
                    faceVeneer.VeneerID = new Model1().veneers.Count() + 1;


                    //add the new faceVeneer to the database and store the new ID

                    db.veneers.Add(faceVeneer);
                    db.cores.Where(x => x.CoreID == OpenCoreID).FirstOrDefault().FaceVeneerID = faceVeneer.VeneerID;
                    db.SaveChanges();
                }
                else
                {
                    //update thickness and veneerTypeID
                    faceVeneer.VeneerTypeID = Convert.ToInt32(comboBox2.SelectedValue);

                }



                //get the backVeneer
                Veneer backVeneer = db.veneers.Where(x => x.VeneerID == db.cores.Where(y => y.CoreID == OpenCoreID).FirstOrDefault().BackVeneerID).FirstOrDefault();
                if (checkBox1.Checked)
                {
                    if (backVeneer == null)
                    {
                        backVeneer = new Veneer();
                        backVeneer.VeneerTypeID = Convert.ToInt32(comboBox1.SelectedValue);
                        backVeneer.VeneerID = db.veneers.Count() + 1;

                        db.veneers.Add(backVeneer);
                        db.cores.Where(x => x.CoreID == OpenCoreID).FirstOrDefault().BackVeneerID = backVeneer.VeneerID;
                        db.SaveChanges();
                    }else
                    {
                        //update thickness and veneerTypeID
                        backVeneer.VeneerTypeID = Convert.ToInt32(comboBox1.SelectedValue);

                    }

                }
                else {

                    if (backVeneer != null)
                    {
                        //remove backVeneer from database
                        db.veneers.Remove(db.veneers.Where(x => x.VeneerID == db.cores.Where(y => y.CoreID == OpenCoreID).FirstOrDefault().BackVeneerID).FirstOrDefault());
                        db.cores.Where(x => x.CoreID == OpenCoreID).FirstOrDefault().BackVeneerID = -1;
                    }
                }

                //save changes
                db.SaveChanges();
            }

            // go back to the previous form
            OrderDetailsForm orderDetailsForm = new OrderDetailsForm(OpenOrderID);
            orderDetailsForm.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if the checkbox is checked, add the backVeneer to the database and set the core's backVeneerID to the new backVeneer's ID
            if (checkBox1.Checked)
            {
                //enable fields
                comboBox1.Enabled = true;
            }
            else {
                comboBox1.Enabled = false;
            }

        }
    }
}
