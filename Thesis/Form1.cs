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

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            using (var db = new Model1())
            {
                var Customer = new Customer { CustomerID=1, companyName = "abc", address = "Address", taxNumber = "TaxNumber" };
                db.customers.Add(Customer);

                var Order = new Order { OrderID = 1 ,CustomerID = 1, payment_method = "Cash", orderDate = DateTime.Now, mentions = "Mentions", workID = "WorkID" };
                db.orders.Add(Order);

                var Core = new Core { CoreID = 1, OrderID = 1, CoreTypeID = 1, quantity = 1, length = 1, width = 1, thickness = 1, total_required_area = 1 };
                db.cores.Add(Core);

                var CoreType = new CoreType { CoreTypeID = 1 ,coreTypeName = "MDF" };
                db.coreTypes.Add(CoreType);

                var CorePrice = new CorePrice { CoreTypeID = 1, price = 1, date = DateTime.Now };
                db.corePrices.Add(CorePrice);

                

                var VeneerType = new VeneerType { VeneerTypeID = 1, veneerTypeName = "Bukk" };
                db.veneerTypes.Add(VeneerType);

                var FaceVeneer = new Veneer { VeneerID = 1, CoreID = 1, VeneerTypeID = 1, parallelLength = 1, crossLength = 1, thickness = 1, isFaceVeneer = true };
                db.veneers.Add(FaceVeneer);

                var BackVeneer = new Veneer { VeneerID = 2, CoreID = 1, VeneerTypeID = 1, parallelLength = 1, crossLength = 1, thickness = 1, isFaceVeneer = false };
                db.veneers.Add(BackVeneer);

                var VeneerPrice = new VeneerPrice { VeneerTypeID = 1, price = 1, date = DateTime.Now};
                db.veneerPrices.Add(VeneerPrice);

                db.SaveChanges();

                //display all customers from the database
                var query = from b in db.customers
                            orderby b.companyName
                            select b;

                // print all queried customers in a dialog box
                string output = ""; 
                foreach (var item in query)
                {
                    output += item.companyName + "\n";
                }
                MessageBox.Show(output);


            }
        }

    }
}
