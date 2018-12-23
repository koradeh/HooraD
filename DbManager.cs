using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace NancyStandalone
{
    public static class DbManager
    {
        private static readonly string ConnectionString;

        static DbManager()
        {
            var databaseDir = Environment.CurrentDirectory;
            databaseDir = Path.Combine(databaseDir, "App_Data");
            var databasePath = Path.Combine(databaseDir, "exemplo.db");

            if (!Directory.Exists(databaseDir)) Directory.CreateDirectory(databaseDir);
            if (!File.Exists(databasePath))
            {
                var fs = File.Create(databasePath);
                fs.Close();
            }

            ConnectionString = string.Format(@"Filename={0}", databasePath);
        }

        #region Cliente

        public static List<Cliente> GetAllClients()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var model = db.GetCollection<Cliente>("client").FindAll();
                return model.ToList();
            }
        }

        public static Cliente GetClientById(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var model = db.GetCollection<Cliente>("client").FindById(id);
                return model;
            }
        }

        public static bool SaveClient(Cliente model)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                if(db.GetCollection<Cliente>("client").Update(model))
                {
                    return true;
                }
                else{
                    var result=db.GetCollection<Cliente>("client").Insert(model);
                    return true;
                }
            }
        }

        #endregion

        #region Estabelecimento

        public static List<Estabelecimento> GetAllEstablishments()
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var model = db.GetCollection<Estabelecimento>("establishment").FindAll();
                return model.ToList();
            }
        }

        public static Estabelecimento GetEstablishmentById(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var model = db.GetCollection<Estabelecimento>("establishment").FindById(id);
                return model;
            }
        }

        public static bool SaveEstablishment(Estabelecimento model)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                if(db.GetCollection<Estabelecimento>("establishment").Update(model))
                {
                    return true;
                }
                else{
                    var result=db.GetCollection<Estabelecimento>("establishment").Insert(model);
                    return true;
                }
            }
        }

        public static bool DeleteClient(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                return db.GetCollection<Cliente>("client").Delete(id);
            }
        }

        #endregion

        #region Pagamento
        
        public static Pagamento GetPaymentByIdEstablishment(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                var model = db.GetCollection<Pagamento>("payment").FindOne(x => x.IdEstabelecimento == id);
                return model;
            }
        }

        public static bool SavePayment(Pagamento model)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                if(db.GetCollection<Pagamento>("payment").Update(model))
                {
                    return true;
                }
                else{
                    var result=db.GetCollection<Pagamento>("payment").Insert(model);
                    return true;
                }
            }
        }

        public static bool DeletePayment(int id)
        {
            using (var db = new LiteDatabase(ConnectionString))
            {
                return db.GetCollection<Pagamento>("payment").Delete(id);
            }
        }

        #endregion
    }
}