using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using DotNet.Core2.Web.Skeleton.Models;
using Newtonsoft.Json;
using System.Linq;
using DotNet.Core2.Web.Skeleton.Code;
using Newtonsoft.Json.Converters;

namespace DotNet.Core2.Web.Skeleton
{
    public class Program
    {

        public static List<AdminUser> AdminUsrList { get; set; }

        public static void Main(string[] args)
        {


            IConfigurationRoot config;



            string baseDir = Directory.GetCurrentDirectory();

                config = new ConfigurationBuilder()
                .SetBasePath(baseDir)
                .AddJsonFile("config" + Path.DirectorySeparatorChar + "hosting.json", true)
                .Build();


            string f = baseDir + Path.DirectorySeparatorChar +  "config" + Path.DirectorySeparatorChar + "admin_users.json";



            using (StreamReader file = File.OpenText(f))
            {

                JsonSerializer serializer = new JsonSerializer();
                AdminUsrList = (List<AdminUser>)serializer.Deserialize(file, typeof(List<AdminUser>));
            }


            string dataFile  = baseDir + Path.DirectorySeparatorChar + "config" + Path.DirectorySeparatorChar + "UserDataItems.json";

           

            using (StreamReader fileData = File.OpenText(dataFile))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                List< UserDataItem> listData = (List<UserDataItem>)serializer.Deserialize(fileData, typeof(List<UserDataItem>));
                foreach (var item in listData)
                {
                    DataCache.UserDataCache.Add(item);
                }
            }




            string url = config.GetValue<string>("server.urls");

            var host = new WebHostBuilder()
               .UseKestrel()
               .UseLibuv(options =>
               {
                   options.ThreadCount = 10;
               })
                 .UseUrls(url)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();

        }




    }
}
