using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using DotNet.Core2.Web.Skeleton.Models;
using Newtonsoft.Json;
using System.Linq;
using DotNet.Core2.Web.Skeleton.Code;
using Newtonsoft.Json.Converters;
/*

Copyright (C) 2016-2018 by Vladimir Novick http://www.linkedin.com/in/vladimirnovick ,

    vlad.novick@gmail.com , http://www.sgcombo.com , https://github.com/Vladimir-Novick
	

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/
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
