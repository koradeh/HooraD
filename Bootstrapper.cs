using Nancy;
using Nancy.Configuration;
using Nancy.Conventions;
using Nancy.Diagnostics;
using System;

namespace Nancy_Standalone
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions conventions)
        {
            Console.WriteLine("CustomBootstrapper ConfigureConventions...");
            // conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("client", @"client"));
            // conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("lib", @"lib"));
            // base.ConfigureConventions(conventions);
        }
        public override void Configure(INancyEnvironment environment)
        {
            environment.Diagnostics(
                enabled: true,
                password: "password",
                path: "/_Nancy",
                cookieName: "__custom_cookie",
                slidingTimeout: 30);

            environment.Tracing(
                enabled: true,
                displayErrorTraces: true);

            //environment.MyConfig("Hello World");
            Console.WriteLine("CustomBootstrapper Configure...");
        }
    }
}