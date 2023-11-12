using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Thesis
{
    public class Model1 : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Thesis.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public Model1()
            : base("name=Model8")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Customer> customers { get; set; }
        public virtual DbSet<Order> orders { get; set; }

        public virtual DbSet<Core> cores { get; set; }

        public virtual DbSet<Veneer> veneers { get; set; }

        public virtual DbSet<CoreType> coreTypes { get; set; }

        public virtual DbSet<VeneerType> veneerTypes { get; set; }

        public virtual DbSet<CorePrice> corePrices { get; set; }

        public virtual DbSet<VeneerPrice> veneerPrices { get; set; }
    }

    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string companyName { get; set; }
        public string address { get; set; }
        public string taxNumber { get; set; }
    }

    public class Order {
        [Key]
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer customer { get; set; }
        public string payment_method { get; set; }
        public DateTime orderDate { get; set; }
        public string mentions { get; set; }
    }

    public class Core {
        [Key]
        public int CoreID { get; set; }
        public int OrderID { get; set; }
        public virtual Order order { get; set; }
        public int CoreTypeID { get; set; }
        public virtual CoreType coreType { get; set; }
        public int FaceVeneerID { get; set; }
        public virtual Veneer faceVeneer { get; set; }
        public int BackVeneerID { get; set; }
        public virtual Veneer backVeneer { get; set; }
        public int quantity { get; set; }
        public double length { get; set; }
        public double width { get; set; }
        public double thickness { get; set; } 
    
    }

    public class Veneer
    {
        [Key]
        public int VeneerID { get; set; }
        public int VeneerTypeID { get; set; }
        public virtual VeneerType veneerType { get; set; }
        public double thickness { get; set; }

    }

    public class CoreType
    {
        [Key]
        public int CoreTypeID { get; set; }
        public string coreTypeName { get; set; }

    }

    public class VeneerType
    {
        [Key]
        public int VeneerTypeID { get; set; }
        public string veneerTypeName { get; set; }

    }


    public class CorePrice
    {
        [Key]
        public int CorePriceID { get; set; }
        public int CoreTypeID { get; set; }
        public virtual CoreType coreType { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }
    }

    public class VeneerPrice
    {
        [Key]
        public int VeneerPriceID { get; set; }
        public int VeneerTypeID { get; set; }
        public virtual VeneerType veneerType { get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }
    }



}