using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.CDK;
using Amazon.CDK.RegionInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace orders.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            CreateAwsCDKBuilder(args);
        }

        private static void CreateAwsCDKBuilder(string[] args)
        {
            var aws_info = new
            {

            };
            var app = new App();
            var d = new AWSStack(app, "orderapp", new StackProps() { Env = new Amazon.CDK.Environment() {  Account = "<AWS-ACCOUNT-NUMBER-HERE>", Region = "us-east-1" } });
            app.Synth();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

