using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;

namespace NancyStandalone
{
    public class Stock
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(8)]
        public string Symbol { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]      // One to many relationship with Valuation
        public List<Valuation> Valuations { get; set; }
    }

    public class Valuation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Stock))]     // Specify the foreign key
        public int StockId { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }

        [ManyToOne]      // Many to one relationship with Stock
        public Stock Stock { get; set; }
    }
    public class Ver
    {
        public string Version { get; set; }
    }
    public static class DbManager
    {
        private static readonly string ConnectionString;

        static DbManager()
        {
            var databaseDir = Environment.CurrentDirectory;
            databaseDir = Path.Combine(databaseDir, "App_Data");
            var databasePath = Path.Combine(databaseDir, "example.db");

            if (!Directory.Exists(databaseDir)) Directory.CreateDirectory(databaseDir);
            if (!File.Exists(databasePath))
            {
                var fs = File.Create(databasePath);
                fs.Close();
                using (var db = new SQLiteConnection(databasePath))
                {
                    db.CreateTable<Stock>();
                    db.CreateTable<Valuation>();
                    var euro = new Stock() {
                        Symbol = "�"
                    };
                    db.Insert(euro);   // Insert the object in the database

                    var valuation = new Valuation() {
                        Price = 15,
                        Time = DateTime.Now,
                    };
                    db.Insert(valuation);   // Insert the object in the database

                    // Objects created, let's stablish the relationship
                    euro.Valuations = new List<Valuation> { valuation };

                    db.UpdateWithChildren(euro);   // Update the changes into the database
                    if (valuation.Stock == euro) {
                        Console.WriteLine("Inverse relationship already set, yay!");
                    }
                }
            }

            ConnectionString = databasePath;//string.Format(@"Filename={0}", databasePath);
        }

        #region Stock
        public static List<Stock> GetAllStocks()
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var model = db.Table<Stock>();   //GetCollection<Stock>("Stock").FindAll();
                return model.ToList();
            }
        }

        public static Stock GetStockById(int id)
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var model = db.Find<Stock>(id);
                return model;
            }
        }

        public static bool SaveStock(Stock model)
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var result=db.InsertOrReplace(model);
                return (result>0)? true: false;
            }
        }

        #endregion

        #region Valuation

        public static List<Valuation> GetAllValuations()
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var model = db.Table<Valuation>();
                return model.ToList();
            }
        }

        public static Valuation GetValuationById(int id)
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var model = db.Find<Valuation>(id);
                var storedValuation = db.GetWithChildren<Valuation>(id);
                return storedValuation;
            }
        }

        public static bool SaveValuation(Valuation model)
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var result=db.InsertOrReplace(model);
                return (result>0)? true: false;
            }
        }

        public static bool DeleteStock(int id)
        {
            using (var db = new SQLiteConnection(ConnectionString))
            {
                var result= db.Delete<Stock>(id);
                return (result>0)? true: false;
            }
        }

        #endregion

    }
}