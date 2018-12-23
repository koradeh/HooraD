// using System;
// using System.IO;
// using System.Reflection;

// using Nancy;
// using Nancy.Bootstrapper;
// using Nancy.Conventions;
// using Nancy.Diagnostics;
// using Nancy.Responses;
// using Nancy.TinyIoc;
// using Nancy.ViewEngines;

// using Newtonsoft.Json;



// namespace NancyStandalone
// {
//     public class NancyBootstrapper : DefaultNancyBootstrapper
//     {

//         protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
//         {
//             base.ApplicationStartup(container, pipelines);
//             NancyBootstrapper.container = container;
//         }

//         public static TinyIoCContainer container { get; private set; }

      

//         protected override void ConfigureApplicationContainer(TinyIoCContainer container)
//         {
//             base.ConfigureApplicationContainer(container);

//             ResourceViewLocationProvider
//                 .RootNamespaces
//                 .Add(GetType().Assembly, "Web.Ui.Views");
//         }

//         protected override void ConfigureConventions(NancyConventions nancyConventions)
//         {
//             base.ConfigureConventions(nancyConventions);

//             var convention = EmbeddedResourceStaticResourceConventionBuilder.AddDirectory(
//                 "/Content", Assembly.GetExecutingAssembly(),
//                 @"Web.Ui.Content");

//             nancyConventions.StaticContentsConventions.Add(convention);
//         }
//         protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
//         {

           
//             base.RequestStartup(container, pipelines, context);
//         }

      

      
       

//     }
//     public static class EmbeddedResourceStaticResourceConventionBuilder
//     {
//         public static Func<NancyContext, string, Response> AddDirectory(string requestedPath, Assembly assembly, string namespacePrefix)
//         {
//             return (context, s) =>
//             {
//                 var path = context.Request.Path;

//                 if (!path.StartsWith(requestedPath))
//                 {
//                     return null;
//                 }

//                 string resourcePath;
//                 string name;

//                 var adjustedPath = path.Substring(requestedPath.Length + 1);

//                 if (adjustedPath.IndexOf('/') >= 0)
//                 {
//                     name = Path.GetFileName(adjustedPath);
//                     resourcePath = namespacePrefix + "." + adjustedPath
//                         .Substring(0, adjustedPath.Length - name.Length - 1)
//                         .Replace('/', '.');
//                 }
//                 else
//                 {
//                     name = adjustedPath;
//                     resourcePath = namespacePrefix;
//                 }

//                 return new EmbeddedFileResponse(assembly, resourcePath, name);
//             };
//         }
//     }

// }