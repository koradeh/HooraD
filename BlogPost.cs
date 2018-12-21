using System;
using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.TinyIoc;
using Nancy.ViewEngines.Razor;

namespace NancyStandalone
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
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
    public class MyBootstrapper : Nancy.DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Register<WebApplicationService>().AsSingleton();
        }
    }
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
}