namespace NancyStandalone
{
    using System;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using Nancy;
    using Nancy.Hosting.Self;
    using Nancy.ModelBinding;
    using Nancy_Standalone;

    public static class Consts
    {
        public const string HOSTNAME = "http://localhost";
        public const int PORT =12345; 
        public const string BASE_PREFIX_V1 = "/api/v1/persons";
    }
    public class Version
    {
        public string Ver { get; set; }
    }
    public class Program
    {
        private NancyHost _nancy;

        public Program()
        {
            var uri = new Uri( $"{Consts.HOSTNAME}:{Consts.PORT}/");
            var configuration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
                
            _nancy = new NancyHost(uri, new CustomBootstrapper(), configuration);
            //_nancy = new NancyHost(uri);
        }

        private void Start()
        {
            _nancy.Start();
            Console.WriteLine($"Started listennig port {Consts.PORT}");
            Console.Read();
            _nancy.Stop();
        }

        static void Main(string[] args)
        {
            String folder = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            String fileName = Path.Combine(folder, "badges.db");
            SeedDatabase(fileName);

            var p = new Program();
            p.Start();


        }
        private static void SeedDatabase(string fileName)
        {
            String dbConnection = String.Format("Data Source={0}", fileName);
            String sql = @"
                        create table [Badges] (
                        [Id] INTEGER PRIMARY KEY ASC,
                        [Title] varchar(20) ,
                        [Description] varchar(255),
                        [Level] int)";
            ExecuteNonQuery(dbConnection, sql);

            sql = @"insert into Badges ([Title], [Description], [Level]) 
                    values ('Site MVP', 'Awarded to members who contribute often and wisely', 2);";
            ExecuteNonQuery(dbConnection, sql);
        }
        // Helper extracted from SqliteHelper.cs
		public static int ExecuteNonQuery(string dbConnection, string sql)
		{
			SQLiteConnection cnn = new SQLiteConnection(dbConnection);
			try
			{
				cnn.Open();
				SQLiteCommand mycommand = new SQLiteCommand(cnn);
				mycommand.CommandText = sql;
				int rowsUpdated = mycommand.ExecuteNonQuery();
				return rowsUpdated;
			}
			catch (Exception fail)
			{
				Console.WriteLine(fail.Message);
				return 0;
			}
			finally
			{
				cnn.Close();
			}
		}
        public class SimpleModule : Nancy.NancyModule
        {
            public SimpleModule()
            {
                Get("/version", parameters =>
                {
                    var model = new Version() { Ver = "1.0.0.0" };
                    return Response.AsJson(model, HttpStatusCode.OK);

                });
                Get("/os", x =>
                {
                    return System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                });
                Get("/search", parameters =>
                {
                    string s = this.Request.Query["q"];
                    return Response.AsRedirect($"http://www.google.com/search?q={s}");
                });
                Get("/mine", parameters =>
                {
                    string s = this.Request.Query["textinput"];
                    return String.Format("Hello.\r\nYour textinput is {0}",s);
                });
                Post("/{id:int}", parameters =>
                {
                    PostData  request = this.Bind<PostData>();
                    return request.Email + " end.";
                });
                // Get("/{name}", x => 
                //     {
                //         return string.Concat("Hello ", x.name);
                //     });
                Get("/viewtest", parameters =>
                {
                    PostData data = new PostData()
                    {
                    Name = "Peter Shaw",
                    Email = "top@secret.com"
                    };

                    return View["ViewTest", data];
                });
                Post("/submit", parameters =>
                {
                    var response = "Hello" + " World";
                    return "<html><h1> " + response + " </h1></html>";
                });
                //Get["/"] = _ => "Received GET request";

                //Post["/"] = _ => "Received POST request";

                //Get["/"] = _ => "Let me have the request!";
            }

        }
        public class BadgeModule : NancyModule
    {
        public BadgeModule() : base("/Badges")
        {
            // http://localhost:8088/Badges/99
            Get("/{id}", parameter => { return GetById(parameter.id); });
            Get("/b", parameter => { return "GetById(parameter.id)"; });
        }

        private object GetById(int id)
        {
            // fake a return
            return new {Id = id, Title="Site Admin", Level=2};
        }
    }
    }
    public class PostData
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

}
