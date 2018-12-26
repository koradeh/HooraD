using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace NancyStandalone
{
    public class TestModule : NancyModule
    {
        public TestModule()
        {
            Get("/help", parameters => {
                var blogPost = new BlogPost {
                    Id = 1,
                    Title = "From ASP.NET MVC to Nancy - Part 2",
                    Content = "Lorem ipsum...",
                    Tags = {"c#", "aspnetmvc", "nancy"}
                };
                //return "Mahdi is here!";
                return View["Index", blogPost];
            });
            Get("/new/", parameters => {
                return View["New"];
            });
            Post("/new/", paramters => {
                var blogPost = this.Bind<BlogPost>();
                // Redirects the user to our Index action with a "status" value as a querystring.
                return Response.AsRedirect("/?status=added&title=" + blogPost.Title);
            });
            
        }
    }
    public class AdminModule : NancyModule
    {
        public AdminModule(WebApplicationService webApplicationService) : base("/admin")
        {
            Get("/", parameters => {
                var model = new IndexModel {
                    CreationDate = webApplicationService.GetCreationDate(),
                    TotalRequests = webApplicationService.TimesRequested()
                };

                return View["admin", model];
            });
        }
    }
    // public class MyBootstrapper : Nancy.DefaultNancyBootstrapper
    // {
    //     protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    //     {
    //         base.ApplicationStartup(container, pipelines);
    //         container.Register<WebApplicationService>().AsSingleton();
    //     }
    // }
    public class WebApplicationService
    {
        private readonly DateTime _created;
        private int _totalRequest;

        public WebApplicationService()
        {
            _created = DateTime.Now;
            _totalRequest = 0;
        }

        public DateTime GetCreationDate()
        {
            _totalRequest++;
            return _created;
        }

        public int TimesRequested()
        {
            return _totalRequest;
        }
    }
    public class IndexModel
    {
        public DateTime CreationDate { get; set; }
        public int TotalRequests { get; set; }
    }
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }

        public BlogPost()
        {
            Tags = new List<string>();
        }
    }
    public class HomeModule : NancyModule
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
        public static IList<Product> Products = new List<Product>()
        {
            new Product {Id = 1, Name = "Surface", Price = 499},
            new Product {Id = 2, Name = "iPad", Price = 899},
            new Product {Id = 3, Name = "Nexus 10", Price = 599},
            new Product {Id = 4, Name = "Think Pad", Price = 499},
            new Product {Id = 5, Name = "Yoga", Price = 699},
        };

        public dynamic Model = new ExpandoObject();

        public HomeModule()
        {
            Model.Deleted = false;

            Get("/prod", _ =>
            {
                Model.Products = Products;

                return View["product", Model];
            });
            Get(@"/delete/{id}", _ =>
            {
                var id      = (int) _.id;
                var item    = Products.Single(x => x.Id == id);
                
                Products.Remove(item);

                Model.Products = Products;
                Model.Deleted = true;

                return Negotiate
                    .WithModel((object) Model)
                    .WithMediaRangeModel("application/json", new
                    {
                        Model.Deleted
                    })
                    .WithView("product");
            });
        }
    }
}