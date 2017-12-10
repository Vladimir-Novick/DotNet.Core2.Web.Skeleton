using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

using System.Linq;

using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
using DotNet.Core2.Web.Skeleton.Code;

namespace MQS.NetCore2.Server.Controllers
{

    public class ExampleAjaxController : Controller
    {

        [HttpPost]
        public IActionResult GetKeyList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null, String Key = null)
        {

            IEnumerable<UserDataItem> QueList;

            if (Key == null)
            {
                QueList = DataCache.UserDataCache.Select(c =>
               {
                   return new UserDataItem()
                   {
                       Created = c.Created,
                       Key = c.Key,
                       IntData = c.IntData,
                       TextData = c.TextData
                   };
               });
            }
            else
            {

                var record = DataCache.UserDataCache.Where(c => c.Key == Key);

                QueList = record.Select(c =>
                  {
                      return new UserDataItem()
                      {
                          Created = c.Created,
                          Key = c.Key,
                          IntData = c.IntData,
                          TextData = c.TextData
                      };
                  });

            }

            if (QueList == null)
            {
                QueList = new List<UserDataItem>();
            }

            int totalRecord = QueList.Count();

            if (totalRecord > 0)
            {
                if (string.IsNullOrEmpty(jtSorting) || jtSorting.Equals("Created ASC"))
                {
                    QueList = QueList.OrderBy(p => p.Created);
                }
                else if (jtSorting.Equals("Created DESC"))
                {
                    QueList = QueList.OrderByDescending(p => p.Created);
                }

                if (string.IsNullOrEmpty(jtSorting) || jtSorting.Equals("Key ASC"))
                {
                    QueList = QueList.OrderBy(p => p.Key);
                }
                else if (jtSorting.Equals("Key DESC"))
                {
                    QueList = QueList.OrderByDescending(p => p.Key);
                }

                if (string.IsNullOrEmpty(jtSorting) || jtSorting.Equals("IntData ASC"))
                {
                    QueList = QueList.OrderBy(p => p.IntData);
                }
                else if (jtSorting.Equals("IntData DESC"))
                {
                    QueList = QueList.OrderByDescending(p => p.IntData);
                }


                if (string.IsNullOrEmpty(jtSorting) || jtSorting.Equals("TextData ASC"))
                {
                    QueList = QueList.OrderBy(p => p.TextData);
                }
                else if (jtSorting.Equals("TextData DESC"))
                {
                    QueList = QueList.OrderByDescending(p => p.TextData);
                }


                if (jtPageSize > 0)
                {
                    QueList = QueList.Skip(jtStartIndex).Take(jtPageSize);
                }
            }

            var str = new { Result = "OK", Records = QueList.ToList(), TotalRecordCount = totalRecord };

            String ret = JsonConvert.SerializeObject(str,
                new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

            return Ok(ret);
        }

    }
}
