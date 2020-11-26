using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RosSharp.RosBridgeClient;
using Whitebox.Models;
using Whitebox.Daemons;

namespace Whitebox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var messsageHandler = MesssageHandlerSingleton.Instance;
            var serviceManager = DaemonManagerSingleton.Instance;
            var rosManager = RosConnectorHandlerSingleton.Instance;
            AppManager manager = new AppManager("AppManager");
            Whitebox.Daemons.Communication communication = new Whitebox.Daemons.Communication("Communication");
            Whitebox.Daemons.Navigation navigation = new Whitebox.Daemons.Navigation("Navigation");
            Whitebox.Daemons.Database database = new Whitebox.Daemons.Database("Database");
           
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseUrls("http://192.168.100.142:5123")
                .UseStartup<Startup>();
    }
}
