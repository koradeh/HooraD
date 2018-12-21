namespace NancyStandalone
{
    using System;
    using System.Linq;
    using Nancy;
    using Nancy.Hosting.Self;
    using Nancy.ModelBinding;
    public class Program
    {
        private string _url = "http://localhost";
        private int _port = 12345;
        private NancyHost _nancy;

        public Program()
        {
            var uri = new Uri( $"{_url}:{_port}/");
            var configuration = new HostConfiguration()
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };
                
            _nancy = new NancyHost(configuration, uri);
            //_nancy = new NancyHost(uri);
        }

        private void Start()
        {
            _nancy.Start();
            Console.WriteLine($"Started listennig port {_port}");
            Console.Read();
            _nancy.Stop();
        }

        static void Main(string[] args)
        {
            var p = new Program();
            p.Start();
        }
        public class SimpleModule : Nancy.NancyModule
        {
            public SimpleModule()
            {
                // Get("/", parameters =>
                // {
                //     var response = "Hello" + " World";
                //     return "<h1> " + response + " </h1>";
                // });
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
                Get("/{name}", x => 
                    {
                        return string.Concat("Hello ", x.name);
                    });
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
    }
    public class PostData
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

}
