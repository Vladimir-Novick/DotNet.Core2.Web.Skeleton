using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

using System.Linq;

using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Converters;
using DotNet.Core2.Web.Skeleton.Code;
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
