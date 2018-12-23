using Nancy;
using System;
using System.IO;
using System.Collections.Generic;


namespace NancyStandalone
{
    public class Resources : NancyModule
    {
        string FindClientAsset(params string[] files)
        {
            // find parent of parent of build output folder
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            projectPath = Environment.CurrentDirectory;
            var segments = new List<string>();
            segments.Add(projectPath);
            segments.Add("client");
            segments.AddRange(files);
            return Path.Combine(segments.ToArray());
        }

        public Resources()
        {
            Get("/", args =>
            {
                var index = FindClientAsset("index.html");
                return Response.AsText(File.ReadAllText(index), "text/html");
            });
            // Get("/", _ => Negotiate.WithView("abas")
            //     .WithMediaRangeModel("image/jpeg", () => File.ReadAllBytes(@"1.jpg"))
            //     .WithMediaRangeModel("image/png", () => File.OpenRead(@"1.png")));
            Get("/abbas", parameters =>
            {
                var index = FindClientAsset("abas.html");
                return Response.AsText(File.ReadAllText(index), "text/html");
            });
            Get("/js/{file}", args =>
            {
                var fileName = (string)args.file;
                var filePath = FindClientAsset("js", fileName);
                return Response.AsText(File.ReadAllText(filePath), "text/javascript");
            });
            Get("/img/{file}", args =>
            {
                var fileName = (string)args.file;
                var filePath = FindClientAsset("img", fileName);
                var streem = File.ReadAllBytes(filePath);
                return Response.FromByteArray(streem, "image/jpeg");
                //return Response.AsFile(File.ReadAllText(filePath),"image/jpeg");// .AsText(File.ReadAllText(filePath), "text/javascript");
            });
            Get("/css/{file}", args =>
            {
                var fileName = (string)args.file;
                var filePath = FindClientAsset("css", fileName);
                return Response.AsText(File.ReadAllText(filePath), "text/css");
            });
        }
    }

}